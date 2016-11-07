using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class PostRepository:BaseRepository<Post>
    {
        public PostRepository(): base()
        {
            
        }
        public Post GetPost(string postId, string shopId)
        {
            var result = dbSet.Where(q => (q.ShopId == shopId) && (q.Id == postId)).FirstOrDefault();
            return result;
        }
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

    }
}
