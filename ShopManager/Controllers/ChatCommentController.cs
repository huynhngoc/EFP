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
                param.limit = 1;
                dynamic result = fbApp.Get(c.Id + "/messages", param);
                dynamic detail = result.data[0];

                preview.UserName = detail.from.name;
                preview.UserId = detail.from.id;
                preview.CreatedTime = DateTime.Parse(detail.created_time);
                preview.RecentMess = detail.message;

                Console.WriteLine("UserID=" + preview.UserId);

                param = new ExpandoObject();
                param.access_token = accessToken;
                param.fields = "";
                param.limit = 1;
                string url = preview.UserId + "/picture?type=normal&redirect=false";
                dynamic result2 = fbApp.Get(url, param);
                preview.AvatarUrl = result2.data.url;

                Console.WriteLine("Preview: " + preview.ThreadId + " - time: " + preview.CreatedTime + " - Avatar: " + preview.AvatarUrl);

                listConversationPreview.Add(preview);
            }

            return Json(listConversationPreview, JsonRequestBehavior.AllowGet);
        }
    }
}