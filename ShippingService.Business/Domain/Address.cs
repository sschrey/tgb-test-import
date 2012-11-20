using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public class Address
    {
        public Address()
        {
            this.AddressLine1 = String.Empty;
            this.AddressLine2 = String.Empty;
            this.AddressLine3 = String.Empty;
        }
        private string companyName;
        public virtual string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(companyName))
                {
                    return "/";
                }
                return companyName;
            }
            set
            {
                companyName = value;
            }
        }
        public virtual string AttentionName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }
        public virtual string AddressLine3 { get; set; }
        public virtual string City { get; set; }
        public virtual string PostalCode { get; set; }
        public virtual string CountryCode { get; set; }
        public virtual string State { get; set; }
    }
}
