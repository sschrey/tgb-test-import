using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMPack
    {
        public VMPack()
        {
            OrderLines = new List<VMOrderLine>();
            Cartons = new List<VME1Carton>();
            Containers = new List<VMPackedContainer>();
            Errors = new List<string>();
        }

        public List<VMOrderLine> OrderLines { get; set; }
        public VME1Carton SelectedCarton { get; set; }
        public List<VME1Carton> Cartons { get; set; }
        public List<VMPackedContainer> Containers { get; set; }
        public List<string> Errors { get; set; }
        public string SuccessMessage { get; set; }
        public bool Saved { get; set; }
        public float OrderId { get; set; }
        public bool Packed { get; set; }
    }
}
