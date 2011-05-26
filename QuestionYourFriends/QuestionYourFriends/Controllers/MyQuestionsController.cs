using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class MyQuestionsController : BaseController
    {
        //
        // GET: /MyQuestions/

        public string GetMyQuestion(dynamic res)
        {
            string result = "";
            dynamic myFriends = Session["friend"];

            // Récupérer le id du fid dans User
            QuestionYourFriendsDataAccess.User user = BusinessManagement.User.Get(long.Parse(res.id));

            // Stocker dans la string result la liste des questions dont le récepteur est l'utilisateur
            foreach (QuestionYourFriendsDataAccess.Question a in BusinessManagement.Question.GetListOfReceiver(user.id))
            {
                QuestionYourFriendsDataAccess.User sender = BusinessManagement.User.Get(a.id_owner);
                result += a.text;
                result += " ";

                // Chercher le nom et prénom de l'envoyeur en comparant les fids de la base de données et de Session["friend"]
                foreach (dynamic friend in myFriends.data)
                {
                    if (friend.id == sender.fid)
                    {
                        result += friend.first_name;
                        result += " ";
                        result += friend.last_name;
                    }
                }

                result += "\r\n";
            }
            return result;
        }

        public ActionResult Index()
        {
            dynamic result = Session["user"];
            ViewData["Firstname"] = result.first_name;
            ViewData["Lastname"] = result.last_name;
            ViewData["message"] = GetMyQuestion(result);
            return View();
        }
    }
}