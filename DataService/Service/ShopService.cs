using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModel;

namespace DataService.Service
{
    public class ShopService
    {
        ShopRepository shopRepository = new ShopRepository();
        ShopUserRepository shopuserRepository = new ShopUserRepository();
        public List<ShopViewModel> GetShopByUserId(string userId)
        {
            return shopRepository.GetShopByUserId(userId).Select(q => new ShopViewModel()
            {
                Id = q.Id,
                ShopName = q.ShopName,
                DateCreated = q.DateCreated,
                FbToken = q.FbToken
            }).ToList();
        }

        public bool CreateShop(string shopId, string name, string token, string userId, string picture)
        {
            try
            {
                return shopRepository.CreateShop(shopId, name, token, userId, picture);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateConnection(string shopId, string userId)
        {
            try
            {
                return shopRepository.CreateConnection(shopId, userId);
            }
            catch
            {
                return false;
            }
        }
        public ShopViewModel GetShop(string shopId)
        {
            Shop s = shopRepository.FindByKey(shopId);
            if (s != null)
            {
                return new ShopViewModel()
                {
                    Id = s.Id,
                    ShopName = s.ShopName,
                    DateCreated = s.DateCreated,
                    FbToken = s.FbToken,
                    BannerImg = s.BannerImg,                    
                };
            }
            return null;
        }

        public int GetReplyMode(string shopId)
        {
            try
            {
                return shopRepository.FindByKey(shopId).ReplyMode;
            }
            catch (Exception)
            {
                return 0;
                
            }
            
        }

        public bool SetReplyMode(string shopId, int mode)
        {
            try
            {
                Shop s = shopRepository.FindByKey(shopId);
                s.ReplyMode = mode;
                return shopRepository.Update(s);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetCommentMode(string shopId)
        {
            try
            {
                return shopRepository.FindByKey(shopId).CommentMode;
            }
            catch (Exception)
            {
                return 0;

            }

        }

        public bool SetCommentMode(string shopId, int mode)
        {
            try
            {
                Shop s = shopRepository.FindByKey(shopId);
                s.CommentMode = mode;
                return shopRepository.Update(s);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckShopUser(string shopId, string userId)
        {
            return shopRepository.CheckShopUser(shopId, userId);
        }

        public List<ShopUserViewModel> GetAll()
        {
            return shopRepository.GetAll().ToList();
        }

        public bool SetActive(string shopId, string userId, bool isActive)
        {
            return shopuserRepository.SetActive(shopId, userId, isActive);
        }

    }
}
