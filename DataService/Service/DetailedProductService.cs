using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class DetailedProductService
    {
        DetailedProductRepository repository = new DetailedProductRepository();
        public IQueryable<DetailedProduct> GetDetailedProduct(int masterId, string shopId)
        {
            return repository.GetDetailedProduct(masterId, shopId);
        }
    }
}
