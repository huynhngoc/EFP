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
    }
}
