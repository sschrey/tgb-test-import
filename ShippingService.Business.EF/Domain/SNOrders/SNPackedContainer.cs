using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.SNOrders
{
    public class SNPackedContainer: Entity
    {
        public SNPackedContainer()
        {
            this.Id = Guid.NewGuid().ToString().Replace("-", "").ToLower();
            this.CreatedOn = DateTime.Now;
        }
        [Required]
        public string ContainerId { get; set; }
        [Required]
        public string ContainerName { get; set; }
        [Required]
        public double ContainerWeight { get; set; }
        [Required]
        public string Weight { get; set; }
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string CaseNumber { get; set; }
        [Required]
        public double PartsWeight { get; set; }
        [Required]
        public double TotalWeight { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual List<SNPackedOrderLine> OrderLines { get; set; }
    }
}
