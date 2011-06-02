using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    /// <summary>
    /// Random long source for AutoPoco
    /// </summary>
    public class RandomLongSource : DatasourceBase<long>
    {
        private readonly Random _random = new Random(1337);

        /// <summary>
        /// Get next random long value
        /// </summary>
        /// <param name="context">AutoPoco context</param>
        /// <returns>A next random int value</returns>
        public override long Next(IGenerationContext context)
        {
            return _random.Next();
        }
    }
}