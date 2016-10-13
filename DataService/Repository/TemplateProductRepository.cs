using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModel;

namespace DataService.Repository
{
    public class TemplateProductRepository: BaseRepository<TemplateProduct>
    {
        public List<TemplateProductViewModel> GetTemplate(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId).Select(q => new TemplateProductViewModel()
            {
                Id = q.Id,
                Name = q.Name,
                Attr = new List<string>() { q.Attr1.ToString()
            , q.Attr2.ToString()
            , q.Attr3.ToString()
            , q.Attr4.ToString()
            , q.Attr5.ToString()
            , q.Attr6.ToString()
            , q.Attr7.ToString() }
            }            
            ).ToList();
        }
    }
}
