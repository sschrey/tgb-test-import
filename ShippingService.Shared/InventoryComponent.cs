using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Shared
{
    public class InventoryComponent
    {
        public string PartNumber { get; set; }
        public int Quantity { get; set; }
        public string ItemNumber { get; set; }

        public override string ToString()
        {
            return PartNumber + ", " + Quantity;
        }
    }
}
