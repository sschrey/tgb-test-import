using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Spring.Context;
using ShippingService.Business;
using System.Configuration;

namespace RemoteServices
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
