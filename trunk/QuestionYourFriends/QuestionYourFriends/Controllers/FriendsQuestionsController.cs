using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class FriendsQuestionsController : Controller
    {
        //
        // GET: /FriendsQuestions/
        public ActionResult Index()
        {
            dynamic result = Session["user"];

            if (result == null)
                return RedirectToAction("Index", "Home");

            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            return View();
        }
    }
}