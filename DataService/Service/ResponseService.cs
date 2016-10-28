using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class ResponseService
    {
        ResponseRepository repository = new ResponseRepository();
        public bool SetResponse(string shopId, int intentId, string content)
        {
            Respons r = repository.FindByShopAndIntent(shopId, intentId);
            if (r == null)
            {
                r = new Respons()
                {
                    ShopId = shopId,
                    IntentId = intentId,
                    RespondContent = content
                };
                return repository.Create(r);
            } else
            {
                r.RespondContent = content;
                return repository.Update(r);
            }
        }

    }
}
