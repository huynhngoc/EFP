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
using Newtonsoft.Json;
using Facebook;
using System.Configuration;

namespace ProductPage.Controllers
{
    public class HomeController : Controller
    {

        ShopService shopService = new ShopService();
        public ActionResult Index(string signed_request, string FBId, string shopId)
        {
            FacebookPageViewModel pageInformation = null;
            if (signed_request != null)
            {
                pageInformation = getInfoFromSignedRequest(signed_request);
            }
            else
            {
                if (FBId != null && shopId != null)
                {
                    pageInformation = (FacebookPageViewModel)Session["PageInfo" + FBId + shopId];
                }
            }

            if (pageInformation != null)
            {
                if (pageInformation.FBId == null || pageInformation.FBId == "")
                {
                    Response.Redirect("/Home/NotAuhorize?shopId="+ pageInformation.ShopId);
                }
            }
            else
            {
                Response.Redirect("/Home/NotAuhorize");
            }
            

            ViewBag.PageInfo = pageInformation;

            //Get Category list by shop id
            CategoryService categoryService = new CategoryService();
            List<CategoryViewModel> listCategory = new List<CategoryViewModel>();
            listCategory = categoryService.GetCategoryByShopId(pageInformation.ShopId);
            ViewBag.Category = listCategory;
            Session["PageInfo" + pageInformation.FBId + pageInformation.ShopId] = pageInformation;
            Session["username"] = pageInformation.UserName;

            return View();
        }

        public ActionResult CheckOut(string FBId, string shopId)
        {
            FacebookPageViewModel pageInformation = (FacebookPageViewModel)Session["PageInfo" + FBId + shopId];
            ViewBag.PageInfo = pageInformation;
            return View();
        }

        public ActionResult ViewProfile(string FBId, string shopId)
        {

            FacebookPageViewModel pageInformation = (FacebookPageViewModel)Session["PageInfo" + FBId + shopId];
            ViewBag.PageInfo = pageInformation;
            return View();
        }

        // Get user and shop info
        public FacebookPageViewModel getInfoFromSignedRequest(string signed_request)
        {
            string shopId = null;
            string userId = null;
            string userName = null;
            if (signed_request != null)
            {
                string strSignedRequest = hashSigned_request(signed_request);
                Debug.WriteLine("signed: " + strSignedRequest);
                dynamic signedRequest = System.Web.Helpers.Json.Decode(strSignedRequest);
                shopId = signedRequest.page.id.ToString();
                if (signedRequest["user_id"] != null)
                {
                    userId = signedRequest.user_id;
                    Debug.WriteLine("user id: " + userId);
                }
                ShopViewModel shop = shopService.GetShop(shopId);
                if (shop != null && userId != null)
                {
                    FacebookClient fbApp = new FacebookClient(shop.FbToken);
                    dynamic userInfo = System.Web.Helpers.Json.Decode(fbApp.Get(userId).ToString());
                    userName = userInfo.name;
                }
            }
            //Create model of facebook page information
            FacebookPageViewModel pageInformation = new FacebookPageViewModel();
            pageInformation.UserName = userName;
            pageInformation.ShopId = shopId;
            pageInformation.FBId = userId;
            return pageInformation;
        }


        // Hash signed_request
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

                byte[] appSecretBytes = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["FbAppSecret"]);
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
        public ActionResult NotAuhorize()
        {

            return View();
        }
    }
}