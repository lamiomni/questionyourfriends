using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class MyQuestionsController : Controller
    {
        //
        // GET: /MyQuestions/

        public ActionResult Index()
        {
            return View();
        }

    }
}
