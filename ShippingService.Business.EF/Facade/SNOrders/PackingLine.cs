using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class PackingLine
    {
        public double OrderLineId { get; set; }
        public string CartonId { get; set; }
        public int Quantity { get; set; }
    }

}
