using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Ask pages controller
    /// </summary>
    public class AskController : Controller
    {
        /// <summary>
        /// GET: /Ask/
        /// </summary>
        public ActionResult Index()
        {
            dynamic result = Session["user"];

            if (result == null)
                return RedirectToAction("Index", "Home");

            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";
            return View();
        }
    }
}