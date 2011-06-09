namespace QuestionYourFriendsDataAccess
{
    /// <summary>
    /// Transaction types
    /// </summary>
    public enum TransacType
    {
        Anonymize = 0,
        Privatize,
        Desanonymize,
        Deprivatize,
        Purchase,
        EarningStartup,
        EarningAnswer
    }

    /// <summary>
    /// Transaction statuses
    /// </summary>
    public enum TransacStatus
    {
        Ko = 0,
        Ok
    }
}