using System;
using System.Collections.Generic;
using AutoPoco.Configuration.Providers;

namespace AutoPoco.Configuration
{
    public class EngineConfigurationBuilder : IEngineConfigurationBuilder, IEngineConfigurationProvider
    {
        private readonly EngineConventionConfiguration mConventions = new EngineConventionConfiguration();
        private readonly List<IEngineConfigurationTypeProvider> mTypes = new List<IEngineConfigurationTypeProvider>();

        public IEngineConventionProvider ConventionProvider
        {
            get { return mConventions; }
        }

        #region IEngineConfigurationBuilder Members

        public IEngineConfigurationTypeBuilder<T> Include<T>()
        {
            // Create the configuration
            var configuration = new EngineConfigurationTypeBuilder<T>();

            // Store it locally
            mTypes.Add(configuration);

            //And return the public interface
            return configuration;
        }

        public IEngineConfigurationTypeBuilder Include(Type t)
        {
            // Create the configuration
            var configuration = new EngineConfigurationTypeBuilder(t);

            // Store it locally
            mTypes.Add(configuration);

            //And return the public interface
            return configuration;
        }

        public void Conventions(Action<IEngineConventionConfiguration> config)
        {
            config.Invoke(mConventions);
        }

        public void RegisterTypeProvider(IEngineConfigurationTypeProvider provider)
        {
            mTypes.Add(provider);
        }

        #endregion

        #region IEngineConfigurationProvider Members

        public IEnumerable<IEngineConfigurationTypeProvider> GetConfigurationTypes()
        {
            return mTypes;
        }

        #endregion
    }
}