using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService;
using DataService.Repository;
using DataService.Service;
using DataService.ViewModel;
using Facebook;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class ShopController : Controller
    {
        ResponseService responseService = new ResponseService();
        EntityService entityService = new EntityService();
        ShopService shopService = new ShopService();
        IntentService intentService = new IntentService();
        FacebookClient fbApp = new FacebookClient();
        // GET: Shop
        public ActionResult Index()
        {
            string shopId = (string)Session["ShopId"];
            ShopViewModel shop = shopService.GetShop(shopId);
            fbApp.AccessToken = shop.FbToken;
            dynamic param = new ExpandoObject();
            param.fields = "single_line_address,phone,new_like_count,emails,website,about,description";
            param.locale = "vi_vi";
            try
            {
                dynamic result = fbApp.Get(shopId, param);
                if (HasProperty(result, "single_line_address"))
                {
                    ViewBag.Address = result.single_line_address;
                }
                if (HasProperty(result, "phone"))
                {
                    ViewBag.Phone = result.phone;
                }
                if (HasProperty(result, "new_like_count"))
                {
                    ViewBag.Like = result.new_like_count;
                }
                if (HasProperty(result, "emails"))
                {
                    ViewBag.Email = result.emails;
                }
                if (HasProperty(result, "website"))
                {
                    ViewBag.Website = result.website;
                }
                if (HasProperty(result, "about"))
                {
                    ViewBag.About = result.about;
                }
                if (HasProperty(result, "description"))
                {
                    ViewBag.Description = result.description;
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Facebook Exception: " + e.Message);
            }            
            return View();
        }

        public ActionResult Setting()
        {
            string shopId = (string) Session["ShopId"];
            ViewBag.CommentMode = shopService.GetCommentMode(shopId);
            ViewBag.ReplyMode = shopService.GetReplyMode(shopId);
            ViewBag.Entities = entityService.GetAll(shopId);
            ViewBag.Responses = responseService.GetAll(shopId);
            ViewBag.Intents = intentService.GetAllIntent();
            return View();
        }

        public JsonResult GetAllResponse()
        {
            string shopId = (string)Session["ShopId"];
            return Json(responseService.GetAll(shopId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllEntity()
        {
            string shopId = (string)Session["ShopId"];
            return Json(entityService.GetAll(shopId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetEntity(int id, string name, string value, string description)
        {
            try
            {
                bool result = entityService.SetEntity(id, name, value, description);
                return Json(new { success = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }            
            
        }

        public JsonResult AddEntity(string name, string value, string description)
        {
            string shopId = (string)Session["ShopId"];
            try
            {
                int result = entityService.AddEntity(name, value, true, description, shopId);
                return Json(new { id = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { id = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult DeleteEntity(int id)
        {
            //string shopId = (string)Session["shopId"];
            bool result = entityService.DeleteEntity(id);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetReplyMode(int mode)
        {
            string shopId = (string)Session["shopId"];
            bool result = shopService.SetReplyMode(shopId, mode);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetCommentMode(int mode)
        {
            string shopId = (string)Session["shopId"];
            bool result = shopService.SetCommentMode(shopId, mode);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditResponse(int id, int intent, string content)
        {
            //string shopId = (string)Session["shopId"];
            bool result = responseService.EditResponse(id, intent, content);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddResponse(int intent, string content)
        {
            string shopId = (string)Session["shopId"];
            int result = responseService.SetResponse(shopId, intent, content);
            return Json(new { id = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteResponse(int id)
        {
            //string shopId = (string)Session["shopId"];
            bool result = responseService.DeleteResponse(id);
            return Json(new { success = result }, JsonRequestBehavior.AllowGet);
        }

        private bool HasProperty(dynamic obj, string name)
        {            
            try
            {
                var prop = obj[name];
                return prop != null;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Name = " + name);
                return false;
            }
        }

    }
}