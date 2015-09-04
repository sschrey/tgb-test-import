using ShippingService.Business.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers
{
    public class VMOrder
    {
        public VMOrder() { }
        public VMOrder(Order order)
        {
            OrderLines = new List<VMOrderLine>();
            OrderId = order.Id;
            foreach (var line in order.Lines)
            {
                foreach(var pack in line.Packs)
                {
                    OrderLines.Add(new VMOrderLine()
                    {
                        ContainerName = pack.PackedContainer.Container.Name,
                        PartNumber = line.PartId,
                        Quantity = pack.Qty.ToString(),
                        TrackingNumber = pack.PackedContainer.TrackingNumber
                    });
                }
            }
            Count = OrderLines.Count + 1;
        }
        public int Count { get; set; }
        public string OrderId { get; set; }

        public List<VMOrderLine> OrderLines { get; set; }
    }
}