using System.Collections.Generic;

namespace AutoPoco.Configuration
{
    public class EngineConfigurationTypeMember : IEngineConfigurationTypeMember
    {
        private readonly List<IEngineConfigurationDatasource> mDataSources = new List<IEngineConfigurationDatasource>();

        private readonly EngineTypeMember mMember;

        public EngineConfigurationTypeMember(EngineTypeMember member)
        {
            mMember = member;
        }

        #region IEngineConfigurationTypeMember Members

        public EngineTypeMember Member
        {
            get { return mMember; }
        }

        public void SetDatasource(IEngineConfigurationDatasource action)
        {
            mDataSources.Clear();
            mDataSources.Add(action);
        }

        public void SetDatasources(IEnumerable<IEngineConfigurationDatasource> sources)
        {
            mDataSources.Clear();
            mDataSources.AddRange(sources);
        }

        public IEnumerable<IEngineConfigurationDatasource> GetDatasources()
        {
            return mDataSources.ToArray();
        }

        #endregion
    }
}