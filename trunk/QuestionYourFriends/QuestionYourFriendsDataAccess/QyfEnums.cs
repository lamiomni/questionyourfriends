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
        EarningAnswer // Jr added this here and did nothing else, will it work ?
    }

    /// <summary>
    /// Transaction prices
    /// </summary>
    public enum TransacPrice
    {
        Anonymize = 1000, // min value
        Privatize = 5000, // min value
        Desanonymize = 1000, // min value
        Deprivatize = 5000, // min value
        Purchase = 0,
        EarningStartup = 10000,
        EarningAnswer = 2000,
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