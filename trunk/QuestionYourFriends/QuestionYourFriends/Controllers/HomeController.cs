using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult Index()
        {
            dynamic result = BusinessManagement.Facebook.GetUserInfo();
            Session["user"] = result;
            Session["friends"] = BusinessManagement.Facebook.GetUserFriends();
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";

            long fid = long.Parse(result.id);
            QuestionYourFriendsDataAccess.User u = BusinessManagement.User.Get(fid);
            if (u == null)
            {
                BusinessManagement.User.Create(fid);
            }
           
            return RedirectToAction("Index", "MyQuestions");
        }
    }
}