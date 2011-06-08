using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    /// <summary>
    /// Random enum source for AutoPoco
    /// </summary>
    /// <typeparam name="T">Enum type</typeparam>
    public class RandomEnumSource<T> : DatasourceBase<int>
    {
        private readonly Array _enums;
        private readonly Random _random = new Random(1337);

        /// <summary>
        /// Constructor, gets enum values from type
        /// </summary>
        public RandomEnumSource()
        {
            _enums = Enum.GetValues(typeof (T));
        }

        /// <summary>
        /// Gets a random value from the previously given enum type
        /// </summary>
        /// <param name="context">AutoPoco context</param>
        /// <returns>Gets a random value from the previously given enum type</returns>
        public override int Next(IGenerationContext context)
        {
            return (int) _enums.GetValue(_random.Next(_enums.Length));
        }
    }
}