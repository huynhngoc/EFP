using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.IO;
using DataService.Utils;
using System.Dynamic;
using DataService.Service;
using Facebook;
using System.Configuration;
using System.Text;
using ApiAiSDK;
using ApiAiSDK.Model;
using ShopManager.Api.Ai.Custom;
using System.Diagnostics;

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
        ResponseService responseService = new ResponseService();
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

                bool flagCanHide = false;
                string flagComment = null;
                bool flagPostContent = false;
                string flagPost = null;

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
                        //facebook change policy pageID_PostID ==> authorID_PostID
                        postId = shopId + "_" + postId.Split('_')[1];
                        value.post_id = postId;
                        int? intent = (int)DefaultIntent.UNKNOWN;
                        int status = (int)CommentStatus.SHOWING;
                        string lastContent = null;
                        if (item.Equals(WebhookItem.Comment))
                        {
                            long createTime = value.created_time;
                            string commentId = value.comment_id;
                            bool dbResult = true;
                            switch (verb)
                            {
                                case WebhookVerb.Add:
                                case WebhookVerb.Edit:
                                    if (customerId.Equals(shopId))
                                    {
                                        intent = null;

                                        if (HasProperty(value, "message"))
                                        {
                                            string message = value.message;
                                            if (!string.IsNullOrEmpty(message))
                                            {
                                                lastContent = TruncateLongString(message, 240);
                                            }
                                        }
                                    }
                                    else
                                    {
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
                                            if (!string.IsNullOrEmpty(message))
                                            {
                                                lastContent = TruncateLongString(message, 240);
                                            }
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

                                        if (intent == (int)DefaultIntent.VANDAL)
                                        {
                                            //hide comment
                                            if (shopService.GetCommentMode(shopId) == (int)CommentMode.AUTOHIDE)
                                            {
                                                //hide comment
                                                var accessToken = shopService.GetShop(shopId).FbToken;
                                                dynamic param = new ExpandoObject();
                                                param.access_token = accessToken;
                                                param.is_hidden = true;
                                                try
                                                {
                                                    fbApp.Post(commentId, param);
                                                }
                                                catch (Exception)
                                                {

                                                }

                                            }
                                            status = (int)CommentStatus.WARNING;
                                        }

                                    }
                                    System.Diagnostics.Debug.WriteLine("add comment:" + commentId + "," + customerId + "," + createTime + "," + intent + "," + status + "," + (parentId.Equals(postId) ? null : parentId) + "," + postId + "," + lastContent);
                                    //fb change policy
                                    if (parentId.Split('_')[1].Equals(postId.Split('_')[1]))
                                    {                                        
                                        value.parent_id = shopId + "_" + parentId.Split('_')[1];
                                        parentId = null;
                                    }
                                    dbResult = commentService.AddComment(commentId, customerId, createTime, intent, status, parentId, postId, lastContent);
                                    if (dbResult == false)
                                    {
                                        GetParentToDb(shopId, commentId, postId, parentId);
                                        commentService.AddComment(commentId, customerId, createTime, intent, status, parentId, postId, lastContent);
                                        dbResult = true;
                                    }
                                    flagCanHide = true;
                                    flagComment = commentId;
                                    if (verb.Equals(WebhookVerb.Add) && intent!=null && intent!=(int) DefaultIntent.UNKNOWN 
                                        && intent!= (int) DefaultIntent.VANDAL && intent!= (int) DefaultIntent.GREETING
                                        && (shopService.GetReplyMode(shopId) == (int)ReplyMode.AUTO || shopService.GetReplyMode(shopId) == (int)ReplyMode.COMMENT_ONLY))
                                    {
                                        AutoComment(commentId, shopId, parentId, intent.Value);
                                    }
                                    break;
                                case WebhookVerb.Hide:
                                    dbResult = commentService.SetStatus(commentId, (int)CommentStatus.HIDDEN);                                    
                                    break;
                                case WebhookVerb.Unhide:
                                    dbResult = commentService.SetStatus(commentId, (int)CommentStatus.SHOWING);                                    
                                        break;
                                case WebhookVerb.Remove:
                                    commentService.SetStatus(commentId, (int)CommentStatus.DELETED);
                                    break;
                                default:
                                    dbResult = true;
                                    break;
                            }
                            if (dbResult == false)
                            {
                                GetParentToDb(shopId, commentId, postId, parentId);                             
                            }
                        }
                        else if (item.Equals(WebhookItem.Post) || item.Equals(WebhookItem.Photo))
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
                                    flagPostContent = true;
                                    flagPost = postId;
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
                                    flagPostContent = true;
                                    flagPost = postId;
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

                        SignalRAlert.AlertHub.SendComment(shopId, value, intent.HasValue ? intent.Value : 0);
                        SignalRAlert.AlertHub.SendNotification(shopId);
                        if (flagCanHide)
                        {
                            SetCanHide(shopId, flagComment);
                        }
                        if (flagPostContent)
                        {
                            SetLastContent(shopId, flagPost);
                        }
                    }
                    else if (field.Equals(WebhookField.Conversations))
                    {
                        string threadId = value.thread_id;
                        checkLast(threadId, shopId, time);
                    }

                }
            }


        }

        private void AutoComment(string commentId, string shopId, string parentId, int intent)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            fbApp.AccessToken = accessToken;          
            dynamic param = new ExpandoObject();
            var response = responseService.GetResponse(shopId, intent);
            if (!string.IsNullOrEmpty(response))
            {
                //respond message
                string res = responseService.GetResponse(shopId, intent);
                if (!string.IsNullOrEmpty(res))
                {
                    dynamic replyParam = new ExpandoObject();
                    param.access_token = accessToken;
                    param.message = res;
                    try
                    {
                        if (parentId != null)
                        {
                            fbApp.Post(commentId, param);
                        } else
                        {
                            //comment trực tiếp?
                            fbApp.Post(parentId, param);
                            //comment qua message
                            //fbApp.Post(commentId + "/private_replies", param);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        private void GetParentToDb(string shopId, string commentId, string postId, string parentId)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            fbApp.AccessToken = accessToken;
            dynamic param = new ExpandoObject();
            param.fields = "story,message,created_time,from,is_hidden";
            param.locale = "vi_VI";
            string lastContent = null;
            dynamic result;
            string sender;
            int? intent;
            try
            {
                if (postService.GetPostById(postId) == null)
                {
                    result = fbApp.Get(postId, param);
                    if (HasProperty(result, "story") && HasProperty(result, "message"))
                    {
                        lastContent = TruncateLongString(result.story + ":" + result.message, 240);
                    }
                    else
                    {
                        lastContent = TruncateLongString(result.story + result.message, 240);
                    }
                    sender = result.from.id;
                    intent = null;
                    DateTime dateO = (DateTime.Parse(result.created_time));
                    long dateCreateO = dateO.Subtract(new DateTime(1970,1,1)).Ticks / 10000000;
                    if (sender.Equals(shopId) == false) intent = (int)DefaultIntent.UNKNOWN;
                    postService.AddPost(postId, sender, dateCreateO, intent, false, result.is_hidden? (int)CommentStatus.HIDDEN: (int)CommentStatus.SHOWING, shopId);
                    if (!string.IsNullOrEmpty(lastContent))
                    {
                        postService.SetLastContent(postId, lastContent);
                    }
                }                

                if (parentId != null)
                {
                    param = new ExpandoObject();
                    param.fields = "created_time,from,message,is_hidden";
                    result = fbApp.Get(parentId, param);
                    sender = result.from.id;
                    intent = null;
                    DateTime dateP = (DateTime.Parse(result.created_time));
                    long dateCreateP = dateP.Subtract(new DateTime(1970, 1, 1)).Ticks / 10000000;
                    if (sender.Equals(shopId) == false) intent = (int)DefaultIntent.UNKNOWN;
                    commentService.AddComment(parentId, sender, dateCreateP, intent, result.is_hidden ? (int)CommentStatus.HIDDEN : (int)CommentStatus.SHOWING, null, postId, result.message);
                }

                param = new ExpandoObject();
                param.fields = "created_time,from,message,is_hidden,parent";
                result = fbApp.Get(commentId, param);
                sender = result.from.id;
                intent = null;
                string parent = HasProperty(result, "parent") ? result.parent.id : null;
                if (sender.Equals(shopId) == false) intent = (int)DefaultIntent.UNKNOWN;
                DateTime date = (DateTime.Parse(result.created_time));
                long dateCreate = date.Subtract(new DateTime(1970, 1, 1)).Ticks / 10000000;
                commentService = new CommentService();
                commentService.AddComment(commentId, sender, dateCreate , intent, result.is_hidden ? (int)CommentStatus.HIDDEN : (int)CommentStatus.SHOWING, parent, postId, result.message);
            }
            catch (Exception)
            {

                
            }            
        }

        private void checkLast(string threadId, string shopId, long time)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            param.fields = "from,created_time,message,attachments";
            param.limit = 5;
            param.until = time;
            dynamic result = fbApp.Get(threadId + "/messages", param);
            dynamic detail = result.data[0];

            if (shopId.Equals(detail.from.id))
            {
                SignalRAlert.AlertHub.SendMessage(shopId, result.data, threadId, 0);
                conversationService.SetReadConversation(threadId, time);
            }
            else
            {
                int intent = (int)DefaultIntent.UNKNOWN;
                string message = detail.message;
                //chatbot api here              
                try
                {
                    var respond = apiAi.TextRequest(message);
                    var intentRespond = respond.Result.Metadata.IntentName;
                    //intent = intentRespond == null? (int) DefaultIntent.UNKNOWN : int.Parse(intentRespond);
                    if (intentRespond != null)
                    {
                        intent = int.Parse(intentRespond);
                    }
                }
                catch (Exception)
                {
                }

                if (shopService.GetReplyMode(shopId) == (int)ReplyMode.AUTO || shopService.GetReplyMode(shopId) == (int)ReplyMode.MESSAGE_ONLY)
                {
                    var response = responseService.GetResponse(shopId, intent);
                    if (!string.IsNullOrEmpty(response))
                    {
                        //respond message
                        string res = responseService.GetResponse(shopId, intent);
                        if (!string.IsNullOrEmpty(res))
                        {
                            dynamic replyParam = new ExpandoObject();
                            param.access_token = accessToken;
                            param.message = res;
                            try
                            {
                                fbApp.Post(threadId + "/messages", param);
                            }
                            catch (Exception)
                            {

                            }
                        }
                    }
                }
                SignalRAlert.AlertHub.SendMessage(shopId, result.data, threadId, intent);
                conversationService.AddConversation(threadId, intent, time, shopId);
            }
            SignalRAlert.AlertHub.SendNotification(shopId);
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
            }
            else
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
                return prop != null;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Name = " + name);
                return false;
            }
        }

        private void SetLastContent(string shopId, string postId)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            string lastContent = null;
            fbApp.AccessToken = accessToken;
            dynamic param = new ExpandoObject();
            param.fields = "story,message";
            param.locale = "vi_VI";
            dynamic result = fbApp.Get(postId, param);
            try
            {
                if (HasProperty(result, "story") && HasProperty(result, "message"))
                {
                    lastContent = TruncateLongString(result.story + ":" + result.message, 240);
                }
                else
                {
                    lastContent = TruncateLongString(result.story + result.message, 240);
                }
                if (!string.IsNullOrEmpty(lastContent))
                {
                    postService.SetLastContent(postId, lastContent);
                }
            }
            catch (Exception)
            {


            }
        }

        private void SetCanHide(string shopId, string commentId)
        {
            string accessToken = shopService.GetShop(shopId).FbToken;
            //bool canHide = false;
            fbApp.AccessToken = accessToken;
            dynamic param = new ExpandoObject();
            param.fields = "can_hide";
            //param.locale = "vi_VI";
            dynamic result = fbApp.Get(commentId, param);
            try
            {
                if (result.can_hide == true)
                {
                    commentService.SetCanHide(commentId, true);
                }
            }
            catch (Exception)
            {

                commentService.SetCanHide(commentId, false);
            }
        }

        private string TruncateLongString(string str, int maxLength)
        {
            if (str.Length > maxLength)
            {
                return str.Substring(0, maxLength) + " ...";
            }
            else
            {
                return str;
            }

        }

        public JsonResult SendIntent(string str)
        {
            AIDataServiceCustom apiService = new AIDataServiceCustom(
                new AIConfigurationCustom(ConfigurationManager.AppSettings["ApiAiDeveloper"], SupportedLanguage.English, "intents"));
            try
            {
                var intent = apiService.RequestIntentGet(str);
                intent.SetMoreTemplates("Bạn có thể gọi điện với mình qua số @sys.phone-number:phone-number");
                var result = apiService.RequestIntentPut(intent, str);
                return Json(result,JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);

            }                        
        }
    }
}