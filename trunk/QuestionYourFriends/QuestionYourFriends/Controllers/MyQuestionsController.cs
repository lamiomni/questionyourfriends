using System.Web.Mvc;
using System.Collections.Generic;
using QuestionYourFriends.Models;

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
            dynamic uid = Session["uid"];

            if (uid == null)
                return RedirectToAction("Index", "Home");
            
            List<QuestionYourFriendsDataAccess.Question> receiver = Question.GetListOfReceiver(uid);
            ViewData["questions"] = receiver;
            return View();
        }

        public ActionResult Answeree()
        {
            string answer = Request.Params.Get("answer");
            string qidstring = Request.Params.Get("qid");
            int qid = int.Parse(qidstring);
            QuestionYourFriendsDataAccess.Question q =  Question.Get(qid);
            q.answer = answer;
            Question.Update(q);
            return RedirectToAction("Index", "MyQuestions");
        }
    }
}