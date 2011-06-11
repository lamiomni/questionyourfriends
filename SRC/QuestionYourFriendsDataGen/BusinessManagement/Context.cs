using System.Data.Objects;
using System.Reflection;
using log4net;
using QuestionYourFriendsDataAccess;

namespace QuestionYourFriendsDataGen.BusinessManagement
{
    /// <summary>
    /// EntityContext
    /// </summary>
    public static class Context
    {
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Static instance of our Entity context
        /// </summary>
        public static readonly QuestionYourFriendsEntities QyfEntities;

        /// <summary>
        /// Static constructor
        /// </summary>
        static Context()
        {
            QyfEntities = new QuestionYourFriendsEntities();
            QyfEntities.Users.MergeOption = MergeOption.OverwriteChanges;
            QyfEntities.Transacs.MergeOption = MergeOption.OverwriteChanges;
            QyfEntities.Questions.MergeOption = MergeOption.OverwriteChanges;
            _logger.Info("Context initialized.");
        }
    }
}