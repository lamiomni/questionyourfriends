using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Configuration;
using AutoPoco.Configuration.Providers;
using AutoPoco.Conventions;

namespace AutoPoco.Engine
{
    public class GenerationConfiguration : IGenerationConfiguration
    {
        private readonly IEngineConfiguration mConfiguration;
        private readonly IEngineConventionProvider mConventions;
        private readonly List<IObjectBuilder> mObjectBuilders = new List<IObjectBuilder>();

        public GenerationConfiguration(IEngineConfiguration configuration, IEngineConventionProvider conventionProvider,
                                       int recursionLimit)
        {
            mConfiguration = configuration;
            mConventions = conventionProvider;
            RecursionLimit = recursionLimit;
        }

        #region IGenerationConfiguration Members

        public int RecursionLimit { get; private set; }

        public IObjectBuilder GetBuilderForType(Type searchType)
        {
            IObjectBuilder builder = mObjectBuilders.Where(x => x.InnerType == searchType).SingleOrDefault();
            if (builder == null)
            {
                builder = CreateBuilderForType(searchType);
            }
            return builder;
        }

        #endregion

        private IObjectBuilder CreateBuilderForType(Type searchType)
        {
            EnsureTypeExists(searchType);
            IEngineConfigurationType type = mConfiguration.GetRegisteredType(searchType);
            var builder = new ObjectBuilder(type);
            mObjectBuilders.Add(builder);
            return builder;
        }

        private void EnsureTypeExists(Type searchType)
        {
            if (mConfiguration.GetRegisteredType(searchType) != null) return;

            var provider = new AdhocEngineConfigurationProvider(new[] {searchType});
            var coreConvention = new DefaultEngineConfigurationProviderLoadingConvention();
            coreConvention.Apply(new EngineConfigurationProviderLoaderContext(mConfiguration, provider, mConventions));
        }
    }
}