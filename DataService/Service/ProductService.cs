using DataService.Repository;
using DataService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Service
{
    public class ProductService
    {
        public ProductService()
        {

        }
        //Create repository
        ProductRepository productRepository = new ProductRepository();
        ProductPicturesRepository productPicturesRepository = new ProductPicturesRepository();
        CategoryService categoryService = new CategoryService();
        TemplateProductService templateProductService = new TemplateProductService();

        // Get a number of product by shop, category
        public List<ProductItemViewModel> GetProductByShopAndCategory(string shopId, int categoryId, int start, int quantity)
        {
            //Tao List product theo shop va category (chua chinh sua)
            List<Product> listProduct = new List<Product>();
            // Tao List product item tra ve (chinh sua)
            List<ProductItemViewModel> listProductItemViewModel = new List<ProductItemViewModel>();
            //Tao product model
            ProductItemViewModel model;
            // Get list product nguyen goc
            listProduct = productRepository.GetProductByShopAndCategory(shopId, categoryId).ToList();
            if (start >= 0 && start < listProduct.Count())
            {
                var end = start + quantity;
                if ((start+quantity)> listProduct.Count())
                {
                    end = listProduct.Count();
                }
                for (int i = start; i < end; i++)
                {
                    //Tao moi modal de add vao list viewmodel
                    model = new ProductItemViewModel();
                    //Add cac thong tin tung product vao viewmodel
                    model.Id = listProduct[i].Id;
                    model.ShopId = listProduct[i].ShopId;
                    model.Name = listProduct[i].Name;
                    model.CategoryId = listProduct[i].CategoryId;
                    model.Attr1 = listProduct[i].Attr1;
                    model.Attr2 = listProduct[i].Attr2;
                    model.Attr3 = listProduct[i].Attr3;
                    model.Attr4 = listProduct[i].Attr4;
                    model.Attr5 = listProduct[i].Attr5;
                    model.Attr6 = listProduct[i].Attr6;
                    model.Attr7 = listProduct[i].Attr7;
                    model.Description = listProduct[i].Description;
                    model.IsInStock = listProduct[i].IsInStock;
                    model.Price = listProduct[i].Price;
                    model.PromotionPrice = listProduct[i].PromotionPrice;
                    model.TemplateId = listProduct[i].TemplateId;

                    //Get list product picture url
                    List<ProductPicture> listUrl = productPicturesRepository.GetUrlByProductId(listProduct[i].Id).ToList();
                    //Set list url vao model
                    for (int j = 0; j < listUrl.Count(); j++)
                    {
                        model.Urls.Add(listUrl[j].Urls);
                    }

                    //Add model vao list model
                    listProductItemViewModel.Add(model);
                }

                //Return List product item view model
                return listProductItemViewModel;
            }
            else
            {
                return null;
            }

        }

        // Get single product by product id
        public ProductItemViewModel GetProductByProductId(string shopId, int categoryId, int productId)
        {
            //Tao product view model
            ProductItemViewModel productItemViewModel = new ProductItemViewModel();
            //Get product  by product id
            Product product = productRepository.GetProductByProductId(shopId, categoryId, productId).FirstOrDefault();
            //Get list url by product id
            List<ProductPicture> listUrl = productPicturesRepository.GetUrlByProductId(productId).ToList();
            //Check if not found product return null
            if (product != null)
            {
                //Add information to product view model
                productItemViewModel.Id = product.Id;
                productItemViewModel.ShopId = product.ShopId;
                productItemViewModel.Name = product.Name;
                productItemViewModel.CategoryId = product.CategoryId;
                productItemViewModel.Attr1 = product.Attr1;
                productItemViewModel.Attr2 = product.Attr2;
                productItemViewModel.Attr3 = product.Attr3;
                productItemViewModel.Attr4 = product.Attr4;
                productItemViewModel.Attr5 = product.Attr5;
                productItemViewModel.Attr6 = product.Attr6;
                productItemViewModel.Attr7 = product.Attr7;
                productItemViewModel.Description = product.Description;
                productItemViewModel.IsInStock = product.IsInStock;
                productItemViewModel.Price = product.Price;
                productItemViewModel.PromotionPrice = product.PromotionPrice;
                productItemViewModel.TemplateId = product.TemplateId;
                //Set list url vao model
                for (int i = 0; i < listUrl.Count(); i++)
                {
                    productItemViewModel.Urls.Add(listUrl[i].Urls);
                }
                //Return product view model
                return productItemViewModel;
            }
            else
            {
                return null;
            }
            
        }

        //Get all product of child category
        public List<ProductItemViewModel> GetAllProductOfChildCategory(string shopId, int parentCategoryId, int start, int quantity)
        {
            //Tao List product theo shop va category (chua chinh sua)
            List<Product> listProduct ;
            // Tao List product item tra ve (chinh sua)
            List<ProductItemViewModel> listProductItemViewModel = new List<ProductItemViewModel>();
            //Tao product model
            ProductItemViewModel model;

            //Get list child category
            List<int> listChildId = categoryService.getChildCategoryId(shopId, parentCategoryId);
            if (listChildId.Count() != 0)
            {
                for (int i = 0; i < listChildId.Count(); i++)
                {
                    // Get list product nguyen goc
                    listProduct = new List<Product>();
                    listProduct = productRepository.GetProductByShopAndCategory(shopId, listChildId[i]).ToList();
                    for (int j = 0; j < listProduct.Count(); j++)
                    {
                        model = new ProductItemViewModel();
                        //Add cac thong tin tung product vao viewmodel
                        model.Id = listProduct[j].Id;
                        model.ShopId = listProduct[j].ShopId;
                        model.Name = listProduct[j].Name;
                        model.CategoryId = listProduct[j].CategoryId;
                        model.Attr1 = listProduct[j].Attr1;
                        model.Attr2 = listProduct[j].Attr2;
                        model.Attr3 = listProduct[j].Attr3;
                        model.Attr4 = listProduct[j].Attr4;
                        model.Attr5 = listProduct[j].Attr5;
                        model.Attr6 = listProduct[j].Attr6;
                        model.Attr7 = listProduct[j].Attr7;
                        model.Description = listProduct[j].Description;
                        model.IsInStock = listProduct[j].IsInStock;
                        model.Price = listProduct[j].Price;
                        model.PromotionPrice = listProduct[j].PromotionPrice;
                        model.TemplateId = listProduct[j].TemplateId;

                        //Get list product picture url
                        List<ProductPicture> listUrl = productPicturesRepository.GetUrlByProductId(listProduct[j].Id).ToList();
                        //Set list url vao model
                        for (int k = 0; k < listUrl.Count(); k++)
                        {
                            model.Urls.Add(listUrl[k].Urls);
                        }

                        //Add model vao list model
                        listProductItemViewModel.Add(model);
                    }
                }
                var end = start + quantity;
                if ((start + quantity) > listProductItemViewModel.Count())
                {
                    end = listProductItemViewModel.Count();
                }
                List<ProductItemViewModel> newListProduct = new List<ProductItemViewModel>();
                for (int i = start; i < end; i++)
                {
                    newListProduct.Add(listProductItemViewModel[i]);
                }

                return newListProduct;
            }
            else
            {
                return null;
            }

        }


    }
}
