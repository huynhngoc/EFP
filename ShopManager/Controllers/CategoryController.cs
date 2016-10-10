using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;

namespace ShopManager.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult All(string shopId)
        {
            CategoryService service = new CategoryService();
            return Json(service.GetAllCategory(shopId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int id, string name, string description)
        {
            CategoryService service = new CategoryService();
            return Json(service.EditCategory(id, name, description), JsonRequestBehavior.AllowGet);
        }
    }
}