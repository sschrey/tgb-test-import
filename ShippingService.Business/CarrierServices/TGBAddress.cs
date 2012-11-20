﻿using System;
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
            get { return "E. Walschaertsstraat 15 / 3"; }
        }

        public static string City
        {
            get {return "MECHELEN"; }
        }

        public static string PostalCode
        {
            get {return "2800"; }
        }

        public static string CountryCode
        {
            get { return "BE"; }
        }

    }
}
