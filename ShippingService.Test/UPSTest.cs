using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ShippingService.Business.Domain;
using ShippingService.Business.CarrierServices;

namespace ShippingService.Test
{
    [TestFixture]
    public class UPSTest
    {
        [Test]
        public void CreateUPSShipment()
        {
            var order = CreateFakeOrder();
            var carrierMode = CreateFakeCarrierMode();
            var shipping = new UPSShipping<UPSRRDonnely> { Order = order, ShippingVendor = carrierMode, Facade = ApplicationContextHolder.Instance.Facade };
            //shipping.SetReturnShipment();
            
            var res = shipping.Execute();

            Console.WriteLine(shipping.Message);

        }

        private CarrierMode CreateFakeCarrierMode()
        {
            CarrierMode mode = new CarrierMode();
            mode.Code = "07";
            mode.Name = "UPS Express";

            return mode;    
        }

        private Order CreateFakeOrder()
        {
            Order o = new Order();

            //address
            o.MainAddress.AddressLine1 = TGBAddress.AddressLine1;
            o.MainAddress.AttentionName = "RECEIVER:" + TGBAddress.Attention;
            o.MainAddress.City = TGBAddress.City;
            o.MainAddress.CompanyName = "RECEIVER:" + TGBAddress.CompanyName;
            o.MainAddress.CountryCode = TGBAddress.CountryCode;
            o.MainAddress.PhoneNumber = TGBAddress.PhoneNumber;
            o.MainAddress.PostalCode = TGBAddress.PostalCode;

            o.ShippedCarrier = "UPS BELGIUM";
            o.ShippedCarrierMode = "07"; //UPS Express
            o.Id = "123";

            o.SoldToAddress.AddressLine1 = TGBAddress.AddressLine1;
            o.SoldToAddress.AttentionName = "SOLD TO:" + TGBAddress.Attention;
            o.SoldToAddress.City = TGBAddress.City;
            o.SoldToAddress.CompanyName = "SOLD TO:" + TGBAddress.CompanyName;
            o.SoldToAddress.CountryCode = TGBAddress.CountryCode;
            o.SoldToAddress.PhoneNumber = TGBAddress.PhoneNumber;
            o.SoldToAddress.PostalCode = TGBAddress.PostalCode;
            o.SoldToAddress.SoldToCode = "123soldto";

            o.ShipToAddress.AddressLine1 = TGBAddress.AddressLine1;
            o.ShipToAddress.AttentionName = "SHIP TO:" + TGBAddress.Attention;
            o.ShipToAddress.City = TGBAddress.City;
            o.ShipToAddress.CompanyName = "SHIP TO:" + TGBAddress.CompanyName;
            o.ShipToAddress.CountryCode = TGBAddress.CountryCode;
            o.ShipToAddress.PhoneNumber = TGBAddress.PhoneNumber;
            o.ShipToAddress.PostalCode = TGBAddress.PostalCode;
            o.ShipToAddress.ShipToCode = "123shipto";


            o.Lines = new List<OrderLine>()
            {
                new OrderLine()
                {
                    LineNumber = "1",
                    OrderQty = 1,
                    Packs = new List<PackedOrderLine>()
                    { 
                        new PackedOrderLine()
                        {
                            Qty = 1,
                            PackedContainer =  new PackedContainer(){Weight = 1}
                        }
                    }
                }
            };

            

            return o;
        }
    }
}
