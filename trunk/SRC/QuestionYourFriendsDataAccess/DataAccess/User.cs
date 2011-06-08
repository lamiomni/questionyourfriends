using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                QuestionYourFriendsDataAccess.User user = qyfEntities.Users.CreateObject();
                user.fid = fid;
                user.activated = true;
                user.credit_amount = 0;
                qyfEntities.Users.AddObject(user);
                qyfEntities.SaveChanges();
                return user.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new user", ex);
                Debug.WriteLine(ex);
                return -1;
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
                qyfEntities.Users.AddObject(user);
                qyfEntities.SaveChanges();
                return user.id;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot create a new user", ex);
                Debug.WriteLine(ex);
                return -1;
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
                    userFound.activated = true;
                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update an user", ex);
                Debug.WriteLine(ex);
                return false;
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
                    userFound.activated = true;
                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update an user", ex);
                Debug.WriteLine(ex);
                return false;
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
                    userFound.activated = true;
                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update an user", ex);
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Updates an user
        /// </summary>
        /// <param name="qyfEntities">Entity context</param>
        /// <param name="user">User to update</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsEntities qyfEntities, QuestionYourFriendsDataAccess.User user)
        {
            try
            {
                QuestionYourFriendsDataAccess.User userFound =
                    qyfEntities.Users.Where(x => x.id == user.id).FirstOrDefault();
                if (userFound != null)
                {
                    userFound.fid = user.fid;
                    userFound.credit_amount = user.credit_amount;
                    userFound.activated = user.activated;

                    qyfEntities.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot update an user", ex);
                Debug.WriteLine(ex);
                return false;
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
                return qyfEntities.Users.Where(x => x.id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get an user", ex);
                Debug.WriteLine(ex);
                return null;
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
                return qyfEntities.Users.Where(x => x.fid == fid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get an user", ex);
                Debug.WriteLine(ex);
                return null;
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
                return qyfEntities.Users.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get users", ex);
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.User>();
            }
        }
        #endregion

        #region More...

        public static List<QuestionYourFriendsDataAccess.User> GetUsersFromFids(QuestionYourFriendsEntities qyfEntities, long[] fids)
        {
            try
            {
                return qyfEntities.Users.Where(x => fids.Contains(x.fid)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error("Cannot get users", ex);
                Debug.WriteLine(ex);
                return new List<QuestionYourFriendsDataAccess.User>();
            }
        }

        #endregion
    }
}