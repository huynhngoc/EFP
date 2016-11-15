using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Repository;
using DataService.Service;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class ShopController : Controller
    {
        ResponseService responseService = new ResponseService();
        EntityService entityService = new EntityService();
        ShopService shopService = new ShopService();
        IntentService intentService = new IntentService();
        // GET: Shop
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Setting()
        {
            string shopId = (string) Session["shopId"];
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

        //Long
        public JsonResult GetReplyMode()
        {
            string shopId = (string)Session["shopId"];
            int result = shopService.GetReplyMode(shopId);
            return Json(new { mode = result }, JsonRequestBehavior.AllowGet);
        }
    }
}