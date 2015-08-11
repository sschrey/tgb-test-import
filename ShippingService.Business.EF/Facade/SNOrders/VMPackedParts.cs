using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMPackedParts
    {
        public string OrderLineId { get; set; }
        public string LineNumber { get; set; }
        public string PartNumber { get; set; }
        public double PartWeight { get; set; }
        public int Quantity { get; set; }
    }
}
