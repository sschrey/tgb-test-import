using Seagull.BarTender.Print;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bartender
{
    public class Facade
    {
        public List<Printer> GetPrinters()
        {
            Printers printers = new Printers();

            List<Printer> retprinters = new List<Printer>();
            foreach(var printer in printers)
            {
                retprinters.Add(new Printer() { Name = printer.PrinterName });
            }

            return retprinters;
        }
        
    }
}
