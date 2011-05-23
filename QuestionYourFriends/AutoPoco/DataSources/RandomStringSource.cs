using System;
using System.Text;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class RandomStringSource : DatasourceBase<String>
    {
        private readonly int mMax;
        private readonly int mMin;
        private readonly Random mRandom = new Random(1337);

        public RandomStringSource(int min, int max)
        {
            mMin = min;
            mMax = max;
        }

        public override string Next(IGenerationContext context)
        {
            var builder = new StringBuilder();
            int length = mRandom.Next(mMin, mMax + 1);
            for (int x = 0; x < length; x++)
            {
                int value = mRandom.Next(65, 123);
                builder.Append((char) value);
            }
            return builder.ToString();
        }
    }
}