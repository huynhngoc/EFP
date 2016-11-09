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
        public bool AddComment(string id, string SenderFbId, long date, int? intentId, int status, string parentId, string postId)
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
                    PostId = postId
                };
                return repository.Create(c);
            }
            else
            {
                //c.Id = id;
                c.SenderFbId = SenderFbId;
                c.DateCreated = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date)).ToLocalTime();
                c.IntentId = intentId;
                c.PostId = postId;
                return repository.Update(c);
            }
        }

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

        // ANDND Set status
        public bool SetCommentStatus(string commentId, int statusId)
        {
            return repository.SetStatus(commentId, statusId);
        }

        // ANDND Set intent
        public bool SetIntent(string commentId, int intentId)
        {
            return repository.SetIntent(commentId, intentId);
        }

        // ANDND Get Comment by comment id
        public Comment GetCommentById(string commentId)
        {
            Comment comment = repository.FindByKey(commentId);
            return comment;
        }

        //ANDND Get Comment analysis data
        public List<AnalysisCommentDataChartViewModel> GetAnalysisDataByTime(string shopId, DateTime? startDate, DateTime? endDate)
        {
            var listModel = repository.GetAnalysisDataByTime(shopId, startDate, endDate).ToList();
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
    }
}
