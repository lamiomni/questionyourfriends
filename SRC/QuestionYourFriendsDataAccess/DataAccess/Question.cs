using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using log4net;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    /// <summary>
    /// Data Access to Questions
    /// </summary>
    public static class Question
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
                _logger.Error("Cannot create a new question", ex);
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
                _logger.Error("Cannot create a new question", ex);
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
                QuestionYourFriendsDataAccess.Question questionFound =
                    qyfEntities.Questions.Where(x => x.id == id).FirstOrDefault();
                if (questionFound != null)
                {
                    questionFound.deleted = true;
                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a question", ex);
                Debug.WriteLine(ex);
                throw new ApplicationException("Database error.");
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
                QuestionYourFriendsDataAccess.Question questionFound =
                    qyfEntities.Questions.Where(x => x.id == q.id).FirstOrDefault();
                if (questionFound != null)
                {
                    questionFound.deleted = true;
                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a question", ex);
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
                _logger.Error("Cannot update a question", ex);
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
                return  qyfEntities.Questions.Include("Owner").Include("Receiver").Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get a question", ex);
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
                return qyfEntities.Questions.Include("Owner").Include("Receiver").ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }

        #endregion

        #region More...

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
                var res = qyfEntities.Questions
                    .Include("Owner").Include("Receiver").
                    Where(x => x.id_receiver == id && x.deleted == false && x.undesirable == false)
                    .OrderBy(x => x.date_pub).ToList();
                res.Reverse();
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
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
        public static List<QuestionYourFriendsDataAccess.Question> GetFriendsQuestions(QuestionYourFriendsEntities qyfEntities, int[] friends)
        {
            try
            {
                var res = qyfEntities.Questions
                    .Include("Owner").Include("Receiver")
                    .Where(x => friends.Contains(x.id_receiver) && x.deleted == false && x.undesirable == false && x.private_price == 0)
                    .OrderBy(x => x.date_pub).ToList();
                res.Reverse();
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }

        /// <summary>
        /// Get the list of owners
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Owner id</param>
        /// <returns>List of questions where we are receiver</returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetListOfOwner(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                var res = qyfEntities.Questions
                    .Include("Owner").Include("Receiver")
                    .Where(x => x.id_owner == id && x.deleted == false && x.undesirable == false)
                    .OrderBy(x => x.date_pub).ToList();
                res.Reverse();
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.Question>();
            }
        }

        #endregion
    }
}