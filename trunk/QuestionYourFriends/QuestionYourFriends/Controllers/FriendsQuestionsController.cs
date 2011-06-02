using System.Collections.Generic;
using System.Web.Mvc;


namespace QuestionYourFriends.Controllers
{
    /// <summary>
    /// Friends' questions pages controller
    /// </summary>
    public class FriendsQuestionsController : Controller
    {
        /// <summary>
        /// GET: /FriendsQuestions/
        /// </summary>
        public ActionResult Index()
        {
            dynamic result = Session["user"];

            if (result == null)
                return RedirectToAction("Index", "Home");

            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            dynamic friends = Session["friends"];
            var friendsId = new List<int>();
            var dick = (Dictionary<long, int>) Session["fid2uid"];
            foreach (var friend in friends.data)
            {
                var id = long.Parse(friend.id);
                if(dick.ContainsKey(id))
                    friendsId.Add(dick[id]);
            }

            var questions = BusinessManagement.Question.GetFriendsQuestions(friendsId.ToArray());
            ViewData["questions"] = questions;
            return View();
        }

        public ActionResult MakePublic(int qid)
        {
            dynamic res = Session["user"];
            long uid = long.Parse(res.id);

            var question = BusinessManagement.Question.Get(qid);
            var user = BusinessManagement.User.Get(uid);

            BusinessManagement.Transac.DesanonymizeQuestion(question, user);
            return RedirectToAction("Index", "FriendsQuestions");
        }
    }
}