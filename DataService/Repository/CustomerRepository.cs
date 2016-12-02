using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataService.JqueryDataTable;
using System.Diagnostics;
using DataService.ViewModel;
namespace DataService.Repository
{
    public class CustomerRepository : BaseRepository<Customer>
    {
        //public IEnumerable<Customer> GetCustomerById(string cusId)
        //{
        //    return dbSet.Where(q => q.CustomerId == cusId);
        //}
        //public IEnumerable<Customer> GetAllCustomer()
        //{
        //    return this.dbSet;
        //}

        /// <summary>
        /// get customer by id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Customer GetCustomerById(int cusId, string shopId)
        {
            //Debug.WriteLine("-----id_in " + cusId);
            //Debug.WriteLine("asdasdasd" + dbSet.Where(q => q.Id == cusId).ToString());
            //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
            return dbSet.Where(q => (q.Id == cusId) && (q.ShopId == shopId)).FirstOrDefault();
        }

        public Customer GetCustomerByFbId(string customerFbId, string shopId)
        {
            return dbSet.Where(q => q.CustomerFbId == customerFbId && q.ShopId == shopId).FirstOrDefault();
        }

        /// <summary>
        /// delete a customer by id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// 
        //public bool DeleteCustomer(string cusId)
        //{
        //    try
        //    {
        //        Debug.WriteLine("-----id_out " + cusId.ToString());
        //        List<Customer> _customers = GetCustomerById(cusId).Count();
        //        Customer cus = _customers.FirstOrDefault();
        //        Debug.WriteLine("-----fuk   " + cus.Name);
        //        Delete(cus);
        //    }
        //     catch (Exception a)
        //    {
        //         Debug.WriteLine(a);
        //         return false;
        //    }

        //    return true;
        //}

        public int AddCustomer(Customer _cus)
        {
            try
            {
                //debug what????
                //Debug.WriteLine("cus ID" + _cus.CustomerId);
                //Debug.WriteLine("get cus null?" + GetCustomerById(_cus.CustomerId).Count());
                if (FindByKey(_cus.Id) == null)
                {
                    Create(_cus);
                    this.Save();
                    return 1;
                }
                else return 2;
                
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        public Customer AddCustomerReturnCustomer(Customer _cus)
        {
            try
            {
                if (FindByKey(_cus.Id) == null)
                {
                    var cust = CreateNew(_cus);
                    this.Save();
                    return cust;
                }
                else return null;

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }


        public IQueryable<CustomerViewModel> GetAllCustomer(JQueryDataTableParamModel param, string shopId)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => q.ShopId == shopId);
            var search = param.sSearch;
            Debug.WriteLine("----sort col num " + param.iSortCol_0);
            rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                           (!string.IsNullOrEmpty(param.sSearch)
                           && q.Name.ToLower().Contains(param.sSearch.ToLower())))
           .OrderBy(q => q.Name);
            Debug.WriteLine("---------rs " + rs.Count());
            //if (rs.Count() == 0) return null;
            var data = rs.Select(q => new CustomerViewModel()
            {
                //id dung de edit nen lay int id
                CustomerId = q.Id,
                Name = q.Name,
                Description = q.Description,
                Address = q.Address,
                Phone = q.Phone,
                Email = q.Email
            });
            Debug.WriteLine("---------data " + data.Count());
            return data;
        }
        public void Save()
        {
            entites.SaveChanges();
        }
        public bool EditCustomer(int Id, string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
        {
            Customer customer = FindByKey(Id);
            customer.Name = Name;
            customer.Address = Addr;
            customer.Description = Desc;
            customer.Phone = Phone;
            customer.Email = Email;
            var result = Update(customer);
            this.Save();
            return result;
        }
        
    }
}
