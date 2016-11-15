using DataService.JqueryDataTable;
using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class CommentRepository : BaseRepository<Comment>
    {
        public CommentRepository() : base()
        {

        }
        //public IQueryable<PostWithLastestComment> GetAllPost(string shopId)
        //{
            
        //    var result = dbSet.Where(q => (q.PostId.Contains(shopId)) && (q.Status!=5)).AsEnumerable().Select(q => new Comment()
        //    {
        //        PostId = q.PostId,
        //        Id = q.Id,
        //        IsRead = q.IsRead,
        //        DateCreated = q.DateCreated
        //    }).GroupBy(g => g.PostId).Select(g => new PostWithLastestComment()
        //    {
        //        Id = g.Key,
        //        LastUpdate =  g.Max(x => x.DateCreated),
        //        IsRead = g.All(x => x.IsRead == true),
        //        commentCount = g.Count(),
        //        //newestCommentId = g.Where(x=>x.DateCreated = g.Max((DateTime)x.DateCreated))
        //    }).OrderByDescending(q => q.LastUpdate);

        //    return result.AsQueryable();

        //}
        public bool CheckUnreadRemain(string parentId)
        {
            return dbSet.Where(q => q.ParentId == parentId).Any(q => q.IsRead == false);
        }

        public bool CheckPostUnread(string postId)
        {
            //true = unread
            var post = dbSet.Where(q => q.PostId == postId).Any(q => q.IsRead == false);
            return post;
        }

        public Comment getCommentById(string commentId)
        {
            return dbSet.Where(q => q.Id == commentId).FirstOrDefault();
        }

        public IQueryable<Comment> GetCommentsContainPostId(string postId)
        {
            //Debug.WriteLine("search string" + searchString);
            Debug.WriteLine("-----id_in " + postId);
            //Debug.WriteLine("asdasdasd " + dbSet.Where(q => q.Id.Contains(searchString)).OrderByDescending(q => q.DateCreated));
            //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
            //return dbSet.Where(q => (q.PostId == postId) && (q.Status != 5)).OrderByDescending(q=>q.DateCreated);
            //return dbSet.Where(q => (q.PostId == postId) && (q.Status != 5)).OrderByDescending(q => q.DateCreated);
            return dbSet.Where(q => (q.PostId == postId)).OrderByDescending(q => q.DateCreated);
        }

        public int GetNestedCommentQuan(string commentId)
        {
            return dbSet.Where(q => (q.ParentId == commentId) && (q.Status!=5)).Count();
        }

        public IQueryable<Comment> GetNestedCommentOfParent(string commentId,int skip, int take)
        {
            //return dbSet.Where(q => (q.ParentId == commentId) && (q.Status != 5)).AsEnumerable().OrderByDescending(q=>q.DateCreated).Skip(skip).Take(take).AsQueryable();
            return dbSet.Where(q => (q.ParentId == commentId)).AsEnumerable().OrderByDescending(q => q.DateCreated).Skip(skip).Take(take).AsQueryable();
        }

        public List<Comment>[] GetCommentsWithPostId(string postId, int  skip, int take)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            List<Comment>[] result = new List<Comment>[2];
            var comments = GetCommentsContainPostId(postId);
            //IQueryable<Comment> parentComment = comments.Where(q => (q.ParentId == null) && (q.Status!=5)).OrderByDescending(q =>  q.Comments1.Count>0 ?  q.Comments1.Max(x => x.DateCreated): q.DateCreated).Skip(skip).Take(take);
            IQueryable<Comment> parentComment = comments.Where(q => (q.ParentId == null)).OrderByDescending(q => q.Comments1.Count > 0 ? q.Comments1.Max(x => x.DateCreated) : q.DateCreated).Skip(skip).Take(take);
            List<Comment> nestedComment = new List<Comment>();
            foreach (Comment c in parentComment)
            {
                nestedComment.AddRange(GetNestedCommentOfParent(c.Id, 0, 1).ToList());
            }
            result[0] = parentComment.ToList();
            result[1] = nestedComment;
            return result;
        }
        public Comment GetLastestComment(string postId)
        {
            return dbSet.OrderByDescending(q => q.DateCreated).FirstOrDefault();
        }
        public int GetParentCommentQuan(string postId)
        {
            return dbSet.Where(q => (q.PostId == postId) && (q.ParentId == null)).Count();
        }

        public bool SetIsRead(string commentId)
        {
            try
            {
                var comment = dbSet.Where(q => q.Id == commentId).FirstOrDefault();
                if (comment.IsRead == false)
                {
                    comment.IsRead = true;
                    return Update(comment);
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

        }

        //ANDND Set intent
        public bool SetIntent(string commentId, int intentId)
        {
            try
            {
                var comment = dbSet.Where(q => q.Id == commentId).FirstOrDefault();
                comment.IntentId = intentId;
                return Update(comment);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

        }

        //ANDND Set status
        public bool SetStatus(string commentId, int statusId)
        {
            try
    {
                var comment = dbSet.Where(q => q.Id == commentId).FirstOrDefault();
                comment.Status = statusId;
                return Update(comment);

            }
            catch (Exception e)
        {
                Debug.WriteLine(e);
                return false;
            }

        }
        // ANDND Get comment by condition
        public IQueryable<AnalysisCommentViewModel> GetCommentByShopAndCondition(JQueryDataTableParamModel param, string shopId, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => (q.Post.ShopId == shopId) && ((q.IntentId == intentId && q.IntentId != null) || intentId == null) && (q.Status == status || status == null) && (q.IsRead == isRead || isRead == null) && (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null));
            switch (param.iSortCol_0)
            {
                case 2:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.PostId);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.PostId);
                    }
                    break;
                case 3:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.SenderFbId);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.SenderFbId);
                    }
                    break;
                case 4:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.IntentId);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.IntentId);
                    }
                    break;
                case 5:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.DateCreated);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.DateCreated);
                    }
                    break;
                case 6:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.Status);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.Status);
                    }
                    break;
                default:
                    rs = rs.OrderByDescending(q => q.DateCreated);
                    break;
            }


            var data = rs.Select(q => new AnalysisCommentViewModel()
            {
                Id = q.Id,
                SenderFbId = q.SenderFbId,
                PostId = q.PostId,
                IsRead = q.IsRead,
                Status = q.Status,
                IntentId = q.IntentId.Value,
                DateCreated = q.DateCreated,
                ParentId = q.ParentId
            });
            return data;

        }
        public IEnumerable<Comment> GetAllCommentByParentId(string parentId)
        {
            //return dbSet.Where(q => (q.ParentId == parentId) && (q.Status != 5));
            return dbSet.Where(q => (q.ParentId == parentId));
        }

        public bool CheckUnreadParentComment(string postId)
        {
            //if parent comments remain as unread then return true
            var result = dbSet.Where(q => q.PostId == postId && (q.ParentId == null || q.ParentId== "")).Any(q => q.IsRead == false);
            return result;
        }
        //public IQueryable<Post> GetPost(string shopId, int from, int quantity)
        //{
        //    var result = dbSet.Where(q => q.ShopId == shopId).Select(q => new Post_Comment()
        //    {
        //        PostId = shopId + "_" + q.Id.Split('_')[0],
        //        CommentId = q.Id,
        //        IsRead = q.IsRead,
        //        DateCreated = q.DateCreated
        //    }).GroupBy(g => g.PostId).Select(g => new Post()
        //    {
        //        Id = g.Key,
        //        LastUpdate = g.Max(x => x.DateCreated),
        //        IsRead = g.All(x => x.IsRead == true)

        //    }).Skip(from).Take(quantity);
        //    return result;
        //}
    }

    //public class Post_Comment
    //{
    //    public string PostId { get; set; }
    //    public string CommentId { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public bool IsRead { get; set; }

    //}

    //public class Post
    //{
    //    public string Id { get; set; }
    //    public DateTime LastUpdate { get; set; }
    //    public bool IsRead { get; set; }
    //}
}
