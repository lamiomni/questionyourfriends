using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    return RedirectToAction("Index", "Home");

                dynamic result = RequestCache.Get(fid + "user");
                dynamic friends = RequestCache.Get(fid + "friends");
                dynamic dict = RequestCache.Get(fid + "fid2uid");
                dynamic friendsDict = RequestCache.Get(fid + "friendsDictionary");

                if (result == null || friends == null || dict == null || friendsDict == null)
                    return RedirectToAction("Index", "Home");

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
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
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
                    return RedirectToAction("Index", "Home");

                var question = Question.Get(qid);
                var user = Models.User.Get(uid);

                Transac.DesanonymizeQuestion(question, user);
                ViewData["Info"] = "The user has been successfully revealed.";
                Debug.WriteLine("Reveal called qid: " + qid);
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
            }
            return View("Index");
        }
    }
}