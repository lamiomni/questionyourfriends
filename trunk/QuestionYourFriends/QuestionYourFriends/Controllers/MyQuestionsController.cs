using System.Web.Mvc;
using System.Collections.Generic;
using QuestionYourFriends.Models;
using Facebook.Web.Mvc;
using System;

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
        [CanvasAuthorize(Permissions = "user_about_me,publish_stream")]
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
            DateTime timeNow = DateTime.Now;
            q.date_answer = timeNow;
            Question.Update(q);
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult Delete(int qid)
        {
            Question.Delete(qid);
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult Cancel()
        {
            Request.Params.Set("answer", "qid");
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult Reveal()
        {
            dynamic uid = Session["uid"];
            string qidstring = Request.Params.Get("qid");
            int qid = int.Parse(qidstring);
            QuestionYourFriendsDataAccess.Question question = Question.Get(qid);
            QuestionYourFriendsDataAccess.User user = QuestionYourFriends.Models.User.Get(uid);
            if (user.credit_amount > question.anom_price)
            {
                user.credit_amount -= question.anom_price;
                question.anom_price = 0;
                Question.Update(question);
                QuestionYourFriends.Models.User.Update(user);
            }
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult ToPublic()
        {
            dynamic uid = Session["uid"];
            string qidstring = Request.Params.Get("qid");
            int qid = int.Parse(qidstring);
            QuestionYourFriendsDataAccess.Question question = Question.Get(qid);
            QuestionYourFriendsDataAccess.User user = QuestionYourFriends.Models.User.Get(uid);
            if (user.credit_amount > question.private_price)
            {
                user.credit_amount -= question.private_price;
                question.private_price = 0;
                Question.Update(question);
                QuestionYourFriends.Models.User.Update(user);
            }
            return RedirectToAction("Index", "MyQuestions");
        }
    }
}