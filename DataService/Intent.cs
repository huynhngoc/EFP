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
    
    public partial class Intent
    {
        public Intent()
        {
            this.Conversations = new HashSet<Conversation>();
            this.Responses = new HashSet<Respons>();
            this.Comments = new HashSet<Comment>();
            this.Posts = new HashSet<Post>();
        }
    
        public int Id { get; set; }
        public string IntentName { get; set; }
    
        public virtual ICollection<Conversation> Conversations { get; set; }
        public virtual ICollection<Respons> Responses { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
