using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShippingService.Business.Domain;

namespace ShippingService.Business.CarrierServices
{
    public interface IShipping
    {
        event ShippingFactory.ErrorMessage Error;
        CarrierMode ShippingVendor { get; set; }
        string Message { get; set; }
        Order Order { get; set; }
        bool Execute();
        void SetReturnShipment();
    }
}
