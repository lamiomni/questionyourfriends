using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Friends' questions pages controller
    /// </summary>
    public class FriendsQuestionsController : Controller
    {
        /// <summary>
        /// GET: /FriendsQuestions/
        /// </summary>
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