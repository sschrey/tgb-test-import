using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade
{
    public class E1Data : DbContext
    {

        ContextConfiguration configuration;

        public E1Data()
            : base("name=E1Data")
        {
            this.configuration = new ContextConfiguration();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            foreach (var map in configuration.Configurations)
            {
                modelBuilder.Configurations.Add((dynamic)map);
            }

            foreach (var dbset in configuration.DbSets)
            {
                MethodInfo method = modelBuilder.GetType().GetMethod("Entity");
                method = method.MakeGenericMethod(new Type[] { dbset });
                method.Invoke(modelBuilder, null);

            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
