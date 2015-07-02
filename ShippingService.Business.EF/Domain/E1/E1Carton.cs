using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.E1
{
    public class E1Carton: Entity
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public string WeightUOM { get; set; }

        public double WeightInGram
        {
            get
            {
                double weight = 0;
                switch (WeightUOM)
                {
                    case "KG":
                        weight = Weight * 1000;
                        break;
                    case "GM":
                        weight = Weight;
                        break;
                }
                return weight;
            }
        }
    }
}
