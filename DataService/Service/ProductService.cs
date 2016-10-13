using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.JqueryDataTable;
using DataService.ViewModel;

namespace DataService.Service
{
    public class ProductService
    {
        ProductRepository repository = new ProductRepository();
        public ProductService()
        {                        
        }

        //public List<MasterProduct> GetAllMasterProductByShopId(string shopId)
        //{
        //    return repository.GetProductByShopId(shopId).ToList();
        //}

        public IQueryable<Product> GetAllMasterProductByShopId(string shopId)
        {
            return repository.GetProductByShopId(shopId);
        }

        public List<Product> GetProductByCategory(int cateId, string shopId)
        {
            return repository.GetProductByCategory(cateId, shopId).ToList();
        }

        //public async Task<> GetProduct(JQueryDataTableParamModel param, string shopId)
        //{
        //    var masterProducts = repository.GetProductByShopId(shopId);
        //    var count = param.iDisplayStart + 1;
        //    try
        //    {
        //        var rs = (await masterProducts.Where(q => string.IsNullOrEmpty(param.sSearch) ||
        //                    (!string.IsNullOrEmpty(param.sSearch)
        //                    && q.Name.ToLower().Contains(param.sSearch.ToLower())))
        //                    .OrderByDescending(q => q.Name)
        //                    .Skip(param.iDisplayStart)
        //                    .Take(param.iDisplayLength)
        //                    .ToListAsync())
        //                    .Select(q => new IConvertible[] {
        //                        count++,
        //                        q.Name,
        //                        q.Description,
        //                        q.Status,
        //                        q.Id
        //                    });
        //        var totalRecords = rs.Count();

        //        return Json(new
        //        {
        //            sEcho = param.sEcho,
        //            iTotalRecords = totalRecords,
        //            iTotalDisplayRecords = totalRecords,
        //            aaData = rs
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }            

        //}

        public IQueryable<ProductViewModel> GetProduct(JQueryDataTableParamModel param, string shopId, bool sName, bool sCate, bool sDesc)
        {
            var rs = repository.GetProduct(param, shopId, sName, sCate, sDesc);            
            return rs;
        }

        public ProductViewModel GetProductById(int id)
        {
            return repository.GetProductById(id);
        }
    }
}
