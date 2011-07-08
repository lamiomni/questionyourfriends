using System.Reflection;
using System.Web.Mvc;
using System.Collections.Generic;
using Facebook;
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
                List<QuestionYourFriendsDataAccess.Question> receiver = Question.GetListOfReceiver(uid);
                ViewData["questions"] = receiver;
                ViewData["tab"] = "toMe";
                var user = Models.User.Get(uid);
                int nbRemain = QyfData.EarningMessagePerDay - Transac.GetNumberOfResponseToday(user);
                string msg = string.Format("Question Your Friends helps people find out more about each other through sharing interesting & personal responses.\n<br /><br />You can earn {0} credits each time you answer a question, {1} times a day.",
                                           QyfData.EarningAnswer, QyfData.EarningMessagePerDay);
                msg += nbRemain > 0 ? string.Format(" {0} more today!", nbRemain) : " No more today!";
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
                if (Session["uid"] == null || Session["fid"] == null)
                {
                    _logger.InfoFormat("Session fault, uid({0}), fid({1})", Session["uid"], Session["fid"]);
                    return RedirectToAction("Index", "Home");
                }
                var uid = (int)Session["uid"];
                var fid = (long)Session["fid"];

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

                // Publication sur Fb
                try
                {
                    long ffid = Models.User.Get(q.Owner.id).fid;
                    if (q.anom_price == 0 && q.private_price == 0)
                        Models.Facebook.Publish(ffid, "I just answered to your question.", 
                            "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif", 
                            "http://apps.facebook.com/questionyourfriends/", 
                            "Question Your Friend", 
                            Models.Facebook.GetFriendName(fid) + " just answered to your question on Question Your Friends! Come on and check the answer!");
                }
                catch (FacebookApiException e)
                {
                    _logger.Error(e.Message);
                }
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
                QuestionYourFriendsDataAccess.Question question = Question.Get(qid);
                User user = Models.User.Get(uid);
                Transac.DesanonymizeQuestion(question, user);

                // Publication sur Fb
                try
                {
                    long ffid = Models.User.Get(question.Owner.id).fid;
                    if (question.anom_price == 0 && question.private_price == 0)
                        Models.Facebook.Publish(ffid, "I just unanonymized your question!",
                            "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif",
                            "http://apps.facebook.com/questionyourfriends/",
                            "Question Your Friend",
                            Models.Facebook.GetFriendName(fid) + " just unanonymized your question on Question Your Friends!");
                }
                catch (FacebookApiException e)
                {
                    _logger.Error(e.Message);
                }
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
                QuestionYourFriendsDataAccess.Question question = Question.Get(qid);
                User user = Models.User.Get(uid);
                Transac.DeprivatizeQuestion(question, user);

                // Publication sur Fb
                try
                {
                    long ffid = Models.User.Get(question.Owner.id).fid;
                    if (question.anom_price == 0 && question.private_price == 0)
                        Models.Facebook.Publish(ffid, "I just deprivatized your question!",
                            "https://fbcdn-photos-a.akamaihd.net/photos-ak-snc1/v27562/215/131910193550231/app_1_131910193550231_8016.gif",
                            "http://apps.facebook.com/questionyourfriends/",
                            "Question Your Friend",
                            Models.Facebook.GetFriendName(fid) + " just deprivatized your question on Question Your Friends!");
                }
                catch (FacebookApiException e)
                {
                    _logger.Error(e.Message);
                }
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