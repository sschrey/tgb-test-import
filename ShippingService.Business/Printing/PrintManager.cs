using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace ShippingService.Business.Printing
{
    /// <summary>
    /// Summary description for PrintManager
    /// </summary>
    public static class PrintManager
    {
        public static void Print(string imagePath, string printerName)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("No image file found");

            var pd = new PrintDocument();
            pd.DocumentName = Path.GetFileName(imagePath);
            pd.PrinterSettings.PrinterName = printerName;
            if (!pd.PrinterSettings.IsValid)
                throw new ApplicationException("Printer Name is not valid");
            pd.DefaultPageSettings.Landscape = true;
            //label is 6 x 4 (in 100th of an inch)
            pd.DefaultPageSettings.PaperSize = new PaperSize("Label", 600, 400);
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            pd.OriginAtMargins = true;
            pd.PrintPage += delegate(object sender, PrintPageEventArgs e)
            {
                var g = e.Graphics;
                var img = Image.FromFile(imagePath);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                //width is 4 inches * dots per inch
                int width = (int)(4 * g.DpiX * 0.86);
                //height is 6 inches * dots per inch
                int height = (int)(6 * g.DpiY * 0.33);

                g.DrawImage(img,
                    pd.DefaultPageSettings.HardMarginX,
                    pd.DefaultPageSettings.HardMarginY,
                    width, height);
            };
            pd.Print();
        }

        public static IEnumerable<string> InstalledPrinters()
        {
            return PrinterSettings.InstalledPrinters.OfType<string>();
        }

        /// <summary>
        /// Prints the PDF.
        /// </summary>
        /// <param name="ghostScriptPath">The ghost script path. Eg "C:\Program Files\gs\gs8.71\bin\gswin32c.exe"</param>
        /// <param name="numberOfCopies">The number of copies.</param>
        /// <param name="printerName">Name of the printer. Eg \\server_name\printer_name</param>
        /// <param name="pdfFileName">Name of the PDF file.</param>
        /// <returns></returns>
        public static PrintPDFResults PrintPDF(string ghostScriptPath, int numberOfCopies, string printerName, string pdfFileName)
        {
            PrintPDFResults results = new PrintPDFResults();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Arguments = " -dPrinted -dBATCH -dNOPAUSE -dNOSAFER -q -dNumCopies=" + Convert.ToString(numberOfCopies) + " -sDEVICE=ljet4 -sOutputFile=\"\\\\spool\\" + printerName + "\" \"" + pdfFileName + "\" ";
            startInfo.FileName = ghostScriptPath;
            startInfo.UseShellExecute = false;

            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;

            Process process = Process.Start(startInfo);

            Console.WriteLine(process.StandardError.ReadToEnd() + process.StandardOutput.ReadToEnd());

            process.WaitForExit(30000);
            if (process.HasExited == false) process.Kill();

            results.ExitCode = process.ExitCode;
            results.Results = process.StandardError.ReadToEnd() + process.StandardOutput.ReadToEnd();

            return results;
        }

        public class PrintPDFResults
        {
            public int ExitCode { get; set; }
            public string Results { get; set; }

        }
    }
}
