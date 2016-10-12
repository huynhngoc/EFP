using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class TemplateProductService
    {
        TemplateProductRepository repository = new TemplateProductRepository();
        public string[] GetTemplate(string shopId)
        {
            return repository.GetTemplate(shopId);
        }
    }
}
