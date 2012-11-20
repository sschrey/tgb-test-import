using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShippingService.Business.Domain;
using System.Xml;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.IO;
using Tweddle.Commons.Extensions;
using System.Globalization;

namespace ShippingService.Business.CarrierServices
{
    public class UPSShipping<T> : IShipping where T : IUPS, new()
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UPSShipping<T>));
        public event ShippingFactory.ErrorMessage Error;
        public CarrierMode ShippingVendor { get; set; }
        public IFacade Facade { get; set; }
        public string Message { get; set; }

        public Order Order { get; set; }

        internal XmlDocument ShipmentConfirmRequest;
        internal XmlDocument ShipmentConfirmResponse;
        internal XmlDocument AcceptResponse;

        private bool isReturnShipment { get; set; }
        

        public UPSShipping()
        {
            Message = string.Empty;
            isReturnShipment = false;
        }

        public List<PackedContainer> Packs
        {
            get
            {
                return Order.PackedContainers;
            }
        }

        internal void Init()
        {
            ShipmentConfirmRequest = LoadShipmentConfirmRequest(ShippingVendor);
        }

        public static string LabelStoragPath
        {
            get
            {
                //Get the path to where the GIF labels will be saved and verify that it is valid
                string gifStoragePath = ConfigurationManager.AppSettings["UPSLabelDirectory"];
                if (string.IsNullOrEmpty(gifStoragePath))
                    gifStoragePath = @"\\192.168.0.25\Data\UPSLabels";

                if (!Directory.Exists(gifStoragePath))
                    Directory.CreateDirectory(gifStoragePath);

                return gifStoragePath;
            }
        }

        public bool Execute()
        {
            Init();
            FillShipmentConfirmRequest();
            ShipmentConfirmResponse = PostToUPS(ShipmentConfirmRequest, MessageType.ShipConfirm);

            if (HasError()) return false;

            var accept = PostShipAccept();
            AcceptResponse = PostToUPS(accept, MessageType.ShipAccept);


            WriteGifs(LabelStoragPath);

            string UpdateE1AsString = ConfigurationManager.AppSettings["UPDATE_E1"];
            bool updateE1 = false;
            if (!string.IsNullOrEmpty(UpdateE1AsString))
            {
                updateE1 = Convert.ToBoolean(UpdateE1AsString);
            }

            if (Order.Status == OrderStatus.Shipped)
            {
                if (isReturnShipment)
                    Facade.SaveReturnLabel(Order);
                else
                    Facade.ReShip(Order);
            }
            else
            {
                Facade.Ship(Order, updateE1);
            }
            
            return true;
        }

        public void SetReturnShipment()
        {
            isReturnShipment = true;
        }

        internal void WriteGifs(string gifStoragePath)
        {

            var packages = AcceptResponse.SelectNodes("/ShipmentAcceptResponse/ShipmentResults/PackageResults");
            if (packages == null) return;

            var totalchargeXPath = "/ShipmentAcceptResponse/ShipmentResults/ShipmentCharges/TotalCharges/";
            var totalCharges = Convert.ToDecimal(AcceptResponse.SelectSingleNode(totalchargeXPath + "MonetaryValue").InnerText);
            var currency = AcceptResponse.SelectSingleNode(totalchargeXPath + "CurrencyCode").InnerText;
            decimal shipWeight = Decimal.Parse(AcceptResponse.SelectSingleNode("/ShipmentAcceptResponse/ShipmentResults/BillingWeight/Weight").InnerText);
            int counter = 0;
            if (Packs.Count != packages.Count)
                throw new ApplicationException("Number of packages in response doesn't match serials");
            foreach (XmlNode package in packages)
            {
                //there should be one serial per package
                counter++;
                var tracker = package.SelectSingleNode("TrackingNumber").InnerText;
                var gif = package.SelectSingleNode("LabelImage/GraphicImage").InnerText;
                var fileName = Path.Combine(gifStoragePath, string.Format((isReturnShipment?"Return":"") + "Order{0}_{1}.gif", Order.ReferenceNumber, tracker));

                WriteGif(fileName, tracker, gif);

                if (!isReturnShipment)
                    Order.PackedContainers[counter - 1].TrackingNumber = tracker;

                if(isReturnShipment)
                    Order.PackedContainers[counter - 1].ReturnUPSLabel = string.Format("ReturnOrder{0}_{1}.gif", Order.ReferenceNumber, tracker);
                else
                    Order.PackedContainers[counter - 1].UPSLabel = string.Format("Order{0}_{1}.gif", Order.ReferenceNumber, tracker);
            }
        }

        internal static void WriteGif(string fileName, string tracker, string gif)
        {
            Debug.WriteLine("Writing gif " + fileName);
            var gifBytes = Convert.FromBase64CharArray(gif.ToCharArray(), 0, gif.Length);
            File.WriteAllBytes(fileName, gifBytes);
        }

        

        internal static string AccessRequest()
        {
            //            return @"<?xml version=""1.0"" encoding=""utf-8""?><AccessRequest>
            //<AccessLicenseNumber>4B79233EC9244430</AccessLicenseNumber>
            //<UserId>gchapman310</UserId>
            //Password>tweddle</Password>
            //</AccessRequest>";
            return @"<?xml version=""1.0"" encoding=""utf-8""?><AccessRequest><AccessLicenseNumber>4B79233EC9244430</AccessLicenseNumber><UserId>gchapman310</UserId><Password>tweddle</Password></AccessRequest>";
        }

        internal XmlDocument PostShipAccept()
        {
            var accept = new XmlDocument { PreserveWhitespace = false };
            accept.LoadXml(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<ShipmentAcceptRequest>
	<Request>
		<TransactionReference>
			<CustomerContext>TR01</CustomerContext>
			<XpciVersion>1.0001</XpciVersion>
		</TransactionReference>
		<RequestAction>ShipAccept</RequestAction>
		<RequestOption>01</RequestOption>
	</Request>
	<ShipmentDigest/>
</ShipmentAcceptRequest>");
            accept.SelectSingleNode("ShipmentAcceptRequest/ShipmentDigest").InnerText =
                ShipmentConfirmResponse.SelectSingleNode("ShipmentConfirmResponse/ShipmentDigest").InnerText;
            accept.Normalize();
            return accept;
        }

        internal bool HasError()
        {
            var status =
                ShipmentConfirmResponse.SelectSingleNode("ShipmentConfirmResponse/Response/ResponseStatusCode").
                    InnerText;
            if (status == "1") return false;
            var errCode = ShipmentConfirmResponse.SelectSingleNode("ShipmentConfirmResponse/Response/Error/ErrorCode");
            if (errCode != null && errCode.InnerText == "")
                //"The XML document is well formed but the document is not valid" show "Required information is incorrect or missing. No labels can be produced."
                Message = "Required information is incorrect or missing. No labels can be produced.";
            else
                Message = ShipmentConfirmResponse.SelectSingleNode("ShipmentConfirmResponse/Response/Error/ErrorDescription").
                    InnerText;
            return true;
        }

        internal enum MessageType
        {
            ShipConfirm,
            ShipAccept
        }

        internal static XmlDocument PostToUPS(XmlDocument doc, MessageType messageType)
        {
            string upsShippingUrl;
            switch (messageType)
            {
                case MessageType.ShipAccept:
                    upsShippingUrl = ConfigurationManager.AppSettings["UPSUrlShipAccept"]; //"https://wwwcie.ups.com/ups.app/xml/ShipAccept";
                    break;
                default: //MessageType.ShipConfirm:
                    upsShippingUrl = ConfigurationManager.AppSettings["UPSUrlShipConfirm"]; // "https://wwwcie.ups.com/ups.app/xml/ShipConfirm";
                    break;
            }
            string post = AccessRequest() + doc.OuterXml;
            Log.Debug("UPS Posting to " + upsShippingUrl + " :" + post);
            //post = post.Replace(Environment.NewLine, "");
            //if cannot cope with UTF8
            //post = System.Text.RegularExpressions.Regex.Replace(post, "[^\x20-\x7E]", "");
            Debug.WriteLine("Posting to " + upsShippingUrl);
            Debug.WriteLine("====");
            Debug.WriteLine(post);
            Debug.WriteLine("====");

            byte[] postBytes = Encoding.UTF8.GetBytes(post);

            var request = (HttpWebRequest)WebRequest.Create(upsShippingUrl);

            request.KeepAlive = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(postBytes, 0, postBytes.Length);
            }

            var response = new XmlDocument();
            response.Load(request.GetResponse().GetResponseStream());
            Log.Debug("UPS Post Response " + response.OuterXml);
            return response;
        }

        private static XmlDocument LoadShipmentConfirmRequest(CarrierMode vendorInfo)
        {
            var shippingData = new T();
            var doc = new XmlDocument { PreserveWhitespace = false };
            doc.LoadXml(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<ShipmentConfirmRequest xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <Request>
        <RequestAction>ShipConfirm</RequestAction>
        <RequestOption>NonValidate</RequestOption>
        <TransactionReference>
            <CustomerContext>Ship Confirm / validate</CustomerContext>
            <ToolVersion>1.0001</ToolVersion>
        </TransactionReference>
        </Request>
        <Shipment>
        <PaymentInformation>
            <Prepaid>
                <BillShipper>
                    <AccountNumber>" + shippingData.CustomerShipperNumber + @"</AccountNumber>
                </BillShipper>
            </Prepaid>
        </PaymentInformation>
        <Description>Reference Materials</Description>
        <Shipper>
            <Name>Tweddle Litho of Europe</Name>
            <AttentionName>Distribution Center</AttentionName>
            <ShipperNumber>" + shippingData.CustomerShipperNumber + @"</ShipperNumber>
            <PhoneNumber>+3215451820</PhoneNumber>
            <Address>
                <AddressLine1>E. Walschaertsstraat 15 / 3</AddressLine1>
                <City>Mechelen</City>
                <PostalCode>2800</PostalCode>
                <CountryCode>BE</CountryCode>
            </Address>
        </Shipper>
        <ShipTo>
            <CompanyName></CompanyName>
            <AttentionName>No contactperson</AttentionName>
            <PhoneNumber>0000000000</PhoneNumber>
            <Address>
                <AddressLine1></AddressLine1>
                <AddressLine2></AddressLine2>
                <AddressLine3></AddressLine3>
                <City></City>
                <PostalCode></PostalCode>
                <CountryCode></CountryCode>
                <StateProvinceCode></StateProvinceCode>
            </Address>
        </ShipTo>
        <SoldTo>
            <CompanyName></CompanyName>
            <AttentionName>No contactperson</AttentionName>
            <PhoneNumber>0000000000</PhoneNumber>
            <Address>
                <AddressLine1></AddressLine1>
                <AddressLine2></AddressLine2>
                <AddressLine3></AddressLine3>
                <City></City>
                <PostalCode></PostalCode>
                <CountryCode></CountryCode>
                <StateProvinceCode></StateProvinceCode>
            </Address>
        </SoldTo>
        <ShipFrom>
            <CompanyName></CompanyName>
            <AttentionName></AttentionName>
            <PhoneNumber></PhoneNumber>
            <Address>
                <AddressLine1></AddressLine1>
                <AddressLine2></AddressLine2>
                <AddressLine3></AddressLine3>
                <City></City>
                <PostalCode></PostalCode>
                <CountryCode></CountryCode>
                <StateProvinceCode></StateProvinceCode>
            </Address>
        </ShipFrom>
        <ReferenceNumber>
            <Code/>
            <Value>33656-76567</Value>
        </ReferenceNumber>
        <Service>
            <Code>" + vendorInfo.Code + @"</Code>
            <Description>" + vendorInfo.Name + @"</Description>
        </Service>
    </Shipment>
    <LabelSpecification>
        <LabelPrintMethod>
            <Code>GIF</Code>
            <Description>GIF</Description>
        </LabelPrintMethod>
        <HTTPUserAgent>Mozilla/4.5</HTTPUserAgent>
        <LabelImageFormat>
            <Code>GIF</Code>
            <Description>GIF</Description>
        </LabelImageFormat>
    </LabelSpecification>
</ShipmentConfirmRequest>");
            return doc;
        }


        public void FillShipmentConfirmRequest()
        {
            //sp= xmlsp_UPSBuildParams
            //complicated by conditions for @USA = 01 (for TLOE it's not) and validation
            var Shipment = ShipmentConfirmRequest.SelectSingleNode("ShipmentConfirmRequest/Shipment");

            var referenceNumber = Shipment.SelectSingleNode("ReferenceNumber");
            var referenceNumberValue = referenceNumber.SelectSingleNode("Value");
            referenceNumberValue.InnerText = Order.ReferenceNumber;

            if (!string.IsNullOrEmpty(Order.ZoneNumberDescription))
            {
                var referenceNumber2 = referenceNumber.Clone();
                var referenceNumber2Value = referenceNumber2.SelectSingleNode("Value");
                referenceNumber2Value.InnerText = Order.ZoneNumberDescription;
                Shipment.InsertAfter(referenceNumber2, referenceNumber);
            }

            
            //shipFrom is fairly static (we don't change Shipper at all)
            XmlNode ShipFrom = null;
            if(isReturnShipment)
                ShipFrom = Shipment.SelectSingleNode("ShipTo");
            else
                ShipFrom = Shipment.SelectSingleNode("ShipFrom");

            //max 35 characters
            ShipFrom.SelectSingleNode("CompanyName").InnerText = TGBAddress.CompanyName.MaxLength(35);
            ShipFrom.SelectSingleNode("AttentionName").InnerText = TGBAddress.Attention.MaxLength(35);
            ShipFrom.SelectSingleNode("PhoneNumber").InnerText = TGBAddress.PhoneNumber;
            ShipFrom.SelectSingleNode("Address/AddressLine1").InnerText = TGBAddress.AddressLine1.MaxLength(35);
            ShipFrom.SelectSingleNode("Address/City").InnerText = TGBAddress.City;
            ShipFrom.SelectSingleNode("Address/PostalCode").InnerText = TGBAddress.PostalCode;
            ShipFrom.SelectSingleNode("Address/CountryCode").InnerText = TGBAddress.CountryCode;

            XmlNode ShipTo = null;
            if (isReturnShipment)
                ShipTo = Shipment.SelectSingleNode("ShipFrom");
            else
                ShipTo = Shipment.SelectSingleNode("ShipTo");

            ShipTo.SelectSingleNode("CompanyName").InnerText = Order.MainAddress.CompanyName;
            if (!string.IsNullOrEmpty(Order.MainAddress.AttentionName))//default:No contactperson
                ShipTo.SelectSingleNode("AttentionName").InnerText = Order.MainAddress.AttentionName.Trim().MaxLength(35);
            if (!string.IsNullOrEmpty(Order.MainAddress.PhoneNumber))//default:0000000000
                ShipTo.SelectSingleNode("PhoneNumber").InnerText = System.Text.RegularExpressions.Regex.Replace(Order.MainAddress.PhoneNumber, @"\D", "").MaxLength(15);
            ShipTo.SelectSingleNode("Address/AddressLine1").InnerText = Order.MainAddress.AddressLine1.MaxLength(35);
            ShipTo.SelectSingleNode("Address/AddressLine2").InnerText = Order.MainAddress.AddressLine2.MaxLength(35);
            ShipTo.SelectSingleNode("Address/AddressLine3").InnerText = Order.MainAddress.AddressLine3.MaxLength(35);
            ShipTo.SelectSingleNode("Address/City").InnerText = Order.MainAddress.City.MaxLength(30);
            ShipTo.SelectSingleNode("Address/PostalCode").InnerText = Order.MainAddress.PostalCode.MaxLength(9);
            ShipTo.SelectSingleNode("Address/CountryCode").InnerText = Order.MainAddress.CountryCode;
            ShipTo.SelectSingleNode("Address/StateProvinceCode").InnerText = Order.MainAddress.State;
            

            Shipment.SelectSingleNode("Service/Code").InnerText = ShippingVendor.Code;

            if (Order.ShipToAddress.ShipToCode != Order.SoldToAddress.SoldToCode)
            {
                var SoldTo = Shipment.SelectSingleNode("SoldTo");
                SoldTo.SelectSingleNode("CompanyName").InnerText = Order.ShipToAddress.CompanyName;
                if (!string.IsNullOrEmpty(Order.ShipToAddress.AttentionName))//default:No contactperson
                    SoldTo.SelectSingleNode("AttentionName").InnerText = Order.ShipToAddress.AttentionName.Trim().MaxLength(35);
                if (!string.IsNullOrEmpty(Order.ShipToAddress.PhoneNumber))//default:0000000000
                    SoldTo.SelectSingleNode("PhoneNumber").InnerText = System.Text.RegularExpressions.Regex.Replace(Order.ShipToAddress.PhoneNumber, @"\D", "").MaxLength(15);
                SoldTo.SelectSingleNode("Address/AddressLine1").InnerText = Order.ShipToAddress.AddressLine1.MaxLength(35);
                SoldTo.SelectSingleNode("Address/AddressLine2").InnerText = Order.ShipToAddress.AddressLine2.MaxLength(35);
                SoldTo.SelectSingleNode("Address/AddressLine3").InnerText = Order.ShipToAddress.AddressLine3.MaxLength(35);
                SoldTo.SelectSingleNode("Address/City").InnerText = Order.ShipToAddress.City.MaxLength(30);
                SoldTo.SelectSingleNode("Address/PostalCode").InnerText = Order.ShipToAddress.PostalCode.MaxLength(9);
                SoldTo.SelectSingleNode("Address/CountryCode").InnerText = Order.ShipToAddress.CountryCode;
                SoldTo.SelectSingleNode("Address/StateProvinceCode").InnerText = Order.ShipToAddress.State;
            }
            else
            {
                Shipment.RemoveChild(Shipment.SelectSingleNode("SoldTo"));
            }

            foreach (PackedContainer pc in Order.PackedContainers)
            {
                
                    //<Package>
                    //    <PackagingType>
                    //        <Code>02</Code>
                    //    </PackagingType>
                    //    <PackageWeight>
                    //        <UnitofMeasurement>
                    //            <Code>KGS</Code>
                    //        </UnitofMeasurement>
                    //        <Weight>0.3</Weight>
                    //    </PackageWeight>
                    //</Package>
                    var package = Shipment.AddElement("Package");
                    if (isReturnShipment)
                    {
                        package.AddElement("Description").InnerText = "Return package";
                    }
                    //02 seems to hard coded in the sproc?
                    package.AddElement("PackagingType").AddElement("Code").InnerText = "02";
                    var PackageWeight = package.AddElement("PackageWeight");
                    PackageWeight.AddElement("UnitOfMeasurement").AddElement("Code").InnerText = "KGS";


                    // UPS weight fix: ups only allows one digit after the decimal
                    // if weight is lower than 100gr, set it to 0.1kg
                    // if weight is above 100gr, round it to the nearest decimal
                    double weightInKg = ((double)pc.Weight / 1000);
                    if(pc.Weight < 100)
                        weightInKg = 0.1;
                    else
                        weightInKg = Math.Round(weightInKg, 1);
                    PackageWeight.AddElement("Weight").InnerText = weightInKg.ToString(CultureInfo.InvariantCulture);
            }

            if (isReturnShipment)
            {
                var returnService = Shipment.AddElement("ReturnService");
                returnService.AddElement("Code").InnerText = "9";

            }
        }
    }

}
