using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class EntityRepository: BaseRepository<Entity>
    {
        public List<Entity> GetAll(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId).ToList();
        }
    }
}
