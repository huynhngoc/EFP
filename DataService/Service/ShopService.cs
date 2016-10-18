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

    }
}
