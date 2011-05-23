﻿using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class DefaultSource<T> : DatasourceBase<T>
    {
        public override T Next(IGenerationContext context)
        {
            return default(T);
        }
    }
}