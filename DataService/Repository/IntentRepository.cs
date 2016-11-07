using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class IntentRepository : BaseRepository<Intent>
    {


        public IEnumerable<Intent> GetAllIntent()
        {
            try
            {
                return this.dbSet;
            }
            catch
            {
                return null;
            }
        }

        public string GetIntentNameById(int id)
        {
            try
            {
                return dbSet.Where(q=> q.Id==id).Select(q=> q.IntentName).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
