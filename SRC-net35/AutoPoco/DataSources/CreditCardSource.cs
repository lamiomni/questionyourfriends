using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class CreditCardSource : DatasourceBase<string>
    {
        #region CreditCardType enum

        public enum CreditCardType
        {
            Random = 0,
            MasterCard = 1,
            Visa = 2,
            AmericanExpress = 3,
            Discover = 4
        }

        #endregion

        private readonly CreditCardType mPreferred;
        private readonly Random mRandom;

        public CreditCardSource()
            : this(CreditCardType.Random)
        {
        }

        public CreditCardSource(CreditCardType preferred)
        {
            mPreferred = preferred;
            mRandom = new Random(1337);
        }

        public override string Next(IGenerationContext context)
        {
            CreditCardType cardType = mPreferred;

            if (mPreferred == CreditCardType.Random)
                cardType = (CreditCardType) mRandom.Next(1, 4);

            switch (cardType)
            {
                case CreditCardType.AmericanExpress:
                    return "3782 822463 10005";
                case CreditCardType.Discover:
                    return "6011 1111 1111 1117";
                case CreditCardType.MasterCard:
                    return "5105 1051 0510 5100";
                case CreditCardType.Visa:
                    return "4111 1111 1111 1111";
                default:
                    return null;
            }
        }
    }
}