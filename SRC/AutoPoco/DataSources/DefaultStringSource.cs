using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class DefaultStringSource : DatasourceBase<String>
    {
        public override string Next(IGenerationContext context)
        {
            return string.Empty;
        }
    }
}