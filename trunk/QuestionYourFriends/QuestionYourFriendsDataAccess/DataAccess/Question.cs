using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    /// <summary>
    /// Data Access to Questions
    /// </summary>
    public static class Question
    {
        #region CRUD methods

        /// <summary>
        /// Adds a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="idOwner">User id of the owner</param>
        /// <param name="idReceiver">User id of the receiver</param>
        /// <param name="text">Question content</param>
        /// <param name="anonPrice">Price to anonymize</param>
        /// <param name="privatePrice">Price to privatize</param>
        /// <param name="datePub">Publication date</param>
        /// <returns>The id of the created Question</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, int idOwner, int idReceiver,
                                 string text, int anonPrice, int privatePrice, DateTime datePub)
        {
            try
            {
                QuestionYourFriendsDataAccess.Question question = qyfEntities.Questions.CreateObject();
                question.id_owner = idOwner;
                question.id_receiver = idReceiver;
                question.text = text;
                question.answer = null;
                question.anom_price = anonPrice;
                question.private_price = privatePrice;
                question.undesirable = false;
                question.date_pub = datePub;
                question.date_answer = null;
                question.deleted = false;
                qyfEntities.Questions.AddObject(question);
                qyfEntities.SaveChanges();
                return question.id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return -1;
            }
        }

        /// <summary>
        /// Adds a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="question">Question to add</param>
        /// <returns>The id of the created Question</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question question)
        {
            try
            {
                qyfEntities.Questions.AddObject(question);
                qyfEntities.SaveChanges();
                return question.id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return -1;
            }
        }

        /// <summary>
        /// Deletes a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Question's id to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                qyfEntities.DeleteObject(qyfEntities.Questions.Where(x => x.id == id).FirstOrDefault());
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="q">Question to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question q)
        {
            try
            {
                qyfEntities.DeleteObject(q);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Updates a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="question">Question to update</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question question)
        {
            try
            {
                QuestionYourFriendsDataAccess.Question questionFound =
                    qyfEntities.Questions.Where(x => x.id == question.id).FirstOrDefault();
                if (questionFound != null)
                {
                    questionFound.id_owner = question.id_owner;
                    questionFound.id_receiver = question.id_receiver;
                    questionFound.text = question.text;
                    questionFound.answer = question.answer;
                    questionFound.anom_price = question.anom_price;
                    questionFound.private_price = question.private_price;
                    questionFound.undesirable = question.undesirable;
                    questionFound.date_pub = question.date_pub;
                    questionFound.date_answer = question.date_answer;
                    questionFound.deleted = question.deleted;

                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets a question thanks to its id
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Question id</param>
        /// <returns>Requested question</returns>
        public static QuestionYourFriendsDataAccess.Question Get(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                return qyfEntities.Questions.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Get list of all questions
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <returns>List of questions</returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetList(QuestionYourFriendsEntities qyfEntities)
        {
            try
            {
                return qyfEntities.Questions.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }

        #endregion

        #region Moar...

        /// <summary>
        /// Get the list of receivers
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Receiver id</param>
        /// <returns>List of questions where we are receiver</returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetListOfReceiver(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                return qyfEntities.Questions.Where(x => x.id_receiver == id).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }
        
        /// <summary>
        /// Get the list of questions of your friends
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="friends">array of friends id</param>
        /// <returns></returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetFriendsQuestions(QuestionYourFriendsEntities qyfEntities, long[] friends)
        {
            try
            {
                return qyfEntities.Questions.Where(x => friends.Contains(x.id_receiver)).Where(x => x.private_price == 0).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }

        #endregion
    }
}