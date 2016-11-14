using DataService.JqueryDataTable;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class PostRepository : BaseRepository<Post>
    {
        public PostRepository() : base()
        {

        }

        //ngochb
        public int NewPostCount(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId && q.IsRead == false).Count();
        }

        //ANDND set post id read
        //ANDND set post is read
        public bool SetPostIsRead(string postId)
        {
            try
            {
                var post = dbSet.Where(q => q.Id == postId).FirstOrDefault();
                post.IsRead = true;
                return Update(post);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

        }

        //ANDND Get post by time
        public IQueryable<Post> GetPostByTime(JQueryDataTableParamModel param, string shopId, DateTime? startDate, DateTime? endDate)
        {
            var rs = dbSet.Where(q => q.ShopId == shopId && (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null));
            switch (param.iSortCol_0)
            {
                case 0:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.DateCreated);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.DateCreated);
                    }
                    break;
                default:
                    rs = rs.OrderByDescending(q => q.DateCreated);
                    break;
            }
            return rs;
        }

        //ANDND Set intent
        public bool SetIntent(string postId, int intentId)
        {
            try
            {
                var comment = dbSet.Where(q => q.Id == postId).FirstOrDefault();
                comment.IntentId = intentId;
                return Update(comment);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

        }

        //ANDND Get post by id
        public Post GetPostById(string postId)
        {
            return dbSet.Where(q => q.Id == postId).FirstOrDefault();
        }

    }
}
