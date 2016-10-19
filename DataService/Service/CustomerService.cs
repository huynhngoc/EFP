using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using System.Diagnostics;
namespace DataService.Service
{
    public class CustomerService
    {

        CustomerRepository repository = new CustomerRepository();
        public CustomerService()
        {

        }

        public Customer GetCustomerByCustomerId(string customerId)
        {
            Customer Customer = repository.GetCustomerById(customerId).FirstOrDefault();
            return Customer;
        }


    }


}
