using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class OrderRepository : BaseRepository<Order>
    {
        public IEnumerable<Order> GetAllOrderByShopId(string shopId)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            return dbSet.Where(q => q.ShopId == shopId).OrderBy(q => q.DateModified);
        }

        public OrderViewModel GetOrderByOrderId(int orderId)
        {
            return dbSet.Where(q => q.Id == orderId).Select(q => new OrderViewModel()
            {
                Id = q.Id,
                CustomerId = q.CustomerId,
                CustomerName = q.Customer.Name,
                DateCreated = q.DateCreated,
                DateModified = q.DateModified,
                Note = q.UserNote,
                Total = 0,
                Status = q.Status,
                ShippingAddress = q.ShippingAddress,
                Receiver = q.Receiver,
                Phone = q.Phone
        }).First();
        }

        public bool EditOrderByOrderId(int orderId, int status, string receiver, string address, string phone)
        {
            Order order = this.FindByKey(orderId);
            order.Status = status;
            order.Receiver = receiver;
            order.Phone = phone;
            order.ShippingAddress = address;
            order.DateModified = DateTime.Now;

            return this.Update(order);
        }

        public Order AddOrder(Order o)
        {
            var transaction = entites.Database.BeginTransaction();

            o = CreateNew(o);
            
            if (o!=null)
            {
                entites.SaveChanges();
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
                return null;
            }

            return o;
        }
    }

    public class OrderDetailRepository : BaseRepository<OrderDetail>
    {
        public IEnumerable<OrderDetail> GetDetailByOrderId(int orderId)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            return dbSet.Where(q => q.OrderId == orderId);
        }

        public TemplateProduct GetMasterProductByProductId(int productId)
        {
            return dbSet.Where(q => q.ProductId == productId)
                .Select(q => q.Product.TemplateProduct).First();

        }

        public Product GetDetailedProductByProductId(int productId)
        {
            return dbSet.Where(q => q.ProductId == productId)
                .Select(q => q.Product).First();
        }

        public bool AddOrderDetails(int orderId, List<OrderDetailViewModel> listDetail)
        {
            bool rs = true;
            var transaction = entites.Database.BeginTransaction();
            foreach (OrderDetailViewModel o in listDetail)
            {
                OrderDetail od = new OrderDetail();
                od.OrderId = orderId;
                od.ProductId = o.ProductId;
                od.Properties = o.Properties;
                od.Quantity = o.Quantity;
                od.Price = o.Price;
                try
                {
                    rs = Create(od);
                    if (!rs)
                    {
                        break;
                    }
                }
                catch (Exception)
                {
                    rs = false;
                }
            }
            if (rs)
            {
                entites.SaveChanges();
                transaction.Commit();
            }
            else
            {
                transaction.Rollback();
            }
            return rs;
        }
    }
}
