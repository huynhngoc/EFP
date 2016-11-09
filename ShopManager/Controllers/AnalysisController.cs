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
        // GET: Analysis
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

                    for (int i = 0; i < data.Count(); i++)
                    {
                        try
                        {
                            postContent = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].PostId).ToString());
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

                        try
                        {
                            dynamic paramFB = new ExpandoObject();
                            paramFB.fields = "from,id,message,story";
                            commentContent = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].Id).ToString());
                            data[i].CommentContent = TruncateLongString(commentContent.message, 40);
                            if (commentContent.from.name != null)
                            {
                                data[i].SenderName = commentContent.from.name.ToString();

                                try
                                {
                                    Customer registedCustomer = customerService.GetCustomerByFacebookId(commentContent.from.id, shopId);
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
                        }
                        catch (Exception e)
                        {
                            data[i].CommentContent = "Bình luận không tồn tại.";
                            try
                            {
                                fbUser = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].SenderFbId).ToString());
                                data[i].SenderName = fbUser.name;
                                try
                                {
                                    Customer registedCustomer = customerService.GetCustomerByFacebookId(data[i].SenderFbId, shopId);
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
                                data[i].SenderName = "Người dùng không tồn tại";
                                Debug.WriteLine(ex.Message);
                            }
                            Debug.WriteLine(e.Message);
                        }
                        if (data[i].IntentId != null)
                        {
                            data[i].IntentId = data[i].IntentId;
                            data[i].IntentName = intentService.GetIntentNameById(data[i].IntentId.Value);
                        }
                        var customer = customerService.GetCustomerByFacebookId(data[i].SenderFbId, shopId);
                        if (customer != null && customer.CustomerFbId == data[i].SenderFbId)
                        {
                            data[i].IsCustomer = true;
                        }
                        else
                        {
                            data[i].IsCustomer = false;
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
        public JsonResult SetIsRead(string commentId)
        {
            return Json(commentService.SetIsRead(commentId), JsonRequestBehavior.AllowGet);
        }

        //ANDND Set comment Intent
        public JsonResult SetIntent(string commentId, int intentId)
        {
            return Json(commentService.SetIntent(commentId, intentId), JsonRequestBehavior.AllowGet);
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
                                    rs = commentService.SetCommentStatus(commentId[i], (int)CommentStatus.APPROVED);
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
            return Json(postService.SetPostIsRead(postId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetCommentStatus(string commentId, int statusId)
        {
            return Json(commentService.SetCommentStatus(commentId, statusId), JsonRequestBehavior.AllowGet);
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
        public JsonResult GetAnalysisDataByTime(DateTime startDate, DateTime endDate)
        {
            var shopId = (string)Session["ShopId"];
            var data = commentService.GetAnalysisDataByTime(shopId, startDate, endDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


    }
}