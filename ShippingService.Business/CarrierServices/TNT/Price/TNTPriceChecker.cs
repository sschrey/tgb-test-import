using ShippingService.Business.CarrierServices.TNT.Price.PriceRequest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tweddle.Commons.Extensions;

namespace ShippingService.Business.CarrierServices.TNT.Price
{

    public static class ProductType
    {
        public const string DOCUMENT = "D";
        public const string NON_DOCUMENT = "N";
    }

    public class TNTPriceChecker
    {
        const string APP_ID = "PC";
        const decimal APP_VERSION = 3.0m;
        const string ACCOUNT_COUNTRY = "BE";
        const string CURRENCY = "EUR";

       
        public static string CreateRequest(
            string rateId,
            string senderPostCode,
            string senderTown,
            string deliveryCountry,
            string deliveryPostCode,
            string deliveryTown,
            string accountNumber,
            decimal weightInKG,
            decimal volumeInM3,
            int numberOfPieces,
            string productType,
            string productId
            )
        {
            priceRequest req = new priceRequest();
            req.appId = APP_ID;
            req.appVersion = APP_VERSION;

            var pc = new priceCheck();
            req.priceCheck = new List<priceCheck>() { pc }.ToArray();
            pc.collectionDateTime = DateTime.Now;

            pc.rateId = rateId;

            pc.sender = new address();
            pc.sender.country = ACCOUNT_COUNTRY;
            pc.sender.postcode = senderPostCode;
            pc.sender.town = senderTown;

            pc.delivery = new address();
            pc.delivery.country = deliveryCountry;
            pc.delivery.postcode = deliveryPostCode;
            pc.delivery.town = deliveryTown;

            pc.account = new account();
            pc.account.accountCountry = ACCOUNT_COUNTRY;
            pc.account.accountNumber = accountNumber;

            pc.currency = CURRENCY;
            pc.priceBreakDown = false;

            pc.consignmentDetails = new consignmentDetails();
            pc.consignmentDetails.totalWeight = weightInKG;
            pc.consignmentDetails.totalVolume = volumeInM3;
            pc.consignmentDetails.totalNumberOfPieces = numberOfPieces.ToString();
            pc.consignmentDetails.totalVolumeSpecified = true;

            pc.product = new product();
            pc.product.type = productType;
            pc.product.id = productId;

            string xml = req.ToXML(Encoding.UTF8);

            return xml;
        }

        public static string Send(string xml)
        {
            string url = "https://express.tnt.com/expressconnect/pricing/getprice";


            System.Net.Http.HttpContent content = new System.Net.Http.StringContent(xml, Encoding.UTF8, "application/xml");
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("TLITHO:M3CH3LM");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var result = client.PostAsync(url, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            return resultContent;
        }
    }
}
