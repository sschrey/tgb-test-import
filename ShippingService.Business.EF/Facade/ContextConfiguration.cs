using ShippingService.Business.EF.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade
{
    public class ContextConfiguration
    {
        private IEnumerable<object> configurations;
        private IEnumerable<Type> dbsets;
        public ContextConfiguration()
        {
            //get all types that inheret from EntityTypeConfiguration or ComplexTypeConfiguration
            var maps = from t in Assembly.GetExecutingAssembly().GetTypes()
                       where t.BaseType != null && t.BaseType.IsGenericType
                       let baseDef = t.BaseType.GetGenericTypeDefinition()
                       where baseDef == typeof(EntityTypeConfiguration<>) ||
                             baseDef == typeof(ComplexTypeConfiguration<>)
                       select Activator.CreateInstance(t);

            configurations = maps;

            //get all types that inheret from EntityWithTypedId without entity itself
            dbsets = from t in Assembly.GetExecutingAssembly().GetTypes()
                     where t.BaseType != null && t.BaseType.IsGenericType && !t.IsAbstract
                     && t != typeof(Entity)
                     let baseDef = t.BaseType.GetGenericTypeDefinition()
                     where baseDef == typeof(EntityWithTypedId<>)
                     select t;

            //+get all types that inheret from Entity
            dbsets = dbsets.Union(
                from t in Assembly.GetExecutingAssembly().GetTypes()
                where t.IsSubclassOf(typeof(Entity)) && !t.IsAbstract
                select t
                );

        }

        public IEnumerable<object> Configurations
        {
            get
            {
                return configurations;
            }

        }

        public IEnumerable<Type> DbSets
        {
            get
            {
                return dbsets;
            }
        }
    }
}
