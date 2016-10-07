using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class MasterProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Category { get; set; }
        public List<string> Attr { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public decimal Promotion { get; set; }
        
    }
}
