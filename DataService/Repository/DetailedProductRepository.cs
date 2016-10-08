using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.JqueryDataTable;
using DataService.ViewModel;

namespace DataService.Repository
{
    public class DetailedProductRepository: BaseRepository<DetailedProduct>
    {
        public IEnumerable<DetailedProduct> GetDetailedProductByShopId(string shopId)
        {
            return dbSet.Where(q => q.MasterProduct.ShopId == shopId);
        }
        public IEnumerable<DetailedProduct> GetDetailedProductByMasterProduct(int masterId, string shopId)
        {
            return dbSet.Where(q => q.MasterId == masterId && q.MasterProduct.ShopId == shopId);
        }

        public IQueryable<DetailedProduct> GetDetailedProduct(int masterId)
        {
            return dbSet.Where(q => q.MasterId == masterId);
        }

        public IQueryable<DetailedProductViewModel> GetDetailedProduct(JQueryDataTableParamModel param, int masterId)
        {
            var rs = GetDetailedProduct(masterId);
            var search = param.sSearch;
            rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                                (!string.IsNullOrEmpty(param.sSearch)
                                && q.Attr1.ToLower().Contains(param.sSearch.ToLower())))
                .OrderBy(q => q.Id);
            Debug.WriteLine("---------rs " + rs.Count());
            var data = rs.Select(q => new DetailedProductViewModel()
            {
                Id = q.Id,                                                
                Attr = (new List<string>  {q.MasterProduct.Attr1 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr1)
                                , q.MasterProduct.Attr2 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr2)
                                , q.MasterProduct.Attr3 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr3)//.ToString()
                                , q.MasterProduct.Attr4 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr4)//.ToString()
                                , q.MasterProduct.Attr5 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr5)//.ToString()
                                , q.MasterProduct.Attr6 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr6)//.ToString()
                                , q.MasterProduct.Attr7 ==null ? null : (q.MasterProduct.Attr1 + " " + q.Attr7)//.ToString() 
                }),
                Status = q.Status,                
                Price = (decimal?)(q.Price),
                PromotionPrice = (decimal?)(q.Price)
            });
            Debug.WriteLine("---------data " + data.Count());
            return data;                    
        }
    }
}
