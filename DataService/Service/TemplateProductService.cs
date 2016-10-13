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
    }
}
