using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Business.Domain;
using System.Configuration;

namespace ShippingService.Business.CarrierServices
{
    public class GeneralShipping : IShipping
    {
        public CarrierMode ShippingVendor { get; set; }
        public IFacade Facade { get; set; }
        public string Message { get; set; }
        public event ShippingFactory.ErrorMessage Error;

        public Order Order { get; set; }
        public bool CanPrint
        {
            get { return false; }
        }

        public bool Execute()
        {
            foreach (var po in Order.PackedContainers)
            {
                po.TrackingNumber = Order.Id;
            }

            string UpdateE1AsString = ConfigurationManager.AppSettings["UPDATE_E1"];
            bool updateE1 = false;
            if (!string.IsNullOrEmpty(UpdateE1AsString))
            {
                updateE1 = Convert.ToBoolean(UpdateE1AsString);
            }


            if (Order.Status == OrderStatus.Shipped)
            {
                Facade.ReShip(Order);
            }
            else
            {
                Facade.Ship(Order, updateE1);
            }


            return true;
        }

        public void SetReturnShipment()
        {
            throw new NotImplementedException();
        }

        public List<PrintFile> GetPrintFiles()
        {
            throw new NotImplementedException();
        }

        public bool Print(string printername)
        {
            throw new NotImplementedException();
        }
    }
}
