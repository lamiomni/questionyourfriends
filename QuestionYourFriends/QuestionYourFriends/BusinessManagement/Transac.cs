using System.Collections.Generic;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriends.BusinessManagement
{
    public static class Transac
    {
        public static bool Create(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, transac);
        }

        public static bool Create(int amount, int userId, TransacType type, int questionId)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, amount, userId,
                                                                                  type, questionId);
        }

        public static bool Delete(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Delete(Context.QyfEntities, id);
        }

        public static bool Delete(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Delete(Context.QyfEntities, transac);
        }

        public static bool Update(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Update(Context.QyfEntities, transac);
        }

        public static QuestionYourFriendsDataAccess.Transac Get(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Get(Context.QyfEntities, id);
        }

        public static List<QuestionYourFriendsDataAccess.Transac> GetList()
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.GetList(Context.QyfEntities);
        }

        /// <summary>
        /// Higher level methods do deal with the economy
        /// </summary>
        /// <param name="question">Question to anonymize</param>
        /// <param name="bid">Bid value</param>
        /// <returns>Successfulness of the operation</returns>
        public static bool AnonymiseQuestion(QuestionYourFriendsDataAccess.Question question, int bid)
        {
            // A bid can't be lower than a certain value
            if (bid < (int) TransacPrice.Anonymize)
                bid = (int) TransacPrice.Anonymize;

            // Get the user and check his wallet
            QuestionYourFriendsDataAccess.User user = User.Get(question.id_owner);
            if (user.credit_amount < bid)
                return false;

            // Creation of the transaction
            bool transCreateRes = Create(bid, question.id_owner, TransacType.Anonymize, question.id);

            // Update of the user's wallet
            user.credit_amount -= bid;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }
    }
}