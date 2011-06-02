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

        public ActionResult Ask()
        {
            string asked_friend = this.Request.Params.Get("friend_sel");
            string asked_question = this.Request.Params.Get("ask");
            dynamic user = Session["user"];
            long fid = long.Parse(user.id);
            long ffid = long.Parse(asked_friend);
            QuestionYourFriendsDataAccess.User me = BusinessManagement.User.Get(fid);
            QuestionYourFriendsDataAccess.User friend = BusinessManagement.User.Get(ffid);
            if (friend == null)
            {
                BusinessManagement.User.Create(ffid);
                friend = BusinessManagement.User.Get(ffid);
            }
            BusinessManagement.Question.Create(me.id, friend.id, asked_question, 0, 0, System.DateTime.Now);
            return RedirectToAction("Index", "Ask");
        }
    }
}