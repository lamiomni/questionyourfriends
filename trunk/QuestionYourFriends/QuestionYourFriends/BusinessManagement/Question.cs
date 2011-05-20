using System.Collections.Generic;

namespace QuestionYourFriends.BusinessManagement
{
    public static class Question
    {
        public static bool CreateQuestion(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.CreateQuestion(Context.QyfEntities, question);
        }

        public static bool DeleteQuestion(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.DeleteQuestion(Context.QyfEntities, id);
        }

        public static bool UpdateQuestion(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.UpdateQuestion(Context.QyfEntities, question);
        }

        public static QuestionYourFriendsDataAccess.Question GetQuestion(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetQuestion(Context.QyfEntities, id);
        }

        public static List<QuestionYourFriendsDataAccess.Question> GetListQuestion()
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetListQuestion(Context.QyfEntities);
        }
    }
}
