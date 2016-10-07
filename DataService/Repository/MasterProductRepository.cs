using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.ViewModel;
using DataService.JqueryDataTable;

namespace DataService.Repository
{
    public class MasterProductRepository: BaseRepository<MasterProduct>
    {        
        public IQueryable<MasterProduct> GetMasterProductByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }

        public IEnumerable<MasterProduct> GetMasterProductByCategory(int cateId, string shopId)
        {
            IEnumerable<int> query = from c in entites.Categories where c.ParentId == cateId select c.Id;
            Debug.WriteLine(query.ToArray());
            var data = dbSet.Where(q => (q.CategoryId == cateId || query.Contains(q.CategoryId.Value)) && q.ShopId == shopId);            
            
            return data;        
        }

        public IQueryable<MasterProductViewModel> GetMasterProduct(JQueryDataTableParamModel param, string shopId)
        {
            var count = param.iDisplayStart + 1;
            var rs = GetMasterProductByShopId(shopId);
            var search = param.sSearch;
            rs = rs.Where(q => q.Name.ToLower().Contains(param.sSearch.ToLower()));
            switch (param.iSortCol_0)
            {
              
            }
            return null;
        }
    }
}
