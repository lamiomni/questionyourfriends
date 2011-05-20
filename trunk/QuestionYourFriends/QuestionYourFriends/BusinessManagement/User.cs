using System.Collections.Generic;

namespace QuestionYourFriends.BusinessManagement
{
    public static class User
    {
        public static bool CreateUser(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.CreateUser(Context.QyfEntities, user);
        }

        public static bool DeleteUser(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.DeleteUser(Context.QyfEntities, id);
        }

        public static bool UpdateUser(QuestionYourFriendsDataAccess.User user)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.UpdateUser(Context.QyfEntities, user);
        }

        public static QuestionYourFriendsDataAccess.User GetUser(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.GetUser(Context.QyfEntities, id);
        }

        public static List<QuestionYourFriendsDataAccess.User> GetListUser()
        {
            return QuestionYourFriendsDataAccess.DataAccess.User.GetListUser(Context.QyfEntities);
        }

    }
}
