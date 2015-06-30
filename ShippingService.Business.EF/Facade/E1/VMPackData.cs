using ShippingService.Business.EF.Domain.E1;
using ShippingService.Business.EF.Facade.SNOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ShippingService.Business.EF.Facade.E1
{
    public class VMPackData
    {
        public List<E1Carton> Cartons { get; set; }
        public List<VMOrderLine> OrderLines { get; set; }
        public string OrderId { get; set; }
    }
}
