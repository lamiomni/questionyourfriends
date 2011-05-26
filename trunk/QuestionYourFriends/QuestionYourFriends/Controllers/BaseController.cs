using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    [HandleError]
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            dynamic result = Session["user"];
            if (result == null)
            {
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