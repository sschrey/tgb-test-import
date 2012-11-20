using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{
    public class OrderLine: Entity
    {
        public virtual string LineNumber { get; set; }
        public virtual string PartId { get; set; }
        public virtual string PartName { get; set; }
        public virtual int OrderQty { get; set; }
        public virtual IList<PackedOrderLine> Packs { get; set; }
        public virtual int PartWeight { get { return 0; } }
        public int UnitPrice { get; set; }
        /// <summary>
        /// Qty to pack
        /// </summary>
        public int PackQty
        {
            get {
                if (Packs == null)
                    return OrderQty;
                else
                    return OrderQty - Packs.Sum(p => p.Qty); }
        }

       

    }
}
