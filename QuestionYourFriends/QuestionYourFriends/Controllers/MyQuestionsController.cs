using System.Web.Mvc;
using System.Collections.Generic;
using System;
using System.Web.UI.WebControls;

namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// My quesions pages controller
    /// </summary>
    public class MyQuestionsController : Controller
    {
        /// <summary>
        /// GET: /MyQuestions/
        /// </summary>
        /// 

       public ActionResult Index()
        {
            dynamic myAccount = Session["user"];
            dynamic myFriends = Session["friends"];

            if (myFriends == null && myAccount == null)
                return RedirectToAction("Index", "Home");

            // Récupérer le id du fid dans User
            QuestionYourFriendsDataAccess.User user = BusinessManagement.User.Get(long.Parse(myAccount.id));
            List<QuestionYourFriendsDataAccess.Question> receiver = BusinessManagement.Question.GetListOfReceiver(user.id);
            ViewData["questions"] = receiver;
            return View();
        }

        public ActionResult Answeree()
        {
            string answer = this.Request.Params.Get("answer");
            string qidstring = this.Request.Params.Get("qid");
            int qid = int.Parse(qidstring);
            QuestionYourFriendsDataAccess.Question q =  BusinessManagement.Question.Get(qid);
            q.answer = answer;
            BusinessManagement.Question.Update(q);
            return RedirectToAction("Index", "MyQuestion");
        }
    }
}