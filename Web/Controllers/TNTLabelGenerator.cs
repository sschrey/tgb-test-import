using ShippingService.Business.EF.Facade;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Tweddle.Commons;

namespace Web.Controllers
{
    public class TNTLabelGenerator
    {
        
        private string executableFilePath;


        public TNTLabelGenerator()
        {
            
            executableFilePath = ConfigurationManager.AppSettings["FOPExecutableFilePath"];
        }

        public Validation GeneratePDF(string xslFilePath, string xmlFilePath, string pdfFilePath, string barcodefilePath)
        {
            Validation val = new Validation();
            string myparams = string.Format("-param {0} {1}", "barcode_url", barcodefilePath);
            string workingdir = Path.GetDirectoryName(xslFilePath);

            CommandObj co = new CommandObj();
            string args = string.Format("-xsl {0} -xml {1} -pdf {2} {3}", xslFilePath, xmlFilePath, pdfFilePath, myparams);

            var result = co.Run(executableFilePath, args, null, workingdir, 0, System.Diagnostics.ProcessPriorityClass.BelowNormal);

            if (!File.Exists(pdfFilePath))
            {
                val.AddBrokenRule(executableFilePath + " " + args + " failed to create pdf");
                foreach(var error in result.Errors)
                {
                    val.AddBrokenRule(error);
                }
                foreach (var output in result.Output)
                {
                    val.AddBrokenRule(output);
                }
            }

            return val;
        }

        
    }
}