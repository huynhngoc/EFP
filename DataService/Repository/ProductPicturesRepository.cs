using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class ProductPicturesRepository:BaseRepository<ProductPicture>
    {
        public IEnumerable<ProductPicture> GetUrlByProductId(int productId)
        {
            return dbSet.Where(q => (q.ProductId == productId));
        }
    }
}
