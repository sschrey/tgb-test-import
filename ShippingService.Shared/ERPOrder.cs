using ShippingService.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Shared
{
    public class ERPOrder : DataTransferObject
    {
        public string InvoiceNumber { get; set; }
        public string CustomerPONumber { get; set; }
    }
}
