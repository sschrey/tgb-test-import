using ShippingService.Business.EF.Domain.SNOrders;
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

        public VMPackedContainer(SNPackedContainer dbcontainer): this()
        {
            this.Id = dbcontainer.Id;
            this.OrderNumber = dbcontainer.OrderId;
            this.Carton = new VME1Carton(dbcontainer.ContainerId, dbcontainer.ContainerName, Double.Parse(dbcontainer.Weight));
            this.CaseNumber = dbcontainer.CaseNumber;
            this.PartsWeight = dbcontainer.PartsWeight;
            this.TotalWeight = dbcontainer.TotalWeight;
            this.Weight = dbcontainer.Weight;
            foreach(var dbpart in dbcontainer.OrderLines)
            {
                this.PackedParts.Add(new VMPackedParts()
                {
                    OrderLineId = dbpart.Id,
                    PartNumber = dbpart.Partnumber,
                    PartWeight = dbpart.PartWeight,
                    Quantity = dbpart.Quantity
                });
            }
            this.Count = PackedParts.Count + 1;
            this.CreatedOn = dbcontainer.CreatedOn.ToString("yyyy-MM-dd HH:mm");

        }
        public string Id { get; set; }
        public string OrderNumber { get; set; }
        public string CaseNumber { get; set; }
        public VME1Carton Carton { get; set; }
        public double PartsWeight { get; set; }
        public double TotalWeight { get; set; }
        public string Weight { get; set; }
        public List<VMPackedParts> PackedParts { get; set; }
        public int Count { get; set; }
        public string CreatedOn { get; set; }
    }
}
