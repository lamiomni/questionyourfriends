using AutoPoco.Configuration.Providers;

namespace AutoPoco.Configuration
{
    public class EngineConfigurationProviderLoaderContext : IEngineConfigurationProviderLoaderContext
    {
        public EngineConfigurationProviderLoaderContext(
            IEngineConfiguration configuration,
            IEngineConfigurationProvider configurationProvider,
            IEngineConventionProvider conventionProvider)
        {
            Configuration = configuration;
            ConfigurationProvider = configurationProvider;
            ConventionProvider = conventionProvider;
        }

        #region IEngineConfigurationProviderLoaderContext Members

        public IEngineConfiguration Configuration { get; private set; }

        public IEngineConfigurationProvider ConfigurationProvider { get; private set; }

        public IEngineConventionProvider ConventionProvider { get; private set; }

        #endregion
    }
}