using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class ShopRepository: BaseRepository<Shop>
    {
        public ShopRepository(): base()
        {

        }

        public IQueryable<Shop> GetShopByUserId(string userId)
        {
            return dbSet.Where(q => q.ShopUsers.Any(u => u.UserId == userId));
        }

        public bool CheckShopUser(string shopId, string userId)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            Shop s = FindByKey(shopId);
            if (s != null)
            {
                return s.ShopUsers.Any(q => q.UserId == userId);
            }
            else
            {
                return false;
            }
        }
        
        public bool CreateShop(string shopId, string name, string token, string userId)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            Shop s = FindByKey(shopId);
            if (s == null)
            {
                s = new Shop()
                {
                    Id = shopId,
                    ShopName = name,
                    FbToken = token,
                    DateCreated = DateTime.Now,
                };
                if (Create(s))
                {
                    s.ShopUsers.Add(new ShopUser()
                    {
                        ShopId = shopId,
                        UserId = userId,
                        IsActive = true
                    });
                    return Update(s);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (s.ShopUsers.Any(q => q.UserId == userId))
                {
                    return false;
                }
                else
                {
                    s.ShopUsers.Add(new ShopUser()
                    {
                        ShopId = shopId,
                        UserId = userId,
                        IsActive = true
                    });
                    return Update(s);
                }
            }
        }

        public bool CreateConnection(string shopId, string userId)
        {
            entites.Configuration.ProxyCreationEnabled = true;
            Shop s = FindByKey(shopId);                        
            if (s.ShopUsers.Any(q => q.UserId == userId))
            {
                return true;
            }
            else
            {
                s.ShopUsers.Add(new ShopUser()
                {
                    ShopId = shopId,
                    UserId = userId,
                    IsActive = true
                });
                return Update(s);
            }
                        
        }
    }
}
