using DataService.JqueryDataTable;
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

        public string GetShopIdOfOrder(int orderId)
        {
            return dbSet.Where(q => q.Id == orderId).Select(q => q.ShopId).First();
        }

        public bool EditOrderByOrderId(int orderId, int status, string receiver, string address, string phone, string note)
        {
            Order order = this.FindByKey(orderId);
            order.Status = status;
            order.Receiver = receiver;
            order.Phone = phone;
            order.ShippingAddress = address;
            order.UserNote = note;
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

        //Andnd Get all order by shop and customer id
        public List<OrderViewModel> GetOrderByShopIdAndCustomerId(string shopId, int customerId)
        {
            return dbSet.Where(q => (q.ShopId == shopId) && (q.CustomerId == customerId)).Select(q => new OrderViewModel()
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
            }).ToList();
        }

        public IQueryable<OrderViewModel> GetOrderByShopIdAndCustomerId(string shopId, int customerId, JQueryDataTableParamModel param)
        {
            //var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => (q.ShopId == shopId)&&(q.CustomerId == customerId));
            var search = param.sSearch;
            //Debug.WriteLine("----sort col num " + param.iSortCol_0);
            rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                           (!string.IsNullOrEmpty(param.sSearch)
                           && q.Id.ToString().Contains(param.sSearch.ToLower())))
           .OrderBy(q => q.DateCreated);
            //Debug.WriteLine("---------rs " + rs.Count());
            //if (rs.Count() == 0) return null;
            var data = rs.Select(q => new OrderViewModel()
            {
                //id dung de edit nen lay int id
                Id = q.Id,
                DateModified = q.DateModified,
                Total = q.OrderDetails.Sum(d => d.Price * d.Quantity),
            });
            //Debug.WriteLine("---------data " + data.Count());
            return data;
        }


        //ngochb
        public int GetCompletedOrderNumber(string shopId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = dbSet.Where(q => q.ShopId == shopId && q.Status == (int)Utils.OrderStatus.COMPLETED && q.DateModified < endDate && q.DateModified >= startDate).Count();
                return result;
            }
            catch (Exception)
            {

                return 0;
            }
            
        }

        //ngochb
        public int GetCanceledOrderNumber(string shopId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = dbSet.Where(q => q.ShopId == shopId && q.Status == (int)Utils.OrderStatus.CANCELED && q.DateModified < endDate && q.DateModified >= startDate).Count();
                return result;
            }
            catch (Exception)
            {

                return 0;
            }
            
        }

        //ngochb
        public int GetIncompleteOrderNumber(string shopId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = dbSet.Where(q => q.ShopId == shopId && (q.Status == (int)Utils.OrderStatus.PROCESSING || q.Status == (int)Utils.OrderStatus.DELIVERING) && q.DateModified < endDate && q.DateModified >= startDate).Count();
                return result;
            }
            catch (Exception)
            {

                return 0;
            }
            
        }

        //ngochb
        public decimal GetOrderRevenue(string shopId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = dbSet.Where(q => q.ShopId == shopId && q.Status == (int)Utils.OrderStatus.COMPLETED && q.DateModified < endDate && q.DateModified >= startDate).Select(q => q.OrderDetails.Sum(d => d.Price * d.Quantity)).Sum();
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
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
