using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class ShopViewModel
    {
        public string Id { get; set; }
        public string ShopName { get; set; }
        public string FbToken { get; set; }
        //public string BannerImg { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string UserId { get; set; }
    }
}
