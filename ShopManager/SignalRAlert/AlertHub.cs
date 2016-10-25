using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ShopManager.SignalRAlert
{
    public class AlertHub: Hub
    {
        public static void Send(string shopId, object message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<AlertHub>();
            context.Clients.Group(shopId).sendToPage(message);                        
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