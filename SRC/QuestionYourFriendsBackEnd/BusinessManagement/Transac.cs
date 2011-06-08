using System.Collections.Generic;
using System.Diagnostics;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriendsBackEnd.BusinessManagement
{
    /// <summary>
    /// Transaction management
    /// </summary>
    public static class Transac
    {
        #region CRUD methods

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="transac">Transaction to Add</param>
        /// <returns>The id of the created transaction</returns>
        public static int Create(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, transac);
        }

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="amount">Amount of the transaction</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="type">Type of the transaction</param>
        /// <param name="questionId">Id of the question</param>
        /// <returns>The id of the created question</returns>
        public static int Create(int amount, int userId, TransacType type, int? questionId)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, amount, userId,
                                                                                  type, questionId);
        }

        /// <summary>
        /// Deletes a transaction
        /// </summary>
        /// <param name="id">Id to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(int id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Delete(Context.QyfEntities, id);
        }

        /// <summary>
        /// Deletes a transaction
        /// </summary>
        /// <param name="transac">Transaction to delete</param>
        /// <returns>True if the deletion is ok</returns>
        public static bool Delete(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Delete(Context.QyfEntities, transac);
        }

        /// <summary>
        /// Updates a transaction
        /// </summary>
        /// <param name="transac">The transaction to updates</param>
        /// <returns>True if the update is ok</returns>
        public static bool Update(QuestionYourFriendsDataAccess.Transac transac)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Update(Context.QyfEntities, transac);
        }

        /// <summary>
        /// Get a transaction from its id
        /// </summary>
        /// <param name="id">Id of the wanted transaction</param>
        /// <returns>The wanted transaction</returns>
        public static QuestionYourFriendsDataAccess.Transac Get(long id)
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.Get(Context.QyfEntities, id);
        }
        
        /// <summary>
        /// Get all the transactions
        /// </summary>
        /// <returns>The list of all the transactions</returns>
        public static List<QuestionYourFriendsDataAccess.Transac> GetList()
        {
            return QuestionYourFriendsDataAccess.DataAccess.Transac.GetList(Context.QyfEntities);
        }

        #endregion

        #region Higher level methods do deal with the economy

        private static bool withMinValue = false;

        /// <summary>
        /// Anonymize a question
        /// </summary>
        /// <param name="question">Question to anonymize</param>
        /// <param name="user">Concerned user</param>
        /// <param name="bid">Bid value</param>
        /// <returns>True if the process is ok</returns>
        public static bool AnonymiseQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user,
            int bid)
        {
            // A bid can't be lower than a certain value
            if (withMinValue && bid < (int)TransacPrice.Anonymize)
                bid = (int) TransacPrice.Anonymize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
            {
                Debug.WriteLine("You are out of cash: " + user.credit_amount + " it costs: " + bid);
                return false;
            }

            // Creation of the transaction
            bool check = Create(bid, user.id, TransacType.Anonymize, question.id) != -1;

            if (check)
            {
                // Update Question's price
                question.anom_price = bid;

                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount -= bid;
                check &= User.Update(user);
            }
            return check;
        }

        /// <summary>
        /// Privatize a question
        /// </summary>
        /// <param name="question">Question to privatize</param>
        /// <param name="user">Concerned user</param>
        /// <param name="bid">Bid value</param>
        /// <returns>True if the process is ok</returns>
        public static bool PrivatizeQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user,
            int bid)
        {
            // A bid can't be lower than a certain value
            if (withMinValue && bid < (int)TransacPrice.Privatize)
                bid = (int)TransacPrice.Privatize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
            {
                Debug.WriteLine("You are out of cash: " + user.credit_amount + " it costs: " + bid);
                return false;
            }

            // Creation of the transaction
            bool check = Create(bid, question.id_owner, TransacType.Privatize, question.id) != -1;

            if (check)
            {
                // Update Question's price
                question.private_price = bid;

                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount -= bid;
                check &= User.Update(user);
            }
            return check;
        }

        /// <summary>
        /// Desanonymize a question
        /// </summary>
        /// <param name="question">Question to desanonymize</param>
        /// <param name="user">Concerned user</param>
        /// <returns>True if the process is ok</returns>
        public static bool DesanonymizeQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user)
        {
            var bid = question.anom_price;
            // A bid can't be lower than a certain value or not
            if (withMinValue && bid < (int)TransacPrice.Desanonymize)
                bid = (int) TransacPrice.Desanonymize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
            {
                Debug.WriteLine("You are out of cash: " + user.credit_amount + " it costs: " + bid);
                return false;
            }
                
            // Creation of the transaction
            bool check = Create(bid, user.id, TransacType.Desanonymize, question.id) != -1;

            if (check)
            {
                // Update Question's price
                question.anom_price = 0;

                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount -= bid;
                check &= User.Update(user);
            }
            return check;
        }

        /// <summary>
        /// Deprivatize a question
        /// </summary>
        /// <param name="question">Question to deprivatize</param>
        /// <param name="user">Concerned user</param>
        /// <returns>True if the process is ok</returns>
        public static bool DeprivatizeQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user)
        {
            var bid = question.private_price;
            // A bid can't be lower than a certain value
            if (withMinValue && bid < (int)TransacPrice.Deprivatize)
                bid = (int)TransacPrice.Deprivatize;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
            {
                Debug.WriteLine("You are out of cash: " + user.credit_amount + " it costs: " + bid);
                return false;
            }

            // Creation of the transaction
            bool check = Create(bid, user.id, TransacType.Deprivatize, question.id) != -1;

            if (check)
            {
                // Update Question's price
                question.private_price = 0;

                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount -= bid;
                check &= User.Update(user);
            }
            return check;
        }

        /// <summary>
        /// Purchase credit
        /// </summary>
        /// <param name="user">Concerned user</param>
        /// <param name="amount">Amount value</param>
        /// <returns>True if the process is ok</returns>
        public static bool Purchase(
            QuestionYourFriendsDataAccess.User user,
            int amount)
        {
            // Creation of the transaction
            bool check = Create(amount, user.id, TransacType.Purchase, 0) != -1;

            if (check)
            {
                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount += amount;
                check &= User.Update(user);
            }
            return check;
        }

        /// <summary>
        /// Get earning from the beginning
        /// </summary>
        /// <param name="user">Concerned user</param>
        /// <returns>True if the process is ok</returns>
        public static bool EarningStartup(
            QuestionYourFriendsDataAccess.User user)
        {
            // Creation of the transaction
            bool check = Create((int)TransacPrice.EarningStartup, user.id, TransacType.EarningStartup, 0) != -1;

            if (check)
            {
                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount += (int) TransacPrice.EarningStartup;
                check &= User.Update(user);
            }
            return check;
        }

        /// <summary>
        /// Get earning from an answer
        /// </summary>
        /// <param name="user">Concerned user</param>
        /// <returns>True if the process is ok</returns>
        public static bool EarningAnswer(
            QuestionYourFriendsDataAccess.User user)
        {
            // Creation of the transaction
            bool transCreateRes = Create((int)TransacPrice.EarningAnswer, user.id, TransacType.EarningAnswer, 0) != -1;
            bool userModifyRes = transCreateRes;

            if (userModifyRes)
            {
                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount += (int) TransacPrice.EarningAnswer;
                userModifyRes &= User.Update(user);
            }
            return userModifyRes;
        }

        /// <summary>
        /// Unprivatize and unanonymize a question
        /// </summary>
        /// <param name="question">Concerned question</param>
        /// <param name="user">Concerned user</param>
        /// <returns>True if the process is ok</returns>
        public static bool SpendAndQuestion(
            QuestionYourFriendsDataAccess.Question question,
            QuestionYourFriendsDataAccess.User user)
        {
            // Creation of the transaction
            bool transCreateRes = Create(question.anom_price, user.id, TransacType.Anonymize, question.id) != -1;
            bool transCreateRes2 = Create(question.private_price, user.id, TransacType.Privatize, question.id) != -1;

            bool check = transCreateRes && transCreateRes2;
            if (check)
            {
                // Update of the user's wallet
                // Todo: recalculate credit_amount from transactions
                user.credit_amount -= question.anom_price;
                user.credit_amount -= question.private_price;
                check &= User.Update(user);
            }
            return check;
        }

        #endregion
    }
}