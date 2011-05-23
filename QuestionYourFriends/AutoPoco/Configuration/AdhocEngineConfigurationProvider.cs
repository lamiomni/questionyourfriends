using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Configuration.Providers;

namespace AutoPoco.Configuration
{
    public class AdhocEngineConfigurationProvider : IEngineConfigurationProvider
    {
        private readonly IEnumerable<IEngineConfigurationTypeProvider> mTypes;

        public AdhocEngineConfigurationProvider(IEnumerable<Type> types)
        {
            mTypes = types.Select(x => new AdhocEngineTypeProvider(x)).ToArray();
        }

        #region IEngineConfigurationProvider Members

        public IEnumerable<IEngineConfigurationTypeProvider> GetConfigurationTypes()
        {
            return mTypes;
        }

        #endregion
    }
}