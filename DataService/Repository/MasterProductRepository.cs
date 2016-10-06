using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class MasterProductRepository: BaseRepository<MasterProduct>
    {        
        public IQueryable<MasterProduct> GetMasterProductByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }

        public IEnumerable<MasterProduct> GetMasterProductByCategory(int cateId, string shopId)
        {
            IEnumerable<int> query = from c in entites.Categories where c.ParentId == cateId select c.Id;
            Debug.WriteLine(query.ToArray());
            var data = dbSet.Where(q => (q.CategoryId == cateId || query.Contains(q.CategoryId.Value)) && q.ShopId == shopId);            
            
            return data;        
        }

    }
}
