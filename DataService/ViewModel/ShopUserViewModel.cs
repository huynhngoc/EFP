using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class ShopUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
