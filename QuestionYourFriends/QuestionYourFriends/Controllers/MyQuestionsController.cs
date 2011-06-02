using System.Web.Mvc;

namespace QuestionYourFriends.Controllers
{
    public class MyQuestionsController : BaseController
    {
        //
        // GET: /MyQuestions/

        public ActionResult Index()
        {
            dynamic myFriends = Session["friend"];
            dynamic res = Session["user"];
            int n = 0;

            ViewData["Firstname"] = res.first_name;
            ViewData["Lastname"] = res.last_name;
            ViewData["Id"] = res.id;
            // Récupérer le id du fid dans User
            QuestionYourFriendsDataAccess.User user = BusinessManagement.User.Get(long.Parse(res.id));

            // Stocker dans la view la liste des questions dont le récepteur est l'utilisateur
            var receiver = BusinessManagement.Question.GetListOfReceiver(user.id);
            if (receiver != null)
            {
                foreach (QuestionYourFriendsDataAccess.Question a in receiver)
                {
                    QuestionYourFriendsDataAccess.User sender = BusinessManagement.User.Get(a.id_owner);
                    ViewData["question" + n] += a.text;
                    ViewData["question" + n] += " ";

                    // Chercher le nom et prénom de l'envoyeur en comparant les fids de la base de données et de Session["friend"]
                    if (myFriends.data != null)
                    {
                        foreach (dynamic friend in myFriends.data)
                        {
                            if (friend.id == sender.fid)
                            {
                                ViewData["question" + n] += friend.first_name;
                                ViewData["question" + n] += " ";
                                ViewData["question" + n] += friend.last_name;
                            }
                        }

                        ViewData["question" + n] += "\r\n";
                        n++;
                    }
                }
            }
            ViewData["questionCount"] = n + 1;

            return View();
        }
    }
}