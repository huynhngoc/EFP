using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class CategoryRepository: BaseRepository<Category>
    {
        public IEnumerable<Category> GetCategoryByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }
    }
}
