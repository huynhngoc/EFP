using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.JqueryDataTable;
using DataService.Service;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ShopManager.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }        

        public JsonResult GetProductById(int id)
        {
            ProductService service = new ProductService();
            try
            {
                return Json(service.GetProductById(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMasterProduct(JQueryDataTableParamModel param, string shopId, bool sName, bool sCate, bool sDesc )
        {
            ProductService service = new ProductService();                        
            try
            {
                var masterProducts = service.GetProduct(param, shopId, sName, sCate, sDesc);
                Debug.WriteLine("----x " + masterProducts.Count());                
                
                var totalRecords = masterProducts.Count();
                var data = masterProducts.Skip(param.iDisplayStart)
                    .Take(param.iDisplayLength);
                var displayRecords = data.Count();
                Debug.WriteLine("-----l ");
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,//displayRecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }            
        }
        //public JsonResult GetDetailedProduct(JQueryDataTableParamModel param, int masterId)
        //{
        //    //string shopId = "1";
        //    DetailedProductService service = new DetailedProductService();                        
        //    try
        //    {
        //        var detailedProducts = service.GetDetailedProduct(param, masterId);
        //        Debug.WriteLine("----x " + detailedProducts.Count());                
                
        //        var totalRecords = detailedProducts.Count();
        //        var data = detailedProducts.Skip(param.iDisplayStart)
        //            .Take(param.iDisplayLength);
        //        var displayRecords = data.Count();
        //        Debug.WriteLine("-----l ");
        //        return Json(new
        //        {
        //            sEcho = param.sEcho,
        //            iTotalRecords = totalRecords,
        //            iTotalDisplayRecords = displayRecords,
        //            aaData = data
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch(Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
        //        //var data = new string[0];
        //        //Debug.WriteLine(e.Message);
        //        //return Json(new
        //        //{
        //        //    sEcho = param.sEcho,
        //        //    iTotalRecords = 0,
        //        //    iTotalDisplayRecords = 0,
        //        //    aaData = data
        //        //}, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //public ActionResult GetProduct(JQueryDataTableParamModel param, string shopId)
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