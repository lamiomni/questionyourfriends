

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class QuestionController : Controller
    {
        //
        // GET: /Question/

        public ActionResult Index()
        {
            string qidstring = Request.Params.Get("qid");
            int qid;
            try {
                qid = int.Parse(qidstring);
            }
            catch (Exception e){
                qid = 0;
            }
            if (qid > 0)
            {
                QuestionYourFriendsDataAccess.Question q = QuestionYourFriends.Models.Question.Get(qid);
                ViewData["question"] = q;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "MyQuestions");
            }
            
        }

    }
}
