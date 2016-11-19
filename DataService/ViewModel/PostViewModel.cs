using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class PostViewModel
    {
        public PostWithLastestComment post {get; set; }
        public string from { get; set; }
        public string message { get; set; }
        public string imageContent { get; set; }
        
        public PostViewModel()
        {
            this.post = null;
            this.from = "";
            this.message = "";
        }
    }
}
