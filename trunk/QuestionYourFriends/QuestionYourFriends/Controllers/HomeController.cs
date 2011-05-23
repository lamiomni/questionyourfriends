using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";

            return View();
        }

        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult About()
        {
            dynamic result = BusinessManagement.Facebook.GetUserInfo();
            ViewData["Firstname"] = (string) result.first_name;
            ViewData["Lastname"] = (string) result.last_name;

            return View();
        }
    }
}