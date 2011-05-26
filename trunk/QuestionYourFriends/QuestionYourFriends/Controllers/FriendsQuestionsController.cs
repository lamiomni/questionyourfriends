using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class FriendsQuestionsController : BaseController
    {
        //
        // GET: /FriendsQuestions/
        public ActionResult Index()
        {
            dynamic result = Session["user"];
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            return View();
        }
    }
}