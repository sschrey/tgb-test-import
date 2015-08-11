using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class PackingList
    {
        public PackingList()
        {
            PackingLines = new List<PackingLine>();
        }

        public List<PackingLine> PackingLines { get; set; }
        public float OrderId { get; set; }
    }
}
