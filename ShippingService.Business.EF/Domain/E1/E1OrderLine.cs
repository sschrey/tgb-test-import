﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Domain.E1
{
    public class E1OrderLine: EntityWithTypedId<string>
    {
        public double OrderNumber { get; set; }
        public double LineNumber { get; set; }
        public string Status { get; set; }
        public string CaseNumber { get; set; }
        public string PartNumber { get; set; }
        public double PartWeight { get; set; }
        public double Quantity { get; set; }
    }
}
