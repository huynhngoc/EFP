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
using System.Configuration;

namespace ShopManager.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ShopOwner")]
        public ActionResult ChooseShop()
        {
            ViewBag.Title = "Chọn cửa hàng";
            return View();
        }

        [Authorize(Roles = "ShopOwner")]
        public JsonResult GetShop()
        {
            ShopService service = new ShopService();
            string userId = User.Identity.GetUserId();
            return Json(service.GetShopByUserId(userId));
        }

        [Authorize(Roles = "ShopOwner")]
        public JsonResult AddShop(string id, string token)
        {
            ShopService shopService = new ShopService();
            var shop = shopService.GetShop(id) == null;
            if (shop)
            {
                FacebookClient fbApp = new FacebookClient(token);
                dynamic parameters = new ExpandoObject();
                //parameters.access_token = token;
                parameters.app_id = ConfigurationManager.AppSettings["FbAppId"];


                // add tab
                Facebook.JsonObject fbResult = fbApp.Post(id + "/tabs", parameters);

                // add webhook
                var fbWebhook = fbApp.Post(id + "/subscribed_apps", parameters);

                //get long-lived 
                dynamic getAccessParam = new ExpandoObject();
                getAccessParam.grant_type = "fb_exchange_token";
                getAccessParam.client_id = ConfigurationManager.AppSettings["FbAppId"];
                getAccessParam.client_secret = ConfigurationManager.AppSettings["FbAppSecret"];
                getAccessParam.fb_exchange_token = token;

                var fbToken = fbApp.Get("oauth/access_token", getAccessParam);
                string longToken = fbToken["access_token"];

                //get name
                dynamic nameParam = new ExpandoObject();
                nameParam.fields = "name";
                var fbName = fbApp.Get(id, nameParam);
                string name = fbName["name"];

                string userId = User.Identity.GetUserId();
                bool result = false;
                if (shopService.GetShop(id) == null)
                {
                    shopService.CreateShop(id, name, longToken, userId);

                    EntityService entityService = new EntityService();
                    entityService.InitEntity(id);
                    result = true;
                }


                return Json(new { addTab = fbResult, subscribe = fbWebhook, longtoken = longToken, name = name, result = result });
            } else
            {
                var result = shopService.CreateConnection(id, User.Identity.GetUserId());
                return Json(new { result = result });
            }

            
        }

        [Authorize(Roles = "ShopOwner")]
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