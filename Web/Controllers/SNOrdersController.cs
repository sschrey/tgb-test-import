﻿using ShippingService.Business.EF.Domain.E1;
using ShippingService.Business.EF.Domain.SNOrders;
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

            var orderfacade = FacadeFactory.GetInstance().GetFacade<SNOrderFacade>();

            VMPack data = orderfacade.Barcodescan(orderid, facade);

            if(data.OrderLines.Count==0)
                return View("Index");
            else
               return View("Pack", data);
        }

        public ActionResult Pack(VMPack data)
        {
            var facade = FacadeFactory.GetInstance().GetFacade<SNOrderFacade>();
            data = facade.Pack(data);

            return Json(data);
        }

        public ActionResult Save(VMPack data)
        {
            var facade = FacadeFactory.GetInstance().GetFacade<SNOrderFacade>();
            Validation val = facade.Save(data);

            return Json(data);
        }
        public ActionResult Search()
        {
            return View(new VMSearch());
        }

        [HttpPost]
        public ActionResult Search(VMSearch data)
        {
            var facade = FacadeFactory.GetInstance().GetFacade<SNOrderFacade>();

            var pcs = facade.GetAll<SNPackedContainer>();

            DateTime from;
            if(DateTime.TryParse(data.From, out from))
            {
                pcs = pcs.Where(pc => pc.CreatedOn > from);
            }

            DateTime to;
            if (DateTime.TryParse(data.To, out to))
            {
                to = to.AddDays(1);
                pcs = pcs.Where(pc => pc.CreatedOn < to);
            }

            var containers = pcs.ToList();
            data.Containers = new List<VMPackedContainer>();
            foreach (var container in containers)
            {
                data.Containers.Add(new VMPackedContainer(container));
            }

            return Json(data);
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
