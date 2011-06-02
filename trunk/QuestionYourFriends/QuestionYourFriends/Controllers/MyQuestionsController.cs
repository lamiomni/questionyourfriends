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

        public string SearchFriend(dynamic myFriends, QuestionYourFriendsDataAccess.Question question)
        {
            string buffer = "";

            // Chercher le nom et prénom de l'envoyeur en comparant les fids de la base de données et de Session["friend"]
            if (myFriends.data != null)
            {
                QuestionYourFriendsDataAccess.User sender = BusinessManagement.User.Get(question.id_owner);
                foreach (dynamic friend in myFriends.data)
                {
                    if (sender != null && long.Parse(friend.id) == sender.fid)
                    {
                        buffer += "De ";
                        buffer += friend.name;
                        break;
                    }
                }
            }
            return buffer;
        }

        public void storeQuestionInView(dynamic myFriends, QuestionYourFriendsDataAccess.User user)
        {
            // Stocker dans la view la liste des questions dont le récepteur est l'utilisateur

            var receiver = BusinessManagement.Question.GetListOfReceiver(user.id);
            if (receiver != null)
            {
                List<string> textBufferList = new List<string>();
                List<string> friendBufferList = new List<string>();
                foreach (QuestionYourFriendsDataAccess.Question question in receiver)
                {
                    string textBuffer = "";
                    string friendBuffer = "";
                    textBuffer += question.text;
                    textBuffer += " ";

                    friendBuffer += SearchFriend(myFriends, question);

                    textBufferList.Add(textBuffer);
                    friendBufferList.Add(friendBuffer);
                }
                ViewData["questions"] = textBufferList;
                ViewData["friend"] = friendBufferList;
                ViewData["questionCount"] = textBufferList.Count;
            }
        }

        public ActionResult Index()
        {
            dynamic myAccount = Session["user"];
            dynamic myFriends = Session["friends"];

            if (myFriends == null && myAccount == null)
                return RedirectToAction("Index", "Home");

            // Récupérer le id du fid dans User
            QuestionYourFriendsDataAccess.User user = BusinessManagement.User.Get(long.Parse(myAccount.id));

            storeQuestionInView(myFriends, user);

            return View();
        }
    }
}