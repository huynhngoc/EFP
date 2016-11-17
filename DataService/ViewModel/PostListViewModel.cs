using DataService.JqueryDataTable;
using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class PostListViewModel
    {
        public int postQuan { get; set; }
        public List<PostViewModel> postviewlist { get; set; }
    }
}
