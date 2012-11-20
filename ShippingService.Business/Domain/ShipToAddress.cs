using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public class ShipToAddress: Address
    {
        public virtual string ShipToCode { get; set; }
    }
}
