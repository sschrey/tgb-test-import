using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{
    public class Container: Entity
    {
        public virtual string Name { get; set; }
        /// <summary>
        /// Width in mm
        /// </summary>
        public virtual int Width { get; set; }
        /// <summary>
        /// Height in mm
        /// </summary>
        public virtual int Height { get; set; }
        /// <summary>
        /// Depth in mm
        /// </summary>
        public virtual int Depth { get; set; }
        /// <summary>
        /// Weight in grams, can be decimal
        /// </summary>
        public virtual double Weight { get; set; }

        public long VolumeInMm3
        {
            get
            {
                return (long)Width * (long)Height * (long)Depth;
            }
        }

        public double VolumeInM3
        {
            get
            {
                double volumeInM3 = (double)VolumeInMm3 / 1000000000;
                return volumeInM3;

            }
        }



    }
}
