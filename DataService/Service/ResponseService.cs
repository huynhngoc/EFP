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
        public int SetResponse(string shopId, int intentId, string content)
        {
            try
            {
                Respons r = new Respons()
                {
                    ShopId = shopId,
                    IntentId = intentId,
                    RespondContent = content
                };
                return repository.CreateNew(r).Id;
            }
            catch (Exception)
            {

                return 0;
            }                            
        }

        public string GetResponse(string shopId, int intentId)
        {
            Respons response = repository.FindByShopAndIntent(shopId, intentId)
                .OrderBy(x => Guid.NewGuid())
                .FirstOrDefault();
            if (response!=null)
            {
                return response.RespondContent;
            }
            else
            {
                return null;
            }
        }

        public List<string> GetAllResponseContent(string shopId, int intentId)
        {
            return repository.FindByShopAndIntent(shopId,intentId).Select(q => q.RespondContent).ToList() ;
        }

        public List<Respons> GetAllResponseByIntent(string shopId, int intentId)
        {
            return repository.FindByShopAndIntent(shopId, intentId);
        }

        public List<Respons> GetAll(string shopId)
        {
            return repository.GetAll(shopId);
        }

        public bool DeleteResponse(int id)
        {
            return repository.Delete(id);
        }

        public bool EditResponse(int id, string resContent)
        {
            try
            {
                Respons r = repository.FindByKey(id);
                r.RespondContent = resContent;
                return repository.Update(r);
            }
            catch (Exception)
            {

                return false;
            }            
        }

    }
}
