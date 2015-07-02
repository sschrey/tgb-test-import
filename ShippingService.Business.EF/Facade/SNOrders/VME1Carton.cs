using ShippingService.Business.EF.Domain.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.SNOrders
{
    public class VME1Carton
    {
        public VME1Carton(){}
        public VME1Carton(E1Carton carton)
        {
            Id = carton.Id;
            Name = carton.Name;
            Weight = carton.WeightInGram;

        }
        public VME1Carton(string id, string name, double weight)
        {
            this.Id = id;
            this.Name = name;
            this.Weight = weight;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
    }
}
