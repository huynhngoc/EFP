using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class PostDetailModel
    {
        public string Id { get; set; }
        public DateTime LastUpdate { get; set; }
        public string postContent { get; set; }
        public string from { get; set; }
        public string postImageContent { get; set; }
        public string fromAvatar { get; set; }
        public string SenderFbId { get; set; }
        public string storyContent { get; set; }
        public int commentQuan { get; set; }
        public int Status { get; set; }
        public Nullable<int> IntentId { get; set; }
        public List<CommentDetailModel> Comments { get; set; }
        public List<CommentDetailModel> nestedComments { get; set; }


    }
}
