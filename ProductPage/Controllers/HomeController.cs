using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;
using ProductPage.Models;
using DataService.ViewModel;
using DataService;
using System.Diagnostics;
using System.Text;

namespace ProductPage.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index(string signed_request)
        {
            if (signed_request != null)
            {
                string signedRequest = hashSigned_request(signed_request);
                Debug.WriteLine("signed_requets: " + signedRequest);
            }

            //Create model of facebook page information
            FacebookPageViewModel pageInformation = new FacebookPageViewModel();
            pageInformation.UserName = "Nguyễn Văn A";
            pageInformation.ShopId = "1";
            pageInformation.FBId = "1";
            ViewBag.PageInfo = pageInformation;

            //Get Category list by shop id
            CategoryService categoryService = new CategoryService();
            List<CategoryViewModel> listCategory = new List<CategoryViewModel>();
            listCategory = categoryService.GetCategoryByShopId(pageInformation.ShopId);
            ViewBag.Category = listCategory;


            return View();
        }

        public ActionResult CheckOut()
        {
            //Create model of facebook page information
            FacebookPageViewModel pageInformation = new FacebookPageViewModel();
            pageInformation.UserName = "Nguyễn Văn A";
            pageInformation.ShopId = "1";
            pageInformation.FBId = "1";
            ViewBag.PageInfo = pageInformation;
            return View();
        }

        public ActionResult ViewProfile()
        {
            FacebookPageViewModel pageInformation = new FacebookPageViewModel();
            pageInformation.UserName = "Nguyễn Văn A";
            pageInformation.ShopId = "1";
            pageInformation.FBId = "1";
            ViewBag.PageInfo = pageInformation;
            return View();
        }

        public string hashSigned_request(string signed_request)
        {
            if (signed_request.Contains("."))
            {
                string[] split = signed_request.Split('.');

                string signatureRaw = FixBase64String(split[0]);
                string dataRaw = FixBase64String(split[1]);

                // the decoded signature
                byte[] signature = Convert.FromBase64String(signatureRaw);

                byte[] dataBuffer = Convert.FromBase64String(dataRaw);

                // JSON object
                string data = Encoding.UTF8.GetString(dataBuffer);

                byte[] appSecretBytes = Encoding.UTF8.GetBytes("40d95140ed2f41994dffa498bf62bb4c");
                System.Security.Cryptography.HMAC hmac = new System.Security.Cryptography.HMACSHA256(appSecretBytes);
                byte[] expectedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(split[1]));
                if (expectedHash.SequenceEqual(signature))
                {
                    signed_request = data;
                }
            }
            return signed_request;
        }

        private static string FixBase64String(string str)
        {
            while (str.Length % 4 != 0)
            {
                str = str.PadRight(str.Length + 1, '=');
            }
            return str.Replace("-", "+").Replace("_", "/");
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