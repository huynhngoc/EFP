using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public IQueryable<PostWithLastestComment> GetAllPost(string shopId)
        {
            
                var result = dbSet.Where(q => q.PostId.Contains(shopId)).AsEnumerable().Select(q => new Comment()
                {
                    PostId = q.PostId,
                    Id = q.Id,
                    IsRead = q.IsRead,
                    DateCreated = q.DateCreated
                }).GroupBy(g => g.PostId).Select(g => new PostWithLastestComment()
                {
                    Id = g.Key,
                    LastUpdate = g.Max(x => x.DateCreated),
                    IsRead = g.All(x => x.IsRead == true),
                    commentCount = g.Count(),
                    //newestCommentId = g.Where(x=>x.DateCreated = g.Max((DateTime)x.DateCreated))
                }).OrderByDescending(q => q.LastUpdate);

                return result.AsQueryable();
            

        }


        //public IEnumerable<Comment> GetCommentsByShopId(string shopId)
        //{
        //    Debug.WriteLine("-----id_in " + shopId);
        //    Debug.WriteLine("asdasdasd" + dbSet.Where(q => q.ShopId == shopId).ToString());
        //    //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
        //    return dbSet.Where(q => q.ShopId == shopId).OrderByDescending(q => q.DateCreated);
        //}

        public IEnumerable<Comment> GetCommentsContainPostId(string postId)
        {
            string searchString = postId.Split('_')[1] + "_";
            Debug.WriteLine("search string" + searchString);
            Debug.WriteLine("-----id_in " + postId);
            Debug.WriteLine("asdasdasd " + dbSet.Where(q => q.Id.Contains(searchString)).OrderBy(q => q.DateCreated));
            //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
            return dbSet.Where(q => q.Id.Contains(searchString)).OrderBy(q => q.DateCreated);
        }


        //public IQueryable<Post> GetPost(string shopId, int from, int quantity)
        //{
        //    var result = dbSet.Where(q => q.ShopId == shopId).Select(q => new Post_Comment()
        //    {
        //        PostId = shopId + "_" + q.Id.Split('_')[0],
        //        CommentId = q.Id,
        //        IsRead = q.IsRead,
        //        DateCreated = q.DateCreated
        //    }).GroupBy(g => g.PostId).Select(g => new Post()
        //    {
        //        Id = g.Key,
        //        LastUpdate = g.Max(x => x.DateCreated),
        //        IsRead = g.All(x => x.IsRead == true)

        //    }).Skip(from).Take(quantity);
        //    return result;
        //}
    }

    //public class Post_Comment
    //{
    //    public string PostId { get; set; }
    //    public string CommentId { get; set; }
    //    public DateTime DateCreated { get; set; }
    //    public bool IsRead { get; set; }

    //}

    //public class Post
    //{
    //    public string Id { get; set; }
    //    public DateTime LastUpdate { get; set; }
    //    public bool IsRead { get; set; }
    //}
    
}
