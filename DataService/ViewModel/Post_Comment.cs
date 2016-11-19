using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class Post_Comment
    {
        public string PostId { get; set; }
        public string CommentId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRead { get; set; }
    }
}