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
                _logger.InfoFormat("New transaction creation: amount({0}), userId({1}), type({2})", amount, userId, type);
                QuestionYourFriendsDataAccess.Transac transac = qyfEntities.Transacs.CreateObject();
                transac.amount = amount;
                transac.userId = userId;
                transac.SetTransacStatus(TransacStatus.Ok);
                transac.SetTransacType(type);
                transac.questionId = questionId;
                qyfEntities.Transacs.AddObject(transac);
                try
                {
                    qyfEntities.SaveChanges();
                }
                catch (OptimisticConcurrencyException e)
                {
                    _logger.Error("Concurrency error:", e);

                    // Resolve the concurrency conflict by refreshing the 
                    // object context before re-saving changes. 
                    qyfEntities.Refresh(RefreshMode.ClientWins, transac);

                    // Save changes.
                    qyfEntities.SaveChanges();
                }
                _logger.InfoFormat("New transaction id: {0}", transac.id);
                return transac.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new transaction", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="t">Transaction to add</param>
        /// <returns>Id of the created transaction</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.Transac t)
        {
            try
            {
                _logger.InfoFormat("New transaction creation: amount({0}), userId({1}), type({2})", t.amount, t.userId, t.type);
                qyfEntities.Transacs.AddObject(t);
                try
                {
                    qyfEntities.SaveChanges();
                }
                catch (OptimisticConcurrencyException e)
                {
                    _logger.Error("Concurrency error:", e);

                    // Resolve the concurrency conflict by refreshing the 
                    // object context before re-saving changes. 
                    qyfEntities.Refresh(RefreshMode.ClientWins, t);

                    // Save changes.
                    qyfEntities.SaveChanges();
                }
                _logger.InfoFormat("New transaction id: {0}", t.id);
                return t.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new transaction", ex);
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
                    _logger.InfoFormat("Transaction deletion: amount({0}), userId({1}), type({2})",
                        transacFound.amount, transacFound.userId, transacFound.type);
                    transacFound.status = 0;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, transacFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a transaction", ex);
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
                    _logger.InfoFormat("Transaction deletion: amount({0}), userId({1}), type({2})",
                        transacFound.amount, transacFound.userId, transacFound.type);
                    transacFound.status = 0;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, transacFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a transaction", ex);
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
                    _logger.InfoFormat("Transaction update: amount({0}), userId({1}), type({2})",
                        transacFound.amount, transacFound.userId, transacFound.type);
                    transacFound.amount = transac.amount;
                    transacFound.status = transac.status;
                    transacFound.userId = transac.userId;
                    transacFound.type = transac.type;
                    transacFound.questionId = transac.questionId;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, transacFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update a transaction", ex);
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
                _logger.InfoFormat("Get transaction: id({0})", id);
                return qyfEntities.Transacs.Include("Question").Include("User")
                    .Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get a transaction", ex);
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
                _logger.Info("Get transactions");
                return qyfEntities.Transacs.Include("Question").Include("User").ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get transactions", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        #endregion
    }
}