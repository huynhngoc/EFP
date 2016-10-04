using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;

namespace DataService.Service
{
    public class MasterProductService
    {
        MasterProductRepository repository = new MasterProductRepository();
        public MasterProductService()
        {                        
        }

        public List<MasterProduct> GetAllMasterProductByShopId(string shopId)
        {
            return repository.GetMasterProductByShopId(shopId).ToList();
        }

        public List<MasterProduct> GetMasterProductByCategory(int cateId)
        {
            return repository.GetMasterProductByCategory(cateId).ToList();
        }
    }
}
