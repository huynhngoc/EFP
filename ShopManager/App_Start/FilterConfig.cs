using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DataService.Service;

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
        public CommentService commentService = new CommentService();
        public PostService postService = new PostService();
        public ConversationService conversationService = new ConversationService();

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
            else
            {
                string shopId = (string)filterContext.HttpContext.Session["ShopId"];
                filterContext.Controller.ViewBag.NewCommentCount = commentService.NewCommentCount(shopId);
                filterContext.Controller.ViewBag.NewPostCount = postService.NewPostCount(shopId);
                filterContext.Controller.ViewBag.NewConversationCount = conversationService.NewConversationCount(shopId);
            }
            //base.OnActionExecuting(filterContext);
            //else
            //{
            //    
            //}            
        }
    }
}
