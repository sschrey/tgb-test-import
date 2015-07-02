using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMSearch
    {
        public VMSearch()
        {
            Containers = new List<VMPackedContainer>();
            From = DateTime.Now.ToString("yyyy-MM-dd");
            To = DateTime.Now.ToString("yyyy-MM-dd");
        }
        public List<VMPackedContainer> Containers { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
