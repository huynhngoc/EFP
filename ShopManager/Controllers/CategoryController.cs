using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;

namespace ShopManager.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Parent()
        {
            CategoryService service = new CategoryService();
            return Json(service.GetParentCategory("1"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Children(int parentId)
        {
            CategoryService service = new CategoryService();
            return Json(service.GetCategoryByParent(parentId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult All(string shopId)
        {
            shopId = (string)Session["ShopId"];
            CategoryService service = new CategoryService();
            return Json(service.GetAllCategory(shopId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Edit(int id, string name, string description)
        {
            CategoryService service = new CategoryService();
            return Json(service.EditCategory(id, name, description), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Delete(int id)
        {
            CategoryService service = new CategoryService();
            return Json(service.DeleteCategory(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckDelete(int id)
        {
            CategoryService service = new CategoryService();
            return Json(service.CheckDelete(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Add(string name, string description, int? parentId)
        {
            string shopId = (string) Session["ShopId"];
            CategoryService service = new CategoryService();
            return Json(service.AddNewCategory(name,description,parentId, shopId), JsonRequestBehavior.AllowGet);
        }
    }
}