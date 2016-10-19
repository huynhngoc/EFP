using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DataService.ViewModel;
namespace DataService.Repository
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        public IEnumerable<Customer> GetCustomerById(string cusId)
        {
            return dbSet.Where(q => q.CustomerId == cusId);
        }
    }
}
