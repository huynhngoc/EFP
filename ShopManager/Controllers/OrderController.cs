using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataService.Service;
using System.Threading.Tasks;
using DataService.ViewModel;
using DataService.JqueryDataTable;
using DataService;

namespace ShopManager.Controllers
{
    [SessionRequiredFilter]
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
            //Session["ShopId"] = "1";
            //Session["CustId"] = "3";
            return View("All");
        }

        public JsonResult GetOrderFromShopId(JQueryDataTableParamModel param)
        {
            var shopId = (string)Session["ShopId"];
            OrderService service = new OrderService();
            var orders = service.GetAllOrders(shopId);
            var count = param.iDisplayStart + 1;

            var whichCol = param.iSortCol_0;// getcol
            var whichOrder = param.sSortDir_0;// asc or desc
            try
            {
                var rs = (orders.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                            (!string.IsNullOrEmpty(param.sSearch)
                            && q.CustomerName.ToLower().Contains(param.sSearch.ToLower())))
                            .OrderByDescending(q => q.DateModified)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength));
                var totalRecords = rs.Count();

                switch (param.iSortCol_0)
                {
                    case 0:
                        if (param.sSortDir_0 == "asc")
                        {
                            rs = rs.OrderBy(q => q.Id);
                        }
                        else
                        {
                            rs = rs.OrderByDescending(q => q.Id);
                        }
                        break;
                    case 1:
                        if (param.sSortDir_0 == "asc")
                        {
                            rs = rs.OrderBy(q => q.DateCreated);
                        }
                        else
                        {
                            rs = rs.OrderByDescending(q => q.DateCreated);
                        }
                        break;
                    case 2:
                        if (param.sSortDir_0 == "asc")
                        {
                            rs = rs.OrderBy(q => q.DateModified);
                        }
                        else
                        {
                            rs = rs.OrderByDescending(q => q.DateModified);
                        }
                        break;
                    case 3:
                        if (param.sSortDir_0 == "asc")
                        {
                            rs = rs.OrderBy(q => q.Status);
                        }
                        else
                        {
                            rs = rs.OrderByDescending(q => q.Status);
                        }
                        break;
                    case 4:
                        if (param.sSortDir_0 == "asc")
                        {
                            rs = rs.OrderBy(q => q.Total);
                        }
                        else
                        {
                            rs = rs.OrderByDescending(q => q.Total);
                        }
                        break;
                    case 5:
                        if (param.sSortDir_0 == "asc")
                        {
                            rs = rs.OrderBy(q => q.CustomerName);
                        }
                        else
                        {
                            rs = rs.OrderByDescending(q => q.CustomerName);
                        }
                        break;
                    default: rs = rs.OrderBy(q => q.DateModified); break;
                }

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = rs.ToList()
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
            var shopId = (string)Session["ShopId"];
            if (service.CheckOrderBelongsToShop(orderId, shopId))
            {
                var orders = service.GetOrderDetailsFromOrderId(orderId);
                var count = 1;
                try
                {
                    var rs = (orders
                                .ToList())
                                .Select(q => new IConvertible[] {
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
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDetailModel(int orderId)
        {
            OrderService service = new OrderService();
            return Json(service.GetOrderModelFromOrderId(orderId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateOrder(int orderId, int status, string receiver, string address, string phone, string note)
        {
            OrderService service = new OrderService();
            if (!(receiver!=null && receiver.Length > 0))
            {
                receiver = null;
            }
            return Content(service.EditOrder(orderId, status, receiver, address, phone, note).ToString());
        }

        [HttpPost]
        public ActionResult CreateOrder(int custId, string note, int status, string address, string receiver, string phone
            , List<OrderDetailViewModel> listDetail)
        {
            string shopId = (string)Session["ShopId"];// (string)Session["ShopId"];
            //string custId = (string)Session["CustId"];//(string)Session["CustId"];
            OrderService service = new OrderService();
            bool rs = service.AddOrder(shopId, note, custId, status, address, receiver, phone, listDetail);
            return Content(rs.ToString());
        }

        [HttpPost]
        public ActionResult CreateOrderReturnOrder(int custId, string note, int status, string address, string receiver, string phone
            , List<OrderDetailViewModel> listDetail)
        {
            string shopId = (string)Session["ShopId"];// (string)Session["ShopId"];
            //string custId = (string)Session["CustId"];//(string)Session["CustId"];
            OrderService service = new OrderService();
            Order rs = service.AddOrderReturnORder(shopId, note, custId, status, address, receiver, phone, listDetail);
            return Json(rs, JsonRequestBehavior.AllowGet);
        }
    }
}