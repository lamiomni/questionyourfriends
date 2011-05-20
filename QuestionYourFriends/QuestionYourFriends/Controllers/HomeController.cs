using System.Web.Mvc;
using Facebook;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Configuration;
using QuestionYourFriends;

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

        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult About()
        {

            dynamic result = BusinessManagement.Facebook.getUserInfo();
            ViewData["Firstname"] = (string)result.first_name;
            ViewData["Lastname"] = (string)result.last_name;


            
            return View();
        }
    }
}
