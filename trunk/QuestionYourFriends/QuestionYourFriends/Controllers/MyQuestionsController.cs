using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class MyQuestionsController : Controller
    {
        //
        // GET: /MyQuestions/

        public ActionResult Index()
        {
            return View();
        }
    }
}