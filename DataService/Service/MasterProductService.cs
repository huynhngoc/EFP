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
    public class MasterProductService
    {
        MasterProductRepository repository = new MasterProductRepository();
        public MasterProductService()
        {                        
        }

        //public List<MasterProduct> GetAllMasterProductByShopId(string shopId)
        //{
        //    return repository.GetMasterProductByShopId(shopId).ToList();
        //}

        public IQueryable<MasterProduct> GetAllMasterProductByShopId(string shopId)
        {
            return repository.GetMasterProductByShopId(shopId);
        }

        public List<MasterProduct> GetMasterProductByCategory(int cateId, string shopId)
        {
            return repository.GetMasterProductByCategory(cateId, shopId).ToList();
        }

        //public async Task<> GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        //{
        //    var masterProducts = repository.GetMasterProductByShopId(shopId);
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

        public IQueryable<MasterProductViewModel> GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        {
            var rs = repository.GetMasterProduct(param, shopId);            
            return rs;
        }
    }
}
