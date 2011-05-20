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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // YOU DONT NEED ANY OF THIS IN YOUR APPLICATION
            // THIS METHOD JUST CHECKS TO SEE IF YOU HAVE SETUP
            // THE SAMPLE. IF THE SAMPLE IS NOT SETUP YOU WILL
            // BE SENT TO THE GETTING STARTED PAGE.

            base.OnActionExecuting(filterContext);

            bool isSetup = false;
            var settings = ConfigurationManager.GetSection("facebookSettings");
            if (settings != null)
            {
                var current = settings as IFacebookApplication;
                if (current.AppId != "169175053143325" &&
                    current.AppSecret != "4dfcc76db4348c32ba1578e43b1de3d6" &&
                    current.CanvasUrl != "http://apps.facebook.com/hellototo/")
                {
                    isSetup = true;
                }
            }

            if (!isSetup)
            {
                filterContext.Result = View("GettingStarted");
            }

        }

        public ActionResult Index()
        {
            ViewData["Message"] = "Bienvenue dans ASP.NET MVC !";

            return View();
        }

        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult About()
        {
            var fb = new FacebookWebClient();
            dynamic result = fb.Get("me");
            ViewData["Firstname"] = (string)result.first_name;
            ViewData["Lastname"] = (string)result.last_name;
            return View();
        }
    }
}
