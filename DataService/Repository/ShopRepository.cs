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
            return dbSet.Where(q => q.UserId == userId);
        }
        

    }
}
