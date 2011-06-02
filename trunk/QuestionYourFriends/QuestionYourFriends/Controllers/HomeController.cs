using System.Collections.Generic;
using System.Web.Mvc;
using Facebook.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Home pages controller
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// GET: /Home/
        /// </summary>
        /// <returns></returns>
        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult Index()
        {
            dynamic result = Session["user"];
            if (result == null)
            {
                result = BusinessManagement.Facebook.GetUserInfo();
                Session["user"] = result;
                Session["friends"] = BusinessManagement.Facebook.GetUserFriends();
                Session["friendsDictionary"] = BusinessManagement.Facebook.GetUserFriendsDictionary();

                dynamic friends = Session["friends"];
                var friendsId = new List<long>();

                foreach (var friend in friends.data)
                {
                    friendsId.Add(long.Parse(friend.id));
                }
                Session["fid2uid"] = BusinessManagement.Facebook.GetUidFromFid(friendsId.ToArray());

                long fid = long.Parse(result.id);
                QuestionYourFriendsDataAccess.User u = BusinessManagement.User.Get(fid);
                if (u == null)
                    BusinessManagement.User.Create(fid);
            }

            result = Session["user"];
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";
            
            return RedirectToAction("Index", "MyQuestions");
        }
    }
}