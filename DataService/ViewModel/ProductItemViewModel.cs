using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class ProductItemViewModel
    {
        public int Id { get; set; }
        public string ShopId { get; set; }
        public string Name { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string Attr1 { get; set; }
        public string Attr2 { get; set; }
        public string Attr3 { get; set; }
        public string Attr4 { get; set; }
        public string Attr5 { get; set; }
        public string Attr6 { get; set; }
        public string Attr7 { get; set; }
        public string Description { get; set; }
        public Nullable<bool> IsInStock { get; set; }
        public decimal Price { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public Nullable<int> TemplateId { get; set; }
        public List<string> Urls { get; set; }

        public ProductItemViewModel()
        {
            Urls = new List<string>();
        }
    }
}
