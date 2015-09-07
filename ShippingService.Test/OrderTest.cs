using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace ShippingService.Test
{
    [TestFixture]
    public class OrderTest
    {

        [Test]
        public void CanGetOneOrder()
        {

            var orders = ApplicationContextHolder.Instance.Facade.GetOrders(new Business.Domain.OrderCriteria() { CustomerPOs = new List<string>() { "ONLINESUB_2013003132" }.ToArray() });
            
            Assert.IsNotNull(orders.Count()>0);
        }


    }
}
