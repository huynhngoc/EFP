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
        public JsonResult GetCommentByShopAndCondition(JQueryDataTableParamModel param, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var shopId = (string)Session["ShopId"];
            try
            {
                var listModel = commentService.GetCommentByShopAndCondition(param, shopId, intentId, status, isRead, startDate, endDate);
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
                            fbUser = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].SenderFbId).ToString());
                            data[i].SenderName = fbUser.name;

                        }
                        catch (Exception e)
                        {
                            data[i].SenderName = "Người dùng không tồn tại";
                            Debug.WriteLine(e.Message);
                        }
                        try
                        {
                            commentContent = System.Web.Helpers.Json.Decode(fbApp.Get(data[i].Id).ToString());
                            data[i].CommentContent = TruncateLongString(commentContent.message, 40);
                        }
                        catch (Exception e)
                        {
                            data[i].CommentContent = "Bình luận không tồn tại.";
                            Debug.WriteLine(e.Message);
                        }
                        data[i].IntentId = data[i].IntentId;
                        data[i].IntentName = intentService.GetIntentNameById(data[i].IntentId);
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
        public JsonResult DeleteComment(string commentId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbApp = new FacebookClient(accessToken);
            try
            {
                dynamic result = fbApp.Delete(commentId);
                if (result.success)
                {
                    var rs = commentService.SetCommentStatus(commentId, (int)CommentStatus.DELETED);
                    if (rs)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //ANDND Hide a comment by comment id
        public JsonResult HideComment(string commentId, bool isHide)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbApp = new FacebookClient(accessToken);
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.is_hidden = isHide;
                var result = fbApp.Post(commentId, param);
                if (result.success)
                {
                    bool rs;
                    if (isHide)
                    {
                        rs = commentService.SetCommentStatus(commentId, (int)CommentStatus.HIDDEN);
                    }
                    else
                    {
                        rs = commentService.SetCommentStatus(commentId, (int)CommentStatus.APPROVED);
                    }
                    
                    if (rs)
                    {
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //ANDND Set post is read
        public JsonResult SetPostIsRead(string postId)
        {
            return Json(postService.SetPostIsRead(postId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetCommentStatus(string commentId, int statusId)
        {
            return Json(commentService.SetCommentStatus(commentId,statusId), JsonRequestBehavior.AllowGet);
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

    }
}