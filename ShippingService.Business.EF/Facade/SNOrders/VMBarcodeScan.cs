using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMBarcodeScan
    {
        public VMBarcodeScan()
        {
            Errors = new List<string>();
        }
        public string OrderId { get; set; }
        public List<string> Errors { get; set; }
    }
}
