using DataService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public decimal Total { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }

        public OrderViewModel()
        {
        }

        public OrderViewModel(int id, DateTime dateCreated, DateTime dateModified, string customerId,
            string customerName, string note, decimal total, string status, string shippingAddress, string receiver, string phone)
        {
            this.Id = id;
            this.DateCreated = dateCreated;
            this.DateModified = dateModified;
            this.CustomerId = customerId;
            this.CustomerName = customerName;
            this.Note = note;
            this.Total = total;
            this.Status = status;
            this.ShippingAddress = shippingAddress;
            this.Receiver = receiver;
            this.Phone = phone;
        }
    }

}
