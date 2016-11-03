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
using System.Configuration;
using System.Text;
using ApiAiSDK;

namespace ShopManager.Controllers
{
    public class WebhookController : Controller
    {        
        ShopService shopService = new ShopService();
        CommentService commentService = new CommentService();
        PostService postService = new PostService();
        ConversationService conversationService = new ConversationService();
        FacebookClient fbApp = new FacebookClient();
        ApiAi apiAi = new ApiAi(new AIConfiguration(ConfigurationManager.AppSettings["ApiAiClient"], SupportedLanguage.English));        
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
            // check if facebook request
            if (param3.Equals(ConfigurationManager.AppSettings["WebhookPassword"]))
            {
                return param1;
            }
            else return "You are not request from facebook";
        }

        [HttpPost]
        [ActionName("Subscribe")]
        public void SubscribeData()
        {
            this.Request.InputStream.Position = 0;
            var reader = new StreamReader(this.Request.InputStream).ReadToEnd();
            //unescape utf-8
            this.Request.InputStream.Position = 0;
            String jsonData = Regex.Unescape(new StreamReader(this.Request.InputStream, this.Request.ContentEncoding).ReadToEnd());
            System.Diagnostics.Debug.WriteLine(jsonData);
            System.Diagnostics.Debug.WriteLine(reader);
            System.Diagnostics.Debug.WriteLine("header");
            System.Diagnostics.Debug.WriteLine(this.Request.Headers["X-Hub-Signature"]);

            //verify signature            
            // read without unescape
            var hmac = SignWithHmac(UTF8Encoding.UTF8.GetBytes(reader), UTF8Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["FbAppSecret"]));
            var hmacHex = ConvertToHexadecimal(hmac);
            System.Diagnostics.Debug.WriteLine(hmacHex);
            string signature = this.Request.Headers["X-Hub-Signature"];
            bool isValid = signature.Split('=')[1] == hmacHex;

            

            //Decode to json data
            dynamic fbJson = System.Web.Helpers.Json.Decode(jsonData);

            System.Diagnostics.Debug.WriteLine(isValid);            
            //only process when valid            
            if (isValid)
            {
                try
                {
                    ProcessItem(fbJson);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.StackTrace);
                }

            }

            return;
        }


        private static byte[] SignWithHmac(byte[] dataToSign, byte[] keyBody)
        {
            using (var hmacAlgorithm = new System.Security.Cryptography.HMACSHA1(keyBody))
            {
                return hmacAlgorithm.ComputeHash(dataToSign);
            }
        }

        private static string ConvertToHexadecimal(IEnumerable<byte> bytes)
        {
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
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

                        string item = value.item;
                        string verb = value.verb;
                        string customerId = Convert.ToString(value.sender_id);
                        string parentId = Convert.ToString(value.parent_id);
                        string postId = Convert.ToString(value.post_id);
                        int? intent = (int)DefaultIntent.UNKNOWN;
                        int status = (int)CommentStatus.SHOWING;
                        if (item.Equals(WebhookItem.Comment))
                        {
                            long createTime = value.created_time;
                            string commentId = value.comment_id;
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
                                        var respond = apiAi.TextRequest(message);
                                        var intentRespond = respond.Result.Metadata.IntentName;
                                        if (intentRespond != null)
                                        {
                                            try
                                            {
                                                if (int.Parse(intentRespond) != (int)DefaultIntent.UNKNOWN)
                                                {
                                                    intent = int.Parse(intentRespond);
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                
                                            }                                            
                                        }
                                    }
                                    commentService.AddComment(commentId, customerId, createTime, intent.Value, status, parentId.Equals(postId) ? null : parentId, postId);
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
                        } else if (item.Equals(WebhookItem.Post)|| item.Equals(WebhookItem.Photo))
                        {
                            //string postId = value.comment_id;
                            switch (verb)
                            {
                                case WebhookVerb.Add:
                                case WebhookVerb.Edit:
                                    if (HasProperty(value, "photo"))
                                    {
                                        //chatbot api intent photo here
                                        intent = (int)DefaultIntent.PHOTO_EMO;
                                    }
                                    //if (CheckTagOthers(postId, shopId))
                                    //{
                                    //    //chatbot api intent tag other here
                                    //    intent = (int)DefaultIntent.TAG;
                                    //}
                                    if (HasProperty(value, "message"))
                                    {
                                        string message = value.message;
                                        //chatbot api for message here
                                        var respond = apiAi.TextRequest(message);
                                        var intentRespond = respond.Result.Metadata.IntentName;
                                        if (intentRespond != null)
                                        {
                                            try
                                            {
                                                if (int.Parse(intentRespond) != (int)DefaultIntent.UNKNOWN)
                                                {
                                                    intent = int.Parse(intentRespond);
                                                }
                                            }
                                            catch (Exception)
                                            {

                                            }
                                        }
                                    }
                                    if (customerId.Equals(shopId))
                                    {
                                        intent = null; 
                                    }
                                    postService.AddPost(postId, customerId, time, intent, false, status, shopId);
                                    break;
                                case WebhookVerb.Hide:
                                    postService.SetStatus(postId, (int)CommentStatus.HIDDEN);
                                    break;
                                case WebhookVerb.Unhide:
                                    postService.SetStatus(postId, (int)CommentStatus.SHOWING);
                                    break;
                                case WebhookVerb.Remove:
                                    postService.SetStatus(postId, (int)CommentStatus.DELETED);
                                    break;
                                default: break;
                            }
                        }
                        else if (item.Equals(WebhookItem.Status))
                        {
                            //string postId = value.comment_id;
                            switch (verb)
                            {
                                case WebhookVerb.Add:
                                case WebhookVerb.Edit:
                                    intent = null;         
                                    postService.AddPost(postId, customerId, time, null, true, status, shopId);
                                    break;
                                case WebhookVerb.Hide:
                                    postService.SetStatus(postId, (int)CommentStatus.HIDDEN);
                                    break;
                                case WebhookVerb.Unhide:
                                    postService.SetStatus(postId, (int)CommentStatus.SHOWING);
                                    break;
                                case WebhookVerb.Remove:
                                    postService.SetStatus(postId, (int)CommentStatus.DELETED);
                                    break;
                                default: break;
                            }
                        }

                        SignalRAlert.AlertHub.SendComment(shopId, value, intent.HasValue ? intent.Value: 0);
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
            param.fields = "from,created_time,message,attachments";
            param.limit = 1;
            dynamic result = fbApp.Get(threadId + "/messages", param);
            dynamic detail = result.data[0];
            
            if (shopId.Equals(detail.from.id))
            {
                SignalRAlert.AlertHub.SendMessage(shopId, detail, threadId, 0);
                conversationService.SetReadConversation(threadId, time);
            } else
            {
                int intent = (int)DefaultIntent.UNKNOWN;
                string message = detail.message;
                //chatbot api here              
                var respond = apiAi.TextRequest(message);
                var intentRespond = respond.Result.Metadata.IntentName;
                //intent = intentRespond == null? (int) DefaultIntent.UNKNOWN : int.Parse(intentRespond);
                if (intentRespond != null)
                {
                    try
                    {
                        if (int.Parse(intentRespond) != (int)DefaultIntent.UNKNOWN)
                        {
                            intent = int.Parse(intentRespond);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                SignalRAlert.AlertHub.SendMessage(shopId, detail, threadId, intent);
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
            //Type objType = obj.GetType();

            //if (objType == typeof(ExpandoObject))
            //{
            //    return ((IDictionary<string, object>)obj).ContainsKey(name);
            //}

            //return objType.GetProperty(name) != null;
            try
            {
                var prop = obj[name];
                return prop!=null;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Name = " + name);
                return false;
            }
        }


    }
}