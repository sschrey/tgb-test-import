using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMPackedContainer
    {
        public VMPackedContainer()
        {
            PackedParts = new List<VMPackedParts>();
            Count = 1;
        }
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string CaseNumber { get; set; }
        public string Carton { get; set; }
        public string CartonWeight { get; set; }
        public string PartsWeight { get; set; }
        public string TotalWeight { get; set; }
        public List<VMPackedParts> PackedParts { get; set; }
        public int Count { get; set; }
    }
}
