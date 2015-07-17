using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.SNOrders
{
    public class SNPackedOrderLine: Entity
    {
        protected SNPackedOrderLine() { }
        public SNPackedOrderLine(string id)
        {
            this.Id = id;
            this.CreatedOn = DateTime.Now;
        }
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string LineNumber { get; set; }
        [Required]
        public string Partnumber { get; set; }
        [Required]
        public double PartWeight { get; set; }
        [Required]
        public virtual SNPackedContainer PackedContainer { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
