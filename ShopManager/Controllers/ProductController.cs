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

        public async Task<JsonResult> GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        {
            //string shopId = "1";
            MasterProductService service = new MasterProductService();
            var masterProducts = service.GetAllMasterProductByShopId(shopId);
            var count = param.iDisplayStart + 1;
            try
            {
                var rs = (await masterProducts.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                                (!string.IsNullOrEmpty(param.sSearch)
                                && q.Name.ToLower().Contains(param.sSearch.ToLower())
                                )
                            )
                            .OrderByDescending(q => q.Name)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength)
                            .ToListAsync())
                            .Select(q => new IConvertible[] {
                                count++,
                                q.Name,
                                q.Category == null ? null : q.Category.Name,
                                q.Description,
                                q.Status,
                                q.Id
                            });
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

        public JsonResult GetDetailedProduct(JQueryDataTableParamModel param, string shopId, int masterId)
        {
            //string shopId = "1";
            DetailedProductService service = new DetailedProductService();
            var detailedProducts = service.GetDetailedProduct(2,shopId);
            var count = param.iDisplayStart + 1;
            try
            {
                var rs = (detailedProducts.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                                (!string.IsNullOrEmpty(param.sSearch)
                                && (q.Attr1.ToLower().Contains(param.sSearch.ToLower())
                                    || q.Attr2.ToLower().Contains(param.sSearch.ToLower())
                                    || q.Attr3.ToLower().Contains(param.sSearch.ToLower())
                                    || q.Attr4.ToLower().Contains(param.sSearch.ToLower())
                                    || q.Attr5.ToLower().Contains(param.sSearch.ToLower())
                                    || q.Attr6.ToLower().Contains(param.sSearch.ToLower())
                                    || q.Attr7.ToLower().Contains(param.sSearch.ToLower())
                                    )
                                )
                            )
                            .OrderByDescending(q => q.Id)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength)
                            //.ToListAsync()
                            )
                            .Select(q => new IConvertible[] {
                                count,
                                //(q.MasterProduct.Attr1 !=null ? (q.MasterProduct.Attr1 + ": "+ q.Attr1): ""
                                // + q.MasterProduct.Attr2 !=null ? (", " + q.MasterProduct.Attr2 + ": "+ q.Attr2):""
                                // + q.MasterProduct.Attr3 !=null ? (", " + q.MasterProduct.Attr3 + ": "+ q.Attr3):""
                                // + q.MasterProduct.Attr4 !=null ? (", " + q.MasterProduct.Attr4 + ": "+ q.Attr4):""
                                // + q.MasterProduct.Attr5 !=null ? (", " + q.MasterProduct.Attr5 + ": "+ q.Attr5):""
                                // + q.MasterProduct.Attr6 !=null ? (", " + q.MasterProduct.Attr6 + ": "+ q.Attr6):""
                                // + q.MasterProduct.Attr7 !=null ? ", " + (q.MasterProduct.Attr7 + ": "+ q.Attr7):""),
                                q.Status,
                                q.Id
                            });
                var totalRecords = rs.Count();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = rs
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }

        }

        //public ActionResult GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        //{
        //    MasterProductService service = new MasterProductService();
        //    var masterProducts = service.GetAllMasterProductByShopId(shopId);
        //    var count = param.iDisplayStart + 1;
        //    try
        //    {
        //        var rs = service.GetAllMasterProductByShopId(shopId).ToList();
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
    }
}