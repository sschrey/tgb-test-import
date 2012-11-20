using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ShippingService.Shared;

namespace Web.Services.TIM.Inventory
{
    /// <summary>
    /// 
    /// </summary>
    public class Download : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var list = ApplicationContextHolder.Instance.Facade.GetTIMInventory();

            context.Response.ContentType = "application/octet-stream";
            context.Response.AppendHeader("Content-Disposition",
                                 "attachment; filename=inventory.zip");

            ShippingService.Shared.Facade f = new Facade();
            f.SendTimInventory(list, context.Response.OutputStream);
            
            context.Response.End();
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
