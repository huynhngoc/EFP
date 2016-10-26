using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.Utils
{
    public static class WebhookObject
    {
        public const string User = "user";
        public const string Page = "page";
        public const string Permission = "permission";
        public const string Payment = "payments";
    }

    public static class WebhookField
    {
        public const string Feed = "feed";
        public const string Conversations = "conversations";
    }

    public static class WebhookItem
    {
        public const string Comment = "comment";
        public const string Reaction = "reaction";
        public const string Like = "like";
    }

    public static class WebhookVerb
    {
        public const string Add = "add";
        public const string Edit = "edited";
        public const string Remove = "remove";
        public const string Hide = "hide";
        public const string Unhide = "unhide";
    }
    
}
