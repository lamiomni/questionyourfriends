using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomEnumSource<T> : DatasourceBase<int>
    {
        private readonly Array _enums;
        private readonly Random _random = new Random(1337);

        public RandomEnumSource()
        {
            _enums = Enum.GetValues(typeof (T));
        }

        public override int Next(IGenerationContext context)
        {
            return (int) _enums.GetValue(_random.Next(_enums.Length));
        }
    }
}