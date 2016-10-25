using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class CommentService
    {
        CommentRepository repository = new CommentRepository();
        public bool AddComment(string id, string fbUserId, long date, int intentId, int status, string shopId)
        {
            Comment c = repository.FindByKey(id);
            if (c == null)
            {
                c = new Comment()
                {
                    Id = id,
                    FbUserId = fbUserId,
                    DateCreated = (new DateTime(1970,1,1) + TimeSpan.FromSeconds(date)).ToLocalTime(),
                    IntentId = intentId,
                    Status = status,
                    ShopId = shopId
                };
                return repository.Create(c);
            } else
            {
                //c.Id = id;
                c.FbUserId = fbUserId;
                c.DateCreated = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(date)).ToLocalTime();
                c.IntentId = intentId;
                c.ShopId = shopId;
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
            } else
            {
                return false;
            }
        }
    }
}
