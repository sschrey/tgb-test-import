using ShippingService.Business.EF.Facade.Carriers.TNT.Label.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweddle.Commons.Extensions;

namespace ShippingService.Business.EF.Facade.Carriers.TNT.Label
{
    public static class TNTLabelRequest
    {
        const string ACCOUNT_COUNTRY = "BE";

        public static string CreateRequest(
            string consignmentnumber,
            string consignmentReference,
            string senderName,
            string senderAddressLine1,
            string senderPostcode,
            string senderTown,
            string deliveryName,
            string deliveryAddressLine1,
            string deliveryCountry,
            string deliveryPostCode,
            string deliveryTown,
            string productType,
            string productId,
            int lineOfBusiness,
            string accountNumber,
            List<TNTPieceLine> pieceLines
            )
        {
            labelRequest request = new labelRequest();
            labelConsignmentsType consignment = new labelConsignmentsType();
            request.consignment = new List<labelConsignmentsType>() { consignment }.ToArray();

            consignment.consignmentIdentity = new consignmentIdentityType()
            {
                consignmentNumber = consignmentnumber,
                customerReference = consignmentReference
            };

            consignment.collectionDateTime = DateTime.Now;

            var sender = new nameAndAddressRequestType();
            consignment.sender = sender;
            sender.addressLine1 = senderAddressLine1;
            sender.name = senderName;
            sender.postcode = senderPostcode;
            sender.town = senderTown;
            sender.country = ACCOUNT_COUNTRY;

            var delivery = new nameAndAddressRequestType();
            consignment.delivery = delivery;
            delivery.addressLine1 = deliveryAddressLine1;
            delivery.country = deliveryCountry;
            delivery.postcode = deliveryPostCode;
            delivery.name = deliveryName;
            delivery.town = deliveryTown;

            var product = new productType();
            consignment.product = product;
            product.type = productType == "N" ? productTypeEnum.N : productTypeEnum.D;
            product.id = productId;
            product.lineOfBusiness = lineOfBusiness;

            var account = new accountType();
            consignment.account = account;
            account.accountCountry = ACCOUNT_COUNTRY;
            account.accountNumber = accountNumber;


            var pieceLineList = new List<pieceLineType>();
            foreach (var tntpieceLine in pieceLines)
            {
                var pieceLine = new pieceLineType();
                pieceLineList.Add(pieceLine);
                pieceLine.identifier = pieceLines.IndexOf(tntpieceLine) + 1;
                pieceLine.goodsDescription = tntpieceLine.GoodsDescription;

                var pieceMeasurements = new measurementsType();
                pieceLine.pieceMeasurements = pieceMeasurements;
                pieceMeasurements.height = tntpieceLine.HeightInM;
                pieceMeasurements.weight = tntpieceLine.WeightInKG;
                pieceMeasurements.width = tntpieceLine.WidthInM;
                pieceMeasurements.length = tntpieceLine.LengthInM;

                var pieces = new List<pieceType>();
                foreach (var tntpiece in tntpieceLine.Pieces)
                {
                    var piece = new pieceType();
                    pieces.Add(piece);
                    piece.sequenceNumbers = (tntpieceLine.Pieces.IndexOf(tntpiece) + 1).ToString();
                    piece.pieceReference = tntpiece.Reference;
                }
                pieceLine.pieces = pieces.ToArray();
            }
            consignment.pieceLine = pieceLineList.ToArray();
            consignment.totalNumberOfPieces = pieceLines.Sum(pl => pl.Pieces.Count);

            //encode string without the bom
            var encoding = new UTF8Encoding(false);

            string xml = request.ToXML(new UTF8Encoding(false));

            return xml;
        }

        public static string Send(string xml)
        {
            string url = "https://express.tnt.com/expresslabel/documentation/getlabel";

            System.Net.Http.HttpContent content = new System.Net.Http.StringContent(xml, Encoding.UTF8, "text/xml");
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("TLITHO:M3CH3LM");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            var result = client.PostAsync(url, content).Result;
            string resultContent = result.Content.ReadAsStringAsync().Result;

            return resultContent;
        }

    }
}
