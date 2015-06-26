using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade
{
    
    public class FacadeFactory
    {
        private volatile static FacadeFactory factory;
        private IWindsorContainer container;

        private FacadeFactory()
        {
            container = new WindsorContainer();
            //we create a facade/db once per web request if there is a httpcontext
            //or transient if there is no httpcontext
            //transient means create when you call resolve
            container.Register(Classes.FromThisAssembly().BasedOn<BaseFacade>()
                .LifestyleScoped<HybridPerWebRequestTransientScopeAccessor>());
            container.Register(Classes.FromThisAssembly().BasedOn<DbContext>()
                .LifestyleScoped<HybridPerWebRequestTransientScopeAccessor>());
                


        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static FacadeFactory GetInstance()
        {
            if (factory == null)
                factory = new FacadeFactory();

            return factory;
        }

        public T GetFacade<T>() where T : BaseFacade
        {
            return container.Resolve<T>();
        }
    }

    public class FacadeFactorytof
    {
        private volatile static FacadeFactory factory;
        private IWindsorContainer container;

        private FacadeFactorytof()
        {
            container = new WindsorContainer();
        }
    }

}
