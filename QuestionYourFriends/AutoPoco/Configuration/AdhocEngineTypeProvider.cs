using System;
using System.Collections.Generic;
using AutoPoco.Configuration.Providers;

namespace AutoPoco.Configuration
{
    public class AdhocEngineTypeProvider : IEngineConfigurationTypeProvider
    {
        private readonly Type mType;

        public AdhocEngineTypeProvider(Type type)
        {
            mType = type;
        }

        #region IEngineConfigurationTypeProvider Members

        public Type GetConfigurationType()
        {
            return mType;
        }

        public IEnumerable<IEngineConfigurationTypeMemberProvider> GetConfigurationMembers()
        {
            return new IEngineConfigurationTypeMemberProvider[] {};
        }

        public IEngineConfigurationDatasource GetFactory()
        {
            return null;
        }

        #endregion
    }
}