using ShippingService.Business.EF.Facade.Carriers.TNT.Label;
using ShippingService.Business.EF.Facade.Carriers.TNT.Price;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tweddle.Commons.Extensions;

namespace Web.Controllers
{
    public class TNTController : Controller
    {
        
        public ActionResult CheckPrice()
        {
            return View();
        }

        public ActionResult GetLabel()
        {
            return View();
        }


        [HttpPost]
        public string CreateLabelRequest(string consignmentnumber,
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
            string accountNumber,
            double weightInKG,
            double heightInM,
            double widthInM,
            double lengthInM,
            string goodsDescription,
            string reference)
        {
            TNTPieceLine line = new TNTPieceLine();
            line.GoodsDescription = goodsDescription;
            line.HeightInM = heightInM;
            line.LengthInM = lengthInM;
            line.WeightInKG = weightInKG;
            line.WidthInM = widthInM;
            line.Pieces = new List<TNTPiece>() { new TNTPiece() { Reference = reference} };


            return TNTLabelRequest.CreateRequest(
                consignmentnumber: consignmentnumber,
                consignmentReference: consignmentReference,
                senderName: senderName,
                senderAddressLine1:senderAddressLine1,
                senderPostcode:senderPostcode,
                senderTown:senderTown,
                deliveryName: deliveryName,
                deliveryAddressLine1: deliveryAddressLine1,
                deliveryCountry: deliveryCountry,
                deliveryPostCode: deliveryPostCode,
                deliveryTown: deliveryTown,
                productType: productType,
                productId: productId,
                accountNumber: accountNumber,
                pieceLines: new List<TNTPieceLine>() { line }
                );
        }

        [HttpPost, ValidateInput(false)]
        public string SendLabelRequest(string xml)
        {
            string response = TNTLabelRequest.Send(xml);

            //var responseObject = response.ToObject<ShippingService.Business.EF.Facade.Carriers.TNT.Label.Response.labelResponse>();

            return response;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateLabelAsPDF(string xml)
        {

            string virtualFolder = "~/Content/TNT/LABELS/" +  Guid.NewGuid().ToString();
            string virtualOutputPDF = Path.Combine(virtualFolder, "tnt.pdf");

            var tempFilePath = Server.MapPath(virtualFolder);

            Directory.CreateDirectory(tempFilePath);
            
            string xmlFilePath = Path.Combine(tempFilePath, "tnt.xml");
            StreamWriter sw = new StreamWriter(xmlFilePath);
            sw.WriteLine(xml);
            sw.Close();
            sw.Dispose();

            string pdfFilePath = Path.Combine(tempFilePath, "tnt.pdf");

            var responseobj = xml.ToObject<ShippingService.Business.EF.Facade.Carriers.TNT.Label.Response.labelResponse>();

            if (responseobj.consignment != null)
            { 
                //generate all barcodes:
                foreach (var cons in responseobj.consignment)
                {
                    foreach (var piece in cons.pieceLabelData)
                    {
                        string barcode = piece.barcode.Value;

                        //barcode lib from: http://www.codeproject.com/Articles/20823/Barcode-Image-Generation-Library
                        BarcodeLib.Barcode bc = new BarcodeLib.Barcode();
                        bc.Encode(BarcodeLib.TYPE.CODE128C, barcode, 418, 140);
                        bc.SaveImage(Path.Combine(tempFilePath, barcode), BarcodeLib.SaveTypes.JPG);

                    }
                }
            }

            string xslFilePath = Server.MapPath("~/Content/TNT/XSL/PDF/PDFRoutingLabelRenderer.xsl");

            TNTLabelGenerator gen = new TNTLabelGenerator();
            var val = gen.GeneratePDF(xslFilePath, xmlFilePath, pdfFilePath, tempFilePath + @"\\");

            return Json(new
            {
                Link = Url.Content(virtualOutputPDF),
                BrokenRules = val.BrokenRules
            });

        }

        

        [HttpPost]
        public string CreatePriceRequest(string rateId,
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
            string productId)
        {

            string xml = TNTPriceChecker.CreateRequest(rateId, senderPostCode, senderTown, deliveryCountry,
                deliveryPostCode, deliveryTown, accountNumber, weightInKG, volumeInM3, numberOfPieces, productType, productId);


            return xml;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SubmitRequest(string xml)
        {
            string response = TNTPriceChecker.Send(xml);

            var responseObject = response.ToObject<ShippingService.Business.EF.Facade.Carriers.TNT.Price.PriceResponse.document>();
            
            return Json(responseObject);
        }
    }
}