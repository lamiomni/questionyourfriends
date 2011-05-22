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

        /*
         *  Higher level methods do deal with the economy 
         * 
         */
        public static bool AnonymiseQuestion(int qid, int bid, int uid)
        {
            // a bid can't be lower than a certain value
            if (bid < (int) QuestionYourFriendsDataAccess.TransacPrice.Anonymize)
                bid = (int) QuestionYourFriendsDataAccess.TransacPrice.Anonymize;

            // get the user and check his wallet
            var user = BusinessManagement.User.GetUser(uid);
            if (user.credit_amount < bid)
                return false;

            // creation of the transaction
            var trans = new QuestionYourFriendsDataAccess.Transac
                            {
                                amount = bid,
                                questionId = qid,
                                userId = uid,
                                type = (int) QuestionYourFriendsDataAccess.TransacType.Anonymize,
                                status = (int) QuestionYourFriendsDataAccess.TransacStatus.Ok
                            };
            var transCreateRes = CreateTransac(trans);
           
            // update of the user's wallet
            user.credit_amount -= bid;
            var userModifyRes = BusinessManagement.User.UpdateUser(user);

            return transCreateRes && userModifyRes;
        }
    }
}
