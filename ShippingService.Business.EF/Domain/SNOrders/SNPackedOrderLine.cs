using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.SNOrders
{
    public class SNPackedOrderLine: Entity
    {
        public string OrderId { get; set; }
        public string CaseNumber { get; set; }
        public string Partnumber { get; set; }
        public double PartWeight { get; set; }

        public SNPackedContainer PackedContainer { get; set; }
        public int Quantity { get; set; }
    }
}
