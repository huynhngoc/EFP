using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public IEnumerable<Product> GetProductByShopAndCategory(string shopId, int categoryId)
        {
            return dbSet.Where(q => q.ShopId == shopId && q.CategoryId == categoryId && q.Status == true);
        }
        public IEnumerable<Product> GetProductByProductId(string shopId, int categoryId, int Id)
        {
            return dbSet.Where(q => q.ShopId == shopId && q.CategoryId == categoryId && q.Id == Id && q.Status == true);
        }
    }
}
