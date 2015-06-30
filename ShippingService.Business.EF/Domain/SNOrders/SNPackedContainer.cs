using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.SNOrders
{
    public class SNPackedContainer: Entity
    {
        public string ContainerId { get; set; }
        public double Weight { get; set; }

    }
}
