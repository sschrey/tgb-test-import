using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{

    /// <summary>
    /// Packagecode = ID = unique!
    /// </summary>
    public class PackedContainer: Entity
    {
        public Container Container { get; set; }
        public int Weight { get; set; }
        public string TrackingNumber { get; set; }
        public string UPSLabel { get; set; }
        public string ReturnUPSLabel { get; set; }
       

        public override string ToString()
        {
            return Container.Name;
        }
    }
}
