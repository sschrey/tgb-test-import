using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public class OrderCriteria
    {
        public string Id { get; set; }
        public string TrackingNumber { get; set; }
        public DateTime? ShippedDate { get; set; }
        public string E1Status { get; set; }
        public string[] Ids { get; set; }
    }
}
