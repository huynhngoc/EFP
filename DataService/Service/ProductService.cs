using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataService.Repository;
using DataService.JqueryDataTable;
using DataService.ViewModel;
using System.Diagnostics;

namespace DataService.Service
{
    public class ProductService
    {
        ProductRepository repository = new ProductRepository();
        public ProductService()
        {                        
        }

        //public List<MasterProduct> GetAllMasterProductByShopId(string shopId)
        //{
        //    return repository.GetProductByShopId(shopId).ToList();
        //}

        public IQueryable<Product> GetAllMasterProductByShopId(string shopId)
        {
            return repository.GetProductByShopId(shopId);
        }

        public bool UpdateProduct(int id, string name, string description, int categoryId, decimal price, decimal? promotion, bool status, bool isInStock, int? templateId, string[] attr)
        {
            try
            {
                Product p = repository.FindByKey(id);
                p.Id = id;
                p.Name = name;
                p.Description = description;
                p.CategoryId = categoryId;
                p.Price = price;
                p.PromotionPrice = promotion;
                p.Status = status;
                p.IsInStock = isInStock;
                p.DateModified = DateTime.Now;
                p.TemplateId = templateId;
                p.Attr1 = attr[0];
                p.Attr2 = attr[1];
                p.Attr3 = attr[2];
                p.Attr4 = attr[3];
                p.Attr5 = attr[4];
                p.Attr6 = attr[5];
                p.Attr7 = attr[6];
                repository.Update(p);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public List<Product> GetProductByCategory(int cateId, string shopId)
        {
            return repository.GetProductByCategory(cateId, shopId).ToList();
        }       

        public IQueryable<ProductViewModel> GetProduct(JQueryDataTableParamModel param, string shopId, bool sName, bool sCate, bool sDesc)
        {
            var rs = repository.GetProduct(param, shopId, sName, sCate, sDesc);            
            return rs;
        }

        public ProductViewModel GetProductById(int id)
        {
            return repository.GetProductById(id);
        }

        public bool SetStatus(int[] idList, bool status)
        {
            try
            {
                foreach (var id in idList)
                {
                    repository.SetStatus(id, status);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public bool SetInStock(int[] idList, bool inStock)
        {
            try
            {
                foreach (var id in idList)
                {
                    repository.SetInStock(id, inStock);
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }

        public Product AddProduct(string name, string description, int categoryId, decimal price, decimal? promotion, bool status, bool isInStock, int? templateId, string[] attr, string shopId)
        {
            try
            {
                Product p = new Product()
                {
                    Name = name,
                    Description = description,
                    CategoryId = categoryId,
                    Price = price,
                    PromotionPrice = promotion,
                    Status = status,
                    IsInStock = isInStock,
                    DateModified = DateTime.Now,
                    TemplateId = templateId,
                    DateCreated = DateTime.Now,
                    ShopId = shopId,
                    Attr1 = attr[0],
                    Attr2 = attr[1],
                    Attr3 = attr[2],
                    Attr4 = attr[3],
                    Attr5 = attr[4],
                    Attr6 = attr[5],
                    Attr7 = attr[6]
                };
                return repository.Add(p);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
        }
    }
}
