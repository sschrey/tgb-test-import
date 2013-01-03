using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.CarrierServices
{
    public interface IUPS
    {
        string CustomerShipperNumber { get; }
        Payment PaymentOption { get; }
        string PaymentAccountNumber { get; }
    }

    public enum Payment
    { 
        Prepaid,
        Collect,
        BillThirdParty
    }
}
