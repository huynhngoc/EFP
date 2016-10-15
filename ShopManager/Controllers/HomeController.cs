using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;
namespace ShopManager.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectToAction("Customer", "Customer", new { area = "" });
            return RedirectToAction("Index", "Customer", new { area = "" });
        }

        //public ActionResult GetAllCustomer()
        //{
        //    CustomerService service = new CustomerService();
        //    var data = service.GetAllCustomer();
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Test(string id)
        {
            CategoryService service = new CategoryService();
            var data = service.CategoryByShop(id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}