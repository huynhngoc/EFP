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
        public bool AddComment(string id, string fbUserId, long date, int intentId, string shopId)
        {
            Comment c = repository.FindByKey(id);
            if (c == null)
            {
                c = new Comment()
                {
                    Id = id,
                    FbUserId = fbUserId,
                    DateCreated = new DateTime(date),
                    IntentId = intentId,
                    ShopId = shopId
                };
                return repository.Create(c);
            } else
            {
                //c.Id = id;
                c.FbUserId = fbUserId;
                c.DateCreated = new DateTime(date);
                c.IntentId = intentId;
                c.ShopId = shopId;
                return repository.Update(c);
            }                       
        }

        public bool HideComment(string commentId)
        {
            Comment c = repository.FindByKey(commentId);
            if (c != null)
            {                
                return true;
            } else
            {
                return false;
            }
        }
    }
}
