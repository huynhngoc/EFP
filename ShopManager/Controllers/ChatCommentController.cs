using DataService;
using DataService.Service;
using DataService.ViewModel;
using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class ChatCommentController : Controller
    {
        ConversationService conversationService = new ConversationService();
        ShopService shopService = new ShopService();
        CustomerService custService = new CustomerService();
        FacebookClient fbApp = new FacebookClient();

        public ActionResult Index()
        {
            ViewData["MessageNext"] = "null";
            return View();
        }

        public ActionResult GetConversationPreviews()
        {
            var listConversationPreview = new List<ConversationPreviewViewModel>();
            string shopId = (string)Session["ShopId"];

            var listConversation = conversationService.GetConversationsByShopId(shopId);


            //calling fb
            string accessToken = shopService.GetShop(shopId).FbToken;

            foreach (var c in listConversation)
            {

                var preview = GetConversationPreviewFromFb(accessToken, c);

                Console.WriteLine("Preview: " + preview.ThreadId + " - time: " + preview.CreatedTime + " - Avatar: " + preview.AvatarUrl);

                listConversationPreview.Add(preview);
            }

            return Json(listConversationPreview, JsonRequestBehavior.AllowGet);
        }

        private ConversationPreviewViewModel GetConversationPreviewFromFb(string accessToken, Conversation c)
        {
            string shopId = (string)Session["ShopId"];
            var preview = new ConversationPreviewViewModel();
            preview.ThreadId = c.Id;
            preview.IsRead = c.IsRead;

            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            param.fields = "from,created_time,message";
            dynamic result = fbApp.Get(c.Id + "/messages", param);

            int i = 0;
            dynamic first = result.data[0];
            dynamic detail;
            while (true)
            {
                if (result.data[i].from.id != shopId)
                {
                    detail = result.data[i];
                    break;
                }
                i++;
            }

            preview.UserName = detail.from.name;
            preview.UserFbId = detail.from.id;
            preview.CreatedTime = DateTime.Parse(first.created_time);
            preview.RecentMess = first.message;

            param = new ExpandoObject();
            param.access_token = accessToken;
            param.fields = "";
            param.limit = 1;
            string url = preview.UserFbId + "/picture?type=normal&redirect=false";
            dynamic result2 = fbApp.Get(url, param);
            preview.AvatarUrl = result2.data.url;

            return preview;
        }

        public ActionResult GetUserFromDb(string userFbId)
        {
            string shopId = (string)Session["ShopId"];
            Customer cust = custService.GetCustomerByFacebookId(userFbId, shopId);
            if (cust == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(cust, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetConversationContent(string conversationId)
        {
            var conversationContent = new ConversationContentViewModel();
            string shopId = (string)Session["ShopId"];
            string accessToken = shopService.GetShop(shopId).FbToken;

            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            param.fields = "from,created_time,message";
            dynamic result = fbApp.Get(conversationId + "/messages", param);

            foreach (var mess in result.data)
            {
                var mc = new MessageContentViewModel();
                mc.MessId = mess.id;
                mc.MessContent = mess.message;
                mc.DateCreated = DateTime.Parse(mess.created_time);
                mc.UserId = mess.from.id;
                mc.UserName = mess.from.name;

                conversationContent.Messages.Add(mc);
            }

            if (conversationContent.Messages.Count == 25)
            {
                conversationContent.NextUrl = result.paging.next;
            }

            return Json(conversationContent, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetConversationContentNext(string url)
        {
            var conversationContent = new ConversationContentViewModel();
            string shopId = (string)Session["ShopId"];
            string accessToken = shopService.GetShop(shopId).FbToken;

            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            dynamic result = fbApp.Get(url, param);
            if (result.data.Count != 0)
            {
                foreach (var mess in result.data)
                {
                    var mc = new MessageContentViewModel();
                    mc.MessId = mess.id;
                    mc.MessContent = mess.message;
                    mc.DateCreated = DateTime.Parse(mess.created_time);
                    mc.UserId = mess.from.id;
                    mc.UserName = mess.from.name;

                    conversationContent.Messages.Add(mc);
                }

                if (conversationContent.Messages.Count == 25)
                {
                    conversationContent.NextUrl = result.paging.next;
                }
            }
            else
            {
                conversationContent.NextUrl = "null";
            }

            return Json(conversationContent, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetUserAvatar(string uid)
        {
            string shopId = (string)Session["ShopId"];
            string accessToken = shopService.GetShop(shopId).FbToken;

            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            dynamic result = fbApp.Get(uid + "/picture?type=normal&redirect=false", param);
            return Content(result.data.url);
        }

        public ActionResult GetPageAvatar()
        {
            string shopId = (string)Session["ShopId"];
            string accessToken = shopService.GetShop(shopId).FbToken;

            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            dynamic result = fbApp.Get(shopId + "/picture?type=normal&redirect=false", param);
            return Content(result.data.url);
        }
        public ActionResult SetConversationRead(string conversationId)
        {
            var rs = conversationService.SetReadConversation(conversationId);

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SearchSender(string search)
        {
            string shopId = (string)Session["ShopId"];
            var listConversation = conversationService.GetConversationsByShopId(shopId);
            string accessToken = shopService.GetShop(shopId).FbToken;

            var listSearched = new List<ConversationPreviewViewModel>();

            foreach (var c in listConversation)
            {
                var preview = GetConversationPreviewFromFb(accessToken, c);

                if (preview.UserName.ToLower().Contains(search.ToLower()))
                {
                    listSearched.Add(preview);
                }
            }

            if (listSearched.Count > 0)
            {
                return Json(listSearched, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SendMessage(string threadId, string message)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.message = message;
                var result = fbApp.Post(threadId + "/messages", param);
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReplyComment(string commentId, string message)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.message = message;
                var result = fbApp.Post(commentId + "/comments", param);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult HideComment(string commentId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.is_hidden = true;
                var result = fbApp.Post(commentId, param);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteComment(string commentId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            fbApp.AccessToken = accessToken;
            dynamic param = new ExpandoObject();
            try
            {
                var result = fbApp.Delete(commentId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult PrivateReplyComment(string commentId, string message)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.message = message;
                var result = fbApp.Post(commentId + "/private_replies", param);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}