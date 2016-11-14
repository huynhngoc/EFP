using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using DataService.Service;
using Microsoft.AspNet.SignalR;

namespace ShopManager.SignalRAlert
{
    public class AlertHub: Hub
    {
        public static CommentService commentService= new CommentService();
        public static PostService postService = new PostService();
        public static ConversationService conversationService = new ConversationService();
        public static void SendMessage(string shopId, object message, string threadId, int intent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AlertHub>();
            context.Clients.Group(shopId).sendMessageToPage(message, threadId, intent);                        
        }

        public static void SendComment(string shopId, object comment, int intent)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AlertHub>();
            context.Clients.Group(shopId).sendCommentToPage(comment, intent);
        }

        public static void SendNotification(string shopId)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AlertHub>();
            context.Clients.Group(shopId).sendNotification(conversationService.NewConversationCount(shopId),
                postService.NewPostCount(shopId),
                commentService.NewCommentCount(shopId));
        }

        public override Task OnConnected()
        {
            //string name = Context.User.Identity.Name;
            var name = Context.QueryString["Name"];
            Groups.Add(Context.ConnectionId, name);
            var id = Context.ConnectionId;
            //            Groups.Add()

            
            
            return base.OnConnected();
        }

    }
}