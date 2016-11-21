using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.ViewModel;

namespace DataService.Service
{
    public class ShopRepository : BaseRepository<Shop>
    {
        public ShopRepository() : base()
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
                return s.ShopUsers.Any(q => q.UserId == userId && q.IsActive == true);
            }
            else
            {
                return false;
            }
        }

        public bool CreateShop(string shopId, string name, string token, string userId, string picture)
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
                    BannerImg = picture,
                    DateCreated = DateTime.Now
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

        public IEnumerable<ShopUserViewModel> GetAll()
        {
            return dbSet.Select(q => new ShopUserViewModel
            {
                Id = q.Id,
                Name = q.ShopName,
                Users = q.ShopUsers.Select(t => new User
                {
                    Id = t.UserId,
                    Email = t.AspNetUser.Email,
                    IsActive = t.IsActive
                })
            });
        }

    }

    public class ShopUserRepository : BaseRepository<ShopUser>
    {
        public ShopUserRepository() : base()
        {

        }

        public bool SetActive (string shopId, string userId, bool isActive)
        {
            ShopUser s = dbSet.Where(q => q.ShopId == shopId && q.UserId == userId).FirstOrDefault();
            if (s != null)
            {
                s.IsActive = isActive;
                return Update(s);
            } else
            {
                return false;
            }
        }
    }
}
