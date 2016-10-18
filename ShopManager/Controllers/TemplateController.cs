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

        public JsonResult AddTemplate(string name, string[] attr)
        {
            if (attr.Length < 7)
            {                
                Array.Resize(ref attr, 7);                

            }
            TemplateProductService service = new TemplateProductService();
            return Json(service.AddTemplate(name, attr, "1"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTemplate(int id, string[] attr)
        {
            if (attr.Length < 7)
            {
                Array.Resize(ref attr, 7);

            }
            TemplateProductService service = new TemplateProductService();
            return Json(service.UpdateTemplate(id,attr, "1"), JsonRequestBehavior.AllowGet);
        }
    }
}