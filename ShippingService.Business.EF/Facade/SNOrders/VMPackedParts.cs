using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMPackedParts
    {
        public string PartNumber { get; set; }
        public string PartWeight { get; set; }
        public string Quantity { get; set; }
    }
}
