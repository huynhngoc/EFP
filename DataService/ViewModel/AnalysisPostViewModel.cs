using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class AnalysisPostViewModel
    {
        public string PostId { get; set; }
        public string PostContent { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public int ShareCount { get; set; }
        public string SenderFBId { get; set; }
        public string SenderFBName { get; set; }
        public bool IsCustomer { get; set; }
        public string ShopId { get; set; }
        public bool IsRead { get; set; }
        public int? IntentId { get; set; }
        public string IntentName { get; set; }
        public int Status { get; set; }
    }
}
