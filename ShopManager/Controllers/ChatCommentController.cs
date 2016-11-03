using DataService;
using DataService.Service;
using DataService.ViewModel;
using Facebook;
using System;
using System.Collections.Generic;
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
            //Session["ShopId"] = "1802287933384032";
            //Session["CustId"] = "3";
            return View();
        }

        public ActionResult GetConversationPreviews()
        {
            var listConversationPreview = new List<ConversationPreviewViewModel>();
            string shopId = (string)Session["ShopId"];

            var listConversation = conversationService.GetConversationsByShopId(shopId);


            //calling fb
            string accessToken = shopService.GetShop(shopId).FbToken;
            //dynamic param = new ExpandoObject();
            //param.access_token = accessToken;
            //param.fields = "from,created_time,message";
            //param.limit = 1;
            //dynamic result = fbApp.Get(threadId + "/messages", param);

            foreach (var c in listConversation)
            {
                var preview = new ConversationPreviewViewModel();
                preview.ThreadId = c.Id;
                preview.IsRead = c.IsRead;

                dynamic param = new ExpandoObject();
                param.access_token = accessToken;
                param.fields = "from,created_time,message";
                dynamic result = fbApp.Get(c.Id + "/messages", param);

                int i = 0;
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
                preview.CreatedTime = DateTime.Parse(detail.created_time);
                preview.RecentMess = detail.message;

                Console.WriteLine("UserID=" + preview.UserFbId);

                param = new ExpandoObject();
                param.access_token = accessToken;
                param.fields = "";
                param.limit = 1;
                string url = preview.UserFbId + "/picture?type=normal&redirect=false";
                dynamic result2 = fbApp.Get(url, param);
                preview.AvatarUrl = result2.data.url;

                Console.WriteLine("Preview: " + preview.ThreadId + " - time: " + preview.CreatedTime + " - Avatar: " + preview.AvatarUrl);

                listConversationPreview.Add(preview);
            }

            return Json(listConversationPreview, JsonRequestBehavior.AllowGet);
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

            return Json(conversationContent, JsonRequestBehavior.AllowGet);
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
            var rs=conversationService.SetReadConversation(conversationId);

            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}