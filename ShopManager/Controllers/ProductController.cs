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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Globalization;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Configuration;

namespace ShopManager.Controllers
{
    public class ProductController : Controller
    {
        static Cloudinary m_cloudinary = new Cloudinary(new Account(
            ConfigurationManager.AppSettings["CloudName"],
            ConfigurationManager.AppSettings["AppId"],
            ConfigurationManager.AppSettings["AppSecret"]));        
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

        public JsonResult Upload()
        {
            PictureService service = new PictureService();            
            try
            {
                int id = int.Parse(Request["id"]);
                for (int i = 0; i < HttpContext.Request.Files.Count; i++)
                {
                    var file = HttpContext.Request.Files[i];

                    if (file.ContentLength == 0)
                    {
                        //return PartialView("Upload", new Model(m_cloudinary));
                        break;
                    }

                    var result = m_cloudinary.Upload(new ImageUploadParams()
                    {
                        File = new CloudinaryDotNet.Actions.FileDescription(file.FileName,
                            file.InputStream),
                        Transformation = new Transformation().Height(400).Width(400).Crop("fit")
                    });

                    service.AddPicture(id, result.Uri.AbsoluteUri);

                    //foreach (var token in result.JsonObj.Children())
                    //{
                    //    if (token is JProperty)
                    //    {
                    //        JProperty prop = (JProperty)token;
                    //        results.Add(prop.Name, prop.Value.ToString());
                    //    }
                    //}

                    //Photo p = new Photo()
                    //{
                    //    Bytes = (int)result.Length,
                    //    CreatedAt = DateTime.Now,
                    //    Format = result.Format,
                    //    Height = result.Height,
                    //    Path = result.Uri.AbsolutePath,
                    //    PublicId = result.PublicId,
                    //    ResourceType = result.ResourceType,
                    //    SecureUrl = result.SecureUri.AbsoluteUri,
                    //    Signature = result.Signature,
                    //    Type = result.JsonObj["type"].ToString(),
                    //    Url = result.Uri.AbsoluteUri,
                    //    Version = Int32.Parse(result.Version),
                    //    Width = result.Width,
                    //};

                    //album.Photos.Add(p);
                    Debug.WriteLine(result);
                    Debug.WriteLine(result.Uri.AbsoluteUri);
                }
                return Json(new {success= true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Update(int id, string name, string description, int categoryId, decimal price, decimal? promotion, bool status, bool isInStock, int? templateId, string[] attr)
        {
            ProductService service = new ProductService();            

            try
            {                
                return Json(service.UpdateProduct(id, name,description,categoryId, price, promotion, status, isInStock, templateId, attr ), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult Add(string name, string description, int categoryId, decimal price, decimal? promotion, bool status, bool isInStock, int? templateId, string[] attr)
        {
            ProductService service = new ProductService();
            string shopId = "1";
            try
            {
                return Json(new { Id = service.AddProduct(name, description, categoryId, price, promotion, status, isInStock, templateId, attr, shopId).Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetStatus(int[] idList, bool status)
        {
            ProductService service = new ProductService();
            try
            {
                return Json(service.SetStatus(idList, status), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetInStock(int[] idList, bool inStock)
        {
            ProductService service = new ProductService();
            try
            {
                return Json(service.SetInStock(idList, inStock), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetProduct(JQueryDataTableParamModel param, string shopId, bool sName, bool sCate, bool sDesc )
        {
            ProductService service = new ProductService();                        
            try
            {
                var products = service.GetProduct(param, shopId, sName, sCate, sDesc);
                Debug.WriteLine("----x " + products.Count());                
                
                var totalRecords = products.Count();
                var data = products.Skip(param.iDisplayStart)
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
       


    }
}