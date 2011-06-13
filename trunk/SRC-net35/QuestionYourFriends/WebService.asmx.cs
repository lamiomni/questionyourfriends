using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Xml.Serialization;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriends
{
    /// <summary>
    /// Description résumée de WebService
    /// </summary>
    [WebService(Namespace = "http://lamiomni.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod]
        public string GetMyQuestionsSent(long fid)
        {
            User user = Models.User.Get(fid);
            StringWriter sw = new StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Question>));
            xmlSerializer.Serialize(sw, Models.Question.GetListOfOwner(user.id));
            return sw.ToString();
        }

        [WebMethod]
        public string GetMyQuestionsReceived(long fid)
        {
            User user = Models.User.Get(fid);
            StringWriter sw = new StringWriter();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Question>));
            xmlSerializer.Serialize(sw, Models.Question.GetListOfReceiver(user.id));
            return sw.ToString();
        }

        [WebMethod]
        public void AskQuestion(long fid, long ffid, int privateCost, int annonCost, string askedQuestion)
        {
            // Parameters checking
            if (string.IsNullOrEmpty(askedQuestion))
                throw new ApplicationException("Please formulate a question.");
            if (privateCost < 0
                || privateCost > 9999999
                || annonCost < 0
                || annonCost > 9999999)
                throw new ApplicationException("Please enter a valid number (> 0 and < 9,999,999) for the prices!");

            // Do work
            User me = Models.User.Get(fid);
            if (me.credit_amount > (annonCost + privateCost))
            {
                User friend = Models.User.Get(ffid);
                int qid = Models.Question.Create(me.id, friend.id, askedQuestion, annonCost, privateCost, System.DateTime.Now);
                Question q = Models.Question.Get(qid);
                Models.Transac.SpendAndQuestion(q, me);
            }
        }
    }
}
