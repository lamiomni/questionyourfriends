namespace QuestionYourFriendsDataAccess
{
    public enum TransacType
    {
        Anonymize = 0,
        Privatize,
        Desanonymize,
        Deprivatize,
        Purchase,
        Earning, // should be changed to EarningStartup ?
        EarningAnswer // Jr added this here and did nothing else, will it work ?
    }

    public enum TransacPrice
    {
        Anonymize = 1000, // min value
        Privatize = 5000, // min value
        Desanonymize = 1000, // min value
        Deprivatize = 5000, // min value
        Purchase = 0,
        EarningStartup = 10000,
        EarningAnswer = 2000
    }

    public enum TransacStatus
    {
        Ko = 0,
        Ok
    }

}
