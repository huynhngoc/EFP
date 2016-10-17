using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class OrderDetailViewModel
    {
        public string details { get; set; }
        public int quantity { get; set; }
        public decimal price { get; set; }

        public int productId { get; set; }

        public OrderDetailViewModel()
        {
        }

        public OrderDetailViewModel(string details, int quantity, decimal price)
        {
            this.quantity = quantity;
            this.price = price;
            this.details = details;
        }

        public OrderDetailViewModel(int pid, string details, int quantity, decimal price)
        {
            this.productId = pid;
            this.quantity = quantity;
            this.price = price;
            this.details = details;
        }
    }
}
