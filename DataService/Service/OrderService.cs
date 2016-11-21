using DataService.JqueryDataTable;
using DataService.Repository;
using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class OrderService
    {
        OrderRepository orderRepo = new OrderRepository();
        OrderDetailRepository orderDetailRepo = new OrderDetailRepository();

        public List<OrderViewModel> GetAllOrders(string shopId)
        {
            var orders = orderRepo.GetAllOrderByShopId(shopId);
            var orderViewModelList = orders.Select(q => new OrderViewModel()
            {
                Id = q.Id,
                CustomerId = q.CustomerId,
                CustomerName = q.Customer.Name,
                DateCreated = q.DateCreated,
                DateModified = q.DateModified,
                Note = q.UserNote,
                Total = CalculateTotalPrice(q.OrderDetails.ToList()),
                Status = q.Status,
                ShippingAddress = q.ShippingAddress,
                Receiver = q.Receiver,
                Phone = q.Phone
            });
            return orderViewModelList.ToList();
        }

        public List<OrderDetailViewModel> GetOrderDetailsFromOrderId(int orderId)
        {
            var ods = orderDetailRepo.GetDetailByOrderId(orderId);
            var viewModelList = ods.Select(q => new OrderDetailViewModel()
            {
                ProductId = q.ProductId,
                Quantity = q.Quantity,
                Price = q.Price,
                Properties = q.Properties,
            });
            return viewModelList.ToList();
        }

        private decimal CalculateTotalPrice(List<OrderDetail> list)
        {
            decimal total = 0;
            foreach (OrderDetail o in list)
            {
                total += o.Quantity * o.Price;
            }
            return total;
        }

        public OrderViewModel GetOrderModelFromOrderId(int orderId)
        {
            OrderViewModel order = orderRepo.GetOrderByOrderId(orderId);
            return order;
        }

        public bool EditOrder(int orderId, int status, string receiver, string address, string phone, string note)
        {
            return orderRepo.EditOrderByOrderId(orderId, status, receiver, address, phone, note);
        }

        public bool AddOrder(string shopId, string note, int custId, int status, string address, string receiver, string phone
            , List<OrderDetailViewModel> listDetail)
        {
            bool rs = true;
            if (listDetail == null || listDetail.Count == 0)
            {
                rs = false;
            }
            else
            {
                Order o = new Order();
                o.ShopId = shopId;
                o.UserNote = note;
                o.DateCreated = DateTime.Now;
                o.DateModified = DateTime.Now;
                o.CustomerId = custId;
                o.Status = status;
                o.ShippingAddress = address;
                o.Receiver = receiver;
                o.Phone = phone;

                o = orderRepo.AddOrder(o);

                if (o != null)
                {
                    rs = orderDetailRepo.AddOrderDetails(o.Id, listDetail);
                }
                else
                {
                    rs = false;
                }
            }

            return rs;
        }

        public Order AddOrderReturnORder(string shopId, string note, int custId, int status, string address, string receiver, string phone
            , List<OrderDetailViewModel> listDetail)
        {
            bool rs = true;
            Order o = new Order();

            if (listDetail == null || listDetail.Count == 0)
            {
                return null;
            }
            else
            {
                o.ShopId = shopId;
                o.UserNote = note;
                o.DateCreated = DateTime.Now;
                o.DateModified = DateTime.Now;
                o.CustomerId = custId;
                o.Status = status;
                o.ShippingAddress = address;
                o.Receiver = receiver;
                o.Phone = phone;

                o = orderRepo.AddOrder(o);

                if (o != null)
                {
                    rs = orderDetailRepo.AddOrderDetails(o.Id, listDetail);
                    if (rs == false)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return o;
        }

        public bool CheckOrderBelongsToShop(int orderId, string shopId)
        {
            var sid = orderRepo.GetShopIdOfOrder(orderId);
            if (sid.Equals(shopId))
            {
                return true;
            }
            return false;
        }

        //Andnd get order by shop and customer id
        public List<OrderViewModel> GetOrderByShopIdAndCustomerId(string shopId, int customerId)
        {
            var listOrders = orderRepo.GetOrderByShopIdAndCustomerId(shopId, customerId);

            return listOrders;
        }
        // get order by shop and customer id and  fill into datatable
        public IQueryable<OrderViewModel> GetOrderByShopIdAndCustomerId(string shopId, int customerId, JQueryDataTableParamModel param)
        {
            var listOrders = orderRepo.GetOrderByShopIdAndCustomerId(shopId, customerId, param);

            return listOrders;
        }

        //ngochb test
        public decimal GetOrderRevenue(string shopId, DateTime startDate, DateTime endDate)
        {
            return orderRepo.GetOrderRevenue(shopId, startDate, endDate);
        }

        //ngochb
        public List<List<object>> GetOrderAnalysis(string shopId, long startDate, long endDate, int divide)
        {
            var result = new List<List<object>>();
            var orderCompletedNumbers = new List<object>();
            var orderCanceledNumbers = new List<object>();
            var orderIncompletedNumbers = new List<object>();
            var orderRevenue = new List<object>();
            var timeLine = new List<object>();

            orderCompletedNumbers.Add("Đơn hàng hoàn thành");
            orderCanceledNumbers.Add("Đơn hàng đã hủy");
            orderIncompletedNumbers.Add("Đơn hàng chưa xong");
            orderRevenue.Add("Doanh thu");
            timeLine.Add("x");

            long diff = endDate - startDate;
            long level = diff / divide;
            startDate = startDate - level;
            while (startDate < endDate - level)
            {
                orderCompletedNumbers.Add(
                    orderRepo.GetCompletedOrderNumber(
                        shopId, ToDate(startDate), ToDate(startDate + level)));
                orderCanceledNumbers.Add(
                    orderRepo.GetCanceledOrderNumber(
                        shopId, ToDate(startDate), ToDate(startDate + level)));
                orderIncompletedNumbers.Add(
                    orderRepo.GetIncompleteOrderNumber(
                        shopId, ToDate(startDate), ToDate(startDate + level)));
                orderRevenue.Add(
                    orderRepo.GetOrderRevenue(
                        shopId, ToDate(startDate), ToDate(startDate + level)));                
                startDate += level;
                timeLine.Add(DateString(startDate));
            }
            if (startDate/1000*1000 < endDate/1000*1000)
            {
                orderCompletedNumbers.Add(
                    orderRepo.GetCompletedOrderNumber(
                        shopId, ToDate(startDate), ToDate(endDate)));
                orderCanceledNumbers.Add(
                    orderRepo.GetCanceledOrderNumber(
                        shopId, ToDate(startDate), ToDate(endDate)));
                orderIncompletedNumbers.Add(
                    orderRepo.GetIncompleteOrderNumber(
                        shopId, ToDate(startDate), ToDate(endDate)));
                orderRevenue.Add(
                    orderRepo.GetOrderRevenue(
                        shopId, ToDate(startDate), ToDate(endDate)));
                timeLine.Add(DateString(endDate));
            }            

            result.Add(timeLine);
            result.Add(orderCompletedNumbers);
            result.Add(orderIncompletedNumbers);
            result.Add(orderCanceledNumbers);
            result.Add(orderRevenue);

            return result;
        }

        private DateTime ToDate(long date)
        {
            return (new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(date)).ToLocalTime();
        }

        private string DateString(long date)
        {
            DateTime dateStr = (new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(date)).ToLocalTime();
            return string.Format("{0}/{1}/{2} {3:D2}:{4:D2}", dateStr.Day, dateStr.Month, dateStr.Year, dateStr.Hour, dateStr.Minute);
        }
    }
}
