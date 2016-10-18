using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.JqueryDataTable;
using DataService.Service;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Facebook;
using System.Dynamic;

namespace ShopManager.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ChooseShop()
        {
            return View();
        }

        [Authorize]
        public JsonResult GetShop()
        {
            ShopService service = new ShopService();
            string userId = User.Identity.GetUserId();
            return Json(service.GetShopByUserId(userId));
        }

        [Authorize]
        public JsonResult AddShop(string id, string token)
        {
            FacebookClient fbApp = new FacebookClient(token);
            dynamic parameters = new ExpandoObject();
            parameters.access_token = token;
            parameters.app_id = "161282120980764";
            var fbResult = fbApp.Post(id + "/tabs", parameters);

            return Json(fbResult);
        }


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