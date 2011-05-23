using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomIntegerSource : DatasourceBase<int>
    {
        private readonly int _max;
        private readonly Random _random = new Random(1337);

        public RandomIntegerSource() : this(100)
        {
        }

        public RandomIntegerSource(int max)
        {
            _max = max;
        }

        public override int Next(IGenerationContext context)
        {
            return _random.Next()%_max;
        }
    }
}