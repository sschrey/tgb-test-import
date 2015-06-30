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
    }
}
