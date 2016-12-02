using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using DataService.Service;
using DataService.JqueryDataTable;
using DataService;
using DataService.ViewModel;
using Facebook;
using System.Dynamic;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        CustomerService cusService = new CustomerService();
        OrderService orderService = new OrderService();
        /// <summary>
        /// get all customer
        /// </summary>
        /// <returns>json-list of customer</returns>

        public JsonResult GetAllCustomer(JQueryDataTableParamModel param)
        {
            string shopId = Session["ShopId"].ToString();
            try
            {
                var whichCol = Convert.ToInt32(Request["iSortCol_0"]);// getcol
                var whichOrder = Request["sSortDir_0"]; // asc or desc


                var customers = cusService.GetAllCustomer(param, shopId);
                Debug.WriteLine("----x " + customers.Count());
                
                if (whichOrder == "asc")
                    customers = customers.OrderBy(q => whichCol == 0 ? q.Name :
                                                            whichCol == 1 ? q.Description :
                                                            whichCol == 2 ? q.Address :
                                                            whichCol == 3 ? q.Phone : q.Email);
                else
                    customers = customers.OrderByDescending(q => whichCol == 0 ? q.Name :
                                                            whichCol == 1 ? q.Description :
                                                            whichCol == 2 ? q.Address :
                                                            whichCol == 3 ? q.Phone : q.Email);
                var totalRecords = customers.Count();
                //var data = customers;
                IQueryable<CustomerViewModel> data = customers.Skip(param.iDisplayStart).Take(param.iDisplayLength);
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

        public JsonResult GetAllOrderByCustomerId(int cusId, JQueryDataTableParamModel param)
        {
            string shopId = Session["ShopId"].ToString();
            try
            {
                var whichCol = Convert.ToInt32(Request["iSortCol_0"]);// getcol
                var whichOrder = Request["sSortDir_0"]; // asc or desc


                var orders = orderService.GetOrderByShopIdAndCustomerId(shopId, cusId, param);
                Debug.WriteLine("----x " + orders.Count());
                
                //if (whichOrder == "asc")

                //    data = data.OrderBy(q => whichCol == 0 ? q.Id : whichCol == 1 ? q.DateModified : q.Total);
                //else
                //    data = data.OrderByDescending(q => whichCol == 0 ? q.Id : whichCol == 1 ? q.DateModified : q.Total);


                switch (whichCol)
                {
                    case 0:
                        if (whichOrder == "asc")
                        {
                            orders = orders.OrderBy(q => q.Id);
                        }
                        else
                        {
                            orders = orders.OrderByDescending(q => q.Id);
                        }
                        break;

                    case 1:
                        if (whichOrder == "asc")
                        {
                            orders = orders.OrderBy(q => q.DateModified);
                        }
                        else
                        {
                            orders = orders.OrderByDescending(q => q.DateModified);
                        }
                        break;
                    case 2:
                        if (whichOrder == "asc")
                        {
                            orders = orders.OrderBy(q => q.Total);
                        }
                        else
                        {
                            orders = orders.OrderByDescending(q => q.Total);
                        }
                        break;
                    default: orders = orders.OrderBy(q => q.DateModified); break;
                }
                IQueryable<OrderViewModel> data = orders.Skip(param.iDisplayStart).Take(param.iDisplayLength);
                var totalRecords = orders.Count();
                //var data = customers;
                Debug.WriteLine("display start " + param.iDisplayStart + "display length " + param.iDisplayLength);
                Debug.WriteLine("-----data3 " + orders.ToString());
                var displayRecords = data.Count();
                Debug.WriteLine("-----l ");

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = orders.Count(),
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
            string shopId = (string)Session["ShopId"];
            //var data = service.AddCustomer(Id, Name, Addr, Desc, Phone, Email, ShopId);
            //Debug.WriteLine("counttttttttttttttttt " + tmp_cus.Count);
            var data = cusService.AddCustomer(Id, Name, Addr, Desc, Phone, Email, shopId);
            return data;
            //1 = success / 2 = success but fail in creating / 3 = exception
        }
        public bool EditCustomer(int Id, string Name, string Addr, string Desc, string Phone, string Email, string ShopId)
        {
            ShopId = (string)Session["ShopId"];
            Debug.WriteLine("id: " + Id + "| name: " + Name + "|address: " + Addr + "|description: " + Desc + "|Phone: "
                + Phone + "|Email: " + Email + "|ShopId: " + ShopId);
            return cusService.EditCustomer(Id, Name, Addr, Desc, Phone, Email, ShopId);
        }

        public JsonResult AddCustomerReturnId(string Id, string Name, string Addr, string Desc, string Phone, string Email)
        {
            string shopId = (string)Session["ShopId"];
            CustomerService service = new CustomerService();
            var data = service.AddCustomerReturnCustomer(Id, Name, Addr, Desc, Phone, Email, shopId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerById(int id)
        {
            string shopId = (string)Session["ShopId"];
            try
            {
                //string shopId = (string)Session["ShopId"];
                Customer newCustomer = cusService.GetCustomerByCustomerId(id, shopId);
                if (newCustomer != null)
                    return Json(newCustomer, JsonRequestBehavior.AllowGet);
                else
                {
                    Debug.WriteLine("nullll");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }
        
        //Long
        public JsonResult GetAllCustomers(JQueryDataTableParamModel param)
        {
            ProductService service = new ProductService();
            var shopId = (string)Session["ShopId"];
            try
            {
                var customers = cusService.GetAllCustomer(param, shopId);

                var totalRecords = customers.Count();
                var data = customers;
                Debug.WriteLine("-----l ");
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,//displayRecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return Json(new { success = false, e }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}