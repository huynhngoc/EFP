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
        public IEnumerable<Customer> GetCustomerById(string cusId)
        {
            Debug.WriteLine("-----id_in " + cusId);
            Debug.WriteLine("asdasdasd" + dbSet.Where(q => q.CustomerId == cusId).ToString());
            //Debug.WriteLine("fuk" + dbSet.Find(cusId).ToString());
            return dbSet.Where(q => q.CustomerId == cusId);
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
                Debug.WriteLine("cus ID" + _cus.CustomerId);
                Debug.WriteLine("get cus null?" + GetCustomerById(_cus.CustomerId).Count());
                if (GetCustomerById(_cus.CustomerId).Count() == 0)
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


        public IQueryable<CustomerViewModel> GetAllCustomer(JQueryDataTableParamModel param, string shopId)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => q.ShopId == shopId);
            var search = param.sSearch;
            Debug.WriteLine("----sort col num " + param.iSortCol_0);
            if (param.sSortDir_0 == "asc")
                rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                               (!string.IsNullOrEmpty(param.sSearch)
                               && q.Name.ToLower().Contains(param.sSearch.ToLower())))
               .OrderBy(q => param.iSortCol_0 == 0 ? q.Name :
                                                        param.iSortCol_0 == 1 ? q.Description :
                                                        param.iSortCol_0 == 2 ? q.Address :
                                                        param.iSortCol_0 == 3 ? q.Phone : q.Email);
            else
                rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                               (!string.IsNullOrEmpty(param.sSearch)
                               && q.Name.ToLower().Contains(param.sSearch.ToLower())))
               .OrderByDescending(q => param.iSortCol_0 == 0 ? q.Name :
                                                        param.iSortCol_0 == 1 ? q.Description :
                                                        param.iSortCol_0 == 2 ? q.Address :
                                                        param.iSortCol_0 == 3 ? q.Phone : q.Email);


            Debug.WriteLine("---------rs " + rs.Count());
            //if (rs.Count() == 0) return null;
            var data = rs.Select(q => new CustomerViewModel()
            {
                CustomerId = q.CustomerId,
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
        public bool EditCustomer(string Id, string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
        {
            Customer dummyCustomer = (Customer)GetCustomerById(Id).FirstOrDefault();
            dummyCustomer.Name = Name;
            dummyCustomer.Address = Addr;
            dummyCustomer.Description = Desc;
            dummyCustomer.Phone = Phone;
            dummyCustomer.Email = Email;
            var result = Update(dummyCustomer);
            this.Save();
            return result;
        }
    }
}
