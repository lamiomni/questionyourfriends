using System;
using AutoPoco.DataSources;
using AutoPoco.Engine;

namespace AutoPoco.Configuration
{
    public class TypeMemberConventionContext
    {
        private readonly IEngineConfiguration mConfiguration;
        private readonly IEngineConfigurationTypeMember mTypeMember;

        public TypeMemberConventionContext(IEngineConfiguration configuration, IEngineConfigurationTypeMember member)
        {
            mConfiguration = configuration;
            mTypeMember = member;
        }

        public IEngineConfiguration Configuration
        {
            get { return mConfiguration; }
        }

        public EngineTypeMember Member
        {
            get { return mTypeMember.Member; }
        }

        public void SetValue(object value)
        {
            var factory = new DatasourceFactory(typeof (ValueSource));
            factory.SetParams(value);
            mTypeMember.SetDatasource(factory);
        }

        public void SetSource<T>() where T : IDatasource
        {
            SetSource(typeof (T));
        }

        public void SetSource(Type t)
        {
            mTypeMember.SetDatasource(new DatasourceFactory(t));
        }
    }
}