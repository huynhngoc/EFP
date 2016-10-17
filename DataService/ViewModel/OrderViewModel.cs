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
        public int id { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateModified { get; set; }
        public decimal total { get; set; }
        public string customerId { get; set; }
        public string customerName { get; set; }
        public string note { get; set; }
        public string status { get; set; }
        public string shippingAddress { get; set; }
        public string receiver { get; set; }

        public OrderViewModel()
        {
        }

        public OrderViewModel(int id, DateTime dateCreated, DateTime dateModified, string customerId,
            string customerName, string note, decimal total, string status, string shippingAddress, string receiver)
        {
            this.id = id;
            this.dateCreated = dateCreated;
            this.dateModified = dateModified;
            this.customerId = customerId;
            this.customerName = customerName;
            this.note = note;
            this.total = total;
            this.status = status;
            this.shippingAddress = shippingAddress;
            this.receiver = receiver;
        }
    }

}
