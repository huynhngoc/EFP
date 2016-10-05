using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class CategoryParentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }        
        public string Description { get; set; }
        public string ShopId { get; set; }
        public List<CategoryParentViewModel> Children { get; set; }        
    }
}
