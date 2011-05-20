using System.Collections.Generic;

namespace QuestionYourFriendsDataAccess.BusinessManagement
{
    public class Question
    {
        public static bool CreateQuestion(QuestionYourFriendsDataAccess.Question question)
        {
            return DataAccess.Question.CreateQuestion(question);
        }

        public static bool DeleteQuestion(long id)
        {
            return DataAccess.Question.DeleteQuestion(id);
        }

        public static bool UpdateQuestion(QuestionYourFriendsDataAccess.Question question)
        {
            return DataAccess.Question.UpdateQuestion(question);
        }

        public static QuestionYourFriendsDataAccess.Question GetQuestion(long id)
        {
            return DataAccess.Question.GetQuestion(id);
        }

        public static List<QuestionYourFriendsDataAccess.Question> GetListQuestion()
        {
            return DataAccess.Question.GetListQuestion();
        }
    }
}
