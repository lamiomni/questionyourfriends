using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class MyQuestionsController : BaseController
    {
        //
        // GET: /MyQuestions/
        public ActionResult Index()
        {
            dynamic result = Session["user"];
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            return View();
        }
    }
}