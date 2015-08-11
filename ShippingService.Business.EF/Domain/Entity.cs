using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain
{
    public class Entity : EntityWithTypedId<string>
    {

    }

    public class EntityWithTypedId<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
