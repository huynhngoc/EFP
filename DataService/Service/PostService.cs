using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class PostService
    {
        PostRepository repository = new PostRepository();

        public bool AddPost(string id, string SenderFbId, long date, int? intentId,bool isRead, int status, string shopId)
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

    }
}
