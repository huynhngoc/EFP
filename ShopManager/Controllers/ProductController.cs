using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.JqueryDataTable;
using DataService.Service;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ShopManager.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        //public async Task<JsonResult> GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        //{
        //    //string shopId = "1";
        //    MasterProductService service = new MasterProductService();
        //    var masterProducts = service.GetAllMasterProductByShopId(shopId);
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
        //                        q.Category == null ? null : q.Category.Name,                                
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
        
        public ActionResult GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        {
            MasterProductService service = new MasterProductService();
            var masterProducts = service.GetAllMasterProductByShopId(shopId);
            var count = param.iDisplayStart + 1;
            try
            {
                var rs = service.GetAllMasterProductByShopId(shopId).ToList();
                var totalRecords = rs.Count();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = rs
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}