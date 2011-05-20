using System.Collections.Generic;

namespace QuestionYourFriends.BusinessManagement
{
    public static class Transac
    {
        public static bool CreateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.CreateTransac(Context.QyfEntities, transac);
        }

        public static bool DeleteTransac(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.DeleteTransac(Context.QyfEntities, id);
        }

        public static bool UpdateTransac(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.UpdateTransac(Context.QyfEntities, transac);
        }

        public static QuestionYourFriendsDataAccess.Transac GetTransac(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.GetTransac(Context.QyfEntities, id);
        }

        public static List<QuestionYourFriendsDataAccess.Transac> GetListTransac()
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.GetListTransac(Context.QyfEntities);
        }
    }
}
