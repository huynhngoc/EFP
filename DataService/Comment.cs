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
    
    public partial class Comment
    {
        public Comment()
        {
            this.Comments1 = new HashSet<Comment>();
        }
    
        public string Id { get; set; }
        public string SenderFbId { get; set; }
        public string PostId { get; set; }
        public bool IsRead { get; set; }
        public int Status { get; set; }
        public Nullable<int> IntentId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public string ParentId { get; set; }
        public string LastContent { get; set; }
    
        public virtual ICollection<Comment> Comments1 { get; set; }
        public virtual Comment Comment1 { get; set; }
        public virtual Intent Intent { get; set; }
        public virtual Post Post { get; set; }
    }
}
