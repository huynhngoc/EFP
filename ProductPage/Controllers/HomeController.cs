using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;
using ProductPage.Models;
using DataService.ViewModel;
using DataService;

namespace ProductPage.Controllers
{
    public class HomeController : Controller
    {
        

        public ActionResult Index()
        {
            //Create model of facebook page information
            FacebookPageViewModel pageInformation = new FacebookPageViewModel();
            pageInformation.UserName = "Nguyễn Văn A";
            pageInformation.ShopId = "1";
            pageInformation.UserId = "1";
            ViewBag.PageInfo = pageInformation;

            //Get Category list by shop id
            CategoryService categoryService = new CategoryService();
            List<CategoryViewModel> listCategory = new List<CategoryViewModel>();
            listCategory = categoryService.GetCategoryByShopId(pageInformation.ShopId);
            ViewBag.Category = listCategory;


            return View();
        }

        public ActionResult CheckOut(string customerId)
        {
            //Create model of facebook page information
            FacebookPageViewModel pageInformation = new FacebookPageViewModel();
            pageInformation.UserName = "Nguyễn Văn A";
            pageInformation.ShopId = "1";
            pageInformation.UserId = "1";
            ViewBag.PageInfo = pageInformation;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}