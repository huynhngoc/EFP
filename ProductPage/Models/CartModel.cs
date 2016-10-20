using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductPage.Models
{
    public class CartModel
    {
        public string url { get; set; }
        public string properties { get; set; }
        public int productId { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}