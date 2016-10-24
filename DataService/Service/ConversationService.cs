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
            Conversation c = new Conversation()
            {
                Id = id,
                IntentId = intent,
                LastUpdate = new DateTime(time),
                ShopId = shopId,
                IsRead = false
            };
            return repository.Create(c);

        }

        public bool SetReadConversation(string id)
        {
            Conversation c = repository.FindByKey(id);
            c.IsRead = true;
            return repository.Update(c);
        }
    }
}
