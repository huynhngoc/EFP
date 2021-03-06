﻿using DataService;
using DataService.JqueryDataTable;
using DataService.Service;
using DataService.Utils;
using DataService.ViewModel;
using Facebook;
using ShopManager.SignalRAlert;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        string tmpPost = null;
        List<string> tmpCommentList = null;
        CommentService commentservice = new CommentService();
        FacebookClient fbApp = new FacebookClient();
        PostService postservice = new PostService();
        ResponseService respService = new ResponseService();
        public ActionResult Index()
        {
            Session["shopOwner"] = shopService.GetShop((string)Session["ShopId"]).ShopName;
            ViewData["MessageNext"] = "null";
            return View();
        }

        public JsonResult GetAllPost(int skip, int take)
        { //string shopId, int from, int quantity
            //shopid = 685524511603937
            try
            {
                string shopId = (string)Session["ShopId"];
                //Session["ShopId"] = "1744339729172581";

                //dynamic shopOwnerParam = new ExpandoObject();
                //shopOwnerParam.fields = "picture,name";
                //dynamic shopFBUser = fbApp.Get(shopId, shopOwnerParam);
                //Session["ShopOwner"] = shopFBUser.name;
                //Session["ShopAvatar"] = shopFBUser.picture.data.url;

                dynamic postParam = new ExpandoObject();


                string accessToken = (shopService.GetShop(shopId)).FbToken;
                postParam.access_token = accessToken;
                postParam.fields = "message,from,full_picture,story";
                postParam.locale = "vi_VI";



                Debug.WriteLine("service " + DateTime.Now + " " + shopId);
                var rawPostList = postservice.GetAllPost(shopId);
                List<PostWithLastestComment> postlist = rawPostList.Skip(skip).Take(take).ToList();

                Debug.WriteLine(postlist.Count() + " end service " + DateTime.Now);
                if (postlist != null)
                {
                    List<PostViewModel> postviewlist = new List<PostViewModel>();
                    PostListViewModel postlistviewmodel = new PostListViewModel();
                    PostViewModel postmodel;
                    postlistviewmodel.postQuan = rawPostList.Count();
                    //Debug.WriteLine("sdf " + postlistviewmodel.postQuan);

                    foreach (PostWithLastestComment post in postlist)
                    {
                        postmodel = new PostViewModel();
                        postmodel.post = post;
                        Debug.WriteLine("last edit of post " + post.LastUpdate);
                        Debug.WriteLine("read of unread? " + post.IsRead);
                        Debug.WriteLine(post.Id + " start  fb " + DateTime.Now);
                        //post deleted

                        Debug.WriteLine("status = 5" + "__" + post.SenderFbId);
                        
                        //priority: message (attachment) > story (attachment) > attachment
                        postmodel.message = post.LastContent;

                        if (post.status != (int)CommentStatus.DELETED)
                        {
                            try
                            {
                                dynamic fbPost = fbApp.Get(post.Id, postParam);

                                postmodel.from = fbPost.from.name;
                                //priority: message (attachment) > story (attachment) > attachment
                                if (fbPost.message != null && fbPost.message != "") postmodel.message = fbPost.message;
                                //message = null => picture or activity
                                else
                                {

                                    postmodel.message = fbPost.story;
                                    Debug.WriteLine("story post " + postmodel.message);
                                }
                                if (fbPost.full_picture != null && fbPost.full_picture != "") postmodel.imageContent = fbPost.full_picture;
                            }
                            catch (FacebookApiException)
                            {
                                postservice.SetStatus(postmodel.post.Id, (int)CommentStatus.DELETED);
                                dynamic userParam = new ExpandoObject();
                                userParam.access_token = accessToken;
                                userParam.fields = "name";
                                dynamic fbUser = fbApp.Get(post.SenderFbId, userParam);
                                postmodel.from = fbUser.name;
                                postmodel.post.status = (int)CommentStatus.DELETED;
                            }
                        }
                        else
                        {
                            FacebookClient fbAppWithAccTok = new FacebookClient(accessToken);
                            dynamic userParam = new ExpandoObject();
                            userParam.access_token = accessToken;
                            userParam.fields = "name";
                            var fbUser = fbAppWithAccTok.Get(post.SenderFbId, userParam);
                            postmodel.from = fbUser.name;
                        }

                        Debug.WriteLine("test post " + postmodel.message);

                        //check all commments with post id;

                        //if (commentservice.CheckPostUnread(post.Id) == true)
                        //{
                        //    postservice.SetPostIsUnread(post.Id);
                        //    postmodel.post.IsRead = false;
                        //}
                        //else
                        //{
                        //    postservice.SetPostIsRead(post.Id);
                        //    postmodel.post.IsRead = true;
                        //}


                        postviewlist.Add(postmodel);
                    }
                    postlistviewmodel.postviewlist = postviewlist;
                    return Json(postlistviewmodel, JsonRequestBehavior.AllowGet);

                }
                else return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetPostDetail(string postId, int skip, int take)
        { //string shopId, int from, int quantity

            Debug.WriteLine("GetPostDetailFunction");
            //shopid = 685524511603937
            dynamic commentParam = new ExpandoObject();
            dynamic postParam = new ExpandoObject();


            try
            {
                string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
                //postParam.access_token = accessToken;
                //commentParam.access_token = accessToken;
                postParam.fields = "full_picture,created_time,message,from{picture,name},story";
                commentParam.fields = "attachment,from{picture,name,id},message,created_time,can_hide,can_reply_privately";
                postParam.locale = "vi_VI";
                FacebookClient fbAppWithAccTok = new FacebookClient(accessToken);
                Debug.WriteLine("a ccess tok " + accessToken);
                //commentParam.field = "from";
                //postId = "685524511603937_713357638820624";
                dynamic userParam = new ExpandoObject();
                userParam.access_token = accessToken;
                userParam.fields = "name";


                Post selectedPost = postservice.GetPost(postId, (string)Session["ShopId"]);
                if (selectedPost != null)
                {
                    //get post info

                    PostDetailModel postView = new PostDetailModel();
                    postView.SenderFbId = selectedPost.SenderFbId;
                    Debug.WriteLine("date of the post: " + selectedPost.DateCreated);
                    postView.LastUpdate = selectedPost.DateCreated;
                    postView.Id = postId;
                    postView.Status = selectedPost.Status;
                    postView.IntentId = selectedPost.IntentId;

                    if (postView.Status != (int)CommentStatus.DELETED)
                    {
                        try
                        {
                            var fbPost = fbAppWithAccTok.Get(postId, postParam);
                            postView.from = fbPost.from.name;
                            if (postView.IntentId == null) postView.fromAvatar = "https://graph.facebook.com/" + (string)Session["ShopId"] + "/picture?type=square";
                            else postView.fromAvatar = "https://graph.facebook.com/" + postView.SenderFbId + "/picture?type=square";
                            postView.postContent = fbPost.message;
                            postView.storyContent = fbPost.story;
                            //posted photo
                            postView.postImageContent = fbPost.full_picture;
                        }
                        catch (FacebookApiException)
                        {
                            postservice.SetStatus(postId, (int)CommentStatus.DELETED);
                            postView.fromAvatar = "https://graph.facebook.com/" + postView.SenderFbId + "/picture?type=square";
                            postView.postContent = selectedPost.LastContent;
                            postView.storyContent = null;
                            var fbUser = fbAppWithAccTok.Get(selectedPost.SenderFbId, userParam);
                            postView.from = fbUser.name;
                            postView.postImageContent = null;
                            postView.Status = (int)CommentStatus.DELETED;
                        }

                    }
                    else
                    {
                        postView.fromAvatar = "https://graph.facebook.com/" + postView.SenderFbId + "/picture?type=square";
                        postView.postContent = selectedPost.LastContent;
                        postView.storyContent = null;
                        var fbUser = fbAppWithAccTok.Get(selectedPost.SenderFbId, userParam);
                        postView.from = fbUser.name;
                        postView.postImageContent = null;
                    }
                    List<Comment>[] commentList = commentservice.GetCommentsWithPostId(postId, skip, take);

                    if (commentList != null)
                    {
                        List<CommentDetailModel> commentviewlist = new List<CommentDetailModel>();
                        CommentDetailModel commentdetailmodel;
                        List<CommentDetailModel> nestedcommentlist = new List<CommentDetailModel>();
                        for (var i = 0; i < commentList[0].Count; i++)
                        {
                            Debug.WriteLine("comment     " + commentList[0][i].Id);
                            commentdetailmodel = new CommentDetailModel();
                            commentdetailmodel.SenderFbId = commentList[0][i].SenderFbId;
                            commentdetailmodel.Id = commentList[0][i].Id;
                            commentdetailmodel.datacreated = commentList[0][i].DateCreated;
                            commentdetailmodel.Status = commentList[0][i].Status;
                            Debug.WriteLine("status of comment: " + commentdetailmodel.Status);
                            commentdetailmodel.IsRead = commentList[0][i].IsRead;
                            commentdetailmodel.IntentId = commentList[0][i].IntentId;
                            commentdetailmodel.nestedCommentQuan = commentservice.GetNestedCommentQuan(commentList[0][i].Id);
                            commentservice.SetIsRead(commentList[0][i].Id);

                            //Debug.WriteLine("status of comment is 5: " + commentdetailmodel.Status);
                            userParam.access_token = accessToken;
                            

                            commentdetailmodel.commentImageContent = null;
                            //commentdetailmodel.from = fbComment.from.name;
                            commentdetailmodel.commentContent = commentList[0][i].LastContent;
                            

                            SignalRAlert.AlertHub.SendNotification((string)Session["ShopId"]);

                            commentdetailmodel.avatarUrl = "https://graph.facebook.com/" + commentList[0][i].SenderFbId + "/picture?type=square";
                            //string commentText = @fbComment.message;
                            //commentdetailmodel.commentContent = commentText;commentdetailmodel.IntentId = commentList[0][i].IntentId;

                            if (commentdetailmodel.Status != (int)CommentStatus.DELETED)
                            {
                                try
                                {
                                    //Debug.WriteLine("status of comment is not 5: " + commentdetailmodel.Status);
                                    var fbComment = fbAppWithAccTok.Get(commentList[0][i].Id, commentParam);
                                    if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                                    else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                                    commentdetailmodel.from = fbComment.from.name;
                                    commentdetailmodel.commentContent = fbComment.message;
                                    commentdetailmodel.canHide = fbComment.can_hide;
                                    commentdetailmodel.canReply = fbComment.can_reply_privately;
                                }
                                catch (FacebookApiException)
                                {
                                    commentservice.SetStatus(commentdetailmodel.Id, (int)CommentStatus.DELETED);
                                    var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                    commentdetailmodel.from = fbUser.name;
                                    commentdetailmodel.Status = (int)CommentStatus.DELETED;
                                }

                            }
                            else
                            {
                                var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                commentdetailmodel.from = fbUser.name;
                            }
                            commentviewlist.Add(commentdetailmodel);
                        }
                        //get replies
                        for (var i = 0; i < commentList[1].Count; i++)
                        {
                            Debug.WriteLine("comment     " + commentList[1][i].Id);
                            commentdetailmodel = new CommentDetailModel();
                            commentdetailmodel.Id = commentList[1][i].Id;
                            commentdetailmodel.parentId = commentList[1][i].ParentId;
                            commentdetailmodel.datacreated = commentList[1][i].DateCreated;
                            commentdetailmodel.SenderFbId = commentList[1][i].SenderFbId;
                            commentdetailmodel.Status = commentList[1][i].Status;
                            commentdetailmodel.IsRead = commentList[1][i].IsRead;
                            commentdetailmodel.IntentId = commentList[1][i].IntentId;

                            commentservice.SetIsRead(commentList[1][i].Id);
                            SignalRAlert.AlertHub.SendNotification((string)Session["ShopId"]);
                            commentdetailmodel.isUnreadRepliesRemain = commentservice.CheckUnreadRemain(commentdetailmodel.parentId);
                            commentdetailmodel.avatarUrl = "https://graph.facebook.com/" + commentList[1][i].SenderFbId + "/picture?type=square";

                            userParam.access_token = accessToken;
                            

                            commentdetailmodel.commentContent = commentList[1][i].LastContent;
                            commentdetailmodel.commentImageContent = null;
                            

                            if (commentList[1][i].Status != (int)CommentStatus.DELETED)
                            {
                                try
                                {
                                    var fbComment = fbAppWithAccTok.Get(commentList[1][i].Id, commentParam);
                                    Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                                    commentdetailmodel.from = fbComment.from.name;
                                    commentdetailmodel.commentContent = fbComment.message;
                                    if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                                    else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                                    commentdetailmodel.canHide = fbComment.can_hide;
                                    commentdetailmodel.canReply = fbComment.can_reply_privately;
                                }
                                catch (FacebookApiException)
                                {
                                    commentservice.SetStatus(commentdetailmodel.Id, (int)CommentStatus.DELETED);
                                    var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                    commentdetailmodel.from = fbUser.name;
                                    commentdetailmodel.Status = (int)CommentStatus.DELETED;
                                }

                            }
                            else
                            {
                                var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                commentdetailmodel.from = fbUser.name;
                            }


                            nestedcommentlist.Add(commentdetailmodel);
                            //if (comment.ParentId != null && comment.ParentId != "")
                            //{
                            //    commentdetailmodel.parentId = comment.ParentId;
                            //    Debug.WriteLine("this.name      " + commentdetailmodel.from);
                            //    nestedcommentlist.Add(commentdetailmodel);
                            //}
                            //else commentviewlist.Add(commentdetailmodel);
                        }
                        postView.Comments = commentviewlist;
                        postView.nestedComments = nestedcommentlist;
                        postView.commentQuan = commentservice.GetParentCommentQuan(postId);
                        postView.isUnreadParentRemain = commentservice.CheckUnreadParentComment(postId);
                        //check post read state by browsing comments, true = unread
                        if (commentservice.CheckPostUnread(selectedPost.Id) == true)
                        {
                            postservice.SetPostIsUnread(selectedPost.Id);
                            postView.isRead = false;
                        }
                        else
                        {
                            postservice.SetPostIsRead(selectedPost.Id);
                            postView.isRead = true;
                            SignalRAlert.AlertHub.SendNotification((string)Session["ShopId"]);
                        }
                        Debug.WriteLine(postView.nestedComments.Count);
                        return Json(postView, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        postView.Comments = null;
                        if (commentservice.CheckPostUnread(postView.Id) == true)
                        {
                            postservice.SetPostIsUnread(postView.Id);
                            postView.isRead = false;
                        }
                        else
                        {
                            postservice.SetPostIsRead(postView.Id);
                            postView.isRead = true;
                        }
                        return Json(postView, JsonRequestBehavior.AllowGet);
                    }

                }

                else return null;
            }
            catch (Exception e)
            {
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPost(string postId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic postParam = new ExpandoObject();
            postParam.fields = "message,from,full_picture,story";
            postParam.locale = "vi_VI";
            //dynamic userParam = new ExpandoObject();
            //userParam.access_token = accessToken;
            //userParam.fields = "name";
            try
            {

                FacebookClient fbAppWithAccTok = new FacebookClient(accessToken);

                //commentParam.field = "from";
                //postId = "685524511603937_713357638820624";

                //get post info

                var fbPost = fbAppWithAccTok.Get(postId, postParam);

                PostViewModel postmodel = new PostViewModel();
                //Debug.WriteLine(post.Id + DateTime.Now);
                //string test = fbPost.sdfsdfds;
                //Debug.WriteLine("PostWithLastestComment " + post.Id);
                postmodel.from = fbPost.from.name;
                //priority: message (attachment) > story (attachment) > attachment
                if (fbPost.message != null && fbPost.message != "") postmodel.message = fbPost.message;
                //message = null => picture or activity
                else
                {
                    postmodel.message = fbPost.story;
                    Debug.WriteLine("story post " + postmodel.message);
                }
                if (fbPost.full_picture != null && fbPost.full_picture != "") postmodel.imageContent = fbPost.full_picture;

                Debug.WriteLine("test post " + postmodel.message);
                return Json(postmodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult ReplyComment(string commentId, string message)
        {
            try
            {
                string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
                dynamic param = new ExpandoObject();

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
        public JsonResult GetReplies(string parentId, int skip, int take)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            bool isUnreadRemain = false;
            bool postIsUnread = false;
            FacebookClient fbAppWithAccTok = new FacebookClient(accessToken);
            try
            {
                List<Comment> newcommentlist = commentservice.GetNestedCommentOfParent(parentId, skip, take);

                if (newcommentlist != null)
                {


                    List<CommentDetailModel> newcommentdetaillist = new List<CommentDetailModel>();
                    CommentDetailModel commentdetailmodel;
                    dynamic commentParam = new ExpandoObject();
                    commentParam.fields = "attachment,from{picture,name,id},message,created_time,can_hide,can_reply_privately";

                    foreach (Comment comment in newcommentlist)
                    {
                        Debug.WriteLine("comment     " + comment.Id);
                        commentdetailmodel = new CommentDetailModel();
                        commentdetailmodel.datacreated = comment.DateCreated;
                        commentdetailmodel.Id = comment.Id;
                        commentdetailmodel.parentId = comment.ParentId;
                        commentdetailmodel.SenderFbId = comment.SenderFbId;
                        commentdetailmodel.Status = comment.Status;
                        commentdetailmodel.IsRead = comment.IsRead;
                        commentdetailmodel.avatarUrl = "https://graph.facebook.com/" + commentdetailmodel.SenderFbId + "/picture?type=square";
                        commentdetailmodel.IntentId = comment.IntentId;


                        dynamic userParam = new ExpandoObject();
                        userParam.access_token = accessToken;
                        userParam.fields = "name";
                        //dynamic fbUser = fbApp.Get(comment.SenderFbId, userParam);
                        //commentdetailmodel.from = fbUser.name;
                        commentdetailmodel.commentContent = comment.LastContent;
                        Debug.WriteLine("time  " + commentdetailmodel.datacreated);


                        if (comment.Status != (int)CommentStatus.DELETED)
                        {
                            try
                            {
                                var fbComment = fbAppWithAccTok.Get(comment.Id, commentParam);
                                commentdetailmodel.from = fbComment.from.name;

                                if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                                else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                                commentdetailmodel.commentContent = fbComment.message;
                                commentdetailmodel.canHide = fbComment.can_hide;
                                commentdetailmodel.canReply = fbComment.can_reply_privately;
                            }
                            catch (FacebookApiException)
                            {
                                commentservice.SetStatus(commentdetailmodel.Id, (int)CommentStatus.DELETED);
                                var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                commentdetailmodel.from = fbUser.name;
                                commentdetailmodel.Status = (int)CommentStatus.DELETED;
                            }
                        }
                        else
                        {
                            var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                            commentdetailmodel.from = fbUser.name;
                        }

                        commentservice.SetIsRead(comment.Id);
                        SignalRAlert.AlertHub.SendNotification((string)Session["ShopId"]);
                        newcommentdetaillist.Add(commentdetailmodel);
                    }
                    Comment tmpcomment = commentservice.GetCommentById(parentId);
                    if (commentservice.CheckPostUnread(tmpcomment.PostId) == true)
                    {
                        postservice.SetPostIsUnread(tmpcomment.PostId);
                        postIsUnread = true;
                    }
                    else
                    {
                        postservice.SetPostIsRead(tmpcomment.PostId);
                        postIsUnread = false;
                        SignalRAlert.AlertHub.SendNotification((string)Session["ShopId"]);
                    }
                    isUnreadRemain = commentservice.CheckUnreadRemain(parentId);
                    return Json(new { newcommentdetaillist, isUnreadRemain = isUnreadRemain, postIsUnread = postIsUnread }, JsonRequestBehavior.AllowGet);
                }
                else return null;
            }
            catch (Exception e)
            {
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetCommentRead(string commentId)
        {
            try
            {
                Debug.WriteLine("id of new comment " + commentId);
                var result = commentservice.SetIsRead(commentId);
                Debug.WriteLine("and result " + result);
                SignalRAlert.AlertHub.SendNotification((string)Session["ShopId"]);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }

        }


        public JsonResult GetCommentById(string commentId)
        {
            Debug.WriteLine("ID COMMENT " + commentId);
            dynamic commentParam = new ExpandoObject();
            commentParam.fields = "attachment,from{picture,name,id},message,created_time";

            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            FacebookClient fbAppWithAccTok = new FacebookClient(accessToken);

            Comment tmpComment = commentservice.getCommentById(commentId);

            CommentDetailModel commentdetailmodel = new CommentDetailModel();
            commentdetailmodel.Id = tmpComment.Id;
            commentdetailmodel.parentId = tmpComment.ParentId;
            commentdetailmodel.Status = tmpComment.Status;
            commentdetailmodel.nestedCommentQuan = commentservice.GetNestedCommentQuan(commentId);
            commentdetailmodel.commentContent = tmpComment.LastContent;
            commentdetailmodel.datacreated = tmpComment.DateCreated;
            commentdetailmodel.SenderFbId = tmpComment.SenderFbId;

            try
            {


                if (tmpComment.Status != (int)CommentStatus.DELETED)
                {
                    try
                    {
                        var fbComment = fbAppWithAccTok.Get(tmpComment.Id, commentParam);


                        Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                        commentdetailmodel.commentContent = fbComment.message;
                        commentdetailmodel.from = fbComment.from.name;

                        if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                        else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                        commentdetailmodel.commentContent = fbComment.message;
                    }
                    catch (FacebookApiException)
                    {
                        commentservice.SetStatus(commentdetailmodel.Id, (int)CommentStatus.DELETED);
                        dynamic userParam = new ExpandoObject();
                        userParam.access_token = accessToken;
                        userParam.fields = "name";
                        var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                        commentdetailmodel.from = fbUser.name;
                        commentdetailmodel.Status = (int)CommentStatus.DELETED;
                    }
                }
                else
                {
                    dynamic userParam = new ExpandoObject();
                    userParam.access_token = accessToken;
                    userParam.fields = "name";
                    var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                    commentdetailmodel.from = fbUser.name;
                }

                commentdetailmodel.avatarUrl = "https://graph.facebook.com/" + commentdetailmodel.SenderFbId + "/picture?type=square";
                return Json(commentdetailmodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
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

        public JsonResult DeletePost(string postId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            fbApp.AccessToken = accessToken;
            dynamic param = new ExpandoObject();
            try
            {
                var result = fbApp.Delete(postId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetDeleteComment(string id)
        {
            var result = true;
            Debug.WriteLine("this is idddddd " + id);
            if (id.Split('_')[0] != (string)Session["ShopId"])
            {   //delete comment and its replies
                commentservice.SetStatus(id, 5);
                List<Comment> commentList = commentservice.GetAllCommentByParentId(id).ToList();
                if (commentList != null)
                {
                    foreach (Comment c in commentList)
                    {
                        commentservice.SetStatus(c.Id, (int)CommentStatus.DELETED);
                    }
                }
            }
            else //delete a post and its replies
            {
                postservice.SetStatus(id, 5);
                List<Comment> commentList = commentservice.GetCommentsOfPost(id).ToList();
                if (commentList != null)
                {
                    foreach (Comment c in commentList)
                    {
                        commentservice.SetStatus(c.Id, (int)CommentStatus.DELETED);
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public JsonResult HideComment(string commentId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            Debug.WriteLine(accessToken);
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

        public JsonResult HidePost(string postId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            Debug.WriteLine(accessToken);
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.is_hidden = true;
                var result = fbApp.Post(postId, param);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UnhidePost(string postId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            Debug.WriteLine(accessToken);
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.is_hidden = false;
                var result = fbApp.Post(postId, param);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetHideComment(string commentId)
        {
            var result = true;
            //hide comment
            try
            {
                if (commentId.Split('_')[0] != (string)Session["ShopId"])
                {
                    commentservice.SetStatus(commentId, 3);
                }
                else //hide a post
                {
                    postservice.SetStatus(commentId, 3);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult UnhideComment(string commentId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            Debug.WriteLine(accessToken);
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.is_hidden = false;
                var result = fbApp.Post(commentId, param);
                //hide comment
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SetUnhideComment(string commentId)
        {
            var result = true;
            //unhide a comment
            try
            {
                if (commentId.Split('_')[0] != (string)Session["ShopId"])
                {
                    commentservice.SetStatus(commentId, 1);
                }
                else //unhide a post
                {
                    postservice.SetStatus(commentId, 1);
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
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

            var first = result.data[0];

            param = new ExpandoObject();
            param.access_token = accessToken;
            param.fields = "participants";
            var r = fbApp.Get(c.Id, param);

            var id = "";
            var name = "";
            foreach (var p in r.participants.data)
            {
                if (p.id != shopId)
                {
                    id = p.id;
                    name = p.name;
                }
            }

            preview.UserName = name;
            preview.UserFbId = id;
            preview.CreatedTime = DateTime.Parse(first.created_time);
            preview.RecentMess = first.message;
            preview.IntentId = c.IntentId;

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
            param.fields = "from,created_time,message,attachments";
            param.locale = "vi-vn";
            fbApp.AccessToken = accessToken;
            dynamic result = fbApp.Get(conversationId + "/messages", param);

            foreach (var mess in result.data)
            {
                var mc = new MessageContentViewModel();
                mc.MessId = mess.id;
                mc.MessContent = mess.message;
                mc.DateCreated = DateTime.Parse(mess.created_time);
                mc.UserId = mess.from.id;
                mc.UserName = mess.from.name;

                if (mess.attachments != null)
                {
                    foreach (var att in mess.attachments.data)
                    {
                        string mime = att.mime_type;
                        string type = mime.Split('/')[0];
                        var attachment = new AttachmentViewModel();

                        if (type.Equals("image"))
                        {
                            attachment.Type = "img";
                            attachment.Url = att.image_data.url;
                        }
                        else
                        {
                            attachment.Type = "other";
                            attachment.Filename = att.name;
                            attachment.Url = att.file_url;
                        }

                        mc.Attachments.Add(attachment);
                    }
                }

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

        public ActionResult GetPageInfo()
        {
            string shopId = (string)Session["ShopId"];
            string accessToken = shopService.GetShop(shopId).FbToken;

            dynamic param = new ExpandoObject();
            param.access_token = accessToken;
            dynamic result = fbApp.Get(shopId + "/picture?type=normal&redirect=false", param);
            param = new ExpandoObject();
            param.access_token = accessToken;
            dynamic r2 = fbApp.Get(shopId, param);
            return Json(new { avatar = result.data.url, name = r2.name }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetConversationRead(string conversationId)
        {
            string shopId = (string)Session["ShopId"];
            var rs = conversationService.SetReadConversation(conversationId);
            if (rs)
            {
                AlertHub.SendNotification(shopId);
            }
            return Json(rs, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
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


        public JsonResult GetAvailableResponses(int intentId)
        {
            string shopId = (string)Session["ShopId"];
            EntityService entityService = new EntityService();
            var arrReps = new List<EntityViewModel>();

            try
            {
                //Get reps by Intent
                if (intentId != 1)
                {
                    IntentService intService = new IntentService();
                    var intName = intService.GetIntentNameById(intentId);

                    var repss = respService.GetAllResponseByIntent(shopId, intentId);
                    foreach (var en in repss)
                    {
                        if (en.RespondContent.Trim().Length > 0)
                        {
                            var e = new EntityViewModel();
                            e.Name = intName;
                            e.Value = en.RespondContent;
                            arrReps.Add(e);
                        }
                    }
                }

                //Get reps by sample reps
                var reps = entityService.GetAvailableEntities(shopId);

                foreach (var en in reps.ToList())
                {
                    if (en.Value.Trim().Length > 0) arrReps.Add(en);
                }

                return Json(arrReps.ToList(), JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SendMessage(string threadId, string message)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.message = message;
                dynamic result = fbApp.Post(threadId + "/messages", param);
                return Json(new { success = true, id = result.id }, JsonRequestBehavior.AllowGet);
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

        public JsonResult EditComment(string photo, string commentId, string message)
        {
            try
            {
                string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
                dynamic param = new ExpandoObject();
                Debug.WriteLine("photoooooooo " + photo);
                param.access_token = accessToken;
                param.message = message;
                if (photo != null) param.photo = photo;
                var result = fbApp.Post(commentId, param);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetConversationUser(string threadId)
        {
            try
            {
                string shopId = (string)Session["ShopId"];
                string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
                dynamic param = new ExpandoObject();
                param.access_token = accessToken;
                param.fields = "participants";
                var result = fbApp.Get(threadId, param);

                var id = "";
                var name = "";
                foreach (var p in result.participants.data)
                {
                    if (p.id != shopId)
                    {
                        id = p.id;
                        name = p.name;
                    }
                }

                return Json(new { id = id, name = name }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}