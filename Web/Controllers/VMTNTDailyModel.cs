using ShippingService.Business.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Controllers
{
    public class VMTNTDailyModel
    {
        public VMTNTDailyModel()
        {
            Orders = new List<VMOrder>();
        }
        public List<VMOrder> Orders { get; set; }
        public string SelectedCarrier { get; set; }
        public IList<Carrier> Carriers { get; set; }
        public string CurrentDate { get; set; }

    }
}