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

        public IEnumerable<CategoryViewModel> GetCategory(string shopId)
        {
            return dbSet.Where(q=> q.ShopId ==shopId).Select(q=> new CategoryViewModel()
            {
                Id=q.Id,
                Name=q.Name,
                Description=q.Description,
                ParentId=q.ParentId                
            });
        }      
        
        public bool CheckDelete(int id)
        {
            try
            {
                entites.Configuration.ProxyCreationEnabled = true;
                var c = dbSet.Where(q => q.Id == id).Single();
                if (c == null|| (c.Categories1 !=null && c.Categories1.Count() != 0) || (c.Products != null && c.Products.Count() != 0))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
            
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

        public int AddNewCategory(Category c)
        {
            try
            {
                var cate = dbSet.Add(c);
                entites.SaveChanges();
                return cate.Id;
            }
            catch
            {
                return 0;
            }
        }
    }
}
