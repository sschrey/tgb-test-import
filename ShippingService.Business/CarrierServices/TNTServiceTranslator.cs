using ShippingService.Business.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.CarrierServices
{
    public class TNTServiceTranslator
    {
        public TNTServiceTranslator(string countrycode, CarrierMode carriermode)
        {
            ServiceCode = carriermode.Code;
            ServiceDescription = carriermode.Name;

            if(countrycode.ToUpper() == "BE")
            {
                if (carriermode.Code == "30")
                    ServiceCode = "15N";
            }
            if (ServiceCode == "728")
                ServiceDescription = "Road Freight System";
            if (carriermode.Name.Contains('{') && carriermode.Name.Contains('}'))
                OptionCode = carriermode.Name.Substring(carriermode.Name.IndexOf('{') + 1, carriermode.Name.IndexOf('}') - carriermode.Name.IndexOf('{') - 1);

            if (OptionCode == "SYS")
                OptionDescription = "System Network Delivery";

            LineOfBusiness = countrycode.ToUpper() == "BE" ? 1 : 2;
            ProductId = carriermode.Code == "48N" ? "EC" : "EX";
            ProductType = "N";
        }

        public string ServiceCode { get; set; }
        public string ServiceDescription { get; set; }
        
        public string OptionCode { get; set; }
        
        public string OptionDescription { get; set; }

        public int LineOfBusiness { get; set; }
        public string ProductId { get; set; }
        public string ProductType { get; set; }
        
    }
}
