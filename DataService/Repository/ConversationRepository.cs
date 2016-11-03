using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class ConversationRepository : BaseRepository<Conversation>
    {
        public ConversationRepository() : base()
        {

        }

        public IEnumerable<Conversation> GetConversationsByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId).OrderByDescending(q => q.LastUpdate);
        }
    }
}
