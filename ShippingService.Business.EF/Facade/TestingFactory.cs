using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using ShippingService.Business.EF.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade
{
    public class DbContextMock<T>: Mock<T> where T: DbContext
    {
        public DbContextMock()
            : base()
        {
        }
    }

    public class BaseFacadeMock<T, Y> : Mock<T>
        where T : BaseFacade
        where Y : DbContext
    {
        public BaseFacadeMock(Mock<Y> context)
            : base(context.Object)
        {
        }
    }

    public class TestFactorySetup
    {

        public TestFactorySetup(WindsorContainer container)
        {

            var dbcontexts = from t in Assembly.GetExecutingAssembly().GetTypes()
                             where t.IsSubclassOf(typeof(DbContext))
                             select t;

            foreach (var dbcontext in dbcontexts)
            {
                MethodInfo method = this.GetType().GetMethod("SetupDBContext");
                method = method.MakeGenericMethod(new Type[] { dbcontext });
                method.Invoke(this, new object[] { container });
            }

            var basefacades = from t in Assembly.GetExecutingAssembly().GetTypes()
                              where t.IsSubclassOf(typeof(BaseFacade))
                              select t;

            foreach (var basefacade in basefacades)
            {
                MethodInfo method = this.GetType().GetMethod("SetupFacade");

                //get constructor with a parameter that has a subclass of dbcontext
                var constr =
                basefacade.GetConstructors().FirstOrDefault(c =>
                    c.GetParameters().Any(p => p.ParameterType.IsSubclassOf(typeof(DbContext))));

                if (constr != null)
                {
                    //get the parameter that has a subclass of dbcontext. I need this to setup the mockobject
                    var param = constr.GetParameters().First(p => p.ParameterType.IsSubclassOf(typeof(DbContext)));

                    method = method.MakeGenericMethod(new Type[] { basefacade, param.ParameterType });
                    method.Invoke(this, new object[] { container });

                }
            }
        }

        public void SetupDBContext<T>(WindsorContainer container) where T : DbContext
        {
            container.Register(Component.For<Mock<T>>()
                .ImplementedBy<DbContextMock<T>>());
        }

        public void SetupFacade<T, Y>(WindsorContainer container)
            where T : BaseFacade
            where Y : DbContext
        {
            container.Register(Component.For<Mock<T>>()
                .ImplementedBy<BaseFacadeMock<T, Y>>());

        }
    }

    public class TestingFactory
    {
        WindsorContainer container;
        private TestFactorySetup setup;

        public TestingFactory()
        {
            container = new WindsorContainer();
            setup = new TestFactorySetup(container);
        }

        public Mock<T> Get<T>() where T:class
        {
            return container.Resolve<Mock<T>>(); 
        }
        public T GetObject<T>() where T : class
        {
            return container.Resolve<Mock<T>>().Object;
        }
    }
}
