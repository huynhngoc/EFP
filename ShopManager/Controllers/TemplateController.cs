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
            return Json(service.GetTemplate(shopId), JsonRequestBehavior.AllowGet);
        }
    }
}