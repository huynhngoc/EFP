using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class CommentRepository: BaseRepository<Comment>
    {
        int postNum = 0;

        public IQueryable<PostWithLastestComment> GetAllPost(string shopId, int from, int quantity)
        {
            if (postNum == 0)
            {
                var result = dbSet.Where(q => q.PostId.Contains(shopId)).AsEnumerable().Select(q => new Comment()
                {
                    PostId = q.PostId,
                    Id = q.Id,
                    IsRead = q.IsRead,
                    DateCreated = q.DateCreated
                }).GroupBy(g => g.PostId).Select(g => new PostWithLastestComment()
                {
                    Id = g.Key,
                    LastUpdate = g.Max(x => x.DateCreated),
                    IsRead = g.All(x => x.IsRead == true),
                    commentCount = g.Count(),
                    //newestCommentId = g.Select(x => x.Id),
                }).OrderByDescending(q => q.LastUpdate);
                postNum = result.Count();
                Debug.WriteLine("number of post" + postNum);
                var returnResult = result.Skip(from).Take(quantity);
                return returnResult.AsQueryable();
            }

            //last post returned

            //else if (postNum <= from) return null;
            //else if (postNum > from)
            //{
            //    var result = dbSet.Where(q => q.ShopId == shopId).AsEnumerable().Select(q => new Post_Comment()
            //    {
            //        PostId = shopId + "_" + q.Id.Split('_')[0].ToString(),
            //        CommentId = q.Id,
            //        IsRead = q.IsRead,
            //        DateCreated = q.DateCreated
            //    }).GroupBy(g => g.PostId).Select(g => new Post()
            //    {
            //        Id = g.Key,
            //        LastUpdate = g.Max(x => x.DateCreated),
            //        IsRead = g.All(x => x.IsRead == true)
            //    }).OrderByDescending(q => q.LastUpdate).Skip(from).Take(quantity);
            //    return result.AsQueryable();
            //}
           
            else return null;
        }


        //public IEnumerable<Comment> GetCommentsByShopId(string shopId)
        //{
        //    Debug.WriteLine("-----id_in " + shopId);
        //    Debug.WriteLine("asdasdasd" + dbSet.Where(q => q.ShopId == shopId).ToString());
        //    //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
        //    return dbSet.Where(q => q.ShopId == shopId).OrderByDescending(q => q.DateCreated);
        //}

        public IEnumerable<Comment> GetCommentsContainPostId(string postId)
        {
            string searchString = postId.Split('_')[1] + "_";
            Debug.WriteLine("search string" + searchString);
            Debug.WriteLine("-----id_in " + postId);
            Debug.WriteLine("asdasdasd " + dbSet.Where(q => q.Id.Contains(searchString)).OrderBy(q => q.DateCreated));
            //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
            return dbSet.Where(q => q.Id.Contains(searchString)).OrderBy(q => q.DateCreated);
        }

        //public Comment GetCommentById(string comId)
        //{
        //    Debug.WriteLine("-----id_in " + comId);
        //    Debug.WriteLine("asdasdasd" + dbSet.Where(q => q.ShopId == comId).ToString());
        //    //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
        //    return (Comment)dbSet.Where(q => q.Id == comId).FirstOrDefault();
        //}



        /// <summary>
        /// return list of post of a page
        /// </summary>
        /// <param name="fbApp"></param>
        /// <param name="shopId"></param>
        /// <returns></returns>

        //public List<PostViewModel> getPostList(string shopId)
        //{
        //    List<Comment> commentlist = GetCommentsByShopId("685524511603937").ToList();
        //    List<PostViewModel> postList = new List<PostViewModel>();
        //    if (commentlist == null) { return null; }
        //    else
        //    {   //get post
        //        foreach (Comment comment in commentlist)
        //        {
        //            //is post
        //            if (comment.Id.Contains(shopId + "_"))
        //            {
        //                dynamic commentParam = new ExpandoObject();
        //                commentParam.field = "from";
        //                var fbPostFrom = fbApp.Get(comment.Id, commentParam);


        //                PostViewModel post = new PostViewModel();
        //                post.postId = comment.Id;
        //                post.createdate = comment.DateCreated.ToString();
        //                post.poster = fbPostFrom.from.name;
        //                commentParam.field = "message";
        //                var fbPostDetail = fbApp.Get(comment.Id, commentParam);
        //                post.message = fbPostDetail.message;
        //                postList.Add(post);
        //            }
        //        }
        //        //for each post
        //        foreach (PostViewModel post in postList)
        //        {
        //            //find comment of each post
        //            //shopID_postId
        //            string fullPostId = post.postId;
        //            //get postId
        //            string[] slicedPostId = fullPostId.Split('_');
        //            List<Comment> nestedCommentList = GetCommentsContainPostId(slicedPostId[1]).ToList();
        //            Debug.WriteLine(nestedCommentList.Count);
        //            Debug.WriteLine(post.postId);
        //            if (nestedCommentList.Count != 0)
        //            {
        //                Debug.WriteLine("fking date" + nestedCommentList[nestedCommentList.Count() - 1].DateCreated.ToString());
        //                post.lastupdatedate = nestedCommentList[nestedCommentList.Count() - 1].DateCreated.ToString();
        //            }
        //            else post.lastupdatedate = post.createdate;
        //            post.commentlist = nestedCommentList;
        //        }
        //    }
        //    return postList.OrderByDescending(e => e.lastupdatedate).ToList();
        //}
        //public void getCommentsByPostList(List<PostViewModel> postlist)
        //{
        //    var param = "/me/feed";
        //    dynamic commentParam = new ExpandoObject();
        //    commentParam.field = "message";
        //    var fbComments = fbApp.Get(param, commentParam);
        //        foreach (var post in postlist)
        //    {

        //            foreach (var comment in (JsonArray)fbComments["data"])
        //            {
        //                model = new PostViewModel();
        //                Debug.WriteLine(model.postId);
        //                model.postId = (string)(((JsonObject)post)["id"]);
        //                model.commentlist = null;
        //                //Name = (string)(((JsonObject)friend)["name"])
        //                list.Add(model);
        //            }

        //    }
        //}

        //public List<PostViewModel> GetPostListByComment(IEnumerable<Comment> comments, FacebookClient fbApp)
        //{
        //    //ID1+"_"+ID2;
        //    //var param = "me/feed?fields=comments";
        //    ////CommentModel posts;
        //    //var PageId = "1198116243573212";
        //    ////var NumberofFeeds = 10;
        //    //var token = "EAACEdEose0cBAKBa3Q8DQaZB89udkjK2s7CLRZBTFtAz6aGZCOWuyZCfctWYZBLq5AgIYdOQXAXsliNNsrJEXtMcv3BTqi3JzuA8HYIsYJXVQ926I5wdgeSGVLMVZA8NFShbrw5W57HOr0MgDZAbuj1bZBQEL5PFj7Lze9GtT4BzbhNuBsZAzAVsK";
        //    //var app_sec = "6f12cf3fea94817f0c4fd6842437d089";
        //    //var shopId = "685524511603937";
        //    //parameters.access_token = token;

        //    //parameters.app_id = ConfigurationManager.AppSettings["FbAppId"];

        //    //FacebookClient fbApp = new FacebookClient(token);
        //    //dynamic parameters = new ExpandoObject();
        //    ////parameters.access_token = token;
        //    ////parameters.app_id = ConfigurationManager.AppSettings["FbAppId"];
        //    //parameters.app_id = PageId;
        //    ////parameters.access_token = token;



        //    dynamic commentParam = new ExpandoObject();
        //    commentParam.field = "message";
        //    var fbComments = fbApp.Get("685524511603937/feed", commentParam);


        //    PostViewModel post;
        //    List<PostViewModel> postlist = new List<PostViewModel>();
        //    List<Comment> commentList = comments.ToList();
        //    List<Comment> nestedcommentList = null;
        //    //brwose comment list retrieved from server
        //    for (var a = 0; a < commentList.Count(); a++)
        //    {
        //        //
        //        for (var b = 0; b < fbComments.data.Count(); b++)
        //        {
        //            //comment id in database = comment id on fb => post
        //            if (commentList[a].Id.Equals(fbComments.data[b].id))
        //            {
        //                postlist.Add(new PostViewModel(commentList[a].Id, commentList[a].DateCreated.ToString(), null, null, null, true));
        //                post = new PostViewModel();
        //                post.postId = commentList[a].Id;
        //                nestedcommentList = new List<Comment>();

        //            }

        //        }
        //        dynamic fbresult = fbApp.Get(getParam(commentList[a - 1].Id, commentList[a].Id));
        //        //find post first
        //        if (fbresult.data != null)
        //        {
        //            for (var b = 0; b < fbresult.data.Count; b++)
        //            {
        //                string[] ComId = fbresult.data[b].id.split("_");
        //                if (ComId[0].Equals(commentList[a - 1].Id))
        //                {
        //                    nestedcommentList.Add(GetCommentById(ComId[1]));
        //                }
        //            }
        //        }
        //        // this comment is a post bcause no nested comment
        //        else
        //        {
        //            postlist.Add(new PostViewModel(commentList[a].Id, commentList[a].DateCreated.ToString(), null, null, null, true));
        //            post = new PostViewModel();
        //            post.postId = commentList[a].Id;
        //            nestedcommentList = new List<Comment>();
        //        }

        //    }
        //    return null;
        //}


    }
}

