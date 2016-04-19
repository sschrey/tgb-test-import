using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using ShippingService.Business.Domain;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Xml;
using Tweddle.Commons.Vies;
using ShippingService.Business.CarrierServices.TNT.Label;
using Tweddle.Commons.Extensions;
using Seagull.BarTender.Print;

namespace ShippingService.Business.CarrierServices
{
    public class TNTShipping : IShipping
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TNTShipping));
        public event ShippingFactory.ErrorMessage Error;
        public CarrierMode ShippingVendor { get; set; }
        public Order Order { get; set; }
        public IFacade Facade { get; set; }
        public string Message { get; set; }
        public string WebAppRoot { get; set; }

        private string _consignmentNumber;
        private string _consignmentRef;
        
        public TNTShipping()
        {
            Message = string.Empty;
        }

        public void SetReturnShipment()
        {
            throw new NotImplementedException();
        }
        public bool CanPrint
        {
            get { return true; }
        }

        /// <summary>
        /// After the order has been serialized into XML, send it to the XML service
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>XML response from TNT</returns>
        public string SendLabelRequest(string xml)
        {
            string response = TNTLabelRequest.Send(xml);
            
            return response;
        }

        /// <summary>
        /// Package an order into an XML message for TNT
        /// </summary>
        /// <returns></returns>
        public string CreateLabelRequest()
        {
            var order = Order;
            string tntAccountNumber = ConfigurationManager.AppSettings["TNTAccountNumber"];
            var carriermodes = Facade.GetCarrierModes();
            
            List<TNTPieceLine> lines = new List<TNTPieceLine>();
            int pccounter = 0;
            foreach (var pc in order.PackedContainers)
            {
                pccounter++;
                TNTPieceLine line = new TNTPieceLine();
                lines.Add(line);
                line.GoodsDescription = string.Join("\n", "box" + pccounter);
                line.HeightInM = (double)pc.Container.Height * 0.001;
                line.LengthInM = (double)pc.Container.Depth * 0.001;
                line.WeightInKG = (double)pc.Weight * 0.001;
                line.WidthInM = (double)pc.Container.Width * 0.001;

                line.Pieces.Add(new TNTPiece() { Reference = order.ReferenceNumber });
            }

            TNTServiceTranslator trans = new TNTServiceTranslator(order.MainAddress.CountryCode, carriermodes.First(cm => cm.Id == order.ShippedCarrierModeOption));
            string request = TNTLabelRequest.CreateRequest(
                    consignmentnumber: order.PackedContainers[0].TrackingNumber,
                    consignmentReference: order.ReferenceNumber,
                    senderName: TGBAddress.CompanyName,
                    senderAddressLine1: TGBAddress.AddressLine1,
                    senderPostcode: TGBAddress.PostalCode,
                    senderTown: TGBAddress.City,
                    deliveryName: order.MainAddress.CompanyName,
                    deliveryAddressLine1: order.MainAddress.AddressLine1,
                    deliveryCountry: order.MainAddress.CountryCode,
                    deliveryPostCode: order.MainAddress.PostalCode,
                    deliveryTown: order.MainAddress.City,
                    productType: trans.ProductType,
                    productId: trans.ProductId,
                    lineOfBusiness: trans.LineOfBusiness,
                    accountNumber: tntAccountNumber,
                    pieceLines: lines
                    );

            return request;
        }



        public bool Execute()
        {
            CreateTrackingNumber();
            UpdateTrackingNumber();
            string requestxml = CreateLabelRequest();
            string responsexml = SendLabelRequest(requestxml);
            WriteLabel(responsexml);


            string UpdateE1AsString = ConfigurationManager.AppSettings["UPDATE_E1"];
            bool updateE1 = false;
            if (!string.IsNullOrEmpty(UpdateE1AsString))
            {
                updateE1 = Convert.ToBoolean(UpdateE1AsString);
            }

            if (Order.Status == OrderStatus.Shipped)
            {
                Facade.ReShip(Order);
            }
            else
            {
                Facade.Ship(Order, updateE1);
            }
            return true;
        }

        private void WriteLabel(string xml)
        {
            string filename = string.Format("Order{0}_{1}.xml", Order.ReferenceNumber, _consignmentNumber);
            string filepath = Path.Combine(LabelStoragPath, filename);
            File.WriteAllText(filepath, xml);
            foreach (var pc in Order.PackedContainers)
            {
                pc.TNTLabel = filepath;
            }
        }

        public static string LabelStoragPath
        {
            get
            {
                //Get the path to where the TNT labels will be saved and verify that it is valid
                string tntLabelStoragePath = ConfigurationManager.AppSettings["TNTLabelDirectory"];
                if (string.IsNullOrEmpty(tntLabelStoragePath))
                    tntLabelStoragePath = @"\\192.168.0.25\Data\TNTLabels";

                if (!Directory.Exists(tntLabelStoragePath))
                    Directory.CreateDirectory(tntLabelStoragePath);

                return tntLabelStoragePath;
            }
        }

        internal void UpdateTrackingNumber()
        {
            foreach (var ol in Order.Lines)
            {
                foreach (var po in ol.Packs)
                {
                    po.PackedContainer.TrackingNumber = _consignmentNumber;
                }
            }
        }

        private void CreateTrackingNumber()
        {

            //the TNT consignment number is 9 digits long number including a final checkdigit 
            int nextNumber = Facade.GetNextTNTConsignmentNoteNumber();
            while (nextNumber < 10000000) nextNumber = nextNumber * 10;
            //checkdigit using modulus 7 = con - (integer(con/7)*7
            var check = (int)(nextNumber - (Math.Floor((double)nextNumber / 7) * 7));
            _consignmentNumber = nextNumber.ToString() + check;
        }


        #region GetRecords
        public string GetRecord()
        {
            if (Order == null)
            {
                return "No order found";
            }
            if (Order.Lines == null || Order.Lines.Count == 0)
            {
                return "No orderlines found";
            }
            if (Order.Lines[0].Packs == null || Order.Lines[0].Packs.Count == 0)
            {
                return "No packs found in order line 1";
            }

            _consignmentNumber = Order.Lines[0].Packs[0].PackedContainer.TrackingNumber;
            _consignmentRef = Order.ReferenceNumber;
            var s = CreateRecords();

            return s;
        }

        internal string CreateRecords()
        {
            var sb = new StringBuilder();
            AddRecordA(sb);
            AddRecordB(sb);
            AddRecordC(sb);
            return sb.ToString();
        }

        private bool IsExport()
        {
            return !new Vies().IsVATEligible(Order.MainAddress.CountryCode);
        }

        private void AddRecordA(StringBuilder sb)
        {
            //General Consignment Details
            sb.Append(WriteField("A", 1)); //1 record id
            sb.Append(WriteField("", 12)); //2 filler
            sb.Append(WriteField(_consignmentRef, 24)); //3 consignment ref
            sb.Append(WriteField(DateTime.Now.ToString("yyyyMMdd"), 8)); //4 shipping date
            sb.Append(WriteField(_consignmentNumber, 15)); //5 consignment number including checkdigit
            sb.Append(WriteField("", 60)); //6 Special Instructions len60
            sb.Append(WriteField(Order.ShippedCarrierMode, 5)); //7 Service Code len5
            sb.Append(WriteField("S", 1)); //8 Payment Indicator len1
            sb.Append(WriteField(string.IsNullOrEmpty(Order.ShippedCarrierModeOption) ? "" : Order.ShippedCarrierModeOption, 3)); //9 Sub Service Option 1 len3 (eg PR for priority- we always remove)
            sb.Append(WriteField("", 3)); //10 Sub Service Option 2 len3
            sb.Append(WriteField("", 3)); //11 Sub Service Option 3
            sb.Append(WriteField("", 3)); //12 Sub Service Option 4
            sb.Append(WriteField(Order.PackedContainers.Count, 4)); //13 Total Packages
            sb.Append(WriteField(GrToKg(Order.PackedContainers.Sum(pc => pc.Weight)), 8, 3)); //14 Total Weight
            sb.Append(WriteField("EUR", 3)); //15 Currency Code (EUR) len3
            if (IsExport())
            {

                sb.Append(WriteField(CentsToEuro(Order.Lines.Sum(l => l.UnitPrice * l.OrderQty)), 11, 2)); //16 Consignment Value len11   
            }
            else
            {
                sb.Append(WriteField("", 11)); //16 Consignment Value len11
            }


            sb.Append(WriteField("", 11)); //17 Insurance Value len11
            sb.Append(WriteField("", 3)); //18 Insurance Currency len3
            sb.Append(WriteField(Volume(Order.PackedContainers.Sum(pc => pc.Container.VolumeInM3)), 7, 3)); //19 Total Volume len7
            sb.Append(WriteField("0", 1)); //20 Hazardous Indicator (0) len1
            sb.Append(WriteField("", 18)); //21 Group ID len18
            sb.Append(WriteField("TNT", 5)); //22 Carrier ID (TNT) len5
            sb.Append(WriteField("", 35)); //23 Free Format 1 len35
            sb.Append(WriteField("", 35)); //24 Free Format 2 len35
            sb.Append(WriteField("", 35)); //25 Free Format 3 len35
            sb.Append(WriteField("", 35)); //26 Free Format 4 len35
            sb.Append(WriteField("", 20)); //27 Free Format 5 len20
            sb.Append(WriteField("", 50)); //28 Unilab Printer Name len50
            sb.Append(WriteField("", 2)); //29 Unilab Printer Port No  len2
            sb.Append(WriteField(new Vies().IsVATEligible(Order.MainAddress.CountryCode) ? "N" : "Y", 1)); //30 Customs Controlled Indicator (N) len1
            sb.Append(WriteField(Order.ReferenceNumber, 60)); //31 Invoice Number 1 len60
            sb.Append(WriteField(Order.OrderNumber, 60)); //32 Invoice Number 2 len60
            sb.Append(WriteField(Order.ZoneNumberDescription, 60)); //33 Invoice Number 3 len60
            sb.Append(Environment.NewLine);
        }

        private void AddRecordB(StringBuilder sb)
        {
            sb.Append(WriteField("B", 1)); //1 "Record ID" len1
            sb.Append(WriteField("", 12)); //2 "Filler" len12
            sb.Append(WriteField(_consignmentRef, 24)); //3 "Consignment Reference" len24
            sb.Append(WriteField(ConfigurationManager.AppSettings["TNT_SENDER_SHORT_CODE"], 15)); //4 "Sender Short Code" len15
            sb.Append(WriteField(TGBAddress.CompanyName, 50)); //5 "Sender Name" len50
            sb.Append(WriteField(TGBAddress.AddressLine1, 30)); //6 "Sender Street 1" len30
            sb.Append(WriteField("", 30)); //7 "Sender Street 2" len30
            sb.Append(WriteField("", 30)); //8 "Sender Street 3" len30
            sb.Append(WriteField(TGBAddress.City, 30)); //9 "Sender Town" len30
            sb.Append(WriteField("", 30)); //10 "Sender County" len30
            sb.Append(WriteField(TGBAddress.PostalCode, 9)); //11 "Sender Postcode" len9
            sb.Append(WriteField(TGBAddress.CountryCode, 3)); //12 "Sender Country" len3
            sb.Append(WriteField("BE 458-622-730", 20)); //13 "Sender VAT Number" len20
            sb.Append(WriteField(TGBAddress.Attention, 22)); //14 "Sender Contact Name" len22
            sb.Append(WriteField("", 7)); //15 "Sender Phone Part 1" len7
            sb.Append(WriteField("", 9)); //16 "Sender Phone Part 2" len9
            sb.Append(WriteField("", 50)); //17 "Sender Email Address" len50
            sb.Append(WriteField("", 7)); //18 "Sender Fax Part 1" len7
            sb.Append(WriteField("", 9)); //19 "Sender Fax Part 2" len9
            sb.Append(WriteField("66104", 9)); //20 "Sender TNT Account No" len9
            sb.Append(WriteField("", 15)); //21 "Receiver Short Code" len15
            sb.Append(WriteField(Order.MainAddress.CompanyName, 50)); //22 "Receiver Name" len50
            sb.Append(WriteField(Order.MainAddress.AddressLine1, 30)); //23 "Receiver Street 1" len30
            sb.Append(WriteField(Order.MainAddress.AddressLine2, 30)); //24 "Receiver Street 2" len30
            sb.Append(WriteField(Order.MainAddress.AddressLine3, 30)); //25 "Receiver Street 3" len30
            sb.Append(WriteField(Order.MainAddress.City, 30)); //26 "Receiver Town" len30
            sb.Append(WriteField("", 30)); //27 "Receiver County" len30
            sb.Append(WriteField(Order.MainAddress.PostalCode, 9)); //28 "Receiver Postcode" len9
            sb.Append(WriteField(Order.MainAddress.CountryCode, 3)); //29 "Receiver Country" len3
            sb.Append(WriteField("", 20)); //30 "Receiver VAT Number" len20
            sb.Append(WriteField(Order.MainAddress.AttentionName, 22)); //31 "Receiver Contact Name" len22
            sb.Append(WriteField("", 7)); //32 "Receiver Phone Part 1" len7
            sb.Append(WriteField("", 9)); //33 "Receiver Phone Part 2" len9
            sb.Append(WriteField("", 50)); //34 "Receiver Email Address" len50
            sb.Append(WriteField("", 7)); //35 "Receiver Fax Part 1" len7
            sb.Append(WriteField("", 9)); //36 "Receiver Fax Part 2" len9
            sb.Append(WriteField("", 9)); //37 "Receiver TNT Account No" len9
            sb.Append(WriteField("", 15)); //38 "Pick Up Short Code" len15
            sb.Append(WriteField("", 50)); //39 "Pick up Name" len50
            sb.Append(WriteField("", 30)); //40 "Pick Up Street 1" len30
            sb.Append(WriteField("", 30)); //41 "Pick Up Street 2" len30
            sb.Append(WriteField("", 30)); //42 "Pick Up Street 3" len30
            sb.Append(WriteField("", 30)); //43 "Pick Up Town" len30
            sb.Append(WriteField("", 30)); //44 "Pick Up County" len30
            sb.Append(WriteField("", 9)); //45 "Pick Up Postcode" len9
            sb.Append(WriteField("", 3)); //46 "Pick Up Country" len3
            sb.Append(WriteField("", 20)); //47 "Pick Up VAT No" len20
            sb.Append(WriteField("", 22)); //48 "Pick Up Contact Name" len22
            sb.Append(WriteField("", 7)); //49 "Pick Up Phone Part 1" len7
            sb.Append(WriteField("", 9)); //50 "Pick Up Phone Part 2" len9
            sb.Append(WriteField("", 50)); //51 "Pick Up Email Address" len50
            sb.Append(WriteField("", 7)); //52 "Pick Up Fax Part 1" len7
            sb.Append(WriteField("", 9)); //53 "Pick Up Fax Part 2" len9
            sb.Append(WriteField("", 9)); //54 "Pick Up TNT Account No" len9
            sb.Append(WriteField("", 15)); //55 "Delivery Short Code" len15
            sb.Append(WriteField("", 50)); //56 "Delivery Name" len50
            sb.Append(WriteField("", 30)); //57 "Delivery Street 1" len30
            sb.Append(WriteField("", 30)); //58 "Delivery Street 2" len30
            sb.Append(WriteField("", 30)); //59 "Delivery Street 3" len30
            sb.Append(WriteField("", 30)); //60 "Delivery Town" len30
            sb.Append(WriteField("", 30)); //61 "Delivery County" len30
            sb.Append(WriteField("", 9)); //62 "Delivery Postcode" len9
            sb.Append(WriteField("", 3)); //63 "Delivery Country" len3
            sb.Append(WriteField("", 20)); //64 "Delivery VAT No" len20
            sb.Append(WriteField("", 22)); //65 "Delivery Contact Name" len22
            sb.Append(WriteField("", 7)); //66 "Delivery Phone Part 1" len7
            sb.Append(WriteField("", 9)); //67 "Delivery Phone Part 2" len9
            sb.Append(WriteField("", 50)); //68 "Delivery Email Address" len50
            sb.Append(WriteField("", 7)); //69 "Delivery Fax Part 1" len7
            sb.Append(WriteField("", 9)); //70 "Delivery Fax Part 2" len9
            sb.Append(WriteField("", 9)); //71 "Delivery TNT Account no" len9
            sb.Append(Environment.NewLine);
        }

        private string GrToKg(double grams)
        {
            return (grams / 1000).ToString();

        }

        private string Volume(double m3)
        {
            return m3.ToString("0.000");
        }
        
        private string CentsToEuro(int cents)
        {
            return ((double)cents / 100).ToString();
        }

        private int mmToCm(int mm)
        {
            int cm;
            cm = (int)(Math.Round((double)mm / 10, 0, MidpointRounding.AwayFromZero));
            if (cm == 0)
                return 1;
            else
                return cm;
        }

        private void AddRecordC(StringBuilder sb)
        {
            var countPacks = 0;

            foreach (var pc in Order.PackedContainerByContainerType)
            {
                countPacks++;
                //One record per package type in the consignment is required, e.g. for the following consignment package details two records are required:
                //2 x BOXES
                //1 x JIFFY BAG
                //First package record has a quantity (field 14) of 2, the second record has a quantity of 1. If the quantity field is left blank the default package quantity for the carrier will be used. (Carrier Maintenance – Import Defaults).
                sb.Append(WriteField("C", 1)); //1 "Record ID" len1
                sb.Append(WriteField("", 12)); //2 "Filler" len12
                sb.Append(WriteField(_consignmentRef, 24)); //3 "Consignment Reference" len24
                sb.Append(WriteField(countPacks, 2)); //4 "Package Sequence" len2
                sb.Append(WriteField(pc.Key.Name, 20)); //5 "Type Description" len20
                sb.Append(WriteField(Order.GetProductCountByContainerType(pc.Key), 4)); //6 "Total Article Count" len4
                sb.Append(WriteField("", 10)); //7 "Markings" len10
                sb.Append(WriteField(mmToCm(pc.Key.Height), 3)); //8 "Height" len3
                sb.Append(WriteField(mmToCm(pc.Key.Width), 3)); //9 "Width" len3
                sb.Append(WriteField(mmToCm(pc.Key.Depth), 3)); //10 "Depth" len3
                sb.Append(WriteField(Volume(pc.Key.VolumeInM3), 7, 3)); //11 "Volume" len7
                sb.Append(WriteField(GrToKg(pc.Value.Sum(pac => pac.Weight)), 8, 3)); //12 "Weight" len8
                sb.Append(WriteField("", 70)); //13 "Reference" len70
                sb.Append(WriteField("", 20)); // filler len20
                sb.Append(WriteField(pc.Value.Count, 3)); //14 "Quantity No" len3
                sb.Append(WriteField("", 10)); //15 "Package Code" len10
                sb.Append(WriteField("", 35)); //16 "Free Format 1" len35
                sb.Append(WriteField("", 35)); //17 "Free Format 2" len35
                sb.Append(WriteField("", 35)); //18 "Free Format 3" len35
                sb.Append(WriteField("", 35)); //19 "Free Format 4" len35
                sb.Append(WriteField("", 35)); //20 "Free Format 5" len35
                sb.Append(Environment.NewLine);

                int articleSequence = 0;
                foreach (var package in pc.Value)
                {
                    var orderLines = Order.GetOrderLinesByPackedContainer(package);
                    foreach (var ol in orderLines.Keys)
                    {
                        var packedOrderLine = orderLines[ol];
                        int weight = package.Weight;
                        double weightPerPart = ((double)weight / orderLines.Sum(line => line.Value.Qty));

                        articleSequence++;
                        AddRecordD(sb, countPacks, ol.PartName, packedOrderLine.Qty, ol.UnitPrice, articleSequence, weightPerPart);
                    }
                }
            }
        }

        private void AddRecordD(StringBuilder sb,
            int countPacks,
            string partName,
            int packedQty,
            int value,
            int articleSequence,
            double weightPerPart)
        {
            //This file describes the articles contained within the packages. There can be a number of article types within a package type.
            // The Package Sequence No (field 3) relates the article record back to the package type and the Article Sequence No (field 4) identifies the article type within the package.
            //Each package type record must have at least one article record.
            sb.Append(WriteField("D", 1)); //1 "Record ID" len1
            sb.Append(WriteField("", 12)); //2 "Filler" len12
            sb.Append(WriteField(_consignmentRef, 24)); //3 "Consignment Reference" len24
            sb.Append(WriteField(countPacks, 2)); //4 "Package Sequence No" len2
            sb.Append(WriteField(articleSequence, 2)); //5 "Article Sequence No" len2
            sb.Append(WriteField(partName, 30)); //6 "Description" len30
            sb.Append(WriteField(packedQty, 3)); //7 "Quantity No" len3
            sb.Append(WriteField(GrToKg(weightPerPart * packedQty), 8, 3)); //8 "Article Weight No" len8

            if (IsExport())
            {
                sb.Append(WriteField(CentsToEuro(value * packedQty), 11, 2)); //9 "Invoice Value" len11
                sb.Append(WriteField("BE", 3)); //10 "Origin Country" len3
            }
            else
            {
                sb.Append(WriteField("", 11)); //9 "Invoice Value" len11
                sb.Append(WriteField("", 3)); //10 "Origin Country" len3
            }

            sb.Append(WriteField("", 10)); //11 "Export Licence No" len10
            sb.Append(WriteField("", 15)); //12 "Tariff No" len15
            sb.Append(WriteField("", 30)); //13 "Hazard Code" len30

            if (IsExport())
            {
                sb.Append(WriteField(partName, 78)); //14 "Invoice Description 1" len78
            }
            else
            {
                sb.Append(WriteField("", 78)); //14 "Invoice Description 1" len78
            }


            sb.Append(WriteField("", 78)); //15 "Invoice Description 2" len78
            sb.Append(WriteField("", 78)); //16 "Invoice Description 3" len78
            sb.Append(WriteField("", 8)); //17 "Insurance Value" len8
            sb.Append(WriteField("", 10)); //18 "Article Code" len10
            sb.Append(WriteField("", 35)); //19 "Free Format 1" len35
            sb.Append(WriteField("", 4)); //20 "NAFTA" len4
            sb.Append(WriteField("", 5)); //21 "Package Group" len5
            sb.Append(WriteField("", 4)); //22 "UN Number" len4
            sb.Append(WriteField("", 4)); //23 "N" len4
            sb.Append(WriteField("", 30)); //24 "Certification of Origin Nr" len30
            sb.Append(WriteField("", 4)); //25 "Export Declaration Code" len4
            sb.Append(WriteField("", 13)); //26 "N" len13
            sb.Append(WriteField("", 30)); //27 "Export Declaration City" len30
            sb.Append(WriteField("", 8)); //28 "N" len8
            sb.Append(WriteField("", 4)); //29 "MRN Code" len4
            sb.Append(WriteField("", 15)); //30 "MRN Number" len15
            sb.Append(WriteField("", 30)); //31 "MRN City" len30
            sb.Append(WriteField("", 8)); //32 "MRN Raised Date" len8
            sb.Append(Environment.NewLine);
        }

        private static string WriteField(string value, int length)
        {
            if (value != null) value = value.Trim();
            return string.Format("{0,-" + length + "}", value) //left aligned with spaces
                .Substring(0, length); //if too long
        }

        private static string WriteField(int value, int length)
        {

            return string.Format("{0," + length + "}", value) //numbers are right aligned
                .Substring(0, length); //if too long
        }

        private static string WriteField(string value, int length, int dp)
        {
            return string.Format("{0," + length + "}", //numbers are right aligned
                Decimal.Parse(value).ToString("F" + dp)) //string as decimal with required number of dps
                .Substring(0, length); //if too long
        }

        private static string WriteField(decimal value, int length, int dp)
        {
            return string.Format("{0," + length + "}", //numbers are right aligned
                value.ToString("F" + dp)) //decimal with required number of dps
                .Substring(0, length); //if too long
        }

        #endregion

        public List<PrintFile> GetPrintFiles()
        {
            List<PrintFile> printfiles = new List<PrintFile>();
            foreach (string tntLabel in Order.TNTLabels)
            {
                string fileRef = Path.Combine(LabelStoragPath, tntLabel);
                printfiles.Add(new PrintFile() { Filename = fileRef });
            }
            return printfiles;
        }

        private string GetLabelFilePath()
        {
            string btwfilePath = Path.Combine(WebAppRoot, "Content/TNT/BTW/label.btw");

            if (!System.IO.File.Exists(btwfilePath))
            {
                throw new ApplicationException("Could not find the label template at " + btwfilePath);
            }
            return btwfilePath;
            
        }

        public bool Print(string printername)
        {
            var tntlabel = Order.TNTLabels.First();
            StreamReader sr = new StreamReader(tntlabel);
            string xml = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            bool ok = true;
            
            var responseobj = xml.ToObject<ShippingService.Business.CarrierServices.TNT.Label.Response.labelResponse>();

            var engine = new Engine(true);
            var format = engine.Documents.Open(GetLabelFilePath());
            format.PrintSetup.PrinterName = printername;

            if (responseobj.consignment == null)
            {
                throw new ApplicationException("no consignment found");
            }

            foreach (var consignment in responseobj.consignment)
            {

                var consignmentLabelData = consignment.consignmentLabelData;

                format.SubStrings["marketDisplay"].Value = consignmentLabelData.marketDisplay.Value;
                format.SubStrings["transportDisplay"].Value = consignmentLabelData.transportDisplay.Value;


                var ishazardous = consignmentLabelData.option != null && consignmentLabelData.option.FirstOrDefault(o => o.id == "HZ") != null;
                if (!ishazardous)
                    format.SubStrings["hazardous"].Value = string.Empty;

                format.SubStrings["xrayDisplay"].Value = consignmentLabelData.xrayDisplay.Value;
                format.SubStrings["freeCirculationDisplay"].Value = consignmentLabelData.freeCirculationDisplay.Value;
                format.SubStrings["sortSplitText"].Value = consignmentLabelData.sortSplitText;
                format.SubStrings["consignmentNumber"].Value = consignmentLabelData.consignmentNumber;
                format.SubStrings["service"].Value = consignmentLabelData.product.Value;


                string option = string.Empty;
                if (consignmentLabelData.option != null && consignmentLabelData.option.Count() > 1)
                {
                    foreach (var optionitem in consignmentLabelData.option)
                    {
                        option += optionitem.id + " ";
                    }
                }
                else if (consignmentLabelData.option != null && consignmentLabelData.option.Count() == 1)
                {
                    format.SubStrings["option"].Value = consignmentLabelData.option[0].Value;
                }
                format.SubStrings["option"].Value = option;
                format.SubStrings["accountNumber"].Value = consignmentLabelData.account.accountNumber;
                format.SubStrings["originDepotCode"].Value = consignmentLabelData.originDepot.depotCode;
                format.SubStrings["collectionDate"].Value = consignmentLabelData.collectionDate.ToString("dd MMM yyyy");
                format.SubStrings["deliveryAddress"].Value = consignmentLabelData.delivery.name;
                format.SubStrings["deliveryAddress"].Value += Environment.NewLine + consignmentLabelData.delivery.addressLine1;
                if (!string.IsNullOrEmpty(consignmentLabelData.delivery.addressLine2))
                    format.SubStrings["deliveryAddress"].Value += Environment.NewLine + consignmentLabelData.delivery.addressLine2;
                if (!string.IsNullOrEmpty(consignmentLabelData.delivery.addressLine3))
                    format.SubStrings["deliveryAddress"].Value += Environment.NewLine + consignmentLabelData.delivery.addressLine3;
                format.SubStrings["deliveryAddress"].Value += Environment.NewLine + consignmentLabelData.delivery.postcode + " " + consignmentLabelData.delivery.town;
                format.SubStrings["deliveryAddress"].Value += Environment.NewLine + consignmentLabelData.delivery.country;
                var routing = string.Empty;
                var sortDepotCode = string.Empty;
                foreach (var transitdepot in consignmentLabelData.transitDepots)
                {
                    if (transitdepot is ShippingService.Business.CarrierServices.TNT.Label.Response.sortDepotType)
                    {
                        routing += ((ShippingService.Business.CarrierServices.TNT.Label.Response.sortDepotType)transitdepot).depotCode + "-" + ((ShippingService.Business.CarrierServices.TNT.Label.Response.sortDepotType)transitdepot).sortCellIndicator.Value + Environment.NewLine;
                        sortDepotCode = ((ShippingService.Business.CarrierServices.TNT.Label.Response.sortDepotType)transitdepot).sortLocationCode;
                    }
                    if (transitdepot is ShippingService.Business.CarrierServices.TNT.Label.Response.actionDepotType)
                    {
                        routing += ((ShippingService.Business.CarrierServices.TNT.Label.Response.actionDepotType)transitdepot).depotCode + "-" + ((ShippingService.Business.CarrierServices.TNT.Label.Response.actionDepotType)transitdepot).actionDayOfWeek + Environment.NewLine;
                    }
                    if (transitdepot is ShippingService.Business.CarrierServices.TNT.Label.Response.depotType)
                    {
                        //this type has the name transitDepot
                        routing += ((ShippingService.Business.CarrierServices.TNT.Label.Response.depotType)transitdepot).depotCode + Environment.NewLine;
                    }
                }
                format.SubStrings["routing"].Value = routing;
                format.SubStrings["sortDepotCode"].Value = sortDepotCode;

                format.SubStrings["postcode"].Value = consignmentLabelData.clusterCode;
                format.SubStrings["destDepotCode"].Value = consignmentLabelData.destinationDepot.depotCode + "-" + consignmentLabelData.destinationDepot.dueDayOfMonth;

                foreach (var pieceLabelData in consignment.pieceLabelData)
                {
                    format.SubStrings["pieceData"].Value = pieceLabelData.pieceNumber + " of " + consignmentLabelData.totalNumberOfPieces;
                    var isWeightHighlighted = pieceLabelData.weightDisplay.renderInstructions == ShippingService.Business.CarrierServices.TNT.Label.Response.renderInstructionsTypeRenderInstructions.highlighted;
                    format.SubStrings["weight"].Value = isWeightHighlighted ? string.Empty : pieceLabelData.weightDisplay.Value;
                    format.SubStrings["weightHighlighted"].Value = isWeightHighlighted ? pieceLabelData.weightDisplay.Value : string.Empty;
                    format.SubStrings["customerReference"].Value = pieceLabelData.pieceReference;
                    format.SubStrings["barcode"].Value = pieceLabelData.barcode.Value;

                    var result = format.Print();
                    if(ok)
                    {
                        ok = result == Result.Success;
                    }

                }
            }
            engine.Stop(SaveOptions.DoNotSaveChanges);

            return ok;
        }
    }
}
