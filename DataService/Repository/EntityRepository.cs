using DataService.JqueryDataTable;
using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Repository
{
    public class EntityRepository: BaseRepository<Entity>
    {
        public List<Entity> GetAll(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId).ToList();
        }

        //Long
        public IQueryable<EntityViewModel> GetAvailableEntities(JQueryDataTableParamModel param, string shopId)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => q.ShopId == shopId);
            var search = param.sSearch;
            rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                                (!string.IsNullOrEmpty(param.sSearch)
                                && (q.EntityName.ToLower().Contains(param.sSearch.ToLower())))
                         );

            Debug.WriteLine("---------rs " + rs.Count());

            var data = rs.Select(q => new EntityViewModel()
            {
                Name = q.EntityName,
                Value = q.Value
            });
            Debug.WriteLine("---------data " + data.Count());
            return data;
        }
    }
}
