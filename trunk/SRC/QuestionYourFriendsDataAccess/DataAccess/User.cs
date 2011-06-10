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
    /// Data Access to Users
    /// </summary>
    public static class User
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region CRUD methods

        /// <summary>
        /// Adds an user from its fid
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="fid">User's fid</param>
        /// <returns>The id of the created user</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, long fid)
        {
            try
            {
                _logger.InfoFormat("New user creation: fid({0})", fid);
                QuestionYourFriendsDataAccess.User user = qyfEntities.Users.CreateObject();
                user.fid = fid;
                user.activated = true;
                user.credit_amount = 0;
                qyfEntities.Users.AddObject(user);
                try
                {
                    qyfEntities.SaveChanges();
                }
                catch (OptimisticConcurrencyException e)
                {
                    _logger.Error("Concurrency error:", e);

                    // Resolve the concurrency conflict by refreshing the 
                    // object context before re-saving changes. 
                    qyfEntities.Refresh(RefreshMode.ClientWins, user);

                    // Save changes.
                    qyfEntities.SaveChanges();
                }
                _logger.InfoFormat("New user id: {0}", user.id);
                return user.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="user">User to add</param>
        /// <returns>The id of the created user</returns>
        public static int Create(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                _logger.InfoFormat("New user creation: fid({0})", user.fid);
                qyfEntities.Users.AddObject(user);
                try
                {
                    qyfEntities.SaveChanges();
                }
                catch (OptimisticConcurrencyException e)
                {
                    _logger.Error("Concurrency error:", e);

                    // Resolve the concurrency conflict by refreshing the 
                    // object context before re-saving changes. 
                    qyfEntities.Refresh(RefreshMode.ClientWins, user);

                    // Save changes.
                    qyfEntities.SaveChanges();
                }
                _logger.InfoFormat("New user id: {0}", user.id);
                return user.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Deletes an user thanks to its fid
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="fid">User's fid to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, long fid)
        {
            try
            {
                QuestionYourFriendsDataAccess.User userFound =
                    qyfEntities.Users.Where(x => x.fid == fid).FirstOrDefault();
                if (userFound != null)
                {
                    _logger.InfoFormat("User deletion: id ({0}), fid({1})", userFound.id, fid);
                    userFound.activated = false;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, userFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot delete an user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Deletes an user thanks to its id
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">User's id to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                QuestionYourFriendsDataAccess.User userFound =
                    qyfEntities.Users.Where(x => x.id == id).FirstOrDefault();
                if (userFound != null)
                {
                    _logger.InfoFormat("User deletion: id ({0}), fid({1})", id, userFound.fid);
                    userFound.activated = false;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, userFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot delete an user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="u">User to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User u)
        {
            try
            {
                QuestionYourFriendsDataAccess.User userFound =
                    qyfEntities.Users.Where(x => x.id == u.id).FirstOrDefault();
                if (userFound != null)
                {
                    _logger.InfoFormat("User deletion: id ({0}), fid({1})", userFound.id, userFound.fid);
                    userFound.activated = false;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, userFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot delete an user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Updates an user
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="u">User to update</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User u)
        {
            try
            {
                QuestionYourFriendsDataAccess.User userFound =
                    qyfEntities.Users.Where(x => x.id == u.id).FirstOrDefault();
                if (userFound != null)
                {
                    _logger.InfoFormat("User update: id({0}), fid({1}), credit({2}), activ({3})", u.id, u.fid, u.credit_amount, u.activated);
                    userFound.fid = u.fid;
                    userFound.credit_amount = u.credit_amount;
                    userFound.activated = u.activated;
                    try
                    {
                        qyfEntities.SaveChanges();
                    }
                    catch (OptimisticConcurrencyException e)
                    {
                        _logger.Error("Concurrency error:", e);

                        // Resolve the concurrency conflict by refreshing the 
                        // object context before re-saving changes. 
                        qyfEntities.Refresh(RefreshMode.ClientWins, userFound);

                        // Save changes.
                        qyfEntities.SaveChanges();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update an user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Gets an user from its id
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">User's id</param>
        /// <returns>The requested user</returns>
        public static QuestionYourFriendsDataAccess.User Get(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                _logger.InfoFormat("Get user: id({0})", id);
                return qyfEntities.Users.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get an user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Gets an user from its fid
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="fid">User's fid</param>
        /// <returns>The requested user</returns>
        public static QuestionYourFriendsDataAccess.User Get(QuestionYourFriendsEntities qyfEntities, long fid)
        {
            try
            {
                _logger.InfoFormat("Get user: fid({0})", fid);
                return qyfEntities.Users
                    .Include("QuestionsToMe").Include("MyQuestions").Include("Transacs")
                    .Where(x => x.fid == fid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get an user", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Get the list of all users
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <returns>The list of all users</returns>
        public static List<QuestionYourFriendsDataAccess.User> GetList(QuestionYourFriendsEntities qyfEntities)
        {
            try
            {
                _logger.Info("Get users");
                return qyfEntities.Users
                    .Include("QuestionsToMe").Include("MyQuestions").Include("Transacs")
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get users", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        #endregion

        #region More...

        /// <summary>
        /// Gets a list of users thanks to their fids
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="fids">List of fids</param>
        /// <returns>The requested list of users</returns>
        public static IEnumerable<QuestionYourFriendsDataAccess.User> GetUsersFromFids(QuestionYourFriendsEntities qyfEntities, IEnumerable<long> fids)
        {
            try
            {
                _logger.Info("Get users from fid");
                return qyfEntities.Users
                    .Include("QuestionsToMe").Include("MyQuestions").Include("Transacs")
                    .Where(x => fids.Contains(x.fid)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get users", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        /// <summary>
        /// Recalculate credit amount from the transactions
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="id">Id of the user to update</param>
        /// <returns>True if the process is ok</returns>
        public static bool UpdateMoney(QuestionYourFriendsEntities qyfEntities, int id)
        {
            try
            {
                _logger.InfoFormat("Update money of user: id({0})", id);
                var user = qyfEntities.Users.Where(u => u.id == id).FirstOrDefault();
                if (user == null)
                    return false;
                var transacs = qyfEntities.Transacs
                    .Where(t => t.userId == id && t.status != (int)TransacStatus.Ko);
                int sum = Enumerable.Sum(transacs, transac => transac.amount);
                user.credit_amount = sum;
                try
                {
                    qyfEntities.SaveChanges();
                }
                catch (OptimisticConcurrencyException e)
                {
                    _logger.Error("Concurrency error:", e);

                    // Resolve the concurrency conflict by refreshing the 
                    // object context before re-saving changes. 
                    qyfEntities.Refresh(RefreshMode.ClientWins, user);

                    // Save changes.
                    qyfEntities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update money", ex);
                throw new ApplicationException("A database error occured during the operation.");
            }
        }

        #endregion
    }
}