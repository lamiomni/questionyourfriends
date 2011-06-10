using System;
using System.Web.Mvc;
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
        /// <summary>
        /// GET: /Ask/
        /// </summary>
        public ActionResult Index()
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
           
            return View();
        }

        /// <summary>
        /// GET: /Ask/Ask
        /// </summary>
        public ActionResult Ask()
        {
            // Fetch data
            dynamic fid = Session["fid"];
            if (fid == null)
                return View("Index");
            string askedFriend = Request.Params.Get("friend_sel");
            string askedQuestion = Request.Params.Get("ask");
            string privateCostQuestion = Request.Params.Get("private_cost");
            string annonCostQuestion = Request.Params.Get("annon_cost");
            int privateCost;
            int annonCost;
            long ffid;

            // Parameters checking
            if (!long.TryParse(askedFriend, out ffid))
            {
                ViewData["Error"] = "Please select a friend.";
                return View("Index");
            }
            if (askedQuestion == null)
            {
                ViewData["Error"] = "Please formulate a question.";
                return View("Index");
            }
            if (!int.TryParse(privateCostQuestion, out privateCost)
                || !int.TryParse(annonCostQuestion, out annonCost)
                || privateCost < 0
                || privateCost > 9999999
                || annonCost < 0
                || annonCost > 9999999)
            {
                ViewData["Error"] = "Please enter a valid number (> 0 and < 9,999,999) for the prices!";
                return View("Index");
            }

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
            return View("Index");
        }
    }
}