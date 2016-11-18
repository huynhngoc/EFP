using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.JqueryDataTable;
using System.Diagnostics;
namespace DataService.Service
{
  public class CustomerService
  {
      
      CustomerRepository repository = new CustomerRepository();
      public CustomerService() {
           
      }

        //add customer theo facebook id
      public int AddCustomer(string Id,string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
      {
          Customer _customer = new Customer();
          _customer.CustomerFbId = Id;
          _customer.Name = Name;
          _customer.Address = Addr;
          _customer.Description = Desc;
          _customer.Phone = Phone;
          _customer.Email = Email;
          _customer.ShopId = ShopId;

          var success = repository.AddCustomer(_customer);
          Debug.WriteLine("addcus: " + success);
          return success;
      }
        public Customer AddCustomerReturnCustomer(string Id, string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
        {
            Customer _customer = new Customer();
            _customer.CustomerFbId = Id;
            _customer.Name = Name;
            _customer.Address = Addr;
            _customer.Description = Desc;
            _customer.Phone = Phone;
            _customer.Email = Email;
            _customer.ShopId = ShopId;

            var success = repository.AddCustomerReturnCustomer(_customer);
            Debug.WriteLine("addcus: " + success);
            return success;
        }
        

      public IQueryable<CustomerViewModel> GetAllCustomer(JQueryDataTableParamModel param, string shopId)
     {
         var rs = repository.GetAllCustomer(param, shopId);
         return rs;
     }
     //public Boolean DeleteCustomer(string cusId)
     //{
     //    Debug.WriteLine("fking id " + cusId.ToString());
     //    var success = repository.DeleteCustomer(cusId.ToString());
     //    return success;
     //}

            //edit customer theo int id của customer
     public Boolean EditCustomer(int Id, string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
     {
         return repository.EditCustomer(Id,Name,Addr,Desc,Phone,Email,ShopId);
     }

        
        public Customer GetCustomerByCustomerId(int customerId,string shopid)
        {
            //Customer Customer = repository.GetCustomerById(customerId).FirstOrDefault();
            Customer customer = repository.GetCustomerById(customerId, shopid);
            return customer;
        }

        public Customer GetCustomerByFacebookId(string customerFbId, string shopId)
        {
            Customer customer = repository.GetCustomerByFbId(customerFbId,shopId);
            return customer;
        }

    }
       
        
}
