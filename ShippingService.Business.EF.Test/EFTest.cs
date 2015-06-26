using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using ShippingService.Business.EF.Domain;
using ShippingService.Business.EF.Facade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Test
{
    [TestClass]
    public class EFTest
    {
        [TestMethod]
        public void MoqEntityFramework()
        {
            /*TestingFactory fac = new TestingFactory();
            var service = fac.GetFacade<BlogService>();
            
            service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

            
            fac.GetDBSet<Blog>().Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
            fac.MockContext.Verify(m => m.SaveChanges(), Times.Once());*/
            
        }

        [TestMethod]
        public void MoqService()
        {
            TestingFactory fac = new TestingFactory();
            
            fac.GetObject<BlogService>().AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

            fac.Get<BlogService>().Verify(m => m.Add<Blog>(It.IsAny<Blog>()));
            fac.Get<BlogService>().Verify(m => m.Save(), Times.Once);
        }

    }
}
