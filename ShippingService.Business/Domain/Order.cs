using ShippingService.Business.Mapping;
using ShippingService.Shared;
using ShippingService.Shared.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweddle.Commons.Business;

namespace ShippingService.Business.Domain
{
    public partial class Order: MappableEntity<Order, ERPOrder>
    {
        public virtual string ProposedCarrier { get; set; }
        public virtual string ProposedCarrierMode { get; set; }
        public virtual string ShippedCarrier { get; set; }
        public virtual string ShippedCarrierMode { get; set; }
        public virtual string ShippedCarrierModeOption { get; set; }
        
        public virtual List<OrderLine> Lines { get; set; }
        public virtual string E1Status { get; set; }

        public virtual string CustomerPONumber { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual string OrderNumber { get; set; }
        public virtual string ZoneNumberDescription { get; set; }

        public virtual Address MainAddress { get; set; }
        public virtual ShipToAddress ShipToAddress { get; set; }
        public virtual SoldToAddress SoldToAddress { get; set; }
        public virtual string ReferenceNumber
        {
            get
            {
                string referenceNumber = string.Empty;
                if (SoldToAddress != null && !string.IsNullOrEmpty(SoldToAddress.SoldToCode))
                    referenceNumber += SoldToAddress.SoldToCode;

                if (!string.IsNullOrEmpty(Id))
                {
                    if (referenceNumber.Length > 0)
                        referenceNumber += "-";
                    referenceNumber += Id;
                }

                return referenceNumber;
            }
        }

        public Order()
        {
            MainAddress = new Address();
            ShipToAddress = new ShipToAddress();
            SoldToAddress = new SoldToAddress();
        }

        public override ERPOrder Map()
        {
            var dtoOrder = new ERPOrder();

            dtoOrder.InvoiceNumber = this.InvoiceNumber;
            dtoOrder.CustomerPONumber = this.CustomerPONumber.TrimEnd(' ');
            dtoOrder.ERPOrderID = this.Id;

            return dtoOrder;
        }
    }
}
