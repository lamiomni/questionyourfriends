using System.Reflection;
using System.Web.Mvc;
using System.Collections.Generic;
using log4net;
using QuestionYourFriends.Caching;
using System;
using QuestionYourFriendsDataAccess;
using Question = QuestionYourFriends.Models.Question;
using Transac = QuestionYourFriends.Models.Transac;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// My quesions pages controller
    /// </summary>
    [HandleError]
    public class MyQuestionsController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GET: /MyQuestions/
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
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Compute data
                List<QuestionYourFriendsDataAccess.Question> receiver = Question.GetListOfReceiver(uid);
                ViewData["questions"] = receiver;
                ViewData["tab"] = "toMe";
                var user = Models.User.Get(uid);
                int nbRemain = QyfData.EarningMessagePerDay - Transac.GetNumberOfResponseToday(user);
                string msg = string.Format("You can earn {0} credits each time you answer a question, {1} times a day.",
                                           QyfData.EarningAnswer, QyfData.EarningMessagePerDay);
                msg += nbRemain != 0 ? string.Format(" {0} more today!", nbRemain) : " No more today!";
                ViewData["Info2"] = msg;
            }
            catch (ApplicationException e)
            {
                ViewData["questions"] = new List<QuestionYourFriendsDataAccess.Question>();
                ViewData["tab"] = "toMe";
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return View("Index");
        }

        /// <summary>
        /// GET: /MyQuestions/FromMe
        /// </summary>
        public ActionResult FromMe()
        {
            try
            {
                // Fetch data
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Fetch parameters
                dynamic result = RequestCache.Get(fid + "user");
                dynamic friends = RequestCache.Get(fid + "friends");
                dynamic dict = RequestCache.Get(fid + "fid2uid");
                dynamic friendsDict = RequestCache.Get(fid + "friendsDictionary");
                if (result == null || friends == null || dict == null || friendsDict == null)
                {
                    _logger.Info("Cache fault");
                    return RedirectToAction("Index", "Home");
                }

                // Compute data
                ViewData["friends"] = friendsDict;
                ViewData["Firstname"] = result.first_name;
                ViewData["Lastname"] = result.last_name;

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Compute data
                List<QuestionYourFriendsDataAccess.Question> toAll = Question.GetListOfOwner(uid);
                ViewData["questions"] = toAll;
                ViewData["tab"] = "fromMe";
            }
            catch (ApplicationException e)
            {
                ViewData["questions"] = new List<QuestionYourFriendsDataAccess.Question>();
                ViewData["tab"] = "toMe";
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return View("IndexSent");
        }

        /// <summary>
        /// Answer a question
        /// </summary>
        public ActionResult Answeree()
        {
            try
            {
                // Fetch data
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Question update
                string answer = Request.Params.Get("answer");
                string qidstring = Request.Params.Get("qid");
                int qid = int.Parse(qidstring);
                QuestionYourFriendsDataAccess.Question q = Question.Get(qid);
                q.answer = answer;
                q.date_answer = DateTime.Now;
                Question.Update(q);

                // Earning answer transaction
                User u = Models.User.Get(uid);
                Transac.EarningAnswer(u);
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Delete a question
        /// </summary>
        /// <param name="qid">Question id</param>
        public ActionResult Delete(int qid)
        {
            try
            {
                // Fetch data
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                Question.Delete(qid);
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Cancels an action
        /// </summary>
        public ActionResult Cancel()
        {
            try
            {
                // Fetch data
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                Request.Params.Set("answer", "qid");
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Reveals a question
        /// </summary>
        /// <param name="qid">question id</param>
        public ActionResult Reveal(int qid)
        {
            try
            {
                // Fetch data
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Compute data
                QuestionYourFriendsDataAccess.Question question = Question.Get(qid);
                User user = Models.User.Get(uid);
                Transac.DesanonymizeQuestion(question, user);
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return RedirectToAction("Index", "MyQuestions");
        }

        /// <summary>
        /// Deprivatize a question
        /// </summary>
        /// <param name="qid">question id</param>
        public ActionResult ToPublic(int qid)
        {
            try
            {
                // Fetch data
                dynamic uid = Session["uid"];
                dynamic fid = Session["fid"];
                if (uid == null || fid == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }

                // Logging
                MethodBase mb = MethodBase.GetCurrentMethod();
                string folder = mb.DeclaringType.Name;
                folder = folder.Substring(0, folder.Length - 10);
                _logger.InfoFormat("User {0} (FbId: {1}) accessed page {2}/{3}", uid, fid, folder, mb.Name);

                // Compute data
                QuestionYourFriendsDataAccess.Question question = Question.Get(qid);
                User user = Models.User.Get(uid);
                Transac.DeprivatizeQuestion(question, user);
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
                _logger.Error(e.Message);
            }
            return RedirectToAction("Index");
        }
    }
}