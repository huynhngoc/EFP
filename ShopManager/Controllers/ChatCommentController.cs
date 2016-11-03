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
    public class ChatCommentController: Controller
    {
        //string PageId = "685524511603937";
        CustomerService custService = new CustomerService();
        //var NumberofFeeds = 10;
        string token = "EAAC1A8DIKmYBAEbxcQUGVQjsBoMCZAcPDzsBJNHviC7KnZCElb8lYYi62LlZAgLjQJbkUxCUS5hgPmCZCca9mokZAIOuDCZAeUoZAww8pmY70O47qGnHoE3ZBi2zsNG6nZA6tsFWc5ATBRt3GhJvDtZAuRzp5f573t8Q7FL2H89RB1ZCwZDZD";


        public ActionResult Index()
        {
            //Session["ShopId"] = "1";
            //Session["CustId"] = "3";
            dynamic shopOwnerParam = new ExpandoObject();
            shopOwnerParam.fields = "picture,name";
            FacebookClient fbApp = new FacebookClient("EAAC1A8DIKmYBAEbxcQUGVQjsBoMCZAcPDzsBJNHviC7KnZCElb8lYYi62LlZAgLjQJbkUxCUS5hgPmCZCca9mokZAIOuDCZAeUoZAww8pmY70O47qGnHoE3ZBi2zsNG6nZA6tsFWc5ATBRt3GhJvDtZAuRzp5f573t8Q7FL2H89RB1ZCwZDZD");
            dynamic shopFBUser = fbApp.Get((string)Session["ShopId"], shopOwnerParam);
            Session["ShopOwner"] = shopFBUser.name;
            Session["ShopAvatar"] = shopFBUser.picture.data.url;
            Session["ShopId"] = (string)Session["ShopId"];
            return View();
        }

        public JsonResult GetAllPosts(int skip, int take)
        { //string shopId, int from, int quantity
            CommentService commentservice = new CommentService();
            //shopid = 685524511603937
            string shopId = (string)Session["ShopId"];
            //Session["ShopId"] = "1744339729172581";
            FacebookClient fbApp = new FacebookClient(token);
            

            //dynamic shopOwnerParam = new ExpandoObject();
            //shopOwnerParam.fields = "picture,name";
            //dynamic shopFBUser = fbApp.Get(shopId, shopOwnerParam);
            //Session["ShopOwner"] = shopFBUser.name;
            //Session["ShopAvatar"] = shopFBUser.picture.data.url;

            dynamic postParam = new ExpandoObject();
            postParam.fields = "message,from,full_picture,story";
            Debug.WriteLine("service " + DateTime.Now + " "+shopId);
            var rawPostList = commentservice.GetAllPost(shopId);
            List <PostWithLastestComment> postlist = rawPostList.Skip(skip).Take(take).ToList();

           


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

        public JsonResult GetPostDetail(string postId)
        { //string shopId, int from, int quantity
            Debug.WriteLine("GetPostDetailFunction");
            CommentService commentservice = new CommentService();
            //shopid = 685524511603937
            FacebookClient fbApp = new FacebookClient(token);
            dynamic commentParam = new ExpandoObject();
            dynamic postParam = new ExpandoObject();

            postParam.fields = "full_picture,created_time,message,from{picture,name},story";
            commentParam.fields = "attachment,from{picture,name,id},message,created_time";

            //commentParam.field = "from";
            //postId = "685524511603937_713357638820624";

            //get post info
            var fbPost = fbApp.Get(postId, postParam);
            PostDetailModel post = new PostDetailModel();
            post.Id = postId;
            post.LastUpdate = fbPost.created_time;
            post.from = fbPost.from.name;
            post.fromAvatar = fbPost.from.picture.data.url;
            post.postContent = fbPost.message;
            post.storyContent = fbPost.story;
            //posted photo
            post.postImageContent = fbPost.full_picture;
            post.SenderFbId = fbPost.from.id;

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
                    commentdetailmodel.SenderFbId = fbComment.from.id;
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