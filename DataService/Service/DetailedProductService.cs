using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.JqueryDataTable;
using DataService.Repository;
using DataService.ViewModel;

namespace DataService.Service
{
    public class DetailedProductService
    {
        DetailedProductRepository repository = new DetailedProductRepository();
        public IQueryable<DetailedProduct> GetDetailedProduct(int masterId)
        {
            return repository.GetDetailedProduct(masterId);
        }
        public IQueryable<DetailedProductViewModel> GetDetailedProduct(JQueryDataTableParamModel param, int masterId)
        {
            return repository.GetDetailedProduct(param, masterId);
        }
    }
}
