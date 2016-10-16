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
    public class ProductRepository : BaseRepository<Product>
    {
        public IQueryable<Product> GetProductByShopId(string shopId)
        {
            return dbSet.Where(q => q.ShopId == shopId);
        }

        public IEnumerable<Product> GetProductByCategory(int cateId, string shopId)
        {
            IEnumerable<int> query = from c in entites.Categories where c.ParentId == cateId select c.Id;
            Debug.WriteLine(query.ToArray());
            var data = dbSet.Where(q => (q.CategoryId == cateId ) && q.ShopId == shopId);

            return data;
        }

        public ProductViewModel GetProductById(int id)
        {
            try
            {
                entites.Configuration.ProxyCreationEnabled = true;
                Product p = FindByKey(id);
                return new ProductViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Category = new List<string>() { p.Category.ParentId.ToString(), p.CategoryId.ToString(), p.Category.Name },
                    Description = p.Description,
                    DateCreated = p.DateCreated,
                    DateModified = p.DateModified,
                    IsInStock = p.IsInStock,
                    Status = p.Status,
                    Price = p.Price,
                    Promotion = p.PromotionPrice,
                    TemplateId = p.TemplateId,
                    Attr = p.Attr1?.ToString() + "_" +
                    p.Attr2?.ToString() + "_" +
                    p.Attr3?.ToString() + "_" +
                    p.Attr4?.ToString() + "_" +
                    p.Attr5?.ToString() + "_" +
                    p.Attr6?.ToString() + "_" +
                    p.Attr7?.ToString()
                };
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<ProductViewModel> GetProduct(JQueryDataTableParamModel param, string shopId, bool sName, bool sCate, bool sDesc)
        {
            var count = param.iDisplayStart + 1;
            var rs = dbSet.Where(q => q.ShopId == shopId);            
            var search = param.sSearch;
            rs = rs.Where(q => string.IsNullOrEmpty(param.sSearch) ||
                                (!string.IsNullOrEmpty(param.sSearch)
                                && (
                                    (sName && q.Name.ToLower().Contains(param.sSearch.ToLower())) ||
                                    (sCate && q.Category.Name.ToLower().Contains(param.sSearch.ToLower())) ||
                                    (sDesc && q.Description.ToLower().Contains(param.sSearch.ToLower()))
                                )
                                )
                         );
            // .OrderBy(q => q.Name);
            switch (param.iSortCol_0)
            {
                case 0:
                case 2: if (param.sSortDir_0 == "asc")
                        { 
                            rs = rs.OrderBy(q => q.Name);
                        } else
                        {
                            rs = rs.OrderByDescending(q => q.Name);
                        }
                    break;
                case 3:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.Description);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.Description);
                    }
                    break;
                case 4:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.Category.Name);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.Category.Name);
                    }
                    break;
                case 5:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.Price);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.Price);
                    }
                    break;
                case 6:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.PromotionPrice);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.PromotionPrice);
                    }
                    break;
                case 7:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.Status);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.Status);
                    }
                    break;
                case 8:
                    if (param.sSortDir_0 == "asc")
                    {
                        rs = rs.OrderBy(q => q.IsInStock);
                    }
                    else
                    {
                        rs = rs.OrderByDescending(q => q.IsInStock);
                    }
                    break;
                default: rs = rs.OrderBy(q => q.Name);break;
            }
            Debug.WriteLine("---------rs " + rs.Count());
            //if (rs.Count() == 0) return null;
            var data = rs.Select(q => new ProductViewModel()
            {
                Id = q.Id,
                Name = q.Name,
                Category = (new List<string> { q.CategoryId.ToString()
                                             , q.Category.Name//.ToString() 
                }),
                Description = q.Description,
                //Attr = (new List<string>  {q.Attr1//.ToString()
                //                , q.Attr2//.ToString()
                //                , q.Attr3//.ToString()
                //                , q.Attr4//.ToString()
                //                , q.Attr5//.ToString()
                //                , q.Attr6//.ToString()
                //                , q.Attr7//.ToString() 
                //}),
                Status = q.Status,
                IsInStock = q.IsInStock,
                DateCreated = q.DateCreated,
                DateModified = q.DateModified,
                Price = (decimal)(q.Price),
                Promotion = (decimal?)(q.PromotionPrice)
            });
            Debug.WriteLine("---------data " + data.Count());            
            return data;            
        }

        public Product Add(Product p)
        {
            try
            {
                Product pro = dbSet.Add(p);
                entites.SaveChanges();
                return pro;
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public bool SetStatus(int id, bool status)
        {
            try
            {
                
                Product p = FindByKey(id);
                p.Status = status;
                p.DateModified = DateTime.Now;
                return Update(p);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public bool SetInStock(int id, bool inStock)
        {
            try
            {
                
                Product p = FindByKey(id);
                p.IsInStock = inStock;
                p.DateModified = DateTime.Now;              
                return Update(p);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }


    }
}
