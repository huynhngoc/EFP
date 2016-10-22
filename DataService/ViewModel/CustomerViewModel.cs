using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string shopId { get; set; }

        public CustomerViewModel(int id, string name, string des, string add, string phone, string email, string shopid)
        {
            this.CustomerId = id;
            this.Name = name;
            this.Description = des;
            this.Address = add;
            this.Phone = phone;
            this.Email = email;
            this.shopId = shopid;
        }
        public CustomerViewModel() { }
    }
}
