using ShippingService.Business.EF.Domain.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VMOrderLine
    {
        public VMOrderLine() {
            PackingData = new List<VMPackingData>();
        }
        public VMOrderLine(E1OrderLine orderline): base()
        {
            PackingData = new List<VMPackingData>();
            CaseNumber = orderline.CaseNumber;
            Id = orderline.Id.ToString();
            LineNumber = orderline.LineNumber.ToString();
            OrderNumber = orderline.OrderNumber.ToString();
            PackingData = new List<VMPackingData>();
            PartNumber = orderline.PartNumber;
            PartWeight = orderline.PartWeight;
            Quantity = orderline.Quantity;
            RequestQuantity = orderline.Quantity.ToString();
            Status = orderline.Status;
        }
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string LineNumber { get; set; }
        public string CaseNumber { get; set; }
        public string PartNumber { get; set; }
        public double PartWeight { get; set; }
        public double Quantity { get; set; }
        public string RequestQuantity { get; set; }
        public List<VMPackingData> PackingData { get; set; }
        public bool Packed { get; set; }
        public string Status { get; set; }
    }

   
}
