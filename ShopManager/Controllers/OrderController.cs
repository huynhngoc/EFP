using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;
using System.Threading.Tasks;
using DataService.ViewModel;
using DataService.JqueryDataTable;

namespace ShopManager.Controllers
{
    public class OrderController : Controller
    {
        //public ActionResult GetOrderFromShopId(string shopId)
        //{
        //    OrderService service = new OrderService();
        //    var data = service.GetAllOrders(shopId);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult Index()
        {
            Session["ShopId"] = "1";
            Session["CustId"] = "Cust01";
            return View("All");
        }

        public JsonResult GetOrderFromShopId(JQueryDataTableParamModel param, string shopId)
        {
            OrderService service = new OrderService();
            var orders = service.GetAllOrders(shopId);
            var count = param.iDisplayStart + 1;
            try
            {
                var rs = (orders.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                            (!string.IsNullOrEmpty(param.sSearch)
                            && q.CustomerName.ToLower().Contains(param.sSearch.ToLower())))
                            .OrderByDescending(q => q.DateModified)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength)
                            .ToList())
                            .Select(q => new IConvertible[] {
                                q.Id,
                                q.DateCreated.ToShortDateString(),
                                q.DateModified.ToShortDateString(),
                                q.Status,
                                q.Total,
                                q.CustomerName,
                                q.ShippingAddress,
                                q.Receiver,
                                q.Phone
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
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetOrderDetailFromOrderId(JQueryDataTableParamModel param, int orderId)
        {
            OrderService service = new OrderService();
            var orders = service.GetOrderDetailsFromOrderId(orderId);
            var count = 1;
            try
            {
                var rs = (orders
                            .ToList())
                            .Select(q => new IConvertible[] {
                                ++count,
                                q.Properties,
                                q.Quantity,
                                q.Price,
                                q.Price*q.Quantity
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
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetDetailModel(int orderId)
        {
            OrderService service = new OrderService();
            return Json(service.GetOrderModelFromOrderId(orderId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateOrder(int orderId, string status, string receiver, string address, string phone)
        {
            OrderService service = new OrderService();
            if (!(receiver!=null && receiver.Length > 0))
            {
                receiver = null;
            }
            return Content(service.EditOrder(orderId, status, receiver, address, phone).ToString());
        }

        [HttpPost]
        public ActionResult CreateOrder(string note, string status, string address, string receiver, string phone
            , List<OrderDetailViewModel> listDetail)
        {
            string shopId = "1";// (string)Session["ShopId"];
            string custId = "3";//(string)Session["CustId"];
            OrderService service = new OrderService();
            bool rs = service.AddOrder(shopId, note, custId, status, address, receiver, phone, listDetail);
            return Content(rs.ToString());
        }
    }
}