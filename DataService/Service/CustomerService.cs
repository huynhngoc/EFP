using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public interface ICustomerService: IService<Customer>
    {
        /* write additional method name here */
        IQueryable<Customer> getCustomerByName();
    }
    public class CustomerService: BaseService<Customer>, ICustomerService
    {
        public CustomerService()
        {
            repository = new CustomerRepository(dbContext);            
        }

        /* implement customerized method in interface here */
        public IQueryable<Customer> getCustomerByName()
        {
            throw new NotImplementedException();
        }

        

    }
}
