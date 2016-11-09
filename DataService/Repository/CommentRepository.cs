using DataService.JqueryDataTable;
using DataService.Utils;
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

        Service.IntentService intentService = new Service.IntentService();

        // ANDND Get comment by condition
        public IQueryable<AnalysisCommentViewModel> GetCommentByShopAndCondition(JQueryDataTableParamModel param, string fbId, string shopId, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var rs = dbSet.Where(q => (q.Post.ShopId == shopId) && (q.IntentId != null) && ((q.SenderFbId == fbId) || fbId.Length == 0) && ((q.IntentId == intentId) || intentId == null) && (q.Status == status || status == null) && (q.IsRead == isRead || isRead == null) && (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null));
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

        // ANDND Get User Comment by time
        public List<AnalysisUserViewModel> GetCommentUserList(JQueryDataTableParamModel param, string shopId, DateTime? startDate, DateTime? endDate)
        {
            var rs = dbSet.Where(q => (q.Post.ShopId == shopId) && (q.IntentId != null) &&  (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null));
            switch (param.iSortCol_0)
            {
                case 0:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.SenderFbId);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.SenderFbId);
                    }
                    break;

                default:
                    rs = rs.OrderByDescending(q => q.DateCreated);
                    break;
            }


            var userList = rs.Select(q => new AnalysisUserViewModel()
            {
                UserFBId = q.SenderFbId
            }).Distinct().ToList();

            for (int i = 0; i < userList.Count(); i++)
            {
                var SenderFbId = userList[i].UserFBId;
                var commentList = dbSet.Where(q => (q.SenderFbId == SenderFbId) && (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null)).ToList();
                userList[i].TotalComment = commentList.Count();
                userList[i].UnreadComment = 0;

                userList[i].ListStatusNumber.Add(new StatusTotal() { StatusId = (int)CommentStatus.SHOWING, StatusName = "Đang hiện", StatusCount = 0 });
                userList[i].ListStatusNumber.Add(new StatusTotal() { StatusId = (int)CommentStatus.WARNING, StatusName = "Cảnh báo", StatusCount = 0 });
                userList[i].ListStatusNumber.Add(new StatusTotal() { StatusId = (int)CommentStatus.HIDDEN, StatusName = "Đang ẩn", StatusCount = 0 });
                userList[i].ListStatusNumber.Add(new StatusTotal() { StatusId = (int)CommentStatus.APPROVED, StatusName = "Đã duyệt", StatusCount = 0 });
                userList[i].ListStatusNumber.Add(new StatusTotal() { StatusId = (int)CommentStatus.DELETED, StatusName = "Đã xóa", StatusCount = 0 });

                var listIntent = intentService.GetAllIntent();

                for (int k = 0; k < listIntent.Count(); k++)
                {
                    userList[i].ListIntentNumber.Add(new IntentTotal() { IntentName = listIntent[k].IntentName, IntentCount = 0 });
                }

                for (int j = 0; j < commentList.Count(); j++)
                {
                    //Get Unread number
                    if (commentList[j].IsRead == false)
                    {
                        userList[i].UnreadComment = userList[i].UnreadComment + 1;
                    }

                    //Get status number
                    if (commentList[j].Status == (int)CommentStatus.SHOWING)
                    {
                        userList[i].ListStatusNumber[(int)CommentStatus.SHOWING - 1].StatusCount = userList[i].ListStatusNumber[(int)CommentStatus.SHOWING - 1].StatusCount + 1;
                    }
                    if (commentList[j].Status == (int)CommentStatus.APPROVED)
                    {
                        userList[i].ListStatusNumber[(int)CommentStatus.APPROVED - 1].StatusCount = userList[i].ListStatusNumber[(int)CommentStatus.APPROVED - 1].StatusCount + 1;
                    }
                    if (commentList[j].Status == (int)CommentStatus.HIDDEN)
                    {
                        userList[i].ListStatusNumber[(int)CommentStatus.HIDDEN - 1].StatusCount = userList[i].ListStatusNumber[(int)CommentStatus.HIDDEN - 1].StatusCount + 1;
                    }
                    if (commentList[j].Status == (int)CommentStatus.WARNING)
                    {
                        userList[i].ListStatusNumber[(int)CommentStatus.WARNING - 1].StatusCount = userList[i].ListStatusNumber[(int)CommentStatus.WARNING - 1].StatusCount + 1;
                    }
                    if (commentList[j].Status == (int)CommentStatus.DELETED)
                    {
                        userList[i].ListStatusNumber[(int)CommentStatus.DELETED - 1].StatusCount = userList[i].ListStatusNumber[(int)CommentStatus.DELETED - 1].StatusCount + 1;
                    }

                    for (int m = 0; m < listIntent.Count(); m++)
                    {
                        if (listIntent[m].Id == commentList[j].IntentId)
                        {
                            userList[i].ListIntentNumber[m].IntentCount = userList[i].ListIntentNumber[m].IntentCount + 1;
                        }
                    }
                }
            }


            return userList;
        }

        // ANDND Get analysis
        public IQueryable<AnalysisCommentViewModel> GetAnalysisDataByTime(string shopId, DateTime? startDate, DateTime? endDate)
        {
            var rs = dbSet.Where(q => (q.Post.ShopId == shopId) && (q.IntentId != null) && (q.DateCreated >= startDate || startDate == null) && (q.DateCreated <= endDate || endDate == null));
            var listIntent = intentService.GetAllIntent();
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
