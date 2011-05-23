using AutoPoco.Configuration.Providers;
using AutoPoco.Conventions;

namespace AutoPoco.Configuration
{
    public class EngineConfigurationFactory : IEngineConfigurationFactory
    {
        #region IEngineConfigurationFactory Members

        public virtual IEngineConfiguration Create(IEngineConfigurationProvider configurationProvider,
                                                   IEngineConventionProvider conventionProvider)
        {
            var configuration = new EngineConfiguration();
            var coreConvention = new DefaultEngineConfigurationProviderLoadingConvention();
            coreConvention.Apply(new EngineConfigurationProviderLoaderContext(configuration, configurationProvider,
                                                                              conventionProvider));
            return configuration;
        }

        #endregion
    }
}