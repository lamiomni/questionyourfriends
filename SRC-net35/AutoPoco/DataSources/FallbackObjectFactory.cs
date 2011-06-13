using System;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class FallbackObjectFactory<T> : IDatasource<T>
    {
        #region IDatasource<T> Members

        public object Next(IGenerationContext context)
        {
            Type t = typeof (T);
            return Activator.CreateInstance(t);
        }

        #endregion
    }
}