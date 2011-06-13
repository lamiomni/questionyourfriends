using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
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
                _logger.InfoFormat("New question creation: owner({0}), receiver({1}), text({2}), anon({3}), priv({4}), datePub({5})",
                    idOwner, idReceiver, text, anonPrice, privatePrice, datePub);
                var question = new QuestionYourFriendsDataAccess.Question
                                   {
                                       OwnerReference =
                                           {
                                               EntityKey =
                                                   new EntityKey("QuestionYourFriendsEntities.Users", "id", idOwner)
                                           },
                                       ReceiverReference =
                                           {
                                               EntityKey =
                                                   new EntityKey("QuestionYourFriendsEntities.Users", "id", idReceiver)
                                           },
                                       text = text,
                                       answer = null,
                                       anom_price = anonPrice,
                                       private_price = privatePrice,
                                       undesirable = false,
                                       date_pub = datePub,
                                       date_answer = null,
                                       deleted = false
                                   };

                qyfEntities.AddToQuestions(question);
                qyfEntities.SaveChanges();
                _logger.InfoFormat("New question id: {0}", question.id);
                return question.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new question", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Adds a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="q">Question to add</param>
        /// <returns>The id of the created Question</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question q)
        {
            try
            {
                _logger.InfoFormat("New question creation: owner({0}), receiver({1}), text({2}), anon({3}), priv({4}), datePub({5})",
                    q.Owner.id, q.Receiver.id, q.text, q.anom_price, q.private_price, q.date_pub);
                qyfEntities.AddToQuestions(q);
                qyfEntities.SaveChanges();
                _logger.InfoFormat("New question id: {0}", q.id);
                return q.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new question", ex);
                throw new ApplicationException("A database error occured during the operation.");
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
                    _logger.InfoFormat("Question deletion: id({0})", id);
                    questionFound.deleted = true;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Context error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, questionFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a question", ex);
                throw new ApplicationException("A database error occured during the operation.");
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
                    _logger.InfoFormat("Question deletion: owner({0}), receiver({1}), text({2}), anon({3}), priv({4}), datePub({5})",
                           q.Owner.id, q.Receiver.id, q.text, q.anom_price, q.private_price, q.date_pub);
                    questionFound.deleted = true;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Context error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, questionFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a question", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Updates a question
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="q">Question to update</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Question q)
        {
            try
            {
                QuestionYourFriendsDataAccess.Question questionFound =
                    qyfEntities.Questions.Where(x => x.id == q.id).FirstOrDefault();
                if (questionFound != null)
                {
                    _logger.InfoFormat("Question update: owner({0}), receiver({1}), text({2}), anon({3}), priv({4}), datePub({5})",
                             q.Owner.id, q.Receiver.id, q.text, q.anom_price, q.private_price, q.date_pub);
                    questionFound.Owner = q.Owner;
                    questionFound.Receiver = q.Receiver;
                    questionFound.text = q.text;
                    questionFound.answer = q.answer;
                    questionFound.anom_price = q.anom_price;
                    questionFound.private_price = q.private_price;
                    questionFound.undesirable = q.undesirable;
                    questionFound.date_pub = q.date_pub;
                    questionFound.date_answer = q.date_answer;
                    questionFound.deleted = q.deleted;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        _logger.Error("Context error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, questionFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a question", ex);
                throw new ApplicationException("A database error occured during the operation.");
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
                _logger.InfoFormat("Get question: id({0})", id);
                return qyfEntities.Questions
                    .Include("Owner").Include("Receiver")
                    .Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get a question", ex);
                throw new ApplicationException("A database error occured during the operation.");
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
                _logger.Info("Get questions");
                return qyfEntities.Questions
                    .Include("Owner").Include("Receiver")
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                throw new ApplicationException("A database error occured during the operation.");
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
                _logger.InfoFormat("Get my questions: id({0})", id);
                var res = qyfEntities.Questions
                    .Include("Owner").Include("Receiver").
                    Where(x => x.Receiver.id == id && x.deleted == false && x.undesirable == false && x.date_pub < DateTime.Now)
                    .OrderBy(x => x.date_pub).ToList();
                res.Reverse();
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }
        
        /// <summary>
        /// Get the list of questions of your friends
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="friends">array of friends id</param>
        /// <returns></returns>
        public static List<QuestionYourFriendsDataAccess.Question> GetFriendsQuestions(QuestionYourFriendsEntities qyfEntities, IEnumerable<int> friends)
        {
            try
            {
                _logger.Info("Get friends questions");
                var questions = qyfEntities.Questions.Include("Owner").Include("Receiver").OrderBy(x => x.date_pub);
                var res = new List<QuestionYourFriendsDataAccess.Question>();
                foreach (var q in questions)
                {
                    if (friends.Contains(q.Receiver.id) && q.deleted == false && q.undesirable == false
                        && q.private_price == 0 && q.date_pub < DateTime.Now)
                        res.Add(q);
                }
                res.Reverse();
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                throw new ApplicationException("A database error occured during the operation.");
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
                _logger.InfoFormat("Get questions to me: id({0})", id);
                var res = qyfEntities.Questions
                    .Include("Owner").Include("Receiver")
                    .Where(x => x.Owner.id == id && x.deleted == false && x.undesirable == false && x.date_pub < DateTime.Now)
                    .OrderBy(x => x.date_pub).ToList();
                res.Reverse();
                return res;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get questions", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        #endregion
    }
}