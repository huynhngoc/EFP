using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class PostWithLastestComment
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public bool IsRead { get; set; }
        public int commentCount { get; set; }
        public string newestCommentId { get; set; }
        public int status { get; set; }
        public string SenderFbId { get; set; }
        public string LastContent { get; set; }
    }
}
