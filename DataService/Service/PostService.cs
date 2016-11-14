using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facebook;
using DataService.Repository;
using DataService.JqueryDataTable;
using DataService.ViewModel;

namespace DataService.Service
{
    public class PostService
    {
        PostRepository repository = new PostRepository();

        public bool AddPost(string id, string SenderFbId, long date, int? intentId, bool isRead, int status, string shopId)
        {
            Post c = repository.FindByKey(id);
            if (c == null)
            {
                c = new Post()
                {
                    Id = id,
                    SenderFbId = SenderFbId,
                    DateCreated = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date)).ToLocalTime(),
                    IntentId = intentId,
                    IsRead = isRead,
                    Status = status,
                    ShopId = shopId
                };
                return repository.Create(c);
            }
            else
            {
                //c.Id = id;
                c.SenderFbId = SenderFbId;
                c.DateCreated = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date)).ToLocalTime();
                c.IntentId = intentId;
                c.IsRead = isRead;
                c.Status = status;
                c.ShopId = shopId;
                return repository.Update(c);
            }
        }

        public bool SetStatus(string commentId, int status)
        {
            Post c = repository.FindByKey(commentId);
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

        public int NewPostCount(string shopId)
        {
            return repository.NewPostCount(shopId);
        }

        //ANDND Set post is read
        public bool SetPostIsRead(string postId)
        {
            return (repository.SetPostIsRead(postId));
        }

        //ANDND Get post by time
        public List<Post> GetPostByTime(JQueryDataTableParamModel param, string shopId, DateTime? startDate, DateTime? endDate)
        {
            var data = repository.GetPostByTime(param, shopId, startDate, endDate).ToList();
            return data;
        }

        // ANDND Set intent
        public bool SetIntent(string postId, int intentId)
        {
            return repository.SetIntent(postId, intentId);
        }

        //ANDND Get post by id
        public Post GetPostById(string postId)
        {
            return repository.GetPostById(postId);
        }
    }
}
