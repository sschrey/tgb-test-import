using ShippingService.Business.EF.Domain.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.E1
{
    public class E1TestFacade: IE1Facade
    {

        public List<E1Carton> GetCartons()
        {
            return E1TestData.GetE1Cartons();
        }

        public List<E1OrderLine> GetOrderLines(float orderid)
        {
            return E1TestData.GetE1OrderLines();
        }
    }
}
