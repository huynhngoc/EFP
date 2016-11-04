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

        // ANDND Get comment by condition
        public IQueryable<AnalysisCommentViewModel> GetCommentByShopAndCondition(JQueryDataTableParamModel param, string shopId, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => (q.Post.ShopId == shopId) && (q.IntentId == intentId || intentId == null) && (q.Status == status || status == null) && (q.IsRead == isRead || isRead == null) && (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null));
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

        // ANDND Set id read
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
