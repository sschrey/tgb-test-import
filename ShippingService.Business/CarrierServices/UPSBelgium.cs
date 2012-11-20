using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ShippingService.Business.CarrierServices
{
    public class UPSBelgium : IUPS
    {
        public string CustomerShipperNumber
        {
            get 
            { 
                var num = ConfigurationManager.AppSettings["UPS_BELGIUM_SHIPPER_NUMBER"];
                if (num == null)
                    throw new Exception("Oeps, the UPS_BELGIUM_SHIPPER_NUMBER is specified in web.config file!");
                
                return num.ToString(); 
            }
        }
    }
}
