using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers
{
    public class VMOrderLine
    {
        public string PartNumber { get; set; }
        public string Quantity { get; set; }

        public string ContainerName { get; set; }

        public string TrackingNumber { get; set; }
       

    }
}