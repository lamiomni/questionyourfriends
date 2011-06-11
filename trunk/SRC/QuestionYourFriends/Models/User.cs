using System.Collections.Generic;

namespace QuestionYourFriends.Models
{
    /// <summary>
    /// User management
    /// </summary>
    public static class User
    {
        #region CRUD methods

        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="user">User to add</param>
        /// <returns>Id of the created user</returns>
        public static int Create(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Create(Context.QyfEntities, user);
        }

        /// <summary>
        /// Adds an user
        /// </summary>
        /// <param name="fid">Facebook id of the user</param>
        /// <returns>Id of the created user</returns>
        public static int Create(long fid)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Create(Context.QyfEntities, fid);
        }

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Delete(Context.QyfEntities, id);
        }

        /// <summary>
        /// Deletes an user
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Delete(Context.QyfEntities, user);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User to delete</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Update(Context.QyfEntities, user);
        }

        /// <summary>
        /// Get an user from its facebook id
        /// </summary>
        /// <param name="fid">Facebook id</param>
        /// <returns>Requested user</returns>
        public static QuestionYourFriendsDataAccess.User Get(long fid)
        {
            var res = QuestionYourFriendsDataAccess.DataAccess.User.Get(Context.QyfEntities, fid);
            if (res == null)
            {
                Create(fid);
                res = Get(fid);
                Transac.EarningStartup(res);
            }
            return res;
        }

        /// <summary>
        /// Get an user from its id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Requested user</returns>
        public static QuestionYourFriendsDataAccess.User Get(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Get(Context.QyfEntities, id);
        }

        /// <summary>
        /// Get list of all the user
        /// </summary>
        /// <returns>List of all users</returns>
        public static List<QuestionYourFriendsDataAccess.User> GetList()
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.GetList(Context.QyfEntities);
        }

        #endregion

        #region More...

        /// <summary>
        /// Get users from their fid
        /// </summary>
        /// <param name="fids">List of fids</param>
        /// <returns>List of users</returns>
        public static IEnumerable<QuestionYourFriendsDataAccess.User> GetUsersFromFids(long[] fids)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.GetUsersFromFids(Context.QyfEntities, fids);
        }

        /// <summary>
        /// Update money of an user
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>True if the process is ok</returns>
        public static bool UpdateMoney(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.UpdateMoney(Context.QyfEntities, id);
        }

        #endregion
    }
}