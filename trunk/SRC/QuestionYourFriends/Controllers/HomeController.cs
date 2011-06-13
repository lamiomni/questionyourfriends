using System;
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
            try
            {
                dynamic currentUser = Models.Facebook.GetUserInfo();
                long fid = long.Parse(currentUser.id);
                dynamic result = RequestCache.Get(fid + "user");
                dynamic friends = RequestCache.Get(fid + "friends");
                dynamic dict = RequestCache.Get(fid + "fid2uid");
                dynamic friendsDict = RequestCache.Get(fid + "friendsDictionary");

                if (Session["fid"] == null || (long) Session["fid"] != fid || result == null || friends == null
                    || dict == null || friendsDict == null)
                {
                    Session.Clear();
                    QuestionYourFriendsDataAccess.User u = Models.User.Get(fid);

                    if (!u.activated)
                        return View();

                    Session["fid"] = fid;
                    Session["uid"] = u.id;

                    friends = Models.Facebook.GetUserFriends();

                    RequestCache.Add(fid + "user", currentUser);
                    RequestCache.Add(fid + "friends", friends);
                    RequestCache.Add(fid + "friendsDictionary", Models.Facebook.GetUserFriendsDictionary());
                    var friendsId = new List<long>();
                    foreach (var friend in friends.data)
                        friendsId.Add(long.Parse(friend.id));
                    RequestCache.Add(fid + "fid2uid", Models.Facebook.GetUidFromFid(friendsId.ToArray()));

                    _logger.InfoFormat("Fetch cache for {0} (FbId: {1} - db {2}) done.", u.id, fid, u.fid);
                }
            }
            catch (Exception e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
                return View();
            }
            return RedirectToAction("Index", "MyQuestions");
        }
    }
}