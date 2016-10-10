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

        public List<CategoryParentViewModel> GetCategoryAndParentByShopId(string shopId)
        {
            return repository.GetCategoryAndParentByShopId(shopId);
        }

        public List<CategoryViewModel> GetAllCategory(string shopId)
        {
            return repository.GetCategory(shopId).ToList();
        }

        public bool EditCategory(int id, string name, string description)
        {
            Category c = repository.FindByKey(id);
            c.Name = name;
            c.Description = description;
            return repository.Update(c);
        }
    }
}
