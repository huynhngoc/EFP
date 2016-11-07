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
        public int status { get; set; }
        public int nestedCommentQuan { get; set; }
    }
}
