using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{
    public class CarrierModeFilter: Entity
    {
        public string Carrier { get; set; }
        public string CarrierMode { get; set; }
        public bool IsDefault { get; set; }
    }
}
