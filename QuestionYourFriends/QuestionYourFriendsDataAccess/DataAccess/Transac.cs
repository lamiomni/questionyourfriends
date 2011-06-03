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
                return -1;
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
                return -1;
            }
        }

        /// <summary>
        /// Deletes the transaction thanks to its id
        /// Todo: Please soft delete it
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Transaction's id to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                qyfEntities.DeleteObject(qyfEntities.Transacs.Where(x => x.id == id).FirstOrDefault());
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot delete a transac", ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes the transaction
        /// Todo: Please soft delete it
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="t">Transaction to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac t)
        {
            try
            {
                qyfEntities.DeleteObject(t);
                qyfEntities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot delete a transac", ex);
                return false;
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
                return false;
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
                return qyfEntities.Transacs.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get a transac", ex);
                return null;
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
                return qyfEntities.Transacs.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get transacs", ex);
                return new List<QuestionYourFriendsDataAccess.Transac>();
            }
        }

        #endregion
    }
}