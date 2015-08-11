using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMPackingData
    {
        public string CartonId { get; set; }
        public string CartonName { get; set; }
        public int Quantity { get; set; }
    }
}
