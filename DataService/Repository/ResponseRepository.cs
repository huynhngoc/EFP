using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class ResponseRepository: BaseRepository<Respons>
    {
        public ResponseRepository() : base()
        {

        }

        public Respons FindByShopAndIntent(string shopId, int intentId)
        {
            return dbSet.Where(q => q.ShopId == shopId & q.IntentId == intentId).FirstOrDefault();
        }
    }
}
