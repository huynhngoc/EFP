using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class ConversationService
    {
        ConversationRepository repository = new ConversationRepository();
        public ConversationService()
        {

        }
        public bool AddConversation(string id, int intent, long time, string shopId)
        {

            Conversation c = repository.FindByKey(id);
            if (c == null)
            {
                c = new Conversation()
                {
                    Id = id,
                    IntentId = intent,
                    LastUpdate = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(time)).ToLocalTime(),                    
                    ShopId = shopId,
                    IsRead = false
                };
                return repository.Create(c);
            } else
            {
                c.IntentId = intent;
                c.LastUpdate = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(time)).ToLocalTime();
                c.IsRead = false;
                return repository.Update(c);
            }
                

        }

        public bool SetReadConversation(string id)
        {
            Conversation c = repository.FindByKey(id);
            c.IsRead = true;
            return repository.Update(c);
        }
        public bool SetReadConversation(string id, long time)
        {
            Conversation c = repository.FindByKey(id);
            c.IsRead = true;
            c.LastUpdate = (new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(time)).ToLocalTime();
            return repository.Update(c);
        }

        public IEnumerable<Conversation> GetConversationsByShopId(string shopId)
        {
            return repository.GetConversationsByShopId(shopId);
        }
    }
}
