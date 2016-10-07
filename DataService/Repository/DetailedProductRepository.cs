using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class DetailedProductRepository: BaseRepository<DetailedProduct>
    {
        public IEnumerable<DetailedProduct> GetDetailedProductByShopId(string shopId)
        {
            return dbSet.Where(q => q.MasterProduct.ShopId == shopId);
        }
        public IEnumerable<DetailedProduct> GetDetailedProductByMasterProduct(int masterId, string shopId)
        {
            return dbSet.Where(q => q.MasterId == masterId && q.MasterProduct.ShopId == shopId);
        }

        public IQueryable<DetailedProduct> GetDetailedProduct(int masterId, string shopId)
        {
            return dbSet.Where(q => q.MasterId == masterId && q.MasterProduct.ShopId == shopId);
        }

        
    }
}
