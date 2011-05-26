using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class AskController : BaseController
    {
        //
        // GET: /Ask/
        public ActionResult Index()
        {
            dynamic result = Session["user"];
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";
            return View();
        }
    }
}