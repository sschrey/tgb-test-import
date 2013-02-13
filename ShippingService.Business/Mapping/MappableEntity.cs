using ShippingService.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Mapping
{
    public abstract class MappableEntity<E, DTO> : Entity where E : MappableEntity<E, DTO>
                                                          where DTO :  DataTransferObject, new()
    {
        public abstract DTO Map();
        //public abstract void Map(DTO source);

        public static IList<DTO> Map(IList<E> sources)
        {
            var dtEntities = new List<DTO>();
            foreach (var source in sources)
                dtEntities.Add(source.Map());

            return dtEntities;
        }
    }
}
