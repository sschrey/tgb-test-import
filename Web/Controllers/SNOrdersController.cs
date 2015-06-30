using ShippingService.Business.EF.Domain.E1;
using ShippingService.Business.EF.Facade;
using ShippingService.Business.EF.Facade.E1;
using ShippingService.Business.EF.Facade.SNOrders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class SNOrdersController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BarcodeScan(string orderid)
        {
            IE1Facade facade = null;

            if (IsTestData)
                facade = new E1TestFacade();
            else
                facade = FacadeFactory.GetInstance().GetFacade<E1Facade>();

            if(!string.IsNullOrWhiteSpace(orderid))
            {
                float orderidasfloat;
                if(float.TryParse(orderid, out orderidasfloat))
                {
                    VMPackData data = new VMPackData();
                    data.OrderLines = new List<VMOrderLine>();
                    var orderlines = facade.GetOrderLines(orderidasfloat);
                    foreach(var orderline in orderlines)
                    {
                        data.OrderLines.Add(new VMOrderLine()
                        {
                            CaseNumber = orderline.CaseNumber,
                            Id = orderline.Id.ToString(),
                            OrderNumber = orderline.OrderNumber.ToString(),
                            PackingData = new List<VMPackingData>(),
                            PartNumber = orderline.PartNumber,
                            PartWeight = orderline.PartWeight.ToString(),
                            Quantity = orderline.Quantity,
                            RequestQuantity = orderline.Quantity.ToString()
                        });
                    }

                    
                    data.Cartons = facade.GetCartons();
                    data.OrderId = orderid;

                    return View("Pack", data);
                }
            }
            return View("Index");
        }

        public ActionResult Pack(List<VMOrderLine> orderlines, E1Carton carton, List<VMPackedContainer> containers)
        {
            Validation val = new Validation();
            VMPackedContainer container = null;

            if (containers == null)
                containers = new List<VMPackedContainer>();

            if(carton == null || carton.Id == null)
            {
                val.AddBrokenRule("Please choose a carton");
            }
            
            if(val.IsValid)
            {
                foreach(var orderline in orderlines)
                {
                    int quantity = 0;
                    if (int.TryParse(orderline.RequestQuantity, out quantity))
                    {
                        if (quantity > 0)
                        {
                            var packedQuantity = orderline.PackingData.Sum(p => p.Quantity);
                            if(packedQuantity + quantity > orderline.Quantity)
                            {
                                val.AddBrokenRule("You requested too much for line " + orderline.Id);
                            }
                            else
                            { 
                                orderline.PackingData.Add(new VMPackingData()
                                {
                                    CartonId = carton.Id,
                                    CartonName = carton.Name,
                                    Quantity = quantity
                                });

                                if(container == null)
                                {
                                    container = new VMPackedContainer();
                                    container.OrderNumber = orderline.OrderNumber;
                                    container.Carton = carton.Name;
                                    container.CartonWeight = carton.Weight.ToString();
                                    container.CaseNumber = orderline.CaseNumber;
                                    container.Id = (containers.Count() + 1).ToString();
                                }
                                container.PackedParts.Add(new VMPackedParts()
                                {
                                    PartNumber = orderline.PartNumber,
                                    PartWeight = orderline.PartWeight,
                                    Quantity = quantity.ToString()
                                });
                                container.Count++;
                            }
                       }
                        orderline.RequestQuantity = (orderline.Quantity - orderline.PackingData.Sum(p => p.Quantity)).ToString();
                    }
                }
            }

            if(container != null)
            {
                containers.Add(container);
            }

            var returnObject = new
            {
                OrderLines = orderlines,
                Errors = val.BrokenRules,
                Containers = containers
            };

            return Json(returnObject);
        }

        public bool IsTestData
        {
            get
            {
                string enabletestdata = ConfigurationManager.AppSettings["EnableTestData"];
                
                bool isTestData = false;
                if(Boolean.TryParse(enabletestdata, out isTestData))
                {
                    return isTestData;
                }
                return isTestData;
            }
        }
	}
}
