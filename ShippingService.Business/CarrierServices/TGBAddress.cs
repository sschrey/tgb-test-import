using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.CarrierServices
{
    public static class TGBAddress
    {
        public static string CompanyName 
        {
            get { return "Tweddle Litho of Europe"; }
        }

        public static string Attention
        {
            get { return "Distribution Center"; }
        }

        public static string PhoneNumber
        {
            get { return "+3215451820"; }
        }

        public static string AddressLine1
        {
            get { return "Brownfieldlaan 15-2"; }
        }

        public static string City
        {
            get {return "WILLEBROEK"; }
        }

        public static string PostalCode
        {
            get {return "2830"; }
        }

        public static string CountryCode
        {
            get { return "BE"; }
        }

    }
}
