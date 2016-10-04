using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class CategoryService//: BaseService<Category>
    {
        CategoryRepository repository = new CategoryRepository();
        public CategoryService()
        {            
        }
        public List<Category> CategoryByShop(string shopId)
        {
            return repository.GetCategoryByShopId(shopId).ToList();
        }
        

    }
}
