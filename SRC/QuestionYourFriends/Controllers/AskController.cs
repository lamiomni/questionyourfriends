using System;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using QuestionYourFriends.Caching;
using QuestionYourFriends.Models;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Ask pages controller
    /// </summary>
    [HandleError]
    public class AskController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// GET: /Ask/
        /// </summary>
        public ActionResult Index()
        {
            try
            {
                // Fetch data
                dynamic fid = Session["fid"];
                if (fid == null)
                    return RedirectToAction("Index", "Home");
                dynamic user = RequestCache.Get(fid + "user");
                if (user == null)
                    return RedirectToAction("Index", "Home");

                ViewData["Firstname"] = user.first_name;
                ViewData["Lastname"] = user.last_name;
                ViewData["Message"] = "Bienvenue sur QuestionYourFriends !";
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
            }
            return View();
        }

        /// <summary>
        /// GET: /Ask/Ask
        /// </summary>
        public ActionResult Ask()
        {
            try
            {
                // Fetch data
                dynamic fid = Session["fid"];
                if (fid == null)
                    return RedirectToAction("Index", "Home");
                string askedFriend = Request.Params.Get("friend_sel");
                string askedQuestion = Request.Params.Get("ask");
                string privateCostQuestion = Request.Params.Get("private_cost");
                string annonCostQuestion = Request.Params.Get("annon_cost");
                int privateCost;
                int annonCost;
                long ffid;

                // Parameters checking
                if (!long.TryParse(askedFriend, out ffid))
                    throw new ApplicationException("Please select a friend.");
                if (askedQuestion == null)
                    throw new ApplicationException("Please formulate a question.");
                if (!int.TryParse(privateCostQuestion, out privateCost)
                    || !int.TryParse(annonCostQuestion, out annonCost)
                    || privateCost < 0
                    || privateCost > 9999999
                    || annonCost < 0
                    || annonCost > 9999999)
                    throw new ApplicationException("Please enter valid numbers (> 0 and < 9,999,999) for the prices.");

                // Do work
                QuestionYourFriendsDataAccess.User me = Models.User.Get(fid);
                if (me.credit_amount > (annonCost + privateCost))
                {
                    QuestionYourFriendsDataAccess.User friend = Models.User.Get(ffid);
                    if (friend == null)
                    {
                        Models.User.Create(ffid);
                        friend = Models.User.Get(ffid);
                        Transac.EarningStartup(friend);
                    }
                    int qid = Question.Create(me.id, friend.id, askedQuestion, annonCost, privateCost, DateTime.Now);
                    QuestionYourFriendsDataAccess.Question q = Question.Get(qid);
                    Transac.SpendAndQuestion(q, me);
                }
                ViewData["Info"] = "Your question has been sent successfully.";
            }
            catch (ApplicationException e)
            {
                ViewData["Error"] = e.Message;
            }
            return View("Index");
        }
    }
}