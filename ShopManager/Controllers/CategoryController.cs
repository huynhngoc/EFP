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
    }
}