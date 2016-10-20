using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class PictureService
    {
        PictureRepository repository = new PictureRepository();
        public Dictionary<string,string> GetAll(int productId)
        {
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                foreach(var img in repository.GetAllPicture(productId).ToList())
                {
                    result.Add(img.Id.ToString(), img.Urls);
                }
                return result;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool AddPicture(int productId, string url)
        {
            try
            {
                ProductPicture p =  new ProductPicture()
                {
                    Urls = url,
                    ProductId = productId
                };
                var result = repository.Create(p);
                return result;
            }
            catch (Exception)
            {
                
                return false;
            }            

        }

        public bool DeletePicture(int id)
        {
            return repository.Delete(id);
        }

        public bool DeletePicture(int[] idList)
        {
            try
            {
                foreach (var id in idList)
                {
                    DeletePicture(id);
                }
                return true;
            }
            catch (Exception)
            {                
                return false;
            }            
        }

        public string GetPublicId(int id)
        {
            var url = repository.FindByKey(id).Urls;
            return url.Substring(url.LastIndexOf("/")+1, url.LastIndexOf(".") - url.LastIndexOf("/")-1);
        }

        public List<string> GetPublicId(int[] idList)
        {
            List<string> result = new List<string>();
            try
            {
                foreach (var id in idList)
                {                    
                    result.Add(GetPublicId(id));
                }
                return result;             
            }
            catch (Exception)
            {

                return null;
            }
        }

    }
}
