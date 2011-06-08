using System.IO;
using System.Reflection;
using System.Web.Mvc;
using QuestionYourFriends.Caching;
using QuestionYourFriends.Models;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Ask pages controller
    /// </summary>
    public class AskController : Controller
    {
        /// <summary>
        /// GET: /Ask/
        /// </summary>
        public ActionResult Index()
        {
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
            dynamic fid = Session["fid"];

            if (fid == null)
                return RedirectToAction("Index", "Home");

            string askedFriend = Request.Params.Get("friend_sel");
            string askedQuestion = Request.Params.Get("ask");
            string privateCostQuestion = Request.Params.Get("private_cost");
            string annonCostQuestion = Request.Params.Get("annon_cost");

            if (privateCostQuestion == null)
                privateCostQuestion = "0";
            if (annonCostQuestion == null)
                annonCostQuestion = "0";

            int privateCost = int.Parse(privateCostQuestion);
            int annonCost = int.Parse(annonCostQuestion);
            long ffid = long.Parse(askedFriend);
            QuestionYourFriendsDataAccess.User me = Models.User.Get(fid);

            if (me.credit_amount > (annonCost + privateCost))
            {
                QuestionYourFriendsDataAccess.User friend = Models.User.Get(ffid);
                if (friend == null)
                {
                    Models.User.Create(ffid);
                    friend = Models.User.Get(ffid);
                }
                int qid = Question.Create(me.id, friend.id, askedQuestion, annonCost, privateCost, System.DateTime.Now);
                QuestionYourFriendsDataAccess.Question q = Question.Get(qid);
                Transac.SpendAndQuestion(q, me);
            }
            return RedirectToAction("Index", "Ask");
        }
    }
}