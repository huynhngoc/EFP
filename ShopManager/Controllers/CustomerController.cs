using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using DataService.Service;
using DataService.JqueryDataTable;
using DataService;
namespace ShopManager.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// get all customer
        /// </summary>
        /// <returns>json-list of customer</returns>
        
        public JsonResult GetAllCustomer(JQueryDataTableParamModel param)
        {
            CustomerService service = new CustomerService();
            Session["shopId"] = 7;
            string shopId = Session["shopId"].ToString();
            try
            {
                param.iSortCol_0 = Convert.ToInt32(Request["iSortCol_0"]);// getcol
                param.sSortDir_0 = Request["sSortDir_0"]; // asc or desc


                var customers = service.GetAllCustomer(param, shopId);
                Debug.WriteLine("----x " + customers.Count());
                IQueryable<CustomerViewModel> data = customers.Skip(param.iDisplayStart).Take(param.iDisplayLength);

                var totalRecords = customers.Count();
                //var data = customers;
                Debug.WriteLine("display start " + param.iDisplayStart + "display length " + param.iDisplayLength);
                Debug.WriteLine("-----data2 " + customers.ToString());
                var displayRecords = data.Count();
                Debug.WriteLine("-----l ");

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = customers.Count(),
                    aaData = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
            //var data = service.Get();
            //return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// add new customer
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Addr"></param>
        /// <param name="Desc"></param>
        /// <param name="Phone"></param>
        /// <param name="Email"></param>
        /// <param name="ShopId"></param>
        /// <returns>int</returns>
        public int AddCustomer(string Id, string Name, string Addr, string Desc, string Phone, string Email)
        {
            CustomerService service = new CustomerService();
            //var data = service.AddCustomer(Id, Name, Addr, Desc, Phone, Email, ShopId);
            var data = service.AddCustomer(Id, Name, Addr, Desc, Phone, Email, "7");
            return data;
            //1 = success / 2 = success but fail in creating / 3 = exception
        }
        public bool EditCustomer(string Id, string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
        {
            ShopId = "7";
            Debug.WriteLine("id: " + Id + "| name: " + Name + "|address: " + Addr + "|description: " + Desc + "|Phone: "
                + Phone + "|Email: " + Email + "|ShopId: " + ShopId);
            CustomerService service = new CustomerService();

            return service.EditCustomer(Id,Name,Addr,Desc,Phone,Email,ShopId);
        }
    }
}