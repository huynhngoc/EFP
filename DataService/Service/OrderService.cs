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
                id = q.Id,
                customerId = q.CustomerId,
                customerName = q.Customer.Name,
                dateCreated = q.DateCreated,
                dateModified = q.DateModified,
                note = q.UserNote,
                total = CalculateTotalPrice(q.OrderDetails.ToList()),
                status = q.Status,
                shippingAddress = q.ShippingAddress,
                receiver = q.Receiver
            });
            return orderViewModelList.ToList();
        }

        public List<OrderDetailViewModel> GetOrderDetailsFromOrderId(int orderId)
        {
            var ods = orderDetailRepo.GetDetailByOrderId(orderId);
            var viewModelList = ods.Select(q => new OrderDetailViewModel()
            {
                quantity = q.Quantity,
                price = q.Price,
                details = q.Details,
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
        
        //public string GetCustomerNameFromOrderId(int orderId)
        //{
        //    return orderRepo.GetCustomerNameByCustomerId(orderRepo.GetOrderByOrderId(orderId).CustomerId);
        //}

        //private OrderViewModel ConvertOrderToViewModel(Order o)
        //{
        //    return new OrderViewModel(o.Id, o.DateCreated, o.DateModified, o.CustomerId,
        //        o.Customer.Name, o.UserNote, 0, o.Status, o.ShippingAddress, o.Receiver);
        //}

        //private OrderDetailViewModel ConvertOrderDetailToViewModel(OrderDetail od)
        //{
        //    return new OrderDetailViewModel(od.Details, od.Quantity, od.Price);
        //}

        public OrderViewModel GetOrderModelFromOrderId(int orderId)
        {
            OrderViewModel order = orderRepo.GetOrderByOrderId(orderId);
            return order;
        }

        public bool EditOrder(int orderId, string status)
        {
            return orderRepo.EditOrderByOrderId(orderId, status);
        }

        public bool AddOrder(string shopId, string note, string custId, string status, string address, string receiver
            , List<OrderDetailViewModel> listDetail)
        {
            bool rs = true;
            Order o=orderRepo.AddOrder(shopId, note, custId, status, address, receiver, listDetail);
            if (o != null)
            {
                rs = orderDetailRepo.AddOrderDetails(o.Id, listDetail);
            }
            else
            {
                rs = false;
            }
            return rs;
        }
    }
}
