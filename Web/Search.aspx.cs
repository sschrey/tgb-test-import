using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShippingService.Business.Domain;
using System.Web.Services;

namespace Web
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ddlCarrier.DataSource = ApplicationContextHolder.Instance.Facade.GetCarriers();
                ddlCarrier.DataBind();
                ddlCarrier.Items.Insert(0, new ListItem("Select carrier", ""));
            }
        }

        protected bool isFirstRow
        {
            get
            {
                return (bool)ViewState["FIRST_ROW"];
            }
            set
            {
                ViewState["FIRST_ROW"] = value;
            }
        }

        public class OrderCriteriaCheck
        {
            OrderCriteria criteria;
            public OrderCriteriaCheck(OrderCriteria crit)
            {
                this.criteria = crit;
            }

            public bool IsValid(out List<string> brokenRules)
            {
                brokenRules = new List<string>();
                if (string.IsNullOrEmpty(criteria.Id)
                    && string.IsNullOrEmpty(criteria.TrackingNumber)
                    && !criteria.ShippedDateFrom.HasValue)
                {
                    brokenRules.Add("Please specify order id, trackingnumber or date range");
                    return false;
                }

                if (criteria.ShippedDateFrom.HasValue
                    && criteria.ShippedDateTo.Subtract(criteria.ShippedDateFrom.Value).Days > 29)
                {
                    brokenRules.Add("Maximum days between from and to is 30");
                    return false;
                }

                return true;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var oc = new OrderCriteria();
            oc.Id = !string.IsNullOrEmpty(tbOrderId.Text) ? "00002-SU-" + tbOrderId.Text : null;
            oc.TrackingNumber = tbTrackingNumber.Text;
            oc.ShippedDateFrom = !string.IsNullOrEmpty(tbShippedDateFrom.Text) ? (DateTime?)DateTime.Parse(tbShippedDateFrom.Text) : null;
            oc.ShippedDateTo = !string.IsNullOrEmpty(tbShippedDateTo.Text) ? (DateTime)DateTime.Parse(tbShippedDateTo.Text) : DateTime.Now;
            oc.Carrier = ddlCarrier.SelectedValue;

            OrderCriteriaCheck check = new OrderCriteriaCheck(oc);
            List<string> brokenRules;
            if (!check.IsValid(out brokenRules))
            {
                foreach (string brokenrule in brokenRules)
                {
                    Page.Validators.Add(new BusinessValidationError(brokenrule));
                }
                return;
            }
            

            var orders = ApplicationContextHolder.Instance.Facade.GetOrders(oc);
            repeatOrders.DataSource = orders;
            repeatOrders.DataBind();

            pnlListOrders.Visible = orders.Count>0;

        }

        [WebMethod]
        public static void SendTrackingNumberUpdate(string oldTrackingNumber, string newTrackingNumber)
        {
            ApplicationContextHolder.Instance.Facade.UpdateTrackingNumber(oldTrackingNumber, newTrackingNumber);
        }

        protected void repeatOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            switch (e.CommandName)
            {
                case "RESHIP":
                    Response.Redirect("~/Ship.aspx?id=" + e.CommandArgument + "&ACTION=RESHIP");
                    break;

            }
        }
    }
}
