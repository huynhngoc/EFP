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
                            long createTime = value.time;
                            string customerId = value.sender_id;
                            int intent = 1;
                            switch (verb)
                            {
                                case WebhookVerb.Add:                                                                                                                                                
                                case WebhookVerb.Edit:
                                    if (HasProperty(value, "message"))
                                    {
                                        string message = value.message;
                                        //chatbot api for message here
                                    }
                                    if (HasProperty(value, "photo"))
                                    {
                                        //chatbot api intent photo here

                                    }
                                    if (CheckTagOthers(commentId, shopId))
                                    {
                                        //chatbot api intent tag other here
                                    }
                                    commentService.AddComment(commentId, customerId, createTime, intent, shopId);
                                    break;
                                case WebhookVerb.Hide:

                                    break;
                                case WebhookVerb.Remove: break;
                                default: break;
                            }
                            
                        }
                    } else if (field.Equals(WebhookField.Conversations)){

                    }                    

                }
            }


        }

        private bool CheckTagOthers(string commentId, string shopId)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            FacebookClient fb = new FacebookClient(accessToken);
            dynamic param = new ExpandoObject();
            param.fields = "message_tags";
            dynamic result = fb.Get(commentId, param);
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