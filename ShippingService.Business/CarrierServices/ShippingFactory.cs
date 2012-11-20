using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Business.Domain;

namespace ShippingService.Business.CarrierServices
{
    public class ShippingFactory
    {
        public delegate void ErrorMessage(string msg);

        public static IShipping GetShipping(string carrier, CarrierMode mode, Order order, IFacade facade)
        {

            if (carrier == null) return null;

            IShipping shipper = null;
            switch (carrier)
            {
                case "88284": //"UPS BELGIUM":
                    shipper = new UPSShipping<UPSBelgium> { Order = order, ShippingVendor = mode, Facade = facade };
                    break;

                case "125541": //"UPS RR DONNELY":
                    shipper = new UPSShipping<UPSRRDonnely> { Order = order, ShippingVendor = mode, Facade = facade };
                    break;

                case "87433": //"TNT BENELUX":
                    shipper = new TNTShipping { Order = order, ShippingVendor = mode, Facade = facade };
                    break;
             
                default:
                    shipper = new GeneralShipping { Order = order, ShippingVendor = mode, Facade = facade };
                    break;

            }

            return shipper;
        }
    }
}
