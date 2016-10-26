using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopManager
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new SessionRequiredFilter());
        }
    }

    public class SessionRequiredFilter : ActionFilterAttribute
    {        

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["ShopId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                        {
                            { "controller", "Home" },
                            { "action", "ChooseShop" }
                        }
                    );
            }
            //base.OnActionExecuting(filterContext);
            //else
            //{
            //    
            //}            
        }
    }
}
