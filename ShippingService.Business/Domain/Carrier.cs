using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{
    public class Carrier: Entity
    {
        public virtual string Name { get; set; }
    }
}
