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
        public IQueryable<PostWithLastestComment> GetAllPost(string shopId)
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
                //newestCommentId = g.Where(x=>x.DateCreated = g.Max((DateTime)x.DateCreated))
            }).OrderByDescending(q => q.LastUpdate);

            return result.AsQueryable();


        }


        public Comment getCommentById(string commentId)
        {
            return dbSet.Where(q => q.Id == commentId).FirstOrDefault();
        }

        public IQueryable<Comment> GetCommentsContainPostId(string postId)
        {
            string searchString = postId.Split('_')[1] + "_";
            Debug.WriteLine("search string" + searchString);
            Debug.WriteLine("-----id_in " + postId);
            //Debug.WriteLine("asdasdasd " + dbSet.Where(q => q.Id.Contains(searchString)).OrderByDescending(q => q.DateCreated));
            //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
            return dbSet.Where(q => (q.Id.Contains(searchString)) && (q.Status == 1)).OrderByDescending(q=>q.DateCreated);
        }

        public int GetNestedCommentQuan(string commentId)
        {
            return dbSet.Where(q => (q.ParentId == commentId) && (q.Status!=5)).Count();
        }

        public IQueryable<Comment> GetNestedCommentOfParent(string commentId,int skip, int take)
        {
            return dbSet.Where(q => (q.ParentId == commentId) && (q.Status != 5)).AsEnumerable().OrderByDescending(q=>q.DateCreated).Skip(skip).Take(take).AsQueryable();
        }

        public List<Comment>[] GetCommentsWithPostId(string postId, int  skip, int take)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            List<Comment>[] result = new List<Comment>[2];
            var comments = GetCommentsContainPostId(postId);
            IQueryable<Comment> parentComment = comments.Where(q => (q.ParentId == null) && (q.Status!=5)).OrderByDescending(q =>  q.Comments1.Count>0 ?  q.Comments1.Max(x => x.DateCreated): q.DateCreated).Skip(skip).Take(take);
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
            return dbSet.Where(q => (q.PostId == postId) && (q.ParentId == null) && (q.Status == 1)).Count();
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
    }
}
