using DataService;
using DataService.Service;
using DataService.ViewModel;
using ProductPage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProductPage.Controllers
{
    public class GetDataController : Controller
    {
        //Create Service
        ProductService productService = new ProductService();
        TemplateProductService templateProductService = new TemplateProductService();
        OrderService orderService = new OrderService();
        CustomerService customerService = new CustomerService();

        //Get a number of product by shop and category
        public ActionResult GetProductByShopAndCategory(string shopId, int categoryId, int start, int quantity)
        {
            // Tao List product item tra ve
            List<ProductItemViewModel> listProductItemViewModel = productService.GetProductByShopAndCategory(shopId, categoryId, start, quantity);

            return Json(listProductItemViewModel, JsonRequestBehavior.AllowGet);
        }

        //Get single product by shop and category
        public ActionResult GetProductByProductId(string shopId, int categoryId, int productId)
        {
            // Tao List product item tra ve
            ProductItemViewModel productItemViewModel = productService.GetProductByProductId(shopId, categoryId, productId);

            return Json(productItemViewModel, JsonRequestBehavior.AllowGet);
        }

        //Get all product by shop and parent category
        public ActionResult GetAllProductOfChildCategory(string shopId, int parentCategoryId, int start, int quantity)
        {
            List<ProductItemViewModel> listProductItemViewModel = productService.GetAllProductOfChildCategory(shopId, parentCategoryId, start, quantity);
            return Json(listProductItemViewModel, JsonRequestBehavior.AllowGet);
        }

        // Get template product
        public ActionResult GetTemplateProductByShopAndId(int id, string shopId)
        {
            TemplateProductViewModel model = new TemplateProductViewModel();
            model = templateProductService.GetTemplateByIdAndShop(id, shopId);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Add to cart
        public int AddToCart(int productId, string properties, string url, decimal price, int quantity)
        {
            CartModel cart = new CartModel();
            cart.productId = productId;
            cart.properties = properties;
            cart.url = url;
            cart.price = price;
            cart.quantity = quantity;

            if (Session["Cart"] == null)
            {
                List<CartModel> listCart = new List<CartModel>();
                listCart.Add(cart);
                Session["Cart"] = listCart;
                return listCart.Count();
            }
            else
            {
                List<CartModel> listCart = (List<CartModel>)Session["Cart"];
                for (int i = 0; i < listCart.Count; i++)
                {
                    if (listCart[i].productId == productId)
                    {
                        var a = listCart[i].quantity + quantity;
                        listCart[i].quantity = a;
                        Session["Cart"] = listCart;
                        return listCart.Count();
                    }
                }
                listCart.Add(cart);
                Session["Cart"] = listCart;
                return listCart.Count();
            }
        }

        // Get cart object
        public ActionResult GetCart()
        {
            if (Session["Cart"] == null)
            {
                return null;
            }
            else
            {
                List<CartModel> listCart = (List<CartModel>)Session["Cart"];
                return Json(listCart, JsonRequestBehavior.AllowGet);
            }
        }

        // Delete a item in cart
        public int DeleteItemCart(int productId)
        {
            List<CartModel> listCart = (List<CartModel>)Session["Cart"];
            for (int i = 0; i < listCart.Count; i++)
            {
                if (listCart[i].productId == productId)
                {
                    listCart.Remove(listCart[i]);
                    Session["Cart"] = listCart;
                    return listCart.Count();
                }
            }
            return -1;
        }

        // Update cart
        public decimal UpdateItemCart(int productId, int quantity)
        {
            decimal totalPrice = 0;
            List<CartModel> listCart = (List<CartModel>)Session["Cart"];
            for (int i = 0; i < listCart.Count; i++)
            {
                if (listCart[i].productId == productId)
                {
                    listCart[i].quantity = quantity;
                    Session["Cart"] = listCart;
                }
                totalPrice = totalPrice + (listCart[i].price * listCart[i].quantity);
            }
            return totalPrice;
        }

        // Add an order
        public bool AddOrder(string shopId, string note, string custId, string address, string receiver, string phone)
        {
            if (Session["Cart"] != null)
            {
                List<CartModel> listCart = (List<CartModel>)Session["Cart"];
                OrderDetailViewModel orderDetailModel;
                List<OrderDetailViewModel> listOrderDetail = new List<OrderDetailViewModel>();
                for (int i = 0; i < listCart.Count(); i++)
                {
                    orderDetailModel = new OrderDetailViewModel();
                    orderDetailModel.ProductId = listCart[i].productId;
                    orderDetailModel.Properties = listCart[i].properties;
                    orderDetailModel.Price = listCart[i].price;
                    orderDetailModel.Quantity = listCart[i].quantity;
                    listOrderDetail.Add(orderDetailModel);
                }
                string status = OrderStatus.PROCESSING;
                bool result;
                result = orderService.AddOrder(shopId, note, custId, status, address, receiver, phone, listOrderDetail);
                if (result)
                {
                    Session["Cart"] = null;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        //Get Check out information
        public ActionResult GetCheckOutInfo(string customerId)
        {
            //Get Cart
            List<CartModel> listCart = (List<CartModel>)Session["Cart"];
            if (listCart != null)
            {
                //Create list Customer
                Customer customer = customerService.GetCustomerByCustomerId(customerId);
                //Create checkout view model
                CheckOutViewModel checkOutViewModel = new CheckOutViewModel();
                checkOutViewModel.userName = customer.Name;
                checkOutViewModel.phone = customer.Phone;
                checkOutViewModel.shippingAddress = customer.Address;
                checkOutViewModel.listCart = listCart;
                return Json(checkOutViewModel, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

    }
}