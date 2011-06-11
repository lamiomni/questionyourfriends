using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
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
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.Info("Cache fault");
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Fetch parameters
                dynamic result = RequestCache.Get(fid + "user");
                dynamic friends = RequestCache.Get(fid + "friends");
                dynamic dict = RequestCache.Get(fid + "fid2uid");
                dynamic friendsDict = RequestCache.Get(fid + "friendsDictionary");
                if (result == null || friends == null || dict == null || friendsDict == null)
                    return RedirectToAction("Index", "Home");

                // Compute data
                ViewData["friends"] = friendsDict;
                ViewData["Firstname"] = result.first_name;
                ViewData["Lastname"] = result.last_name;
                var friendsId = new List<int>();
                foreach (var friend in friends.data)
                {
                    var id = long.Parse(friend.id);
                    if (dict.ContainsKey(id))
                        friendsId.Add(dict[id]);
                }
                var questions = Question.GetFriendsQuestions(friendsId.ToArray());
                ViewData["questions"] = questions;
                string info = Info;
                if (!string.IsNullOrWhiteSpace(info))
                    ViewData["Info"] = info;
                string error = Error;
                if (!string.IsNullOrWhiteSpace(error))
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
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.Info("Cache fault");
                    return RedirectToAction("Index", "Home");
                }

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