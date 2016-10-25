using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class ChatCommentController: Controller
    {
        public ActionResult Index()
        {
            //Session["ShopId"] = "1";
            //Session["CustId"] = "3";
            return View();
        }
    }
}