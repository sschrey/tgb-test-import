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
        public DateTime? ShippedDateFrom { get; set; }
        public DateTime ShippedDateTo { get; set; }
        public string E1Status { get; set; }
        public string[] CustomerPOs { get; set; }
        public string[] Ids { get; set; }
        public string Carrier { get; set; }
    }
}
