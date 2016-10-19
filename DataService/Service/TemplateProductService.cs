using DataService.Repository;
using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class TemplateProductService
    {
        TemplateProductRepository templateProductRepository = new TemplateProductRepository();
        public TemplateProductService()
        {

        }
        public TemplateProductViewModel GetTemplateByIdAndShop(int id,string shopId)
        {
            TemplateProductViewModel templateModel = new TemplateProductViewModel();
            TemplateProduct template = templateProductRepository.GetTemplateByIdAndShop(id,shopId).FirstOrDefault();
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
