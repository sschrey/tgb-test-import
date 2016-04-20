using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShippingService.Business;
using ShippingService.Business.Domain;
using ShippingService.Business.CarrierServices;
using ShippingService.Business.Printing;
using System.IO;
using Tweddle.Commons.RAWPrinter;
using System.Configuration;

namespace Web
{
    public partial class Ship : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                ddlCarrier.DataSource = ApplicationContextHolder.Instance.Facade.GetCarriers();
                ddlCarrier.DataBind();

                ddlCarrierMode.DataSource = ApplicationContextHolder.Instance.Facade.GetCarrierModes();
                ddlCarrierMode.DataBind();

                LoadOrder();   
            }
            
        }

        private IList<CarrierModeFilter> GetCarrierModeFilters()
        {
            if(Cache["CarrierModeFilter"] == null)
                Cache["CarrierModeFilter"] = ApplicationContextHolder.Instance.Facade.GetCarrierModeFilters();
            return Cache["CarrierModeFilter"] as IList<CarrierModeFilter>;
            
        }

        private IList<CarrierMode> GetCarrierModes()
        {
            if (Cache["CarrierModes"] == null)
                Cache["CarrierModes"] = ApplicationContextHolder.Instance.Facade.GetCarrierModes();
            return Cache["CarrierModes"] as IList<CarrierMode>;

        }

        private Order GetOrder(string action)
        {
            if (action == "RESHIP" || action == "PRINT_RETURN_LABEL")
            {
                return ApplicationContextHolder.Instance.Facade.GetOrders(new OrderCriteria() { Id = Request["Id"] })[0];
            }

            return ApplicationContextHolder.Instance.Facade.GetOrderbyId(Request["Id"]);
        }
        private Order GetOrder()
        {
            return GetOrder(Request["ACTION"]);
        }


        private void FilterCarrierModes()
        {
            var carrier = ddlCarrier.SelectedValue;
            if(!string.IsNullOrEmpty(carrier))
            {
                var filteredCarrierModes = ApplicationContextHolder.Instance.Facade.GetCarrierModes(carrier, GetCarrierModes(), GetCarrierModeFilters());

                ddlCarrierMode.DataSource = filteredCarrierModes;
                ddlCarrierMode.DataBind();
            }
        }

        private void LoadOrder()
        {
            Page.Form.DefaultButton = btnSave.UniqueID;

            var carrier = GetOrder().ProposedCarrier;
            if (carrier != null)
            {
                ddlCarrier.SelectedValue = carrier;
                FilterCarrierModes();
            }

            string blockedCarrierCodes = ConfigurationManager.AppSettings["BlockedCarrierCodes"];

            if (!string.IsNullOrEmpty(blockedCarrierCodes))
            {
                string[] blockedCarrierCodesA = blockedCarrierCodes.Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries);
                if (blockedCarrierCodesA.Contains(ddlCarrier.SelectedValue))
                {
                    ddlCarrier.Enabled = false;
                }
            }
            
            var carrierMode = GetOrder().ProposedCarrierMode;
            if (carrierMode != null)
            {
                ddlCarrierMode.SelectedValue = carrierMode;
            }

            if (GetOrder().Status == OrderStatus.Shipped)
            {
                ddlCarrier.Enabled = false;
                ddlCarrierMode.Enabled = false;
            }

            ShowReturnLabelCreator(GetOrder());

            rptOrderLines.DataSource = GetOrder().Lines;
            rptOrderLines.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            IShipping shipping = GetShipping();

            if (shipping == null)
            {
                Page.Validators.Add(new BusinessValidationError("Cannot use this shipping vendor"));
                return;
            }
            shipping.Error += Shipping_Error;
            var order = shipping.Order;

            if (!shipping.Execute())
                Page.Validators.Add(new BusinessValidationError(shipping.Message.Replace(Environment.NewLine, "<br/>")));
            else
            {
                //order is shipped and away
                //we set the status here, because of the ups return label creation to work properly
                order.E1Status = "560";
                pnlOrder.Visible = false;
                lblFeedBack.Text = string.Format("Order {0} shipped by {1}/{2}", order.Id, ddlCarrier.SelectedItem.Text, ddlCarrierMode.SelectedItem.Text);
                TryToPrint(shipping);
            }
        }

        private IShipping GetShipping()
        {
            var order = GetOrder();
            if (order == null)
            {
                Page.Validators.Add(new BusinessValidationError("Cannot find order"));
                return null;
            }

            order.ShippedCarrier = ddlCarrier.SelectedValue;
            order.ShippedCarrierMode = ddlCarrierMode.SelectedValue;
            var carrierModeText = ddlCarrierMode.SelectedItem.Text;

            if (carrierModeText.Contains('{') && carrierModeText.Contains('}'))
            {
                order.ShippedCarrierModeOption = carrierModeText.Substring(carrierModeText.IndexOf('{') + 1, carrierModeText.IndexOf('}') - carrierModeText.IndexOf('{') - 1);
            }
            
            var carrierMode = ApplicationContextHolder.Instance.Facade.GetCarrierModeById(ddlCarrierMode.SelectedValue);
            var carrier = ApplicationContextHolder.Instance.Facade.GetCarrierById(ddlCarrier.SelectedValue);
            var shipping = ShippingFactory.GetShipping(ddlCarrier.SelectedValue, carrierMode, order, ApplicationContextHolder.Instance.Facade, Server.MapPath("~/"));
            
            return shipping;
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            var shipping = GetShipping();
            string printer = ddlPrinters.SelectedValue;

            if (string.IsNullOrEmpty(printer)) return;

            var ok = shipping.Print(printer);
            if(ok)
            {
                Response.Redirect("barcode-entry.aspx");
            }
        }

        protected void btnPrintReturnLabel_Click(object sender, EventArgs e)
        {
            var order = GetOrder("PRINT_RETURN_LABEL");
            if (order == null)
            {
                Page.Validators.Add(new BusinessValidationError("Cannot find order"));
                return;
            }

            var carrierMode = ApplicationContextHolder.Instance.Facade.GetCarrierModeById(ddlCarrierMode.SelectedValue);
            var carrier = ApplicationContextHolder.Instance.Facade.GetCarrierById(ddlCarrier.SelectedValue);

            var shipping = ShippingFactory.GetShipping(ddlCarrier.SelectedValue, carrierMode, order, ApplicationContextHolder.Instance.Facade, Server.MapPath("~/"));
            shipping.SetReturnShipment();
            
            shipping.Error += Shipping_Error;
            if (!shipping.Execute())
                Page.Validators.Add(new BusinessValidationError(shipping.Message.Replace(Environment.NewLine, "<br/>")));
            else
            {
                //order is shipped and away
                pnlOrder.Visible = false;
                lblFeedBack.Text = string.Format("return order {0} shipped by {1}/{2}", order.Id, carrier.Name, carrierMode.Name);
                if (shipping.CanPrint)
                {
                    TryToPrint(shipping);
                }
            }

        }

        protected void ShowReturnLabelCreator(Order o)
        {
            pnlReturnLabel.Visible = o.Status == OrderStatus.Shipped && !o.HasReturnLabel;
        }

        protected void TryToPrint(IShipping shipping)
        {
            ShowReturnLabelCreator(shipping.Order);

            bullFiles.Items.Clear();
            bullFiles.Target = "new";
            foreach (var label in shipping.GetPrintFiles())
            {
                bullFiles.Items.Add(new ListItem(label.Filename, "file://" + label.Filename.Replace("\\", "/")));
            }
           
            ddlPrinters.DataSource = PrintManager.InstalledPrinters();
            ddlPrinters.DataBind();
            pnlPrint.Visible = true;
        }

        private void Shipping_Error(string msg)
        {
            Page.Validators.Add(new BusinessValidationError(msg));
        }

        protected void ddlCarrier_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterCarrierModes();
        }
    }
}
