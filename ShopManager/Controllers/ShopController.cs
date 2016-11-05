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

        public JsonResult SetEntity(int id, string name, string value)
        {
            try
            {
                bool result = entityService.SetEntity(id, name, value);
                return Json(new { success = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }            
            
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

    }
}