namespace QuestionYourFriendsDataAccess
{
    /// <summary>
    /// Extensions for Transactions about enums
    /// </summary>
    public static class QyfExtensions
    {
        /// <summary>
        /// Get the transaction type
        /// </summary>
        public static TransacType GetTransacType(this Transac transac)
        {
            return ConvertToTransacType(transac.type);
        }

        /// <summary>
        /// Set the transaction type
        /// </summary>
        /// <param name="type">Type to set</param>
        public static void SetTransacType(this Transac transac, TransacType type)
        {
            transac.type = (int) type;
        }

        /// <summary>
        /// Get the transaction status
        /// </summary>
        public static TransacStatus GetTransacStatus(this Transac transac)
        {
            return ConvertToTransacStatus(transac.status);
        }

        /// <summary>
        /// Set the transaction status
        /// </summary>
        /// <param name="status">Status to set</param>
        public static void SetTransacStatus(this Transac transac, TransacStatus status)
        {
            transac.status = (int) status;
        }

        /// <summary>
        /// Convertion helper from int to TransacType
        /// </summary>
        /// <param name="type">Integer to convert</param>
        /// <returns>The associated TransacType</returns>
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

        /// <summary>
        /// Convertion helper from int to TransacStatus
        /// </summary>
        /// <param name="status">Integer to convert</param>
        /// <returns>The associated TransacStatus</returns>
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