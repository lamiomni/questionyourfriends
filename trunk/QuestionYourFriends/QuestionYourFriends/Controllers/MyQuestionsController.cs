using System.Web.Mvc;
using System.Collections.Generic;
using QuestionYourFriends.Models;
using Facebook.Web.Mvc;

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
            Question.Update(q);
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult Delete()
        {
            string qidstring = Request.Params.Get("qid");
            int qid = int.Parse(qidstring);
            Question.Delete(qid);
            return RedirectToAction("Index", "MyQuestions");
        }

        public ActionResult Cancel()
        {
            Request.Params.Set("answer", "");
            return RedirectToAction("Index", "MyQuestions");
        }
    }
}