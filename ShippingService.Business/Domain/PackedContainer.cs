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
        public PackedContainer()
        {
            EstimatedWeight = -1;
        }

        public Container Container { get; set; }
        public int Weight { get; set; }
        public string TrackingNumber { get; set; }
        public string UPSLabel { get; set; }
        public string ReturnUPSLabel { get; set; }
        public int EstimatedWeight { get; set; }
        public DateTime? ShippedOn { get; set; }
        public string EstimatedWeightAsString
        {
            get
            {
                if (EstimatedWeight > -1)
                {
                    return EstimatedWeight + "gr";
                }
                else
                {
                    return "n/a";
                }
            }
        }
       

        public override string ToString()
        {
            return Container.Name;
        }
    }
}
