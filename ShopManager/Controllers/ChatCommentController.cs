using DataService;
using DataService.Service;
using DataService.ViewModel;
using Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManager.Controllers
{
    public class ChatCommentController: Controller
    {
        string PageId = "1198116243573212";
        CustomerService custService = new CustomerService();
        //var NumberofFeeds = 10;
        string token = "EAACSr2uHmRwBAIGm1Rb9oaHCmg8i2eoswZBFrZCUgr6F21gLgh60LwIU70mQLslj7kScnsZApUjZBxuguM8C1qNqePfOhk3NSguDZBQbZCQxywafEq58sBN87n9TwbppEtNMHywrpO6Xb8PbEz6rg7oH43mHhfXy8kp1j6yMovvQZDZD";

        public ActionResult Index()
        {
            Session["ShopId"] = "1";
            Session["CustId"] = "3";
            return View();
        }

        public JsonResult GetAllPosts(int skip, int take)
        { //string shopId, int from, int quantity
            CommentService commentservice = new CommentService();
            
            //shopid = 685524511603937
            FacebookClient fbApp = new FacebookClient(token);
            dynamic postParam = new ExpandoObject();
            postParam.fields = "message,from,full_picture,story";
            Debug.WriteLine("service " + DateTime.Now);
            List<PostWithLastestComment> postlist = commentservice.GetAllPost("1744339729172581", skip,take).ToList();
            Debug.WriteLine("end service " + DateTime.Now);
            if (postlist != null)
            {
                List<PostViewModel> postviewlist = new List<PostViewModel>();
                PostViewModel postmodel;
                foreach (PostWithLastestComment post in postlist)
                {
                    postmodel = new PostViewModel();
                    postmodel.post = post;
                    Debug.WriteLine(post.Id + " start  fb "  + DateTime.Now);
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
                return Json(postviewlist, JsonRequestBehavior.AllowGet);
            }
            else return null;
        }

        public JsonResult GetPostDetail(string postId)
        { //string shopId, int from, int quantity
            CommentService commentservice = new CommentService();
            //shopid = 685524511603937
            FacebookClient fbApp = new FacebookClient(token);
            dynamic commentParam = new ExpandoObject();
            dynamic postParam = new ExpandoObject();

            postParam.fields = "full_picture,created_time,message,from{picture,name},link,story";
            commentParam.fields = "attachment,from{picture,name,id},message,created_time";

            //commentParam.field = "from";
            //postId = "685524511603937_713357638820624";

            //get post info
            var fbPost = fbApp.Get(postId, postParam);
            PostDetailModel post = new PostDetailModel();
            post.Id = postId;
            post.LastUpdate = fbPost.created_time;
            post.toUrl = fbPost.link;
            post.from = fbPost.from.name;
            post.fromAvatar = fbPost.from.picture.data.url;
            post.postContent = fbPost.message;
            post.storyContent = fbPost.story;
            //posted photo
            post.postImageContent = fbPost.full_picture;
            post.from_userId = fbPost.from.id;

            //get comments info
            List<Comment> commentList = commentservice.GetCommentsOfPost(postId).ToList();

            if (commentList != null)
            {
                List<CommentDetailModel> commentviewlist = new List<CommentDetailModel>();
                CommentDetailModel commentdetailmodel;
                List<CommentDetailModel> nestedcommentlist = new List<CommentDetailModel>();
                foreach (Comment comment in commentList)
                {
                    Debug.WriteLine("comment     " + comment.Id);
                    commentdetailmodel = new CommentDetailModel();
                    commentdetailmodel.id = comment.Id;
                    var fbComment = fbApp.Get(comment.Id, commentParam);
                    commentdetailmodel.datacreated = fbComment.created_time;
                    Debug.WriteLine("time  " + commentdetailmodel.datacreated);
                    commentdetailmodel.commentContent = fbComment.message;
                    commentdetailmodel.from = fbComment.from.name;
                    commentdetailmodel.avatarUrl = fbComment.from.picture.data.url;
                    //string commentText = @fbComment.message;
                    //commentdetailmodel.commentContent = commentText;
                    if (fbComment.attachment == null) commentdetailmodel.commentImageContent = null;
                    else commentdetailmodel.commentImageContent = fbComment.attachment.media.image.src;
                    //Debug.WriteLine("commentdetailmodel.commentImageContent      " + commentdetailmodel.commentImageContent);
                    commentdetailmodel.fromId = fbComment.from.id;
                    if (comment.ParentId != null && comment.ParentId != "")
                    {
                        commentdetailmodel.parentId = comment.ParentId;
                        Debug.WriteLine("this.name      " + commentdetailmodel.from);
                        nestedcommentlist.Add(commentdetailmodel);
                    }
                    else commentviewlist.Add(commentdetailmodel);
                }
                post.comments = commentviewlist;
                post.nestedComments = nestedcommentlist;
                return Json(post, JsonRequestBehavior.AllowGet);
            }

            else
            {
                post.comments = null;
                return Json(post, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetUserFromDb(string userFbId)
        {
            string shopId = (string)Session["ShopId"];
            Customer cust = custService.GetCustomerByFacebookId(userFbId, "1744339729172581");
            if (cust == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(cust, JsonRequestBehavior.AllowGet);
        }
    }
}