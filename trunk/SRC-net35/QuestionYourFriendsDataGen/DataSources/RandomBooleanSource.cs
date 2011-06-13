using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    /// <summary>
    /// Random boolean source for AutoPoco
    /// </summary>
    public class RandomBooleanSource : DatasourceBase<bool>
    {
        private readonly Random _random = new Random(1337);

        /// <summary>
        /// Returns a random boolean
        /// </summary>
        /// <param name="context">AutoPoco context</param>
        /// <returns>A random boolean</returns>
        public override bool Next(IGenerationContext context)
        {
            return _random.NextDouble() > 0.5f;
        }
    }
}