using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Facebook.Web.Mvc;
using log4net;
using QuestionYourFriends.Caching;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Home pages controller
    /// </summary>
    [HandleError]
    public class HomeController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GET: /Home/
        /// </summary>
        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
        public ActionResult Index()
        {
            dynamic fid = Session["fid"];
            if (fid == null || RequestCache.Get(fid + "user") == null)
            {
                dynamic currentUser = Models.Facebook.GetUserInfo();
                fid = long.Parse(currentUser.id);
                QuestionYourFriendsDataAccess.User u = Models.User.Get(fid);

                if (!u.activated)
                    return View();

                Session["fid"] = fid;
                Session["uid"] = u.id;

                RequestCache.Add(fid + "user", currentUser);
                RequestCache.Add(fid + "friends", Models.Facebook.GetUserFriends());
                RequestCache.Add(fid + "friendsDictionary", Models.Facebook.GetUserFriendsDictionary());

                dynamic friends = RequestCache.Get(fid + "friends");
                var friendsId = new List<long>();
                foreach (var friend in friends.data)
                    friendsId.Add(long.Parse(friend.id));
                RequestCache.Add(fid + "fid2uid", Models.Facebook.GetUidFromFid(friendsId.ToArray()));

                _logger.InfoFormat("Fetch cache for {0} (FbId: {1}) done.", u.id, fid);
            }
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult Clear()
        {
            Session.Clear();
            return View("Index");
        }
    }
}