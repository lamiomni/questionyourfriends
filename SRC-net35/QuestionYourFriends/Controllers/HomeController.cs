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
                if (Session["fid"] == null)
                {
                    JsonObject currentUser = Models.Facebook.GetUserInfo();
                    long fid = long.Parse(currentUser["id"].ToString());
                    QuestionYourFriendsDataAccess.User u = Models.User.Get(fid);

                    if (!u.activated)
                        return View();

                    Session["fid"] = fid;
                    Session["uid"] = u.id;

                    RequestCache.Add(fid + "user", currentUser);
                    RequestCache.Add(fid + "friends", Models.Facebook.GetUserFriends());
                    RequestCache.Add(fid + "friendsDictionary", Models.Facebook.GetUserFriendsDictionary());

                    var friends = RequestCache.Get<JsonObject>(fid + "friends");
                    RequestCache.Add(fid + "fid2uid",
                                     Models.Facebook.GetUidFromFid(
                                         (from friend in ((JsonArray)friends[0])
                                          select long.Parse(((JsonObject)friend)["id"].ToString())).ToList()));
                    _logger.InfoFormat("Fetch cache for {0} (FbId: {1}) done.", u.id, fid);
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

        public ActionResult Clear()
        {
            Session.Clear();
            return View("Index");
        }
    }
}