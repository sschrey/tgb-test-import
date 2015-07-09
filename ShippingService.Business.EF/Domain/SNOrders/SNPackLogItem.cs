using ShippingService.Business.EF.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.SNOrders
{
    public class SNPackLogItem: EntityWithTypedId<int>
    {
        public SNPackLogItem()
        {
            CreatedOn = DateTime.Now;
        }
        [Required]
        public string User { get; set; }
        public DateTime CreatedOn { get; set; }
        [Required]
        public string OrderId { get; set; }
        [Required]
        public string CaseNumber { get; set; }
        public double EstimatedWeight { get; set; }
        public double EnteredWeight { get; set; }
        public bool WasCorrect { get; set; }
    }
}
