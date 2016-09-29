using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShopManager.Startup))]
namespace ShopManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
