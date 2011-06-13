using System;
using System.Reflection;
using System.Web.Mvc;
using log4net;

namespace QuestionYourFriends.Controllers
{
    public class QuestionController : Controller
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // GET: /Question/
        public ActionResult Index()
        {
            string qidstring = Request.Params.Get("qid");
            int qid;
            try {
                qid = int.Parse(qidstring);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                qid = 0;
            }
            if (qid > 0)
            {
                QuestionYourFriendsDataAccess.Question q = Models.Question.Get(qid);
                ViewData["question"] = q;
                return View();
            }
            return RedirectToAction("Index", "MyQuestions");
        }

    }
}
