using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModel;

namespace DataService.Repository
{
    public class CategoryRepository: BaseRepository<Category>
    {
        public IEnumerable<Category> GetCategoryByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }

        public List<CategoryParentViewModel> GetCategoryAndParentByShopId(string shopId)
        {
            List<CategoryParentViewModel> list = new List<CategoryParentViewModel>();
            var rootCategories = dbSet.Where(q => q.ShopId == shopId && q.ParentId == null).ToList();
            foreach (Category cate in rootCategories)
            {
                CategoryParentViewModel entry = new CategoryParentViewModel()
                {
                    Id = cate.Id,
                    Description = cate.Description,
                    ShopId = cate.ShopId,
                    Name = cate.Name
                };
                if (GetAllChild(entry.Id) == null)
                {
                    entry.Children = null;
                }
                else
                {
                    entry.Children = GetAllNode(GetAllChild(entry.Id));                    
                }
                list.Add(entry);

            }
            return list;
        }
        private List<CategoryParentViewModel> GetAllChild(int cateId)
        {            
            return dbSet.Where(q => q.ParentId == cateId).Select(q => new CategoryParentViewModel()
            {
                Id = q.Id,
                ShopId = q.ShopId,
                Description = q.Description,
                Name = q.Name                    
            }).ToList();            
        }

        private List<CategoryParentViewModel> GetAllNode(List<CategoryParentViewModel> rootCategories)
        {
            List<CategoryParentViewModel> children = new List<CategoryParentViewModel>();
            foreach (CategoryParentViewModel entry in rootCategories)
            {                
                if (GetAllChild(entry.Id) == null)
                {
                    entry.Children = null;
                }
                else
                {
                    entry.Children = GetAllNode(GetAllChild(entry.Id));                    
                }
                children.Add(entry);
                
            }

            return children;
        }
    }
}
