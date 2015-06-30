using ShippingService.Business.EF.Domain.SNOrders;
using ShippingService.Business.EF.Facade.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class SNOrderFacade : BaseFacade
    {
        private ShippingServiceData db;
        public SNOrderFacade(ShippingServiceData db)
            : base(db)
        {
            this.db = db;
        }

        public List<SNPackedOrderLine> Pack(PackingList packinglist, E1Facade facade)
        {
            var cartons = facade.GetCartons();
            var orderlines = facade.GetOrderLines(packinglist.OrderId);

            List<SNPackedOrderLine> packedorderlines = new List<SNPackedOrderLine>();
            foreach (var packingline in packinglist.PackingLines)
            {
                var carton = cartons.FirstOrDefault(c => c.Id == packingline.CartonId);
                var orderline = orderlines.FirstOrDefault(ol => ol.Id == packingline.OrderLineId);

                SNPackedContainer container = new SNPackedContainer();
                container.ContainerId = packingline.CartonId;
                container.Weight = carton.Weight;

                Add(container);

                SNPackedOrderLine packedorderline = new SNPackedOrderLine();
                packedorderline.CaseNumber = orderline.CaseNumber;
                packedorderline.OrderId = packinglist.OrderId.ToString();
                packedorderline.PackedContainer = container;
                packedorderline.Partnumber = orderline.PartNumber;
                packedorderline.PartWeight = orderline.PartWeight;
                packedorderline.Quantity = packingline.Quantity;

                Add(packedorderline);

                packedorderlines.Add(packedorderline);
            }

            Save();

            return packedorderlines;
        }
    }
}
