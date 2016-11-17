using DataService;
using DataService.Service;
using DataService.ViewModel;
using Facebook;
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
        //FacebookClient fbApp = new FacebookClient();
        string tmpPost = null;
        List<string> tmpCommentList = null;
        //string PageId = "685524511603937";
        //CustomerService custService = new CustomerService();
        //var NumberofFeeds = 10;
        //static string token = "EAAC1A8DIKmYBANZAleY9H1hY3Tb85HvHMH4ZAg6mHOZCusTRmBzQAESZAxwiIlUC3KhH103OpvUcJynkZBZBascE4KDbaWj76e6HvDiYfgfsn7U0po7FsKx1JOYEhpie1LK6YpbhJJFHSE0dYqNaRRrNRTQMfesBHHry0XlT2ssgZDZD";

        //ShopService shopService = new ShopService();
        CommentService commentservice = new CommentService();
        FacebookClient fbApp = new FacebookClient();
        PostService postservice = new PostService();
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
                        if (post.status == 5)
                        {
                            Debug.WriteLine("status = 5" + "__" + post.SenderFbId);
                            dynamic userParam = new ExpandoObject();
                            userParam.access_token = accessToken;
                            userParam.fields = "name";
                            dynamic fbUser = fbApp.Get(post.SenderFbId, userParam);
                            postmodel.from = fbUser.name;
                            //priority: message (attachment) > story (attachment) > attachment
                            postmodel.message = post.LastContent;
                        }
                        else
                        {
                            dynamic fbPost = fbApp.Get(post.Id, postParam);

                            Debug.WriteLine(post.Id + DateTime.Now);
                            //string test = fbPost.sdfsdfds;
                            Debug.WriteLine("PostWithLastestComment " + post.Id);
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
                        Debug.WriteLine("test post " + postmodel.message);
                        //check all commments with post id;
                        if (commentservice.CheckPostUnread(post.Id) == true) postservice.SetPostIsUnread(post.Id);
                        else postservice.SetPostIsRead(post.Id);
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
                commentParam.fields = "attachment,from{picture,name,id},message,created_time,can_hide";
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

                    if (postView.Status != 5)
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

                            commentdetailmodel.avatarUrl = "https://graph.facebook.com/" + commentList[0][i].SenderFbId + "/picture?type=square";
                            //string commentText = @fbComment.message;
                            //commentdetailmodel.commentContent = commentText;commentdetailmodel.IntentId = commentList[0][i].IntentId;

                            if (commentdetailmodel.Status != 5)
                            {
                                Debug.WriteLine("status of comment is not 5: " + commentdetailmodel.Status);
                                var fbComment = fbAppWithAccTok.Get(commentList[0][i].Id, commentParam);
                                if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                                else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                                commentdetailmodel.from = fbComment.from.name;
                                commentdetailmodel.commentContent = fbComment.message;
                                commentdetailmodel.canHide = fbComment.can_hide;
                            }
                            else
                            {
                                Debug.WriteLine("status of comment is 5: " + commentdetailmodel.Status);
                                userParam.access_token = accessToken;
                                var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                commentdetailmodel.commentImageContent = null;
                                //commentdetailmodel.from = fbComment.from.name;
                                commentdetailmodel.commentContent = commentList[0][i].LastContent;
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
                            commentdetailmodel.isUnreadRepliesRemain = commentservice.CheckUnreadRemain(commentdetailmodel.parentId);
                            commentdetailmodel.avatarUrl = "https://graph.facebook.com/" + commentList[1][i].SenderFbId + "/picture?type=square";

                            if (commentList[1][i].Status != 5)
                            {
                                var fbComment = fbAppWithAccTok.Get(commentList[1][i].Id, commentParam);
                                Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                                commentdetailmodel.from = fbComment.from.name;
                                commentdetailmodel.commentContent = fbComment.message;
                                if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                                else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                                commentdetailmodel.canHide = fbComment.can_hide;
                            }
                            else
                            {
                                userParam.access_token = accessToken;
                                var fbUser = fbAppWithAccTok.Get(commentdetailmodel.SenderFbId, userParam);
                                commentdetailmodel.commentContent = commentList[1][i].LastContent;
                                commentdetailmodel.commentImageContent = null;
                                commentdetailmodel.from = fbUser.name;

                                //commentdetailmodel.from = fbComment.from.name;
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
                        }
                        Debug.WriteLine(postView.nestedComments.Count);
                        return Json(postView, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        postView.Comments = null;
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

        public ActionResult GetPost(string postId)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic postParam = new ExpandoObject();
            postParam.fields = "message,from,full_picture,story";

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
                    commentParam.fields = "attachment,from{picture,name,id},message,created_time,can_hide";

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


                        Debug.WriteLine("time  " + commentdetailmodel.datacreated);


                        if (comment.Status != 5)
                        {
                            var fbComment = fbAppWithAccTok.Get(comment.Id, commentParam);
                            commentdetailmodel.from = fbComment.from.name;

                            if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                            else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                            commentdetailmodel.commentContent = fbComment.message;
                            commentdetailmodel.canHide = fbComment.can_hide;
                        }
                        else
                        {
                            dynamic userParam = new ExpandoObject();
                            userParam.access_token = accessToken;
                            userParam.fields = "name";
                            dynamic fbUser = fbApp.Get(comment.SenderFbId, userParam);
                            commentdetailmodel.from = fbUser.name;
                            commentdetailmodel.commentContent = comment.LastContent;
                        }
                        commentservice.SetIsRead(comment.Id);
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
                var result = commentservice.SetIsRead(commentId);
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


            try
            {
                var fbComment = fbAppWithAccTok.Get(tmpComment.Id, commentParam);

                commentdetailmodel.datacreated = tmpComment.DateCreated;
                Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                commentdetailmodel.commentContent = fbComment.message;
                commentdetailmodel.from = fbComment.from.name;

                //string commentText = @fbComment.message;
                //commentdetailmodel.commentContent = commentText;
                if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                commentdetailmodel.nestedCommentQuan = commentservice.GetNestedCommentQuan(commentId);
                //Debug.WriteLine("commentdetailmodel.commentImageContent      " + commentdetailmodel.commentImageContent);
                commentdetailmodel.SenderFbId = fbComment.from.id;

                if (tmpComment.Status != 5)
                {
                    commentdetailmodel.commentContent = fbComment.message;
                }
                else
                {
                    commentdetailmodel.commentContent = tmpComment.LastContent;
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
                        commentservice.SetStatus(c.Id, 5);
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
                        commentservice.SetStatus(c.Id, 5);
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
                    List<Comment> commentList = commentservice.GetAllCommentByParentId(commentId).ToList();
                    Debug.WriteLine(commentList.Count + "  commentttttt nummmmmmmmmm");
                    if (commentList != null)
                    {
                        foreach (Comment c in commentList)
                        {
                            result = commentservice.SetStatus(c.Id, 3);
                            if (result == false) return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else //hide a post
                {
                    postservice.SetStatus(commentId, 3);
                    List<Comment> commentList = commentservice.GetCommentsOfPost(commentId).ToList();
                    Debug.WriteLine(commentList.Count + "  commentttttt");
                    foreach (Comment c in commentList)
                    {
                        result = commentservice.SetStatus(c.Id, 3);
                        if (result == false) return Json(result, JsonRequestBehavior.AllowGet);
                    }
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
                    List<Comment> commentList = commentservice.GetAllCommentByParentId(commentId).ToList();
                    if (commentList != null)
                    {
                        foreach (Comment c in commentList)
                        {
                            result = commentservice.SetStatus(c.Id, 1);
                            if (result == false) return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else //unhide a post
                {
                    postservice.SetStatus(commentId, 1);
                    List<Comment> commentList = commentservice.GetCommentsOfPost(commentId).ToList();
                    if (commentList != null)
                    {
                        foreach (Comment c in commentList)
                        {
                            result = commentservice.SetStatus(c.Id, 1);
                            if (result == false) return Json(result, JsonRequestBehavior.AllowGet);
                        }
                    }
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

                int i = 0;
                dynamic first = result.data[0];
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
                preview.CreatedTime = DateTime.Parse(first.created_time);
                preview.RecentMess = first.message;

                param = new ExpandoObject();
                param.access_token = accessToken;
                param.fields = "";
                param.limit = 1;
                string url = preview.UserFbId + "/picture?type=normal&redirect=false";
                dynamic result2 = fbApp.Get(url, param);
                preview.AvatarUrl = result2.data.url;

            return preview;
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
            var rs = conversationService.SetReadConversation(conversationId);

            return Json(rs, JsonRequestBehavior.AllowGet);
        }

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

        public JsonResult SendMessage(string threadId, string message)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
                param.access_token = accessToken;
                param.message = message;
                var result = fbApp.Post(threadId + "/messages", param);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                return Json(new { success = false, e.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult PrivateReplyComment(string commentId, string message)
        {

            try
            {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
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
    }
}