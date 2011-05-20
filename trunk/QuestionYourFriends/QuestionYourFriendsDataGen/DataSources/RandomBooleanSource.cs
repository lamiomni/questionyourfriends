using System;
using AutoPoco.Engine;

namespace QuestionYourFriendsDataGen.DataSources
{
    public class RandomBooleanSource : DatasourceBase<bool>
    {
        private readonly Random _random = new Random(1337);

        public override bool Next(IGenerationContext context)
        {            
            return _random.NextDouble() > 0.5f;
        }
    }
}
