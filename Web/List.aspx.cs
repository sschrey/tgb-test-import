using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShippingService.Business;

namespace Web
{
    public partial class List : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var orders = ApplicationContextHolder.Instance.Facade.GetTodoOrders();
                repeatOrders.DataSource = orders;
                repeatOrders.DataBind();

                lblFeedBack.Text = string.Format("There are {0} orders waiting to be processed.", orders.Count);

                if (orders.Count == 0)
                {
                    pnlListOrders.Visible = false;
                }
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
        protected void repeatOrders_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

            switch (e.CommandName)
            {
                case "PACK":
                    Response.Redirect("~/Pack.aspx?id=" + e.CommandArgument );
                    break;

                case "SHIP":
                    Response.Redirect("~/Ship.aspx?id=" + e.CommandArgument);
                    break;
            }
        }
    }
}
