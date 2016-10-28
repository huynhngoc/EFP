using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class ConversationPreviewViewModel
    {
        public string ThreadId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string RecentMess { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsRead { get; set; }

        public ConversationPreviewViewModel()
        {

        }
    }
}
