using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.ViewModel;
namespace DataService.Service
{
    public class TemplateProductService
    {
        TemplateProductRepository repository = new TemplateProductRepository();
        public List<TemplateProductViewModel> GetTemplate(string shopId)
        {
            return repository.GetTemplate(shopId);
        }

        public TemplateProductViewModel GetTemplateById(int id)
        {

            TemplateProduct q = repository.FindByKey(id);
            return new TemplateProductViewModel()
            {
                Id = q.Id,
                Name = q.Name,
                Attr = q.Attr1?.ToString() + "_" +
                    q.Attr2?.ToString()
            + "_" + q.Attr3?.ToString()
            + "_" + q.Attr4?.ToString()
            + "_" + q.Attr5?.ToString()
            + "_" + q.Attr6?.ToString()
            + "_" + q.Attr7?.ToString()
            };
        }


        public bool AddTemplate(string name, string[] attr, string shopId)
        {
            try
            {
                TemplateProduct t = new TemplateProduct()
                {
                    Name = name,
                    Attr1 = string.IsNullOrEmpty(attr[0]) ? null : attr[0],
                    Attr2 = string.IsNullOrEmpty(attr[0]) ? null : attr[1],
                    Attr3 = string.IsNullOrEmpty(attr[0]) ? null : attr[2],
                    Attr4 = string.IsNullOrEmpty(attr[0]) ? null : attr[3],
                    Attr5 = string.IsNullOrEmpty(attr[0]) ? null : attr[4],
                    Attr6 = string.IsNullOrEmpty(attr[0]) ? null : attr[5],
                    Attr7 = string.IsNullOrEmpty(attr[0]) ? null : attr[6],
                    ShopId = shopId
                };
                return repository.Create(t);
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateTemplate(int id, string[] attr, string shopId)
        {
            try
            {
                return repository.Update(new TemplateProduct()
                {
                    Id=id,
                    Attr1 = string.IsNullOrEmpty(attr[0]) ? null : attr[0],
                    Attr2 = string.IsNullOrEmpty(attr[0]) ? null : attr[1],
                    Attr3 = string.IsNullOrEmpty(attr[0]) ? null : attr[2],
                    Attr4 = string.IsNullOrEmpty(attr[0]) ? null : attr[3],
                    Attr5 = string.IsNullOrEmpty(attr[0]) ? null : attr[4],
                    Attr6 = string.IsNullOrEmpty(attr[0]) ? null : attr[5],
                    Attr7 = string.IsNullOrEmpty(attr[0]) ? null : attr[6],
                    ShopId = shopId
                });
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
