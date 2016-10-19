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

        public List<CategoryViewModel> GetCategoryByShopId(string shopId)
        {
            // List Category non-order
            List<CategoryViewModel> listCategoryViewModelOriginal = repository.GetCategoryByShopID(shopId).ToList();
            //Create new list category to order
            List<CategoryViewModel> listCategoryViewModelOrdered = new List<CategoryViewModel>();
            CategoryViewModel categoryViewModel;

            // Create Category Level 0
            for (int i = 0; i < listCategoryViewModelOriginal.Count(); i++)
            {
                if (listCategoryViewModelOriginal[i].ParentId == null)
                {
                    //Create view model for category level 0
                    categoryViewModel = new CategoryViewModel();
                    categoryViewModel.CategoryId = listCategoryViewModelOriginal[i].CategoryId;
                    categoryViewModel.CategoryName = listCategoryViewModelOriginal[i].CategoryName;
                    categoryViewModel.Description = listCategoryViewModelOriginal[i].Description;
                    categoryViewModel.Level = 0;
                    //Add Level 0 category to result
                    listCategoryViewModelOrdered.Add(categoryViewModel);
                    // De quy tim cac child category cua category level 0 vua tim duoc
                    loopFindChild(listCategoryViewModelOriginal, 1, categoryViewModel);
                }
            }
            //Return new list category ordered
            return listCategoryViewModelOrdered;
        }

        // De quy tim child category
        public void loopFindChild(List<CategoryViewModel> list, int level, CategoryViewModel categoryParent)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                if (list[i].ParentId == categoryParent.CategoryId)
                {
                    //Create view model for child category
                    CategoryViewModel categoryViewModel = new CategoryViewModel();
                    //Add information for child category
                    categoryViewModel.Level = level;
                    categoryViewModel.CategoryId = list[i].CategoryId;
                    categoryViewModel.CategoryName = list[i].CategoryName;
                    categoryViewModel.Description = list[i].Description;
                    categoryViewModel.ParentId = list[i].ParentId;
                    categoryParent.ChildCategory.Add(categoryViewModel);
                    loopFindChild(list, level + 1, categoryViewModel);
                }
            }
        }

        //Get list child category id by shop and parent cateogry
        public List<int> getChildCategoryId(string shopId, int parentCategoryId)
        {
            List<Category> listCate = repository.getChildCategoryId(shopId, parentCategoryId).ToList();
            if (listCate != null)
            {
                List<int> listChildId = new List<int>();
                for (int i = 0; i < listCate.Count(); i++)
                {
                    listChildId.Add(listCate[i].Id);
                }
                return listChildId;
            }
            else
            {
                return null;
            }


        }


    }
}
