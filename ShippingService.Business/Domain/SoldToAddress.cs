using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public class SoldToAddress: Address
    {
        public string SoldToCode { get; set; }
    }
}
