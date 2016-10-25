using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class CommentRepository: BaseRepository<Comment>
    {
        public CommentRepository(): base()
        {

        }

        public IQueryable<Post> GetPost(string shopId, int from, int quantity)
        {
            var result = dbSet.Where(q => q.ShopId == shopId).Select(q => new Post_Comment()
            {
                PostId = shopId + "_" + q.Id.Split('_')[0],
                CommentId = q.Id,
                IsRead = q.IsRead,
                DateCreated = q.DateCreated
            }).GroupBy(g => g.PostId).Select(g => new Post()
            {
                Id = g.Key,
                LastUpdate = g.Max(x => x.DateCreated),
                IsRead = g.All(x => x.IsRead == true)

            }).Skip(from).Take(quantity);
            return result;
        }
    }

    public class Post_Comment
    {
        public string PostId { get; set; }
        public string CommentId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }

    }

    public class Post
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsRead { get; set; }
    }
}
