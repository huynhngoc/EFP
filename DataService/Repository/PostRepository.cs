using DataService.ViewModel;
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
            Debug.WriteLine("post id " + postId);
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
        public bool SetPostIsUnRead(string postId)
        {
            try
            {
                var post = dbSet.Where(q => q.Id == postId).FirstOrDefault();
                post.IsRead = false;
                return Update(post);

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }

        }

        public IQueryable<PostWithLastestComment> GetAllPost(string shopId)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            var result = dbSet.Where(q => (q.ShopId == shopId)).AsEnumerable().Select(q => new PostWithLastestComment()
            {
                Id = q.Id,
                IsRead = q.Comments.Count() == 0 ? q.IsRead : q.Comments.All(c => c.IsRead == true),
                commentCount = q.Comments.Count(),
                LastUpdate = q.Comments.Count() == 0 ? q.DateCreated : q.Comments.Max(c => c.DateCreated),
                status = q.Status,
                LastContent = q.LastContent,
                SenderFbId = q.SenderFbId,
            }).OrderByDescending(q=>q.LastUpdate);
            return result.AsQueryable();
        }
       
    }
}
