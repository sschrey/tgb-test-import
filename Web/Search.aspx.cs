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
                    && !criteria.ShippedDate.HasValue)
                {
                    brokenRules.Add("Please specify at least one criteria");
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
            oc.ShippedDate = !string.IsNullOrEmpty(tbShippedDate.Text) ? (DateTime?)DateTime.Parse(tbShippedDate.Text) : null;

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
