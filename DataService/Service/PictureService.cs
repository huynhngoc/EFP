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
        public Dictionary<int,string> GetAll(int productId)
        {
            try
            {
                Dictionary<int, string> result = new Dictionary<int, string>();
                foreach(var img in repository.GetAllPicture(productId).ToList())
                {
                    result.Add(img.Id, img.Urls);
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
                return repository.Create(new ProductPicture()
                {
                    Urls = url,
                    ProductId = productId
                });               
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

    }
}
