using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMOrderLine
    {
        public VMOrderLine()
        {
            PackingData = new List<VMPackingData>();
        }
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string CaseNumber { get; set; }
        public string PartNumber { get; set; }
        public string PartWeight { get; set; }
        public double Quantity { get; set; }
        public string RequestQuantity { get; set; }
        public List<VMPackingData> PackingData { get; set; }
    }

   
}
