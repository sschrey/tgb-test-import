using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.CarrierServices
{
    public class ShippingTNTPriceInfo
    {
        
            public string City { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
            public string TotalVolume { get; set; }
            public int Pkgs { get; set; }
            public string Weight { get; set; }
            public string TNTLogin { get; set; }
            public string TNTPass { get; set; }
            public string ShipFromCity { get; set; }
            public string ShipFromCountry { get; set; }
            public string ShipFromPostal { get; set; }
            public string ShipFromCompany { get; set; }
            public string ShipFromAttention { get; set; }
            public string ShipFromPhone { get; set; }
            public string ShipFromAddress { get; set; }

            public string ToXml()
            {
                var sb = new StringBuilder();
                sb.AppendLine("<?xml version=\"1.0\" standalone=\"no\"?><!DOCTYPE PRICEREQUEST SYSTEM 'http://164.39.41.88:81/PriceCheckerDTD1.0/PriceRequestIN.dtd'>");
                sb.AppendLine("<PRICEREQUEST>");
                sb.AppendLine("<LOGIN><COMPANY>" + TNTLogin + "</COMPANY><PASSWORD>" + TNTPass +
                         "</PASSWORD><APPID>PC</APPID></LOGIN>");
                sb.AppendLine("<DATASETS><COUNTRY>1.2</COUNTRY><CURRENCY>1.1</CURRENCY><POSTCODEMASK>1.1</POSTCODEMASK><TOWNGROUP>1.1</TOWNGROUP><SERVICE>1.2</SERVICE><OPTION>1.1</OPTION></DATASETS>");
                sb.AppendLine("<PRICECHECK><RATEID>rate1</RATEID>");
                sb.AppendLine("<ORIGINCOUNTRY>" + ShipFromCountry + "</ORIGINCOUNTRY><ORIGINTOWNNAME>" + ShipFromCity +
                         "</ORIGINTOWNNAME><ORIGINPOSTCODE>" + ShipFromPostal +
                         "</ORIGINPOSTCODE><ORIGINTOWNGROUP></ORIGINTOWNGROUP>");
                sb.AppendLine("<DESTCOUNTRY>" + Country + "</DESTCOUNTRY><DESTTOWNNAME>" + City +
                         "</DESTTOWNNAME><DESTPOSTCODE>" + PostalCode + "</DESTPOSTCODE><DESTTOWNGROUP></DESTTOWNGROUP>");
                sb.AppendLine("<CONTYPE>N</CONTYPE><CURRENCY>EUR</CURRENCY><WEIGHT>" + Weight + "</WEIGHT><VOLUME>" +
                         TotalVolume + "</VOLUME><ACCOUNT>000066104</ACCOUNT><ITEMS>" + Pkgs + "</ITEMS></PRICECHECK>");
                sb.AppendLine("</PRICEREQUEST>");
                return sb.ToString();
            }

            public override string ToString()
            {
                return "Packs=" + Pkgs + " weight=" + Weight + " Vol=" + TotalVolume;
            }
    }
}
