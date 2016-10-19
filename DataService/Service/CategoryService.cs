using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.ViewModel;

namespace DataService.Service
{
    public class CategoryService
    {
        CategoryRepository repository = new CategoryRepository();
        public CategoryService()
        {            
        }
        public List<Category> CategoryByShop(string shopId)
        {
            return repository.GetCategoryByShopId(shopId).ToList();
        }

        //public List<CategoryParentViewModel> GetCategoryAndParentByShopId(string shopId)
        //{
        //    return repository.GetCategoryAndParentByShopId(shopId);
        //}

        public List<CategoryViewModel> GetAllCategory(string shopId)
        {
            return repository.GetCategory(shopId).ToList();
        }

        public List<CategoryViewModel> GetParentCategory(string shopId)
        {
            return repository.GetParentCategory(shopId).ToList();
        }

        public List<CategoryViewModel> GetCategoryByParent(int parentId)
        {
            return repository.GetCategoryByParent(parentId).ToList();
        }

        public bool EditCategory(int id, string name, string description)
        {
            Category c = repository.FindByKey(id);
            c.Name = name;
            c.Description = description;
            return repository.Update(c);
        }

        public bool DeleteCategory(int id)
        {
            Category c = repository.FindByKey(id);
            if (c == null)
            {
                return false;
            }
            if (c.Products == null || c.Categories1 == null || c.Categories1.Count() == 0 || c.Products.Count() == 0)
            {                
                return repository.Delete(id); 
            }
            else
            {
                return false;
            }            
        }

        public bool CheckDelete(int id)
        {
            return repository.CheckDelete(id);
            
        }

        public int AddNewCategory(string name, string description, int? parentId, string shopId)
        {
            Category c = new Category
            {
                Name = name,
                Description = description,
                ParentId = parentId,
                ShopId = shopId
            };
            return repository.AddNewCategory(c);
        }
    }
}
