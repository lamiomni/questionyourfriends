using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Message"] = "Bienvenue dans ASP.NET MVC !";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
