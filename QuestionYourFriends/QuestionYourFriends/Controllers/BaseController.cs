using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dynamic result = Session["user"];
            if (result == null)
            {
                if (filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Home"
                    || filterContext.ActionDescriptor.ActionName != "Index")
                {
                    filterContext.HttpContext.Response.Redirect("Home/Index");
                    return;
                }

                result = BusinessManagement.Facebook.GetUserInfo();
                Session["user"] = result;
                Session["friends"] = BusinessManagement.Facebook.GetUserFriends();
                long fid = long.Parse(result.id);
                QuestionYourFriendsDataAccess.User u = BusinessManagement.User.Get(fid);
                if (u == null)
                    BusinessManagement.User.Create(fid);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}