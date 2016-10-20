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

        public bool UpdateTemplate(int id, string name, string[] attr)
        {
            try
            {
                TemplateProduct t = repository.FindByKey(id);
                t.Name = name;
                t.Attr1 = string.IsNullOrEmpty(attr[0]) ? null : attr[0];
                t.Attr2 = string.IsNullOrEmpty(attr[0]) ? null : attr[1];
                t.Attr3 = string.IsNullOrEmpty(attr[0]) ? null : attr[2];
                t.Attr4 = string.IsNullOrEmpty(attr[0]) ? null : attr[3];
                t.Attr5 = string.IsNullOrEmpty(attr[0]) ? null : attr[4];
                t.Attr6 = string.IsNullOrEmpty(attr[0]) ? null : attr[5];
                t.Attr7 = string.IsNullOrEmpty(attr[0]) ? null : attr[6];                
                return repository.Update(t);
                                    
            }
            catch (Exception)
            {

                return false;
            }
        }
        public TemplateProductViewModel GetTemplateByIdAndShop(int id, string shopId)
        {
            TemplateProductViewModel templateModel = new TemplateProductViewModel();
            TemplateProduct template = templateProductRepository.GetTemplateByIdAndShop(id, shopId).FirstOrDefault();
            templateModel.Id = template.Id;
            templateModel.ShopId = template.ShopId;
            templateModel.Attr1 = template.Attr1;
            templateModel.Attr2 = template.Attr2;
            templateModel.Attr3 = template.Attr3;
            templateModel.Attr4 = template.Attr4;
            templateModel.Attr5 = template.Attr5;
            templateModel.Attr6 = template.Attr6;
            templateModel.Attr7 = template.Attr7;
            return templateModel;
        }
    }
}
