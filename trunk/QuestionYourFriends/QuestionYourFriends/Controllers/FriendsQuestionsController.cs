using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class FriendsQuestionsController : Controller
    {
        //
        // GET: /FriendsQuestions/

        public ActionResult Index()
        {
            return View();
        }
    }
}