using System.Web.Mvc;
using Facebook;
using Facebook.Web;
using Facebook.Web.Mvc;
using System.Configuration;

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

            dynamic result = QuestionYourFriendsDataAccess.BusinessManagement.Facebook.getUserInfo();
            ViewData["Firstname"] = (string)result.first_name;
            ViewData["Lastname"] = (string)result.last_name;
            dynamic myInfo = QuestionYourFriendsDataAccess.BusinessManagement.Facebook.getUserFriends();
            Response.Write(result);
            foreach (dynamic friend in myInfo.data)
            {
                Response.Write("Name: " + friend.name + "<br/>Facebook id: " + friend.id + "<br/><br/>");
            }
            return View();
        }
    }
}
