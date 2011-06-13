using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Facebook;
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
                JsonObject currentUser = Models.Facebook.GetUserInfo();
                long fid = long.Parse(currentUser["id"].ToString());
                var result = RequestCache.Get<JsonObject>(fid + "user");
                var friends = RequestCache.Get<JsonObject>(fid + "friends");
                var dict = RequestCache.Get<IDictionary<long, int>>(fid + "fid2uid");
                var friendsDict = RequestCache.Get<IDictionary<long, JsonObject>>(fid + "friendsDictionary");

                if (Session["fid"] == null || (long)Session["fid"] != fid || result == null || friends == null 
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
                    RequestCache.Add(fid + "fid2uid",
                                     Models.Facebook.GetUidFromFid(
                                         (from friend in ((JsonArray)friends[0])
                                          select long.Parse(((JsonObject)friend)["id"].ToString())).ToList()));
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