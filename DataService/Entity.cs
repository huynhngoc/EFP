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
    
    public partial class Entity
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string ShopId { get; set; }
        public bool IsDynamic { get; set; }
    
        public virtual Shop Shop { get; set; }
    }
}
