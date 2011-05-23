using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class EmailAddressSource : DatasourceBase<String>
    {
        private int mIndex;

        public override string Next(IGenerationContext context)
        {
            return string.Format("{0}@example.com", mIndex++);
        }
    }
}