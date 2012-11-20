using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Shared
{
    [Serializable]
    public class OrderConfirmation
    {
        public string ExternalOrderId { get; set; }
        public string InternalOrderId { get; set; }
        public string LineNumber { get; set; }
        public string PartNumber { get; set; }
        public string Quantity { get; set; }
        public string Stock { get; set; }
        public string Mode { get; set; }
        public string ItemNumber { get; set; }
    }
}
