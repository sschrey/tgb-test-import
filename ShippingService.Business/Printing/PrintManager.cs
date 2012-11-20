using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.IO;

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
    }
}
