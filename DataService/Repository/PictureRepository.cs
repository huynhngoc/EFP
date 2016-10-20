using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class PictureRepository: BaseRepository<ProductPicture>
    {
        public IQueryable<ProductPicture> GetAllPicture(int productId)
        {
            return dbSet.Where(q => q.ProductId == productId);
        }
    }
}
