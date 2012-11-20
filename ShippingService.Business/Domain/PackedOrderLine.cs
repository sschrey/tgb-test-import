using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public class PackedOrderLine
    {
        public PackedContainer PackedContainer { get; set; }
        public int Qty { get; set; }
    }
}
