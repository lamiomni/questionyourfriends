using System;
using System.Collections.Generic;

namespace QuestionYourFriends.BusinessManagement
{
    public static class Question
    {
        public static bool Create(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Create(Context.QyfEntities, question);
        }

        public static bool Create(int idOwner, int idReceiver, string text, int anonPrice, int privatePrice, DateTime datePub)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Create(Context.QyfEntities, idOwner, idReceiver, text, anonPrice, privatePrice, datePub);
        }

        public static bool Delete(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Delete(Context.QyfEntities, id);
        }

        public static bool Delete(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Delete(Context.QyfEntities, question);
        }

        public static bool Update(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Update(Context.QyfEntities, question);
        }

        public static QuestionYourFriendsDataAccess.Question Get(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Get(Context.QyfEntities, id);
        }

        public static List<QuestionYourFriendsDataAccess.Question> GetList()
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetList(Context.QyfEntities);
        }

        public static List<QuestionYourFriendsDataAccess.Question> GetListOfReceiver(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetListOfReceiver(Context.QyfEntities, id);
        }
    }
}