﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult All(string shopId)
        {
            shopId = (string)Session["ShopId"];
            TemplateProductService service = new TemplateProductService();
            return Json(new { aaData = service.GetTemplate(shopId) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTemplateById(int id)
        {
            TemplateProductService service = new TemplateProductService();
            return Json(service.GetTemplateById(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddTemplate(string name, string[] attr)
        {
            var shopId = (string)Session["ShopId"];
            if (attr.Length < 7)
            {                
                Array.Resize(ref attr, 7);                

            }
            TemplateProductService service = new TemplateProductService();
            return Json(service.AddTemplate(name, attr, shopId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTemplate(int id, string name, string[] attr)
        {
            if (attr.Length < 7)
            {
                Array.Resize(ref attr, 7);

            }
            TemplateProductService service = new TemplateProductService();
            return Json(service.UpdateTemplate(id,name, attr), JsonRequestBehavior.AllowGet);
        }
    }
}