using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;

namespace ShopManager.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ShopService shopService = new ShopService();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShopUser()
        {
            ViewBag.Shops = shopService.GetAll();
            return View();
        }

        public JsonResult SetActive(string shopId, string userId, bool isActive)
        {
            return Json(shopService.SetActive(shopId, userId, isActive), JsonRequestBehavior.AllowGet);
        }
    }
}