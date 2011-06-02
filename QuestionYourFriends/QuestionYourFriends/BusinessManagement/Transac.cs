using System.Collections.Generic;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriends.BusinessManagement
{
    public static class Transac
    {
        #region CRUD methods

        public static int Create(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, transac);
        }

        public static int Create(int amount, int userId, TransacType type, int questionId)
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

        #endregion

        #region Higher level methods do deal with the economy

        public static bool AnonymiseQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user,
            int bid)
        {
            // A bid can't be lower than a certain value
            if (bid < (int) TransacPrice.Anonymize)
                bid = (int) TransacPrice.Anonymize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
                return false;

            // Creation of the transaction
            bool transCreateRes = Create(bid, user.id, TransacType.Anonymize, question.id) != -1;

            // Update Question's price
            question.anom_price = bid;
            
            // Update of the user's wallet
            user.credit_amount -= bid;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }

        public static bool PrivatizeQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user,
            int bid)
        {
            // A bid can't be lower than a certain value
            if (bid < (int)TransacPrice.Privatize)
                bid = (int)TransacPrice.Privatize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
                return false;

            // Creation of the transaction
            bool transCreateRes = Create(bid, question.id_owner, TransacType.Privatize, question.id) != -1;

            // Update Question's price
            question.private_price = bid;

            // Update of the user's wallet
            user.credit_amount -= bid;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }

        public static bool DesanonymizeQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user)
        {
            var bid = question.anom_price;
            // A bid can't be lower than a certain value
            if (bid < (int) TransacPrice.Desanonymize)
                bid = (int) TransacPrice.Desanonymize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
                return false;

            // Creation of the transaction
            bool transCreateRes = Create(bid, user.id, TransacType.Desanonymize, question.id) != -1;

            // Update Question's price
            question.anom_price = 0;

            // Update of the user's wallet
            user.credit_amount -= bid;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }

        public static bool DeprivatizeQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user)
        {
            var bid = question.private_price;
            // A bid can't be lower than a certain value
            if (bid < (int)TransacPrice.Deprivatize)
                bid = (int)TransacPrice.Deprivatize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
                return false;

            // Creation of the transaction
            bool transCreateRes = Create(bid, user.id, TransacType.Deprivatize, question.id) != -1;

            // Update Question's price
            question.private_price = 0;

            // Update of the user's wallet
            user.credit_amount -= bid;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }

        public static bool Purchase(
            QuestionYourFriendsDataAccess.User user,
            int amount)
        {
            // Creation of the transaction
            bool transCreateRes = Create(amount, user.id, TransacType.Purchase, 0) != -1;

            // Update of the user's wallet
            user.credit_amount += amount;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }

        public static bool EarningStartup(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user)
        {
            // Creation of the transaction
            bool transCreateRes = Create((int)TransacPrice.EarningStartup, user.id, TransacType.EarningStartup, 0) != -1;

            // Update of the user's wallet
            user.credit_amount += (int)TransacPrice.EarningStartup;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }

        public static bool EarningAnswer(
            QuestionYourFriendsDataAccess.User user)
        {
            // Creation of the transaction
            bool transCreateRes = Create((int)TransacPrice.EarningAnswer, user.id, TransacType.EarningAnswer, 0) != -1;

            // Update of the user's wallet
            user.credit_amount += (int)TransacPrice.EarningAnswer;
            bool userModifyRes = User.Update(user);

            return transCreateRes && userModifyRes;
        }


        #endregion
    }
}