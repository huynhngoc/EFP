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
        //string PageId = "685524511603937";
        CustomerService custService = new CustomerService();
        //var NumberofFeeds = 10;
        static string token = "EAAC1A8DIKmYBAEbxcQUGVQjsBoMCZAcPDzsBJNHviC7KnZCElb8lYYi62LlZAgLjQJbkUxCUS5hgPmCZCca9mokZAIOuDCZAeUoZAww8pmY70O47qGnHoE3ZBi2zsNG6nZA6tsFWc5ATBRt3GhJvDtZAuRzp5f573t8Q7FL2H89RB1ZCwZDZD";

        ShopService shopService = new ShopService();
        CommentService commentservice = new CommentService();
        FacebookClient fbApp = new FacebookClient(token);
        PostService postservice = new PostService();

        public ActionResult Index()
        {
            //Session["ShopId"] = "1";
            //Session["CustId"] = "3";
            dynamic shopOwnerParam = new ExpandoObject();
            shopOwnerParam.fields = "picture,name";

            dynamic shopFBUser = fbApp.Get((string)Session["ShopId"], shopOwnerParam);
            Session["ShopOwner"] = shopFBUser.name;
            Session["ShopAvatar"] = shopFBUser.picture.data.url;
            //Session["ShopId"] = (string)Session["ShopId"];
            return View();
        }

        public JsonResult GetAllPosts(int skip, int take)
        { //string shopId, int from, int quantity
            //shopid = 685524511603937
            string shopId = (string)Session["ShopId"];
            //Session["ShopId"] = "1744339729172581";


            //dynamic shopOwnerParam = new ExpandoObject();
            //shopOwnerParam.fields = "picture,name";
            //dynamic shopFBUser = fbApp.Get(shopId, shopOwnerParam);
            //Session["ShopOwner"] = shopFBUser.name;
            //Session["ShopAvatar"] = shopFBUser.picture.data.url;

            dynamic postParam = new ExpandoObject();
            postParam.fields = "message,from,full_picture,story";
            Debug.WriteLine("service " + DateTime.Now + " " + shopId);
            var rawPostList = commentservice.GetAllPost(shopId);
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
                    Debug.WriteLine(post.Id + " start  fb " + DateTime.Now);
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

                    Debug.WriteLine("test post " + postmodel.message);
                    postviewlist.Add(postmodel);
                }
                postlistviewmodel.postviewlist = postviewlist;
                return Json(postlistviewmodel, JsonRequestBehavior.AllowGet);
            }
            else return null;
        }

        //private Thread thread = null;
        //public JsonResult GetPostDetail(string postId)
        //{
        //    object value = null;
        //    if (thread!=null)
        //    { 
        //           thread.Abort();
        //    }
        //    thread = new Thread(
        //        () =>
        //        {
        //            value = GetPostDetailFunction(postId); // Publish the return value
        //        });
        //        thread.Start();
        //    Debug.WriteLine(value);
        //    thread.Join();
        //    Debug.WriteLine(value.ToString());
        //    return Json(value, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetPostDetail(string postId, int skip, int take)
        { //string shopId, int from, int quantity
            Debug.WriteLine("GetPostDetailFunction");
            //shopid = 685524511603937
            dynamic commentParam = new ExpandoObject();
            dynamic postParam = new ExpandoObject();

            postParam.fields = "full_picture,created_time,message,from{picture,name},story";
            commentParam.fields = "attachment,from{picture,name,id},message,created_time";

            //commentParam.field = "from";
            //postId = "685524511603937_713357638820624";

            Post selectedPost = postservice.GetPost(postId, (string)Session["ShopId"]);
            if (selectedPost != null)
            {
                //get post info
                var fbPost = fbApp.Get(postId, postParam);

                PostDetailModel postView = new PostDetailModel();
                postView.SenderFbId = selectedPost.SenderFbId;
                postView.LastUpdate = selectedPost.DateCreated;
                postView.Id = postId;
                
                postView.from = fbPost.from.name;
                postView.fromAvatar = fbPost.from.picture.data.url;
                postView.postContent = fbPost.message;
                postView.storyContent = fbPost.story;
                //posted photo
                postView.postImageContent = fbPost.full_picture;
               

                //get lastest comment
                //Comment lastestComment = commentservice.GetLastestComment(postId);
                //Debug.WriteLine(lastestComment.ParentId);
                //if lastest comment = nested comment then nested comment mode
                //if (lastestComment.ParentId != "" && lastestComment.ParentId != null)
                //{ 
                //post.lastestIsNested = true;
                //get comments info
                //List<Comment> commentList = commentservice.GetCommentsOfPost(postId, skip, take).ToList();
                List<Comment>[] commentList = commentservice.GetCommentsWithPostId(postId, skip, take);

                if (commentList != null)
                {
                    List<CommentDetailModel> commentviewlist = new List<CommentDetailModel>();
                    CommentDetailModel commentdetailmodel;
                    List<CommentDetailModel> nestedcommentlist = new List<CommentDetailModel>();
                    foreach (Comment comment in commentList[0])
                    {
                        Debug.WriteLine("comment     " + comment.Id);
                        commentdetailmodel = new CommentDetailModel();
                        commentdetailmodel.SenderFbId = comment.SenderFbId;
                        commentdetailmodel.Id = comment.Id;
                        commentdetailmodel.datacreated = comment.DateCreated;
                        commentdetailmodel.status = comment.Status;

                        //Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                        var fbComment = fbApp.Get(comment.Id, commentParam);
                        commentdetailmodel.commentContent = fbComment.message;
                        commentdetailmodel.from = fbComment.from.name;
                        commentdetailmodel.avatarUrl = fbComment.from.picture.data.url;
                        commentdetailmodel.nestedCommentQuan = commentservice.GetNestedCommentQuan(comment.Id);
                        //string commentText = @fbComment.message;
                        //commentdetailmodel.commentContent = commentText;
                        if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                        else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;

                        commentviewlist.Add(commentdetailmodel);
                    }
                    foreach (Comment comment in commentList[1])
                    {
                        Debug.WriteLine("comment     " + comment.Id);
                        commentdetailmodel = new CommentDetailModel();
                        commentdetailmodel.Id = comment.Id;
                        commentdetailmodel.parentId = comment.ParentId;
                        commentdetailmodel.datacreated = comment.DateCreated;
                        commentdetailmodel.SenderFbId = comment.SenderFbId;
                        commentdetailmodel.status = comment.Status;

                        var fbComment = fbApp.Get(comment.Id, commentParam);
                        Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                        commentdetailmodel.commentContent = fbComment.message;
                        commentdetailmodel.from = fbComment.from.name;
                        commentdetailmodel.avatarUrl = fbComment.from.picture.data.url;
                        //string commentText = @fbComment.message;
                        //commentdetailmodel.commentContent = commentText;
                        if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                        else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                        //Debug.WriteLine("commentdetailmodel.commentImageContent      " + commentdetailmodel.commentImageContent);

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
            dynamic postParam = new ExpandoObject();
            postParam.fields = "message,from,full_picture,story";
            //commentParam.field = "from";
            //postId = "685524511603937_713357638820624";

            //get post info
            var fbPost = fbApp.Get(postId, postParam);


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


        public JsonResult ReplyComment(string commentId, string message)
        {
            string accessToken = (shopService.GetShop((string)Session["ShopId"])).FbToken;
            dynamic param = new ExpandoObject();
            try
            {
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
            List<Comment> newcommentlist = commentservice.GetNestedCommentOfParent(parentId, skip, take);
            if (newcommentlist != null)
            {
                List<CommentDetailModel> newcommentdetaillist = new List<CommentDetailModel>();
                CommentDetailModel commentdetailmodel;
                dynamic commentParam = new ExpandoObject();
                commentParam.fields = "attachment,from{picture,name,id},message,created_time";
                foreach (Comment comment in newcommentlist)
                {
                    Debug.WriteLine("comment     " + comment.Id);
                    commentdetailmodel = new CommentDetailModel();
                    commentdetailmodel.datacreated = comment.DateCreated;
                    commentdetailmodel.Id = comment.Id;
                    commentdetailmodel.parentId = comment.ParentId;
                    commentdetailmodel.SenderFbId = comment.SenderFbId;
                    commentdetailmodel.status = comment.Status;

                    var fbComment = fbApp.Get(comment.Id, commentParam);
                    
                    Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                    commentdetailmodel.commentContent = fbComment.message;
                    commentdetailmodel.from = fbComment.from.name;
                    commentdetailmodel.avatarUrl = fbComment.from.picture.data.url;
                    //string commentText = @fbComment.message;
                    //commentdetailmodel.commentContent = commentText;
                    if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                    else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                    //Debug.WriteLine("commentdetailmodel.commentImageContent      " + commentdetailmodel.commentImageContent);
                    
                    newcommentdetaillist.Add(commentdetailmodel);
                }
                return Json(newcommentdetaillist, JsonRequestBehavior.AllowGet);
            }
            else return null;
        }

        public JsonResult GetCommentById(string commentId)
        {
            Debug.WriteLine("ID COMMENT " + commentId);
            dynamic commentParam = new ExpandoObject();
            commentParam.fields = "attachment,from{picture,name,id},message,created_time";
            Comment newcomment = commentservice.getCommentById(commentId);

            CommentDetailModel commentdetailmodel = new CommentDetailModel();
            commentdetailmodel.Id = newcomment.Id;
            commentdetailmodel.parentId = newcomment.ParentId;
            commentdetailmodel.status = newcomment.Status;

            var fbComment = fbApp.Get(newcomment.Id, commentParam);

            commentdetailmodel.datacreated = newcomment.DateCreated;
            Debug.WriteLine("time  " + commentdetailmodel.datacreated);
            commentdetailmodel.commentContent = fbComment.message;
            commentdetailmodel.from = fbComment.from.name;
            commentdetailmodel.avatarUrl = fbComment.from.picture.data.url;
            //string commentText = @fbComment.message;
            //commentdetailmodel.commentContent = commentText;
            if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
            else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
            commentdetailmodel.nestedCommentQuan = commentservice.GetNestedCommentQuan(commentId);
            //Debug.WriteLine("commentdetailmodel.commentImageContent      " + commentdetailmodel.commentImageContent);
            commentdetailmodel.SenderFbId = fbComment.from.id;
            return Json(commentdetailmodel, JsonRequestBehavior.AllowGet);
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
    }
}
