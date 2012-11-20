using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Shared
{
    [Serializable]
    public class ShippingConfirmation
    {
        public string ExternalOrderId { get; set; }
        public string InternalOrderId { get; set; }
        public string TrackNumberList { get; set; }
        public string ItemNumber { get; set; }
        public string Quantity { get; set; }
        public string ShipDate { get; set; }
        public string TransportID { get; set; }
        public string TransportType { get; set; }
    }
}
