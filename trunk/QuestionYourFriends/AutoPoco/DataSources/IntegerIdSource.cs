using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class IntegerIdSource : DatasourceBase<int>
    {
        private int mCurrentId;

        public override int Next(IGenerationContext context)
        {
            return mCurrentId++;
        }
    }
}