using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class ValueSource : IDatasource
    {
        private readonly Object mValue;

        public ValueSource(Object value)
        {
            mValue = value;
        }

        #region IDatasource Members

        public object Next(IGenerationContext context)
        {
            return mValue;
        }

        #endregion
    }

    public class ValueSource<T> : ValueSource, IDatasource<T>
    {
        public ValueSource(Object value) : base(value)
        {
        }
    }
}