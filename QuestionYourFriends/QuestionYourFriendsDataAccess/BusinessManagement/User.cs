using System.Collections.Generic;

namespace QuestionYourFriendsDataAccess.BusinessManagement
{
    public class User
    {
        public static bool CreateUser(QuestionYourFriendsDataAccess.User user)
        {
            return DataAccess.User.CreateUser(user);
        }

        public static bool DeleteUser(long id)
        {
            return DataAccess.User.DeleteUser(id);
        }

        public static bool UpdateUser(QuestionYourFriendsDataAccess.User user)
        {
            return DataAccess.User.UpdateUser(user);
        }

        public static QuestionYourFriendsDataAccess.User GetUser(long id)
        {
            return DataAccess.User.GetUser(id);
        }

        public static List<QuestionYourFriendsDataAccess.User> GetListUser()
        {
            return DataAccess.User.GetListUser();
        }

    }
}
