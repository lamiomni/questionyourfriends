using System.Collections.Generic;

namespace QuestionYourFriends.BusinessManagement
{
    public static class User
    {
        #region CRUD methods

        public static bool Create(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Create(Context.QyfEntities, user);
        }

        public static bool Create(long fid)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Create(Context.QyfEntities, fid);
        }

        public static bool Delete(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Delete(Context.QyfEntities, id);
        }

        public static bool Delete(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Delete(Context.QyfEntities, user);
        }

        public static bool Update(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Update(Context.QyfEntities, user);
        }

        public static QuestionYourFriendsDataAccess.User Get(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.Get(Context.QyfEntities, id);
        }

        public static List<QuestionYourFriendsDataAccess.User> GetList()
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.GetList(Context.QyfEntities);
        }

        #endregion
    }
}