using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShippingService.Business;
using ShippingService.Business.Domain;

namespace Web
{
    public partial class Pack : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ResetOrder();

                List<dynamic> ctrs = new List<dynamic>();
                foreach (var c in ApplicationContextHolder.Instance.Facade.GetContainers())
                {
                    ctrs.Add(new 
                    {
                        Id = c.Id,
                        Name = c.Name + "[" + c.Weight + "gr]"
                    });
                }

                var carriers = ctrs;

                ddlContainers.DataSource = carriers;
                ddlContainers.DataBind();

                rptOrderLines.DataSource = GetOrder().Lines;
                rptOrderLines.DataBind();

                
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            
        }

        private Order GetOrder()
        { 
            if(Session["PACK_ORDER"] == null)
                Session["PACK_ORDER"]  = ApplicationContextHolder.Instance.Facade.GetTodoOrderById(Request["Id"]);
            return Session["PACK_ORDER"] as Order; 
        }

        private void ResetOrder()
        {
            Session["PACK_ORDER"] = null;
        }

        protected void btnPack_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (ddlContainers.SelectedValue == string.Empty)
            {
                Page.Validators.Add(new BusinessValidationError("Select a container"));
            }
            if (!Page.IsValid) return;

           
            var order = GetOrder();
            var containerId = ddlContainers.SelectedValue;

            PackedContainer pc = new PackedContainer()
            {
                Id = order.PackedContainers.Count.ToString(),
                Container = ApplicationContextHolder.Instance.Facade.GetContainerById(containerId)
            };

            foreach (RepeaterItem item in rptOrderLines.Items)
            {
                var field = item.FindControl("txtPacked") as TextBox;
                var line = item.FindControl("labLineNumber") as Label;
                if (field == null || line == null) continue; //guard
                if (field.Text == string.Empty) continue; //empty
                if (field.Text == "0") continue; //empty
                var lineNumber = line.Text;
                int packed;
                if (!int.TryParse(field.Text, out packed))
                {
                    Page.Validators.Add(new BusinessValidationError("Invalid Number, line " + lineNumber));
                    break;
                }
                if (packed == 0) continue;
                var orderLine = order.Lines.Find(ol => ol.Id == lineNumber);
                if (orderLine == null) continue;
                if (orderLine.Packs == null)
                    orderLine.Packs = new List<PackedOrderLine>();

                orderLine.Packs.Add(new PackedOrderLine
                {
                    PackedContainer = pc,
                    Qty = packed, 
                });
            }
            
            if (order.Lines.TrueForAll(ol => ol.PackQty == 0))
            {
                
                rptPacks.DataSource = GetOrder().PackedContainersWithParts;
                rptPacks.DataBind();

                pnlPacks.Visible = true;
                pnlOrders.Visible = false;
                btnPack.Visible = false;
                
            }
            
            rptOrderLines.DataSource = order.Lines;
            rptOrderLines.DataBind();

            ddlContainers.Focus();
            
        }

        private void save()
        {
            Order o = GetOrder();

            foreach (RepeaterItem item in rptPacks.Items)
            {
                int packedContainerId = Convert.ToInt32((item.FindControl("packedContainerId") as HiddenField).Value);
                int weight = Convert.ToInt32((item.FindControl("tbWeight") as TextBox).Text);

                o.PackedContainers[packedContainerId].Weight = weight;
            }

            if (o.PackedContainers.TrueForAll(opc => opc.Weight > 0))
            {
                //we can save
                ApplicationContextHolder.Instance.Facade.Pack(GetOrder());

                rptPacks.DataSource = GetOrder().PackedContainersWithParts;
                rptPacks.DataBind();

                btnSave.Visible = false;
                btnShip.Visible = true;
            }
        }
        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Order o = GetOrder();
            bool valid = true;

            foreach (RepeaterItem item in rptPacks.Items)
            {
                int weight = Convert.ToInt32(((TextBox)item.FindControl("tbWeight")).Text);
                if (weight.ToString().Equals(o.OrderNumber.ToString())) {
                    valid = false;
                    lblPopupTitle.Text += "The weight seems to be the same as the ordernumber. Press confirm if the weight is indeed correct.";
                }

                int estimatedWeight = Convert.ToInt32((item.FindControl("EstimatedWeight") as HiddenField).Value);
                if (estimatedWeight > 0)
                {
                    //5% difference is allowed
                    int estimatedWeightplus = estimatedWeight + estimatedWeight / 5;
                    int estimatedWeightmin = estimatedWeight - estimatedWeight / 5;

                    if (weight < estimatedWeightmin || weight > estimatedWeightplus)
                    {
                        lblPopupTitle.Text += "There is more than 5% difference between the estimated weight and the entered weight.";
                        valid = false;
                    }
                }


            }

            if (valid) {
                save();
            }
            else {
                mpPopup.Show();
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            save();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpPopup.Show();
        }

        protected void btnShip_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Ship.aspx?Id=" + Request["Id"]);
        }
    }
}
