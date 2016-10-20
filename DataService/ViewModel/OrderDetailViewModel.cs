using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class OrderDetailViewModel
    {
        public string Properties { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public int ProductId { get; set; }

        public OrderDetailViewModel()
        {
        }

        public OrderDetailViewModel(string properties, int quantity, decimal price)
        {
            this.Quantity = quantity;
            this.Price = price;
            this.Properties = properties;
        }

        public OrderDetailViewModel(int pid, string properties, int quantity, decimal price)
        {
            this.ProductId = pid;
            this.Quantity = quantity;
            this.Price = price;
            this.Properties = properties;
        }
    }
}
