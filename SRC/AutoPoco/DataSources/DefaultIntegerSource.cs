using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class DefaultIntegerSource : DatasourceBase<int>
    {
        public override int Next(IGenerationContext context)
        {
            return 0;
        }
    }
}