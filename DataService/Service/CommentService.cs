using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.ViewModel;
using DataService.JqueryDataTable;

namespace DataService.Service
{
    public class CommentService
    {
        CommentRepository repository = new CommentRepository();
        IntentService intentService = new IntentService();
        public bool AddComment(string id, string SenderFbId, long date, int? intentId, int status, string parentId, string postId, string lastContent)
        {

            Comment c = repository.FindByKey(id);
            if (c == null)
            {
                c = new Comment()
                {
                    Id = id,
                    SenderFbId = SenderFbId,
                    DateCreated = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date)).ToLocalTime(),
                    IntentId = intentId,
                    Status = status,
                    ParentId = parentId,
                    PostId = postId,
                    LastContent = lastContent,
                    CanHide = false                                                            
                };
                return repository.Create(c);
            }
            else
            {
                c.DateCreated = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date)).ToLocalTime();
                if (c.IntentId != null)
                {
                c.IntentId = intentId;
                    c.SenderFbId = SenderFbId;
                }
                c.PostId = postId;
                if (!string.IsNullOrEmpty(lastContent))
                {
                    c.LastContent = lastContent;
                }
                return repository.Update(c);
            }
        }


        //public IQueryable<PostWithLastestComment> GetAllPost(string shopId)
        //{
        //    return repository.GetAllPost(shopId);
        //}

        public bool CheckUnreadRemain(string parentId)
        {
           return repository.CheckUnreadRemain(parentId);
        }

        public IQueryable<Comment> GetCommentsOfPost(string postId)
        {
            return repository.GetCommentsContainPostId(postId);
        }

        public List<Comment>[] GetCommentsWithPostId(string postId, int skip, int take)
        {
            return repository.GetCommentsWithPostId(postId, skip, take);
        }

        public Comment GetLastestComment(string postId)
        {
            return repository.GetLastestComment(postId);
        }

        public Comment getCommentById(string commentId)
        {
            return repository.getCommentById(commentId);
        }

        public int GetNestedCommentQuan(string parentId)
        {
            return repository.GetNestedCommentQuan(parentId);
        }

        public List<Comment> GetNestedCommentOfParent(string parentId,int skip, int take)
        {
            return repository.GetNestedCommentOfParent(parentId, skip, take).ToList();
        }

        public int GetParentCommentQuan(string postId)
        {
            return repository.GetParentCommentQuan(postId);
        }

        public bool SetIsRead(string commentId)
        {
            return repository.SetIsRead(commentId);
        }

        //ANDND Set intent
        public bool SetIntent(string commentId, int intentId)
        {
            return SetIntent(commentId, intentId);
        }

        //ANDND Set status
        public bool SetStatus(string commentId, int status)
        {
            Comment c = repository.FindByKey(commentId);
            if (c != null)
            {
                c.Status = status;
                return repository.Update(c);
            }
            else
            {
                return false;
            }
        }
        //ngochb
        public int NewCommentCount(string shopId)
        {
            return repository.NewCommentCount(shopId);
        }

        // ANDND Get comment by condition
        public IQueryable<AnalysisCommentViewModel> GetCommentByShopAndCondition(JQueryDataTableParamModel param, string fbId, string shopId, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var listModel = repository.GetCommentByShopAndCondition(param, fbId, shopId, intentId, status, isRead, startDate, endDate);
            return listModel;
        }

        // ANDND Get User Analysis By Time
        public List<AnalysisUserViewModel> GetCommentUserList(JQueryDataTableParamModel param, string shopId, DateTime? startDate, DateTime? endDate)
        {
            var listUser = repository.GetCommentUserList(param, shopId, startDate, endDate);
            return listUser;
        }

        // ANDND Set is read
        public bool SetIsRead(string commentId)
        {
            return repository.SetIsRead(commentId);
        }
        // ANDND Get Comment by comment id
        public Comment GetCommentById(string commentId)
        {
            Comment comment = repository.FindByKey(commentId);
            return comment;
        }
        public bool SetCommentStatus(string commentId, int statusId)
        {
            return repository.SetStatus(commentId, statusId);
        }
        public IEnumerable<Comment> GetAllCommentByParentId (string parentId)
        {
            return repository.GetAllCommentByParentId(parentId);
        }

        public bool CheckPostUnread(string postId)
        {
            return repository.CheckPostUnread(postId);
        }
        public bool CheckUnreadParentComment(string postId)
        {
            return repository.CheckUnreadParentComment(postId);
        }

        //ANDND Get Comment analysis data
        public List<AnalysisCommentDataChartViewModel> GetAnalysisDataByTime(string shopId, int? intentId, int? status, bool? isRead, DateTime? startDate, DateTime? endDate)
        {
            var listModel = repository.GetAnalysisDataByTime(shopId, intentId,status,isRead, startDate, endDate).ToList();
            List<AnalysisCommentDataChartViewModel> listData = new List<AnalysisCommentDataChartViewModel>();
            var listIntent = intentService.GetAllIntent();
            for (int i = 0; i < listIntent.Count(); i++)
            {
                listData.Add(new AnalysisCommentDataChartViewModel { IntentId = listIntent[i].Id, IntentName = listIntent[i].IntentName, CommentNumber = 0 });
            }
            for (int j = 0; j < listModel.Count(); j++)
            {
                for(int k=0;k< listData.Count(); k++)
                {
                    if (listModel[j].IntentId == listData[k].IntentId)
                    {
                        listData[k].CommentNumber = listData[k].CommentNumber + 1;
                    }
                }
            }
            return listData;
        }

        public bool SetCanHide(string commentId, bool canHide)
        {
            Comment c = repository.FindByKey(commentId);
            if (c != null)
            {
                c.CanHide = canHide;
                return repository.Update(c);
            }
            else
            {
                return false;
            }
        }
    }
}
