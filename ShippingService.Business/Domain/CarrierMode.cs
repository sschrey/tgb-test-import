using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{
    public class CarrierMode: Entity
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
