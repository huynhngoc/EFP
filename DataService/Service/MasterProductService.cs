using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.JqueryDataTable;

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

        public List<MasterProduct> GetMasterProductByCategory(int cateId, string shopId)
        {
            return repository.GetMasterProductByCategory(cateId, shopId).ToList();
        }

        public IEnumerable<MasterProduct> GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        {
            return null;
        }
    }
}
