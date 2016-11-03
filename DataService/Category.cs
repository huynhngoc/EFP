//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataService
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
        public Category()
        {
            this.Categories1 = new HashSet<Category>();
            this.Products = new HashSet<Product>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string ShopId { get; set; }
    
        public virtual ICollection<Category> Categories1 { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
