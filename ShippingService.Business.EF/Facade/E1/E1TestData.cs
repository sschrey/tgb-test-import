using ShippingService.Business.EF.Domain.E1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Business.EF.Facade.E1
{
    public static class E1TestData
    {
        public static List<E1Carton> GetE1Cartons()
        {
            return new List<E1Carton>()
            {
                new E1Carton()
                {
                    Id = "CRTV8",
                    Name = "CARTON 25 x 25 x 10",
                    Weight = 0.2,
                    WeightUOM = "KG"
                },
                 new E1Carton()
                {
                    Id = "CRTV7",
                    Name = "Carton 250x180x140 ",
                    Weight = 0.2,
                    WeightUOM = "KG"
                }
            };
        }
        public static List<E1OrderLine> GetE1OrderLines()
        {
           
            return new List<E1OrderLine>()
            {
                new E1OrderLine()
                {
                        Id = "2081900-1000",
                        OrderNumber = 2081900,
                        CaseNumber = "84530944",
                        PartNumber = "PZ471P1129PA",
                        Quantity = 1,
                        PartWeight = 250,
                        LineNumber = 1000
                },
                new E1OrderLine()
                {
                        Id = "2081900-2000",
                        OrderNumber = 2081900,
                        CaseNumber = "84530944",
                        PartNumber = "PZ471Y0501PA",
                        Quantity = 14,
                        PartWeight = 500,
                        LineNumber = 2000
                },
                new E1OrderLine()
                {
                        Id = "2081900-3000",
                        OrderNumber = 2081900,
                        CaseNumber = "84530967",
                        PartNumber = "PZ485P0513NL",
                        Quantity = 5,
                        PartWeight = 750,
                        LineNumber = 3000
                }
            };
           
        }
    }
}
