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
        public List<TemplateProductViewModel1> GetTemplate(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId).Select(q => new TemplateProductViewModel1()
            {
                Id = q.Id,
                Name = q.Name,
                Attr = q.Attr1.ToString() + "_"+
                    q.Attr2.ToString()
            + "_" + q.Attr3.ToString()
            + "_" + q.Attr4.ToString()
            + "_" + q.Attr5.ToString()
            + "_" + q.Attr6.ToString()
            + "_" + q.Attr7.ToString() 
            }            
            ).ToList();
        }

        public IEnumerable<TemplateProduct> GetTemplateByIdAndShop(int id, string shopId)
        {
            return dbSet.Where(q => (q.Id == id) && (q.ShopId == shopId));
        }
    }
}
