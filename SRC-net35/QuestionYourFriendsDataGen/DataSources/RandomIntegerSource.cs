using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    /// <summary>
    /// Random integer source for AutoPoco
    /// </summary>
    public class RandomIntegerSource : DatasourceBase<int>
    {
        private readonly int _max;
        private readonly Random _random = new Random(1337);

        /// <summary>
        /// Constructor with 100
        /// </summary>
        public RandomIntegerSource() : this(100)
        {
        }

        /// <summary>
        /// Constructor with specified max
        /// </summary>
        /// <param name="max"></param>
        public RandomIntegerSource(int max)
        {
            _max = max;
        }

        /// <summary>
        /// Get next random int value
        /// </summary>
        /// <param name="context">AutoPoco context</param>
        /// <returns>A next random int value</returns>
        public override int Next(IGenerationContext context)
        {
            return _random.Next()%_max;
        }
    }
}