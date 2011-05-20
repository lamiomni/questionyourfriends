using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomEnumSource<T> : DatasourceBase<string>
    {
        private readonly Random _random = new Random(1337);
        private readonly Array _enums;

        public RandomEnumSource()
        {
            _enums = Enum.GetValues(typeof(T));
        }

        public override string Next(IGenerationContext context)
        {
            return _enums.GetValue(_random.Next(_enums.Length)).ToString();
        }
    }
}
