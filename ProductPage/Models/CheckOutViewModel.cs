using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductPage.Models
{
    public class CheckOutViewModel
    {
        public string userName { get; set; }
        public string phone { get; set; }
        public string receiver { get; set; }
        public string shippingAddress { get; set; }
        public string userNote { get; set; }
        public List<CartModel> listCart;
        public CheckOutViewModel()
        {
            listCart = new List<CartModel>();
        }
    }
}