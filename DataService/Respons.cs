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
    
    public partial class Respons
    {
        public Respons()
        {
            this.Conversations = new HashSet<Conversation>();
        }
    
        public int Id { get; set; }
        public string ShopId { get; set; }
        public int IntentId { get; set; }
        public string RespondContent { get; set; }
    
        public virtual ICollection<Conversation> Conversations { get; set; }
        public virtual Intent Intent { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
