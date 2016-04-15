using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.CarrierServices.TNT.Label
{
    public class TNTPieceLine
    {
        public TNTPieceLine()
        {
            Pieces = new List<TNTPiece>();
        }
        public double WidthInM { get; set; }
        public double HeightInM { get; set; }
        public double WeightInKG { get; set; }
        public double LengthInM { get; set; }
        public string GoodsDescription { get; set; }

        public List<TNTPiece> Pieces { get; set; }
    }
}
