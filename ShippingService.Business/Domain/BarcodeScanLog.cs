using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShippingService.Business.Domain
{
    public class BarcodeScanLog
    {
        public string UserName { get; set; }
        public DateTime CreatedOn  { get; set; }
        public string PickSlipScan { get; set; }
        public string BoxScan { get; set; }
        public bool Success { get; set; }
    }
}
