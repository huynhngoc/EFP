using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModel;
using DataService.JqueryDataTable;

namespace DataService.Repository
{
    public class ProductRepository: BaseRepository<Product>
    {        
        public IQueryable<Product> GetProductByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }

        public IEnumerable<Product> GetProductByCategory(int cateId, string shopId)
        {
            IEnumerable<int> query = from c in entites.Categories where c.ParentId == cateId select c.Id;
            Debug.WriteLine(query.ToArray());
            var data = dbSet.Where(q => (q.CategoryId == cateId || query.Contains(q.CategoryId.Value)) && q.ShopId == shopId);            
            
            return data;        
        }

        public IQueryable<ProductViewModel> GetProduct(JQueryDataTableParamModel param, string shopId)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => q.ShopId == shopId);            
            var search = param.sSearch;
            rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                                (!string.IsNullOrEmpty(param.sSearch)
                                && q.Name.ToLower().Contains(param.sSearch.ToLower())))
                .OrderBy(q => q.Name);
            Debug.WriteLine("---------rs " + rs.Count());
            //if (rs.Count() == 0) return null;
            var data = rs.Select(q => new ProductViewModel()
            {
                Id = q.Id,
                Name = q.Name,
                Category = (new List<string> { q.CategoryId.ToString()
                                             , q.Category.Name//.ToString() 
                }),
                Description = q.Description,
                //Attr = (new List<string>  {q.Attr1//.ToString()
                //                , q.Attr2//.ToString()
                //                , q.Attr3//.ToString()
                //                , q.Attr4//.ToString()
                //                , q.Attr5//.ToString()
                //                , q.Attr6//.ToString()
                //                , q.Attr7//.ToString() 
                //}),
                Status = q.Status,
                DateCreated = q.DateCreated,
                DateModified = q.DateModified,
                Price = (decimal)(q.Price),
                Promotion = (decimal?)(q.Price)
            });
            Debug.WriteLine("---------data " + data.Count());            
            return data;            
        }
    }
}
