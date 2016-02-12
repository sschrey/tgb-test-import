using ShippingService.Business.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers
{
    public class VMOrder
    {
        public VMOrder() { Include = true; }
        public VMOrder(Order order): base()
        {
            Include = true;
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
                    ShippedOn = pack.PackedContainer.ShippedOn.ToString("yyyy-MM-dd");
                }
            }
            Count = OrderLines.Count + 1;
        }
        public int Count { get; set; }
        public string OrderId { get; set; }

        public List<VMOrderLine> OrderLines { get; set; }
        public string ShippedOn { get; set; }
        public bool Include { get; set; }
    }
}