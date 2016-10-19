using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class TemplateProductRepository: BaseRepository<TemplateProduct>
    {
        public IEnumerable<TemplateProduct> GetTemplateByIdAndShop(int id,string shopId)
        {
            return dbSet.Where(q => (q.Id == id)&& (q.ShopId==shopId));
        }
    }
}
