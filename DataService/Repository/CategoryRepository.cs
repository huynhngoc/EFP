using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class CategoryRepository : BaseRepository<Category>
    {
        public IEnumerable<Category> GetCategoryByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }
        public IEnumerable<CategoryViewModel> GetCategoryByShopID(string shopId)
        {
            return dbSet.Where(q => (q.ShopId == shopId)).Select(q => new CategoryViewModel()
            {
                CategoryId = q.Id,
                Description = q.Description,
                CategoryName = q.Name,
                ParentId = q.ParentId
            });
        }
        public IEnumerable<Category> getChildCategoryId(string shopId, int parentCategoryId)
        {
            var category = dbSet.Where(q => (q.Id == parentCategoryId) && (q.ParentId == null));
            if (category != null)
            {
                return dbSet.Where(q => (q.ShopId == shopId) && (q.ParentId == parentCategoryId));
            }
            else
            {
                return null;
            }
        }

    }
}
