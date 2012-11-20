using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.CarrierServices
{
    public class TNTErrorCode
    {
        private string code;
        private string desc;

        public static string GetDescByCode(string code)
        {
            TNTErrorCode tntcode = All.FirstOrDefault(c => c.code == code);
            if (tntcode != null)
                return tntcode.desc;
            else
                return string.Empty;

        }

        public static List<TNTErrorCode> All
        {
            get
            {
                List<TNTErrorCode> all = new List<TNTErrorCode>();

                all.Add(new TNTErrorCode() { code = "G1", desc = "Error G1 - Login To TNT Rate Checker Failed - Try Again" });
                all.Add(new TNTErrorCode() { code = "G2", desc = "Error G2 - Error Checking Dataset Versions -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "G3", desc = "Error G3 - No Dataset Request Element Found -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "G4", desc = "Error G4 - No Dataset Version Information found -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P1", desc = "Error P1 - Failure To format incomming XML String -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P10", desc = "Error P10 - Invalid Account Number -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P11", desc = "Error P11 - Invalid Country Code -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P12", desc = "Error P12 - Invalid Town Group -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P14", desc = "Error P14 - Domestic Rating Not avaliable -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P15", desc = "Error P15 - Insurance Value must be numeric -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P16", desc = "Error P16 - Service required with options -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P17", desc = "Error P17 - Unable to retreive service/options -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P2", desc = "Error P2 - Failure To Specify available services -- Try Again" });
                all.Add(new TNTErrorCode() { code = "P3", desc = "Error P3 - Failure To Specify available Options  -- Try Again" });
                all.Add(new TNTErrorCode() { code = "P4", desc = "Error P4 - Failure To Specify available service/options -- Try Again" });
                all.Add(new TNTErrorCode() { code = "P5", desc = "Error P5 - Price check Failure - Timeout on Database -- Try Again" });
                all.Add(new TNTErrorCode() { code = "P6", desc = "Error P6 - Price check Failure - Error On Server -- Try Again" });
                all.Add(new TNTErrorCode() { code = "P7", desc = "Error P7 - Price Check Failure - No Rates Returned" });
                all.Add(new TNTErrorCode() { code = "P8", desc = "Error P8 - Field is missing -- PostalCode/Country/Town/Weight/ OR Volume -- Contact MIS" });
                all.Add(new TNTErrorCode() { code = "P9", desc = "Error P9 - No Price Requests Found -- Contact MIS" });

                return all;
            }
        }	
	
    }
}
