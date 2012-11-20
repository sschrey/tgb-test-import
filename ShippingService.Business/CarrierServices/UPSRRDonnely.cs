using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ShippingService.Business.CarrierServices
{
    public class UPSRRDonnely : IUPS
    {
        public string CustomerShipperNumber
        {
            get 
            { 
                var num = ConfigurationManager.AppSettings["UPS_RRDONNELY_SHIPPER_NUMBER"].ToString(); 
                if (num == null)
                    throw new Exception("Oeps, the UPS_RRDONNELY_SHIPPER_NUMBER is specified in web.config file!");

                return num.ToString();
            }
        }
    }
}
