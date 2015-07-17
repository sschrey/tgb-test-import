using ShippingService.Business.EF.Domain.E1;
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
            return View(new VMBarcodeScan());
        }

        public ActionResult BarcodeScan(VMBarcodeScan scan)
        {
            IE1Facade facade = null;

            if (IsTestData)
                facade = new E1TestFacade();
            else
                facade = FacadeFactory.GetInstance().GetFacade<E1Facade>();

            var orderfacade = FacadeFactory.GetInstance().GetFacade<SNOrderFacade>();

            VMPack packdata = orderfacade.Barcodescan(scan, facade);

            if (packdata == null)
                return View("Index", scan);
            else
                return View("Pack", packdata);
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
            Validation val = facade.Save(data, this.User.Identity.Name);

            return Json(data);
        }
        public ActionResult Search()
        {
            return View(new VMSearch());
        }

        [HttpPost]
        public ActionResult Unpack(VMUnpack data)
        {
            var facade = FacadeFactory.GetInstance().GetFacade<SNOrderFacade>();
            facade.Unpack(data);
            return Json(data);
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

            var containers = pcs
                .OrderBy(pc => pc.OrderId).ThenBy(pc => pc.CaseNumber).ToList();
                
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
