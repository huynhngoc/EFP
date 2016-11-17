﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModel;

namespace DataService.Service
{
    public class ShopService
    {
        ShopRepository repository = new ShopRepository();
        public List<ShopViewModel> GetShopByUserId(string userId)
        {
            return repository.GetShopByUserId(userId).Select(q => new ShopViewModel()
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
                return repository.CreateShop(shopId, name, token, userId, picture);
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
                return repository.CreateConnection(shopId, userId);
            }
            catch
            {
                return false;
            }
        }
        public ShopViewModel GetShop(string shopId)
        {
            Shop s = repository.FindByKey(shopId);
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
                return repository.FindByKey(shopId).ReplyMode;
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
                Shop s = repository.FindByKey(shopId);
                s.ReplyMode = mode;
                return repository.Update(s);
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
                return repository.FindByKey(shopId).CommentMode;
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
                Shop s = repository.FindByKey(shopId);
                s.CommentMode = mode;
                return repository.Update(s);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckShopUser(string shopId, string userId)
        {
            return repository.CheckShopUser(shopId, userId);
        }

    }
}
