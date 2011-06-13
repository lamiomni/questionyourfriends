using System;
using AutoPoco.Engine;

namespace AutoPoco.Configuration
{
    public class DatasourceFactory : IEngineConfigurationDatasource
    {
        private readonly Type mDatasourceType;
        private Object[] mParams;

        public DatasourceFactory(Type t)
        {
            mDatasourceType = t;
        }

        #region IEngineConfigurationDatasource Members

        public IDatasource Build()
        {
            return Activator.CreateInstance(mDatasourceType, mParams) as IDatasource;
        }

        #endregion

        public void SetParams(params Object[] args)
        {
            mParams = args;
        }
    }
}