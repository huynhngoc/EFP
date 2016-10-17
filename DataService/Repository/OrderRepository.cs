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
                id = q.Id,
                customerId = q.CustomerId,
                customerName = q.Customer.Name,
                dateCreated = q.DateCreated,
                dateModified = q.DateModified,
                note = q.UserNote,
                total = 0,
                status = q.Status,
                shippingAddress = q.ShippingAddress,
                receiver = q.Receiver
        }).First();
        }

        public bool EditOrderByOrderId(int orderId, string status)
        {
            Order order = this.FindByKey(orderId);
            order.Status = status;
            order.DateModified = DateTime.Now;
            return this.Update(order);
        }

        public Order AddOrder(string shopId, string note, string custId, string status
            , string address, string receiver, List<OrderDetailViewModel> listDetail)
        {
            bool rs = true;
            var transaction = entites.Database.BeginTransaction();
            List<OrderDetail> listOD = new List<OrderDetail>();
            //created = modified
            Order o = new Order();
            o.ShopId = shopId;
            o.UserNote = note;
            o.DateCreated = DateTime.Now;
            o.DateModified = DateTime.Now;
            o.CustomerId = custId;
            o.Status = status;
            o.ShippingAddress = address;
            o.Receiver = receiver;

            o = CreateNew(o);
            if (o != null)
            {
                OrderDetailRepository odRepo = new OrderDetailRepository();
                odRepo.AddOrderDetails(o.Id, listDetail);
            }
            else
            {
                rs = false;
            }
            
            if (rs)
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
                od.ProductId = o.productId;
                od.Details = o.details;
                od.Quantity = o.quantity;
                od.Price = o.price;
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
