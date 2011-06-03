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

        public ActionResult Ask()
        {
            dynamic fid = Session["fid"];

            if (fid == null)
                return RedirectToAction("Index", "Home");

            string asked_friend = Request.Params.Get("friend_sel");
            string asked_question = Request.Params.Get("ask");
            string private_cost_question = Request.Params.Get("private_cost");
            string annon_cost_question = Request.Params.Get("annon_cost");

            if (private_cost_question == null)
                private_cost_question = "0";
            if (annon_cost_question == null)
                annon_cost_question = "0";
            int private_cost = int.Parse(private_cost_question);
            int annon_cost = int.Parse(annon_cost_question);
            long ffid = long.Parse(asked_friend);
            QuestionYourFriendsDataAccess.User me = Models.User.Get(fid);
            if (me.credit_amount > (annon_cost + private_cost))
            {
                QuestionYourFriendsDataAccess.User friend = Models.User.Get(ffid);
                if (friend == null)
                {
                    Models.User.Create(ffid);
                    friend = Models.User.Get(ffid);
                }
                int qid = Question.Create(me.id, friend.id, asked_question, annon_cost, private_cost, System.DateTime.Now);
                QuestionYourFriendsDataAccess.Question q = Question.Get(qid);
                Transac.SpendAndQuestion(q, me);
            }
            return RedirectToAction("Index", "Ask");
        }
    }
}