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

        public bool EditOrder(int orderId, string status, string receiver, string address, string phone)
        {
            return orderRepo.EditOrderByOrderId(orderId, status, receiver, address, phone);
        }

        public bool AddOrder(string shopId, string note, int custId, string status, string address, string receiver, string phone
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

        //Andnd get order by shop and customer id
        public List<OrderViewModel> GetOrderByShopIdAndCustomerId(string shopId, int customerId)
        {
            var listOrders = orderRepo.GetOrderByShopIdAndCustomerId(shopId, customerId);

            return listOrders;
        }
    }
}
