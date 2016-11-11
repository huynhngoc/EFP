using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class CommentDetailModel
    {
		public string Id { get; set; }
		public string avatarUrl { get; set; }
		public string commentContent { get; set; } 
		public string from { get; set; }
		public DateTime datacreated { get; set; }
		public string commentImageContent { get; set; }
        public string SenderFbId { get; set; }
        public string parentId { get; set; }
        public bool IsRead { get; set; }
        public int Status { get; set; }
        public Nullable<int> IntentId { get; set; }
        public int nestedCommentQuan { get; set; }
    }
}
