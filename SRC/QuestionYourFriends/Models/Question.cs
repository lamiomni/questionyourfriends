using System;
using System.Collections.Generic;

namespace QuestionYourFriends.Models
{
    /// <summary>
    /// Question management
    /// </summary>
    public static class Question
    {
        #region CRUD methods

        /// <summary>
        /// Adds a question
        /// </summary>
        /// <param name="question">Question to add</param>
        /// <returns>The id of the newly created question</returns>
        public static int Create(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Create(Context.QyfEntities, question);
        }

        /// <summary>
        /// Adds a question from given data
        /// </summary>
        /// <param name="idOwner">Owner id</param>
        /// <param name="idReceiver">Receiver id</param>
        /// <param name="text">Question content</param>
        /// <param name="anonPrice">Anonymization price</param>
        /// <param name="privatePrice">Privatization price</param>
        /// <param name="datePub">Publication date</param>
        /// <returns>The id of the newly created question</returns>
        public static int Create(int idOwner, int idReceiver, string text, int anonPrice, int privatePrice, DateTime datePub)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Create(Context.QyfEntities, idOwner, idReceiver, text, anonPrice, privatePrice, datePub);
        }

        /// <summary>
        /// Deletes a question thanks to its id
        /// </summary>
        /// <param name="id">Question id to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Delete(Context.QyfEntities, id);
        }

        /// <summary>
        /// Deletes a question
        /// </summary>
        /// <param name="question">Question to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Delete(Context.QyfEntities, question);
        }

        /// <summary>
        /// Updates a question
        /// </summary>
        /// <param name="question">Question to update</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsDataAccess.Question question)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Update(Context.QyfEntities, question);
        }

        /// <summary>
        /// Get a question thanks to its id
        /// </summary>
        /// <param name="id">Question id</param>
        /// <returns>The requested question</returns>
        public static QuestionYourFriendsDataAccess.Question Get(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.Get(Context.QyfEntities, id);
        }

        /// <summary>
        /// Get all the questions
        /// </summary>
        /// <returns>A list of questions</returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetList()
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetList(Context.QyfEntities);
        }

        #endregion

        #region More...

        /// <summary>
        /// Get questions where we are receivers
        /// </summary>
        /// <param name="id">Our id</param>
        /// <returns>A list of questions</returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetListOfReceiver(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetListOfReceiver(Context.QyfEntities, id);
        }

        /// <summary>
        /// Get questions where we are owners
        /// </summary>
        /// <param name="id">Our id</param>
        /// <returns>A list of questions</returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetListOfOwner(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetListOfOwner(Context.QyfEntities, id);
        }
        
        /// <summary>
        /// Get questions of friends
        /// </summary>
        /// <param name="friends">Friends uids</param>
        /// <returns>A list of questions</returns>
        public static  List<QuestionYourFriendsDataAccess.Question> GetFriendsQuestions(int[] friends)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Question.GetFriendsQuestions(Context.QyfEntities, friends);
        }

        #endregion
    }
}