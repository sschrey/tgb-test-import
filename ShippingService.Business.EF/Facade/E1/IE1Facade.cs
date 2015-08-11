using ShippingService.Business.EF.Domain.E1;
using System;
using System.Collections.Generic;
namespace ShippingService.Business.EF.Facade.E1
{
    public interface IE1Facade
    {
        List<E1Carton> GetCartons();
        List<E1OrderLine> GetOrderLines(float orderid);
    }
}
