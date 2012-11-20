using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using ShippingService.Business;
using Spring.Context;

namespace ShippingService.Test
{
    public class ApplicationContextHolder
    {
        private static ApplicationContextHolder instance = new ApplicationContextHolder();
        private IApplicationContext applicationContext;
        private IFacade facade;

        private ApplicationContextHolder()
        {
            applicationContext = ConfigurationSettings.GetConfig("spring/context") as IApplicationContext;
            facade = applicationContext.GetObject("Facade", typeof(IFacade)) as IFacade;
        }

        public static ApplicationContextHolder Instance
        {
            get { return instance; }
        }

        public IApplicationContext ApplicationContext
        {
            get { return applicationContext; }
        }

        public IFacade Facade
        {
            get { return facade; }
        }


    }
}
