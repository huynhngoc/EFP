using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class DetailedProductViewModel
    {
        public int Id { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> PromotionPrice { get; set; }
        public bool Status { get; set; }
        public int MasterId { get; set; }
        public string Properties { get; set; }
        
    }
}
