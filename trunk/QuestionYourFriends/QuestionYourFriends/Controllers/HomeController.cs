using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    [HandleError]
    public class HomeController : BaseController
    {
        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult Index()
        {
            dynamic result = Session["user"];
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";

            return RedirectToAction("Index", "MyQuestions");
        }
    }
}