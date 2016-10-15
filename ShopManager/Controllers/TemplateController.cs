using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;

namespace ShopManager.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult All(string shopId)
        {
            TemplateProductService service = new TemplateProductService();
            return Json(new { aaData = service.GetTemplate(shopId) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTemplateById(int id)
        {
            TemplateProductService service = new TemplateProductService();
            return Json(service.GetTemplateById(id), JsonRequestBehavior.AllowGet);
        }
    }
}