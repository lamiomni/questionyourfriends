using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Facebook;
using log4net;
using QuestionYourFriends.Caching;
using QuestionYourFriends.Models;


namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Friends' questions pages controller
    /// </summary> 
    [HandleError]
    public class FriendsQuestionsController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string _info;
        private static string _error;
        public static string Info
        {
            get { 
                string res = _info;
                _info = null;
                return res;
            }
            set { _info = value; }
        }
        public static string Error
        {
            get
            {
                string res = _error;
                _error = null;
                return res;
            }
            set { _error = value; }
        }

        /// <summary>
        /// GET: /FriendsQuestions/
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                // Fetch data
                if (Session["uid"] == null || Session["fid"] == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }
                var uid = (int)Session["uid"];
                var fid = (long)Session["fid"];

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Fetch parameters
                var result = RequestCache.Get<JsonObject>(fid + "user");
                var friends = RequestCache.Get<JsonObject>(fid + "friends");
                var dict = RequestCache.Get<IDictionary<long, int>>(fid + "fid2uid");
                var friendsDict = RequestCache.Get<IDictionary<long, JsonObject>>(fid + "friendsDictionary");
                if (result == null || friends == null || dict == null || friendsDict == null)
                {
                    _logger.Info("Cache fault");
                    return RedirectToAction("Index", "Home");
                }

                // Compute data
                ViewData["friends"] = friendsDict;
                ViewData["Firstname"] = result["first_name"];
                ViewData["Lastname"] = result["last_name"];
                var questions = Question.GetFriendsQuestions((from friend in (JsonArray) friends[0]
                                                              select long.Parse(((JsonObject)friend)["id"].ToString())
                                                              into id where dict.ContainsKey(id) select dict[id]).ToArray());
                for (int i = 0; i < questions.Count;)
                {
                    if (!friendsDict.ContainsKey(questions[i].Owner.fid))
                    {
                        questions.RemoveAt(i);
                        continue;
                    }
                    i++;
                }
                ViewData["questions"] = questions;
                string info = Info;
                if (!string.IsNullOrEmpty(info))
                    ViewData["Info"] = info;
                string error = Error;
                if (!string.IsNullOrEmpty(error))
                    ViewData["Error"] = error;
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return View();
        }

        /// <summary>
        /// POST: /FriendsQuestions/Reveal
        /// </summary>
        /// <param name="qid">Question id</param>
        public ActionResult Reveal(int qid)
        {
            try
            {
                // Fetch data
                if (Session["uid"] == null || Session["fid"] == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }
                var uid = (int)Session["uid"];
                var fid = (long)Session["fid"];

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Compute data
                var question = Question.Get(qid);
                var user = Models.User.Get(uid);
                Transac.DesanonymizeQuestion(question, user);
                Info = "The user has been successfully revealed.";
            }
            catch (ApplicationException e)
            {
                Error = e.Message;
                _logger.Error(e.Message);
            }
            return RedirectToAction("Index");
        }
    }
}