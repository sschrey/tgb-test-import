using ShippingService.Business.CarrierServices;
using ShippingService.Business.Domain;
using ShippingService.Business.EF.Facade.Carriers.TNT.Label;
using ShippingService.Business.EF.Facade.Carriers.TNT.Price;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        public ActionResult Daily()
        {
            var oc = new OrderCriteria();

            oc.Carrier = "87433";
            oc.ShippedDateFrom = DateTime.Now;
            oc.ShippedDateTo = DateTime.Now;

            VMTNTDailyModel model = new VMTNTDailyModel();

            var orders = ApplicationContextHolder.Instance.Facade.GetOrders(oc);
            foreach (var order in orders)
            {
                model.Orders.Add(new VMOrder(order));
            }

            model.Carriers = ApplicationContextHolder.Instance.Facade.GetCarriers();
            model.SelectedCarrier = oc.Carrier;

            model.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");

            return View(model);

        }

        [HttpPost]
        public ActionResult Daily(VMTNTDailyModel model)
        {
            var oc = new OrderCriteria();

            oc.Carrier = model.SelectedCarrier;
            oc.ShippedDateFrom = DateTime.Parse(model.CurrentDate);
            oc.ShippedDateTo = DateTime.Parse(model.CurrentDate);

            var orders = ApplicationContextHolder.Instance.Facade.GetOrders(oc);
            foreach (var order in orders)
            {
                model.Orders.Add(new VMOrder(order));
            }

            return Json(model);
        }

        [HttpPost]
        public string SendOrders(VMTNTDailyModel model)
        {
            var facade = ApplicationContextHolder.Instance.Facade;
            var oc = new OrderCriteria();

            List<string> ids = new List<string>();
            foreach (var order in model.Orders)
            {
                ids.Add(order.OrderId);
            }
            oc.Ids = ids.ToArray();

            var carriermodes = facade.GetCarrierModes();
            var orders = ApplicationContextHolder.Instance.Facade.GetOrders(oc);

            foreach (var order in orders)
            {
                var carriermode = order.ShippedCarrierMode == null ? carriermodes.First(cm => cm.Id == order.ProposedCarrierMode) : carriermodes.FirstOrDefault(cm => cm.Id == order.ShippedCarrierMode);
                var shipper = new TNTShipping { Order = order, ShippingVendor = carriermode, Facade = facade };
                var record = shipper.GetRecord();
                var virtualPath = "~/Content/TNT/ORDERS/";
                var ordervirtualPath = virtualPath + order.Id + ".txt";
                StreamWriter sw = new StreamWriter(Server.MapPath(ordervirtualPath));
                sw.Write(record);
                sw.Close();
                sw.Dispose();
            }

            return orders.Count() + " orders saved.";

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
            int lineOfBusiness,
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
            line.Pieces = new List<TNTPiece>() { new TNTPiece() { Reference = reference } };


            return TNTLabelRequest.CreateRequest(
                consignmentnumber: consignmentnumber,
                consignmentReference: consignmentReference,
                senderName: senderName,
                senderAddressLine1: senderAddressLine1,
                senderPostcode: senderPostcode,
                senderTown: senderTown,
                deliveryName: deliveryName,
                deliveryAddressLine1: deliveryAddressLine1,
                deliveryCountry: deliveryCountry,
                deliveryPostCode: deliveryPostCode,
                deliveryTown: deliveryTown,
                productType: productType,
                productId: productId,
                lineOfBusiness: lineOfBusiness,
                accountNumber: accountNumber,
                pieceLines: new List<TNTPieceLine>() { line }
                );
        }

        [HttpPost]
        public string CreateManifestSummeryRequest(string accountnumber, 
            string accountcountry, 
            string sendername,
            string senderaddressline1,
            string sendertown,
            string senderpostcode,
            string sendercountry,
            string consignmentnumber,
            string consignmentpieces,
            string consignmentweight,
            string consignmentshipperref,
            string consignmentreceiver,
            string consignmentcity,
            string consignmentdestination,
            string consignmentservice,
            string grandtotalconsignments,
            string grandtotalpieces,
            string grandtotalweigt,
            string printdate,
            string printtime
            )
        {
            ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.manifestsummary ms = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.manifestsummary();
            ms.account = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.account();
            ms.account.accountCountry = accountcountry;
            ms.account.accountNumber = accountnumber;

            ms.sender = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.sender();
            ms.sender.addressLine1 = senderaddressline1;
            ms.sender.country = sendercountry;
            ms.sender.name = sendername;
            ms.sender.postcode = senderpostcode;
            ms.sender.town = sendertown;

            var consignments = new List<ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.consignment>();
            var consignment = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.consignment();
            consignments.Add(consignment);

            consignment.city = consignmentcity;
            consignment.destination = consignmentdestination;
            consignment.number = consignmentnumber;
            consignment.pieces = consignmentpieces;
            consignment.receiver = consignmentreceiver;
            consignment.service = consignmentservice;
            consignment.shipperref = consignmentshipperref;
            consignment.weight = consignmentweight;

            ms.consignment = consignments.ToArray();

            ms.grandtotal = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.grandtotal();
            ms.grandtotal.consignments = grandtotalconsignments;
            ms.grandtotal.pieces = grandtotalpieces;
            ms.grandtotal.weight = grandtotalweigt;

            ms.printdate = printdate;
            ms.printtime = printtime;

            //encode string without the bom
            var encoding = new UTF8Encoding(false);

            string xml = ms.ToXML(new UTF8Encoding(false));

            return xml;
        }

        [HttpPost]
        public string CreateManifestDetailRequest(string accountnumber,
            string accountcountry,
            string sendername,
            string senderaddressline1,
            string sendertown,
            string senderpostcode,
            string sendercountry,
            string shippingoptiontitle,
            string consignmentnumber,
            string consignmentshipperref,
            string consignmentdangerousgoods,
            string consignmentreceivername,
            string consignmentreceiverstreet1,
            string consignmentreceiverstreet2,
            string consignmentreceivercity,
            string consignmentreceiverpostcode,
            string consignmentreceivercountry,
            string consignmentreceiverphone,
            string consignmentreceivercontact,
            string consignmentshippingservice,
            string consignmentshippingoption,
            string consignmentpieces,
            string consignmentweight,
            string consignmentinsurancevalue,
            string consignmentinvoicevalue,
            string consignmentdescription,
            string consignmentdimensions,
            string consignmentvolume,
            string printdate,
            string printtime
            )
        {
            ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.manifestdetail md = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.manifestdetail();
            md.account = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.account();
            md.account.accountCountry = accountcountry;
            md.account.accountNumber = accountnumber;

            md.sender = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.sender();
            md.sender.addressLine1 = senderaddressline1;
            md.sender.country = sendercountry;
            md.sender.name = sendername;
            md.sender.postcode = senderpostcode;
            md.sender.town = sendertown;


            var shippingoptions = new List<ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.shippingoption>();
            var shippingoption = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.shippingoption();
            shippingoptions.Add(shippingoption);
            shippingoption.title = shippingoptiontitle;
            
            
            var consignments = new List<ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.consignment>();
            var consignment = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.consignment();
            consignments.Add(consignment);

            consignment.number = consignmentnumber;
            consignment.barcode = consignmentnumber;
            consignment.pieces = consignmentpieces;
            consignment.dangerousgoods = consignmentdangerousgoods;
            consignment.description = consignmentdescription;
            consignment.dimensions = consignmentdimensions;
            consignment.insurancevalue = consignmentinsurancevalue;
            consignment.invoicevalue = consignmentinvoicevalue;
            consignment.pieces = consignmentpieces;
            consignment.receiver = new ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.receiver();
            consignment.receiver.city = consignmentreceivercity;
            consignment.receiver.contact = consignmentreceivercontact;
            consignment.receiver.country = consignmentreceivercountry;
            consignment.receiver.name = consignmentreceivername;
            consignment.receiver.phone = consignmentreceiverphone;
            consignment.receiver.postcode = consignmentreceiverpostcode;
            consignment.receiver.street1 = consignmentreceiverstreet1;
            consignment.receiver.street2 = consignmentreceiverstreet2;
            consignment.shippingservicecode = consignmentshippingservice;
            consignment.shippingoptioncode = consignmentshippingoption;
            consignment.insurancevalue = consignmentinsurancevalue;
            consignment.invoicevalue = consignmentinvoicevalue;
            consignment.volume = consignmentvolume;
            consignment.shipperref = consignmentshipperref;
            consignment.weight = consignmentweight;

            shippingoption.consignment = consignments.ToArray();
            md.shippingoption = shippingoptions.ToArray();

         
            md.printdate = printdate;
            md.printtime = printtime;

            //encode string without the bom
            var encoding = new UTF8Encoding(false);

            string xml = md.ToXML(new UTF8Encoding(false));

            return xml;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateManifestDetailPDF(string xml)
        {
            string virtualFolder = "~/Content/TNT/MANIFEST_DETAIL/" + Guid.NewGuid().ToString();
            string virtualOutputPDF = Path.Combine(virtualFolder, "tnt.pdf");

            var tempFilePath = Server.MapPath(virtualFolder);

            Directory.CreateDirectory(tempFilePath);

            string xmlFilePath = Path.Combine(tempFilePath, "tnt.xml");
            StreamWriter sw = new StreamWriter(xmlFilePath);
            sw.WriteLine(xml);
            sw.Close();
            sw.Dispose();

            string pdfFilePath = Path.Combine(tempFilePath, "tnt.pdf");

            var responseobj = xml.ToObject<ShippingService.Business.EF.Facade.Carriers.TNT.ManifestDetail.manifestdetail>();

            if (responseobj.shippingoption != null)
            {
                //generate all barcodes:
                foreach (var shippingoption in responseobj.shippingoption)
                {
                    foreach (var cons in shippingoption.consignment)
                    {
                        string barcode = cons.barcode;
                        //barcode lib from: http://www.codeproject.com/Articles/20823/Barcode-Image-Generation-Library
                        BarcodeLib.Barcode bc = new BarcodeLib.Barcode();
                        bc.Encode(BarcodeLib.TYPE.CODE39, barcode, 300, 25);
                        bc.SaveImage(Path.Combine(tempFilePath, barcode), BarcodeLib.SaveTypes.JPG);
                    }
                }
            }


            string xslFilePath = Server.MapPath("~/Content/TNT/XSL/MANIFEST_DETAIL/pdf.xsl");

            TNTLabelGenerator gen = new TNTLabelGenerator();
            var val = gen.GeneratePDF(xslFilePath, xmlFilePath, pdfFilePath, tempFilePath + @"\\");

            return Json(new
            {
                Link = Url.Content(virtualOutputPDF),
                BrokenRules = val.BrokenRules
            });
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CreateManifestSummeryPDF(string xml)
        {
            string virtualFolder = "~/Content/TNT/MANIFEST_SUMMARY/" + Guid.NewGuid().ToString();
            string virtualOutputPDF = Path.Combine(virtualFolder, "tnt.pdf");

            var tempFilePath = Server.MapPath(virtualFolder);

            Directory.CreateDirectory(tempFilePath);

            string xmlFilePath = Path.Combine(tempFilePath, "tnt.xml");
            StreamWriter sw = new StreamWriter(xmlFilePath);
            sw.WriteLine(xml);
            sw.Close();
            sw.Dispose();

            string pdfFilePath = Path.Combine(tempFilePath, "tnt.pdf");

            var responseobj = xml.ToObject<ShippingService.Business.EF.Facade.Carriers.TNT.ManifestSummary.manifestsummary>();

            string xslFilePath = Server.MapPath("~/Content/TNT/XSL/MANIFEST_SUMMARY/pdf.xsl");

            TNTLabelGenerator gen = new TNTLabelGenerator();
            var val = gen.GeneratePDF(xslFilePath, xmlFilePath, pdfFilePath, tempFilePath + @"\\");

            return Json(new
            {
                Link = Url.Content(virtualOutputPDF),
                BrokenRules = val.BrokenRules
            });
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

            string virtualFolder = "~/Content/TNT/LABELS/" + Guid.NewGuid().ToString();
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

        public ActionResult ManifestSummary()
        {
            return View();
        }

        public ActionResult ManifestDetail()
        {
            return View();
        }
    }
}