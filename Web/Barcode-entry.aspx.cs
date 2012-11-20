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
    public partial class Barcode_entry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public class BarCodePostReturnData
        {
            public bool Success
            {
                get
                {
                    return !string.IsNullOrEmpty(RedirectURL);
                }
            }
            public string Message { get; set; }
            public string RedirectURL { get; set; }
        }

        [WebMethod]
        public static BarCodePostReturnData BarCodePosted(string pickSlipScan, string boxScan)
        {
            BarCodePostReturnData ret = TryBarCodePost(pickSlipScan, boxScan);

            BarcodeScanLog scanLog = new BarcodeScanLog()
            {
                BoxScan = boxScan,
                CreatedOn = DateTime.Now,
                PickSlipScan = pickSlipScan,
                Success = ret.Success,
                UserName = HttpContext.Current.User.Identity.Name
            };

            ApplicationContextHolder.Instance.Facade.LogBarcodeScan(scanLog);

            return ret;            
        }

        private static BarCodePostReturnData TryBarCodePost(string pickSlipScan, string boxScan)
        {
            BarCodePostReturnData ret = new BarCodePostReturnData();
            if (string.IsNullOrEmpty(pickSlipScan) || string.IsNullOrEmpty(boxScan))
            {
                ret.Message = "Supply a pick slip scan and a box scan";
                return ret;
            }

            string fullOrderId = CreateOrderIdFromPickSlipScan(pickSlipScan);

            string[] splittedOrderId = fullOrderId.Split('-');

            if (boxScan.Substring(1) != fullOrderId.Split('-')[0])
            {
                ret.Message = "Pick slip scan and box scan does not match";
                return ret;
            }

            //we need to reverse the order of the 3 ordernumber parts :(
            fullOrderId = ReverseOrderId(fullOrderId);

            var order = ApplicationContextHolder.Instance.Facade.GetTodoOrderById(fullOrderId);

            if (order == null)
            {
                ret.Message = "No TODO order found with that order number";
                return ret;
            }
            switch (order.Status)
            {
                case OrderStatus.Unpacked:
                    ret.RedirectURL = "Pack.aspx?Id=" + order.Id;
                    break;
                case OrderStatus.Packed:
                    ret.RedirectURL = "Ship.aspx?Id=" + order.Id;
                    break;
                case OrderStatus.Shipped:
                    ret.Message = "Pick slip scan and box scan does not match";
                    return ret;
            }

            
            return ret;
        }

        private static string CreateOrderIdFromPickSlipScan(string barcode)
        {
            var barcodeList = barcode.Split(new string[]{" "}, StringSplitOptions.RemoveEmptyEntries);
            return string.Join("-", barcodeList);
        }

        private static string ReverseOrderId(string orderid)
        {
            string reversedOrderId = orderid;

            string[] reversedOrderIdList = reversedOrderId.Split('-');
            if (reversedOrderIdList.Length == 3)
            {
                reversedOrderId = reversedOrderIdList[2] + '-' + reversedOrderIdList[1] + '-' + reversedOrderIdList[0];
            }

            return reversedOrderId;
        }
    }
}