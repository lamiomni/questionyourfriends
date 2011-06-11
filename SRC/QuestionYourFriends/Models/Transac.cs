using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriends.Models
{
    /// <summary>
    /// Transaction management
    /// </summary>
    public static class Transac
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region CRUD methods

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="transac">Transaction to Add</param>
        /// <returns>The id of the created transaction</returns>
        public static int Create(QuestionYourFriendsDataAccess.Transac transac)
        {
            if (transac == null)
                return -1;
            var res = QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, transac);
            User.UpdateMoney(transac.userId);
            return res;

        }

        /// <summary>
        /// Adds a transaction
        /// </summary>
        /// <param name="amount">Amount of the transaction, negative if it needs to be paid</param>
        /// <param name="userId">Id of the user</param>
        /// <param name="type">Type of the transaction</param>
        /// <param name="questionId">Id of the question</param>
        /// <returns>The id of the created question</returns>
        public static int Create(int amount, int userId, TransacType type, int? questionId)
        {
            var res = QuestionYourFriendsDataAccess.DataAccess.Transac.Create(Context.QyfEntities, amount, userId,
                                                                              type, questionId);
            User.UpdateMoney(userId);
            return res;
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
            bool check = question != null && user != null;

            if (!check)
                return false;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
            {
                _logger.ErrorFormat("You are out of cash: {0} it costs: {1}", user.credit_amount, bid);
                throw new ApplicationException(string.Format("You run out of cash! It costs {1} credits but you have only {0} credit left!", user.credit_amount, bid));
            }

            // Creation of the transaction
            check = Create(-bid, user.id, TransacType.Anonymize, question.id) != -1;

            // Update Question's price
            if (check)
            {
                question.anom_price = bid;
                Question.Update(question);
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
            bool check = question != null && user != null;

            if (!check)
                return false;

            // Get the user and check his wallet
            if (user.credit_amount < bid)
            {
                _logger.ErrorFormat("You are out of cash: {0} it costs: {1}", user.credit_amount, bid);
                throw new ApplicationException(string.Format("You run out of cash! It costs {1} credits but you have only {0} credit left!", user.credit_amount, bid));
            }

            // Creation of the transaction
            check = Create(-bid, question.id_owner, TransacType.Privatize, question.id) != -1;

            // Update Question's price
            if (check)
            {
                question.anom_price = bid;
                Question.Update(question);
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
            bool check = question != null && user != null;

            if (!check)
                return false;

            // Get the user and check his wallet
            var bid = question.anom_price;
            if (user.credit_amount < bid)
            {
                _logger.ErrorFormat("You are out of cash: {0} it costs: {1}", user.credit_amount, bid);
                throw new ApplicationException(string.Format("You run out of cash! It costs {1} credits but you have only {0} credit left!", user.credit_amount, bid));
            }
                
            // Creation of the transaction
            check = Create(-bid, user.id, TransacType.Desanonymize, question.id) != -1;

            // Update Question's price
            if (check)
            {
                question.anom_price = bid;
                Question.Update(question);
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
            bool check = question != null && user != null;

            if (!check)
                return false;

            // Get the user and check his wallet
            var bid = question.private_price;
            if (user.credit_amount < bid)
            {
                _logger.ErrorFormat("You are out of cash: {0} it costs: {1}", user.credit_amount, bid);
                throw new ApplicationException(string.Format("You run out of cash! It costs {1} credits but you have only {0} credit left!", user.credit_amount, bid));
            }

            // Creation of the transaction
            check &= Create(-bid, user.id, TransacType.Deprivatize, question.id) != -1;

            // Update Question's price
            if (check)
            {
                question.anom_price = bid;
                Question.Update(question);
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
            return Create(amount, user.id, TransacType.Purchase, null) != -1;
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
            return Create(QyfData.EarningStartup, user.id, TransacType.EarningStartup, null) != -1;
        }


        /// <summary>
        /// Get earning 
        /// </summary>
        /// <param name="user">Concerned user</param>
        /// <param name="amount">Concerned Amount</param>
        /// <returns>True if the process is ok</returns>
        public static bool Earning(QuestionYourFriendsDataAccess.User user, int amount)
        {
            return Create(amount, user.id, TransacType.Purchase, null) != -1;
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
            return Create(QyfData.EarningAnswer, user.id, TransacType.EarningAnswer, null) != -1;
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
            int transacId = Create(-question.anom_price, user.id, TransacType.Anonymize, question.id);
            bool check = transacId != -1;
            if (check)
            {
                transacId = Create(-question.private_price, user.id, TransacType.Privatize, question.id);
                check &= transacId != -1;
            }
            else
            {
                var transac = Get(transacId);
                transac.SetTransacStatus(TransacStatus.Ko);
                Update(transac);
            }

            return check;
        }

        #endregion
    }
}