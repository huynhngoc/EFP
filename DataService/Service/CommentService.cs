using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.ViewModel;

namespace DataService.Service
{
    public class CommentService
    {
        CommentRepository repository = new CommentRepository();
        public bool AddComment(string id, string SenderFbId, long date, int intentId, string shopId)
        {
            Comment c = repository.FindByKey(id);
            if (c == null)
            {
                c = new Comment()
                {
                    Id = id,
                    SenderFbId = SenderFbId,
                    DateCreated = new DateTime(date),
                    IntentId = intentId,
                    PostId = shopId + id.Split('_')[0],
                };
                return repository.Create(c);
            } else
            {
                //c.Id = id;
                c.SenderFbId = SenderFbId;
                c.DateCreated = new DateTime(date);
                c.IntentId = intentId;
                c.PostId = shopId + id.Split('_')[0];
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
        public IQueryable<PostWithLastestComment> GetAllPost(string shopId, int from, int quantity)
        {
            return repository.GetAllPost(shopId, from, quantity);
        }
        public IEnumerable<Comment> GetCommentsOfPost(string postId)
        {
            return repository.GetCommentsContainPostId(postId);
        }
    }
}
