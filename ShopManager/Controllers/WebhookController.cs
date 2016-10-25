using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DataService.Utils;
using System.Dynamic;
using DataService.Service;
using Facebook;


namespace ShopManager.Controllers
{
    public class WebhookController : Controller
    {        
        ShopService shopService = new ShopService();
        CommentService commentService = new CommentService();
        ConversationService conversationService = new ConversationService();
        FacebookClient fbApp = new FacebookClient();
        // GET: Webhook
        public ActionResult Index()
        {
            return View();
        }

        public string Subscribe()
        {
            string param1 = this.Request.QueryString["hub.challenge"];
            string param2 = this.Request.QueryString["hub.mode"];
            string param3 = this.Request.QueryString["hub.verify_token"];

            return param1;
        }

        [HttpPost]
        [ActionName("Subscribe")]
        public void SubscribeData()
        {
            String jsonData = Regex.Unescape(new StreamReader(this.Request.InputStream, this.Request.ContentEncoding).ReadToEnd());
            System.Diagnostics.Debug.WriteLine(jsonData);
            
            dynamic fbJson = System.Web.Helpers.Json.Decode(jsonData);
            ProcessItem(fbJson);

            System.Diagnostics.Debug.WriteLine("header");
            System.Diagnostics.Debug.WriteLine(this.Request.Headers["X-Hub-Signature"]);

            var reader = new System.IO.StreamReader(this.HttpContext.Request.InputStream);
            System.Diagnostics.Debug.WriteLine(reader.ReadToEnd());
            return;
        }

        private void ProcessItem(dynamic fbObj)
        {
            dynamic obj = fbObj["object"];
            dynamic entries = fbObj.entry;            
            foreach (dynamic entry in entries)
            {
                if (HasProperty(entry, "messaging"))
                {
                    break;
                }
                string shopId = entry.id;
                long time = entry.time;

                dynamic changes = entry.changes;
                foreach (dynamic change in changes)
                {
                    string field = change.field;
                    dynamic value = change.value;
                    if (field.Equals(WebhookField.Feed))
                    {
                        string commentId = value.comment_id;
                        string item = value.item;
                        if (item.Equals(WebhookItem.Comment))
                        {
                            string verb = value.verb;                            
                            long createTime = value.created_time;
                            string customerId = Convert.ToString(value.sender_id);
                            int intent = (int)DefaultIntent.UNKNOWN;
                            int status = (int)CommentStatus.SHOWING;
                            switch (verb)
                            {
                                case WebhookVerb.Add:                                                                                                                                                
                                case WebhookVerb.Edit:                                    
                                    if (HasProperty(value, "photo"))
                                    {
                                        //chatbot api intent photo here
                                        intent = (int)DefaultIntent.PHOTO_EMO;
                                    }
                                    if (CheckTagOthers(commentId, shopId))
                                    {
                                        //chatbot api intent tag other here
                                        intent = (int)DefaultIntent.TAG;
                                    }
                                    if (HasProperty(value, "message"))
                                    {
                                        string message = value.message;
                                        //chatbot api for message here
                                    }                                    
                                    commentService.AddComment(commentId, customerId, createTime, intent, status, shopId);
                                    break;
                                case WebhookVerb.Hide:
                                    commentService.SetStatus(commentId, (int)CommentStatus.HIDDEN);
                                    break;
                                case WebhookVerb.Unhide:
                                    commentService.SetStatus(commentId, (int)CommentStatus.SHOWING);
                                    break;
                                case WebhookVerb.Remove:
                                    commentService.SetStatus(commentId, (int)CommentStatus.DELETED);
                                    break;
                                default: break;
                            }
                            
                        }
                    } else if (field.Equals(WebhookField.Conversations)){
                        string threadId = value.thread_id;
                        checkLast(threadId, shopId, time);
                    }                    

                }
            }


        }

        private void checkLast(string threadId, string shopId, long time)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            param.fields = "from,created_time,message";
            param.limit = 1;
            dynamic result = fbApp.Get(threadId + "/messages", param);
            dynamic detail = result.data[0];
            if (shopId.Equals(detail.from.id))
            {
                SignalRAlert.AlertHub.Send(shopId, detail);
                conversationService.SetReadConversation(threadId, time);
            } else
            {
                SignalRAlert.AlertHub.Send(shopId, detail);
                string message = detail.message;
                //chatbot api here
                int intent = (int) DefaultIntent.UNKNOWN;
                conversationService.AddConversation(threadId, intent, time, shopId);
            }
        }

        private bool CheckTagOthers(string commentId, string shopId)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            //FacebookClient fb = new FacebookClient(accessToken);
            dynamic param = new ExpandoObject();
            param.fields = "message_tags";
            param.access_token = accessToken;
            dynamic result = fbApp.Get(commentId, param);
            if (HasProperty(result, "message_tags"))
            {
                return true;
            } else
            {
                return false;
            }
        }

        private bool HasProperty(dynamic obj, string name)
        {
            Type objType = obj.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }

            return objType.GetProperty(name) != null;
        }
    }
}