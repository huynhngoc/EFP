using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class AnalysisCommentViewModel
    {
        public string Id { get; set; }
        public string CommentContent { get; set; }
        public string SenderFbId { get; set; }
        public string SenderName { get; set; }
        public bool IsCustomer { get; set; }
        public string PostId { get; set; }
        public string PostContent { get; set; }
        public bool IsRead { get; set; }
        public int Status { get; set; }
        public int? IntentId { get; set; }
        public string IntentName { get; set; }
        public DateTime DateCreated { get; set; }
        public string ParentId { get; set; }
        
    }
}
