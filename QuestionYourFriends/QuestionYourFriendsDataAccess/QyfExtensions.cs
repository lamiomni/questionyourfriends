namespace QuestionYourFriendsDataAccess
{
    public static class QyfExtensions
    {
        public static TransacType GetTransacType(this Transac transac)
        {
            return ConvertToTransacType(transac.type);
        }

        public static void SetTransacType(this Transac transac, TransacType type)
        {
            transac.type = (int) type;
        }

        public static TransacStatus GetTransacStatus(this Transac transac)
        {
            return ConvertToTransacStatus(transac.status);
        }

        public static void SetTransacStatus(this Transac transac, TransacStatus status)
        {
            transac.status = (int) status;
        }

        public static TransacType ConvertToTransacType(int type)
        {
            switch (type)
            {
                case 0:
                    return TransacType.Anonymize;
                case 1:
                    return TransacType.Privatize;
                case 2:
                    return TransacType.Desanonymize;
                case 3:
                    return TransacType.Deprivatize;
                case 4:
                    return TransacType.Purchase;
                case 5:
                    return TransacType.EarningStartup;
                case 6:
                    return TransacType.EarningAnswer;
            }
            return TransacType.Anonymize;
        }

        public static TransacStatus ConvertToTransacStatus(int status)
        {
            switch (status)
            {
                case 0:
                    return TransacStatus.Ko;
                case 1:
                    return TransacStatus.Ok;
            }
            return TransacStatus.Ko;
        }
    }
}