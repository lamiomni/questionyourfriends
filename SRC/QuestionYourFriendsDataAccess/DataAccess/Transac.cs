using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;

namespace QuestionYourFriendsDataAccess.DataAccess
{
    /// <summary>
    /// Data Access to Transactions
    /// </summary>
    public static class Transac
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region CRUD methods

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="amount">Amount of the transaction</param>
        /// <param name="userId">Id of the owner</param>
        /// <param name="type">Type of the transaction</param>
        /// <param name="questionId">Id of the question</param>
        /// <returns>The id of the created transaction</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, int amount, int userId, TransacType type,
                                 int? questionId)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transac = qyfEntities.Transacs.CreateObject();
                transac.amount = amount;
                transac.userId = userId;
                transac.SetTransacStatus(TransacStatus.Ok);
                transac.SetTransacType(type);
                transac.questionId = questionId;
                qyfEntities.Transacs.AddObject(transac);
                qyfEntities.SaveChanges();
                return transac.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new transac", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="transac">Transaction to add</param>
        /// <returns>Id of the created transaction</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                qyfEntities.Transacs.AddObject(transac);
                qyfEntities.SaveChanges();
                return transac.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new transac", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Deletes the transaction thanks to its id
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Transaction's id to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transacFound =
                    qyfEntities.Transacs.Where(x => x.id == id).FirstOrDefault();
                if (transacFound != null)
                {
                    transacFound.status = 0;
                    qyfEntities.SaveChanges();
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
        /// Deletes the transaction
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="t">Transaction to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac t)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transacFound =
                    qyfEntities.Transacs.Where(x => x.id == t.id).FirstOrDefault();
                if (transacFound != null)
                {
                    transacFound.status = 0;
                    qyfEntities.SaveChanges();
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
        /// Updates a transaction
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="transac">The transaction to update</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac transac)
        {
            try
            {
                QuestionYourFriendsDataAccess.Transac transacFound =
                    qyfEntities.Transacs.Where(x => x.id == transac.id).FirstOrDefault();
                if (transacFound != null)
                {
                    transacFound.amount = transac.amount;
                    transacFound.status = transac.status;
                    transacFound.userId = transac.userId;
                    transacFound.type = transac.type;
                    transacFound.questionId = transac.questionId;

                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a transac", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Gets a transaction thanks to its id
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">The id of the transaction</param>
        /// <returns>The requested transaction</returns>
        public static QuestionYourFriendsDataAccess.Transac Get(QuestionYourFriendsEntities qyfEntities, long id)
        {
            try
            {
                return qyfEntities.Transacs.Include("Question").Include("User")
                    .Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get a transac", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Gets the list of all Transactions
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <returns>The list with all the transactions</returns>
        public static List<QuestionYourFriendsDataAccess.Transac> GetList(QuestionYourFriendsEntities qyfEntities)
        {
            try
            {
                return qyfEntities.Transacs.Include("Question").Include("User").ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get transacs", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        #endregion
    }
}