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

        [Authorize]
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
            //parameters.access_token = token;
            parameters.app_id = "161282120980764";
            

            // add tab
            Facebook.JsonObject fbResult = fbApp.Post(id + "/tabs", parameters);

            // add webhook
            var fbWebhook = fbApp.Post(id + "/subscribed_apps", parameters);

            //get long-lived 
            dynamic getAccessParam = new ExpandoObject();
            getAccessParam.grant_type = "fb_exchange_token";
            getAccessParam.client_id = "161282120980764";
            getAccessParam.client_secret = "40d95140ed2f41994dffa498bf62bb4c";
            getAccessParam.fb_exchange_token = token;

            var fbToken = fbApp.Get("oauth/access_token", getAccessParam);
            string longToken = fbToken["access_token"];            

            //get name
            dynamic nameParam = new ExpandoObject();
            nameParam.field = "name";
            var fbName = fbApp.Get(id, nameParam);
            string name = fbName["name"];

            string userId = User.Identity.GetUserId();            
            ShopService shopService = new ShopService();
            bool result = false;
            if (shopService.GetShopByUserId(userId) == null)
            {
                shopService.CreateShop(id, name, longToken, userId);

                EntityService entityService = new EntityService();
                entityService.InitEntity(id);
                result = true;
            }            


            return Json(new { addTab = fbResult, subscribe = fbWebhook, longtoken = longToken, name = name, result = result });
        }

        [Authorize]
        public ActionResult GoToShop(string shopId)
        {
            ShopService service = new ShopService();
            if (service.CheckShopUser(shopId, User.Identity.GetUserId()))
            {
                Session["ShopId"] = shopId;
                return RedirectToAction("Index", "Shop");
            }            
            else
            {
                ViewBag.ErrorMessage = "Cửa hàng này không do bạn quản lý";
                return RedirectToAction("ChooseShop", "Home");
            }
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