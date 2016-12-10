using DataService;
using DataService.JqueryDataTable;
using DataService.Service;
using DataService.Utils;
using DataService.ViewModel;
using Facebook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopManager.SignalRAlert;
using ShopManager.Api.Ai.Custom;
using System.Configuration;
using ApiAiSDK;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class AnalysisController : Controller
    {
        CommentService commentService = new CommentService();
        IntentService intentService = new IntentService();
        ShopService shopService = new ShopService();
        CustomerService customerService = new CustomerService();
        PostService postService = new PostService();
        AIDataServiceCustom apiService = new AIDataServiceCustom(
                new AIConfigurationCustom(ConfigurationManager.AppSettings["ApiAiDeveloper"], SupportedLanguage.English, "intents"));        // GET: Analysis
        public ActionResult Comment()
        {
            return View();
        }

        // ANDND Get comment by condition
        public JsonResult GetCommentByShopAndCondition(JQueryDataTableParamModel param, string fbId, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var shopId = (string)Session["ShopId"];
            try
            {
                var listModel = commentService.GetCommentByShopAndCondition(param, fbId, shopId, intentId, status, isRead, startDate, endDate);
                var totalRecords = listModel.Count();
                var data = listModel.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
                var displayRecords = data.Count();

                ShopViewModel shop = shopService.GetShop(shopId);

                if (shop != null)
                {
                    FacebookClient fbApp = new FacebookClient(shop.FbToken);
                    dynamic postContent;
                    dynamic fbUser;
                    dynamic commentContent;
                    Customer registedCustomer;

                    for (int i = 0; i < data.Count(); i++)
                    {
                        //Get Post Content infor
                        if (data[i].PostContent != null && data[i].PostContent.Length != 0)
                        {
                            data[i].PostContent = TruncateLongString(data[i].PostContent, 40);
                        }
                        else
                        {
                            try
                            {
                                dynamic paramFB = new ExpandoObject();
                                paramFB.locale = "vi_VI";
                                postContent = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].PostId, paramFB).ToString());
                                if (postContent.story == null || postContent.message == null)
                                {
                                    data[i].PostContent = TruncateLongString(postContent.story + postContent.message, 40);
                                }
                                else
                                {
                                    data[i].PostContent = TruncateLongString(postContent.story + ": " + postContent.message, 40);
                                }
                            }
                            catch (Exception e)
                            {
                                data[i].PostContent = "Bài đăng không tồn tại.";
                                Debug.WriteLine(e.Message);
                            }
                        }
                            

                        //Get comment infor

                        if (data[i].LastContent != null && data[i].LastContent.Length != 0)
                        {
                            data[i].CommentContent = TruncateLongString(data[i].LastContent, 40);
                        }
                        else
                        {
                            try
                            {
                                dynamic paramFB = new ExpandoObject();
                                paramFB.fields = "from,id,message";
                                commentContent = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].Id, paramFB).ToString());
                                if (commentContent != null && !string.IsNullOrEmpty(commentContent.message))
                                {
                                    data[i].CommentContent = TruncateLongString(commentContent.message, 40);
                                }
                                else
                                {
                                    data[i].CommentContent = "Bấm để xem nội dung";
                                }

                            }
                            catch (Exception e)
                            {
                                data[i].CommentContent = "Bình luận không tồn tại.";
                                Debug.WriteLine(e.Message);
                            }
                        }

                        //Get Sender Information
                        registedCustomer = customerService.GetCustomerByFacebookId(data[i].SenderFbId, shopId);
                        if (registedCustomer != null)
                        {
                            data[i].SenderName = registedCustomer.Name;
                            data[i].IsCustomer = true;
                        }
                        else
                        {
                            data[i].IsCustomer = false;
                            try
                            {
                                fbUser = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].SenderFbId).ToString());
                                data[i].SenderName = fbUser.name;
                            }
                            catch (Exception ex)
                            {
                                data[i].SenderName = "Người dùng không tồn tại";
                                Debug.WriteLine(ex.Message);
                            }
                        }

                        //Get Intent infor
                        if (data[i].IntentId != null)
                        {
                            data[i].IntentId = data[i].IntentId;
                            data[i].IntentName = intentService.GetIntentNameById(data[i].IntentId.Value);
                        }

                    }
                }


                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,//displayRecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        // Test get list user
        public JsonResult GetUserList(JQueryDataTableParamModel param, DateTime? startDate, DateTime? endDate)
        {
            var shopId = (string)Session["ShopId"];
            var listUser = commentService.GetCommentUserList(param, shopId, startDate, endDate);
            var totalRecords = listUser.Count();
            var data = listUser.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            var displayRecords = data.Count();

            ShopViewModel shop = shopService.GetShop(shopId);
            try
            {
                FacebookClient fbApp = new FacebookClient(shop.FbToken);
                dynamic fbUser;
                for (int i = 0; i < data.Count(); i++)
                {
                    try
                    {
                        fbUser = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].UserFBId).ToString());
                        data[i].UserName = fbUser.name;
                        try
                        {
                            Customer registedCustomer = customerService.GetCustomerByFacebookId(data[i].UserFBId, shopId);
                            if (registedCustomer.Name != null)
                            {
                                data[i].IsCustomer = true;
                            }
                        }
                        catch (Exception)
                        {
                            data[i].IsCustomer = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        data[i].UserName = "Người dùng không tồn tại";
                        Debug.WriteLine(ex.Message);
                    }
                }

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,//displayRecords,
                    aaData = data,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }


        }

        //ANDND Get all intent
        public JsonResult GetAllIntent()
        {
            return Json(intentService.GetAllIntent(), JsonRequestBehavior.AllowGet);
        }

        //ANDND Set comment is read
        public JsonResult SetCommentIsRead(string commentId)
        {
            string shopId = (string) Session["ShopId"];
            var result = commentService.SetIsRead(commentId);
            AlertHub.SendNotification(shopId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //ANDND Set comment Intent
        public JsonResult SetCommentIntent(string commentId, int intentId)
        {
            bool result = commentService.SetIntent(commentId, intentId);
            if (result == true)
            {
                try
                {
                    string content = commentService.getCommentById(commentId).LastContent;
                    string apiai = intentService.GetIntent(intentId).ApiAiId;
                    if (intentId != (int)DefaultIntent.INFO && apiai != null && apiai != "")
                    {
                        var intent = apiService.RequestIntentGet(apiai);
                        intent.SetMoreTemplates(content);
                        apiService.RequestIntentPut(intent, apiai);
                    }
                }
                catch
                {

                }
            }
                     
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //ANDND Set comment Intent
        public JsonResult SetPostIntent(string postId, int intentId)
        {

            var result = postService.SetIntent(postId, intentId);
            if (result == true)
            {
                try
                {
                    string content = postService.GetPostById(postId).LastContent;
                    string apiai = intentService.GetIntent(intentId).ApiAiId;
                    if (intentId != (int)DefaultIntent.INFO && apiai != null && apiai != "")
                    {
                        var intent = apiService.RequestIntentGet(apiai);
                        intent.SetMoreTemplates(content);
                        apiService.RequestIntentPut(intent, apiai);
                    }
                }
                catch
                {

                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //Delete a comment
        public JsonResult DeleteComment(string[] commentId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbApp = new FacebookClient(accessToken);
            List<string> deleteFailed = new List<string>();
            Comment comment;
            for (int i = 0; i < commentId.Length; i++)
            {
                comment = new Comment();
                comment = commentService.GetCommentById(commentId[i]);
                if (comment != null)
                {
                    var status = comment.Status;
                    if (status == (int)CommentStatus.SHOWING || status == (int)CommentStatus.HIDDEN || status == (int)CommentStatus.WARNING || status == (int)CommentStatus.APPROVED)
                    {
                        try
                        {
                            dynamic result = fbApp.Delete(commentId[i]);
                            if (result.success)
                            {
                                var rs = commentService.SetCommentStatus(commentId[i], (int)CommentStatus.DELETED);
                                if (!rs)
                                {
                                    deleteFailed.Add(commentId[i]);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            deleteFailed.Add(commentId[i]);
                            Debug.WriteLine(e.StackTrace);
                        }
                    }
                }
            }

            return Json(new { totalComment = commentId.Length, errorNumber = deleteFailed.Count() }, JsonRequestBehavior.AllowGet);

        }

        //ANDND Hide a comment by comment id
        public JsonResult HideComment(string[] commentId, bool isHide)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbApp = new FacebookClient(accessToken);
            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            param.is_hidden = isHide;
            List<string> hideFailed = new List<string>();
            Comment comment;
            for (int i = 0; i < commentId.Length; i++)
            {
                comment = new Comment();
                comment = commentService.GetCommentById(commentId[i]);
                if (comment != null)
                {
                    var status = comment.Status;
                    if (isHide)
                    {
                        if (status == (int)CommentStatus.SHOWING || status == (int)CommentStatus.WARNING || status == (int)CommentStatus.APPROVED)
                        {
                            try
                            {
                                dynamic result = fbApp.Post(commentId[i], param);
                                if (result.success)
                                {
                                    bool rs;
                                    rs = commentService.SetCommentStatus(commentId[i], (int)CommentStatus.HIDDEN);
                                    if (!rs)
                                    {
                                        hideFailed.Add(commentId[i]);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.StackTrace);
                                hideFailed.Add(commentId[i]);
                            }
                        }
                    }
                    else
                    {
                        if (status == (int)CommentStatus.HIDDEN)
                        {
                            try
                            {
                                dynamic result = fbApp.Post(commentId[i], param);
                                if (result.success)
                                {
                                    bool rs;
                                    rs = commentService.SetCommentStatus(commentId[i], (int)CommentStatus.SHOWING);
                                    if (!rs)
                                    {
                                        hideFailed.Add(commentId[i]);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.StackTrace);
                                hideFailed.Add(commentId[i]);
                            }
                        }
                    }
                }

            };
            return Json(new { totalComment = commentId.Length, errorNumber = hideFailed.Count() }, JsonRequestBehavior.AllowGet);
        }

        //ANDND Set post is read
        public JsonResult SetPostIsRead(string postId)
        {
            var rs = postService.SetPostIsRead(postId);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetCommentStatus(string[] commentId, int statusId)
        {
            List<string> approveFailed = new List<string>();
            for(int i=0;i< commentId.Length; i++)
            {
                var status = commentService.GetCommentById(commentId[i]).Status;
                if (status == (int)CommentStatus.WARNING)
                {
                    if (!commentService.SetCommentStatus(commentId[i], statusId))
                    {
                        approveFailed.Add(commentId[i]);
                    }
                }else
                {
                    approveFailed.Add(commentId[i]);
                }
            }
            return Json(new { totalComment = commentId.Length, errorNumber = approveFailed.Count() }, JsonRequestBehavior.AllowGet);
        }

        //Cut long string
        public string TruncateLongString(string str, int maxLength)
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

        //Get Current month analysis comment
        public JsonResult GetAnalysisDataByTime(int? intentId, int? status, bool? isRead, DateTime startDate, DateTime endDate, int divideNumber)
        {
            var shopId = (string)Session["ShopId"];
            if (divideNumber > 1 && endDate >= startDate && endDate != null && startDate != null)
            {
                double start = (startDate - new DateTime(1970, 1, 1)).TotalSeconds;
                double end = (endDate - new DateTime(1970, 1, 1)).TotalSeconds;
                double period = (end - start) / divideNumber;
                Debug.WriteLine("start: " + start + "end: " + end + "period:" + period);

                List<AnalysisDataForCharViewModel> listdata = new List<AnalysisDataForCharViewModel>();
                AnalysisDataForCharViewModel model;
                while (start + period <= end)
                {
                    model = new AnalysisDataForCharViewModel();
                    var data = commentService.GetAnalysisDataByTime(shopId, intentId, status, isRead, ConvertDoubleToDatetime(start), ConvertDoubleToDatetime(start + period));
                    model.Time = ConvertDoubleToDatetime(start + period);
                    model.ListData = data;
                    listdata.Add(model);
                    start = start + period;
                }
                return Json(listdata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var data = commentService.GetAnalysisDataByTime(shopId, intentId, status, isRead, startDate, endDate);
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        // Get Post by Time
        public JsonResult GetPostByTime(JQueryDataTableParamModel param, DateTime? startDate, DateTime? endDate)
        {
            var shopId = (string)Session["ShopId"];
            var listPost = postService.GetPostByTime(param, shopId, startDate, endDate);
            var totalRecords = listPost.Count();
            var data = listPost.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            var displayRecords = data.Count();

            ShopViewModel shop = shopService.GetShop(shopId);

            if (shop != null)
            {
                FacebookClient fbApp = new FacebookClient(shop.FbToken);
                dynamic postContent;
                dynamic fbUser;
                Customer registedCustomer;

                List<AnalysisPostViewModel> listModel = new List<AnalysisPostViewModel>();
                AnalysisPostViewModel model;

                for (int i = 0; i < data.Count(); i++)
                {
                    model = new AnalysisPostViewModel();
                    model.PostId = data[i].Id;
                    try
                    {
                        //Get Post Information
                        dynamic paramFB = new ExpandoObject();
                        paramFB.locale = "vi_VI";
                        paramFB.fields = "from,id,message,story,likes.summary(true),comments.summary(true),shares";
                        postContent = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].Id, paramFB).ToString());
                        if (postContent.story == null || postContent.message == null)
                        {
                            model.PostContent = TruncateLongString(postContent.story + postContent.message, 40);
                        }
                        else
                        {
                            model.PostContent = TruncateLongString(postContent.story + ": " + postContent.message, 40);
                        };
                        if (postContent.likes != null)
                        {
                            model.LikeCount = postContent.likes.summary.total_count;
                        }
                        else
                        {
                            model.LikeCount = 0;
                        }
                        if (postContent.comments != null)
                        {
                            model.CommentCount = postContent.comments.summary.total_count;
                        }
                        else
                        {
                            model.CommentCount = 0;
                        }
                        if (postContent.shares != null)
                        {
                            model.ShareCount = postContent.shares.count;
                        }
                        else
                        {
                            model.ShareCount = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        model.PostContent = "Bài đăng không tồn tại.";
                        model.LikeCount = 0;
                        model.CommentCount = 0;
                        model.ShareCount = 0;
                        Debug.WriteLine(e.Message);
                    }

                    //Get Sender Information
                    model.SenderFBId = data[i].SenderFbId;
                    registedCustomer = customerService.GetCustomerByFacebookId(data[i].SenderFbId, shopId);
                    if (registedCustomer != null)
                    {
                        model.SenderFBName = registedCustomer.Name;
                        model.IsCustomer = true;
                    }
                    else
                    {
                        model.IsCustomer = false;
                        try
                        {
                            fbUser = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].SenderFbId).ToString());
                            model.SenderFBName = fbUser.name;
                        }
                        catch (Exception ex)
                        {
                            model.SenderFBName = "Người dùng không tồn tại";
                            Debug.WriteLine(ex.Message);
                        }
                    }

                    model.ShopId = data[i].ShopId;
                    model.Status = data[i].Status;
                    model.IsRead = data[i].IsRead;
                    model.IntentId = data[i].IntentId;
                    if (data[i].IntentId != null)
                    {
                        model.IntentName = intentService.GetIntentNameById(data[i].IntentId);
                    }
                    listModel.Add(model);

                }

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,//displayRecords,
                    aaData = listModel
                }, JsonRequestBehavior.AllowGet);
            }else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            
        }

        //ANDND Hide a post by comment id
        public JsonResult HidePost(string[] PostId, bool isHide)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbApp = new FacebookClient(accessToken);
            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            param.is_hidden = isHide;
            List<string> hideFailed = new List<string>();
            Post post;
            for (int i = 0; i < PostId.Length; i++)
            {
                post = new Post();
                post = postService.GetPostById(PostId[i]);
                if (post != null)
                {
                    var status = post.Status;
                    if (isHide)
                    {
                        if (status == (int)CommentStatus.SHOWING || status == (int)CommentStatus.WARNING || status == (int)CommentStatus.APPROVED)
                        {
                            try
                            {
                                dynamic result = fbApp.Post(PostId[i], param);
                                if (result.success)
                                {
                                    bool rs;
                                    rs = postService.SetStatus(PostId[i], (int)CommentStatus.HIDDEN);
                                    if (!rs)
                                    {
                                        hideFailed.Add(PostId[i]);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.StackTrace);
                                hideFailed.Add(PostId[i]);
                            }
                        }
                    }
                    else
                    {
                        if (status == (int)CommentStatus.HIDDEN)
                        {
                            try
                            {
                                dynamic result = fbApp.Post(PostId[i], param);
                                if (result.success)
                                {
                                    bool rs;
                                    rs = postService.SetStatus(PostId[i], (int)CommentStatus.SHOWING);
                                    if (!rs)
                                    {
                                        hideFailed.Add(PostId[i]);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Debug.WriteLine(e.StackTrace);
                                hideFailed.Add(PostId[i]);
                            }
                        }
                    }
                }

            };
            return Json(new { totalPost = PostId.Length, errorNumber = hideFailed.Count() }, JsonRequestBehavior.AllowGet);
        }

        //Delete a post
        public JsonResult DeletePost(string[] PostId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbApp = new FacebookClient(accessToken);
            List<string> deleteFailed = new List<string>();
            Post post;
            for (int i = 0; i < PostId.Length; i++)
            {
                post = new Post();
                post = postService.GetPostById(PostId[i]);
                if (post != null)
                {
                    var status = post.Status;
                    if (status == (int)CommentStatus.SHOWING || status == (int)CommentStatus.HIDDEN || status == (int)CommentStatus.WARNING || status == (int)CommentStatus.APPROVED)
                    {
                        try
                        {
                            dynamic result = fbApp.Delete(PostId[i]);
                            if (result.success)
                            {
                                var rs = postService.SetStatus(PostId[i], (int)CommentStatus.DELETED);
                                if (!rs)
                                {
                                    deleteFailed.Add(PostId[i]);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            deleteFailed.Add(PostId[i]);
                            Debug.WriteLine(e.StackTrace);
                        }
                    }
                }
            }

            return Json(new { totalPost = PostId.Length, errorNumber = deleteFailed.Count() }, JsonRequestBehavior.AllowGet);

        }

        public static DateTime ConvertDoubleToDatetime(double timestamp)
        {
            TimeSpan time = TimeSpan.FromSeconds(timestamp);
            DateTime date = new DateTime(1970, 1, 1) + time;
            return date;
        }

        public static double ConvertDatetimeToDouble(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

    }
}