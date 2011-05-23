using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomLongSource : DatasourceBase<long>
    {
        private readonly Random _random = new Random(1337);

        public override long Next(IGenerationContext context)
        {
            return _random.Next();
        }
    }
}