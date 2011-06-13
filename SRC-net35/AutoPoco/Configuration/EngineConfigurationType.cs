using System;
using System.Collections.Generic;

namespace AutoPoco.Configuration
{
    public class EngineConfigurationType : IEngineConfigurationType
    {
        private readonly List<EngineConfigurationTypeMember> mRegisteredMembers =
            new List<EngineConfigurationTypeMember>();

        private readonly Type mRegisteredType;
        private IEngineConfigurationDatasource mFactory;

        public EngineConfigurationType(Type t)
        {
            mRegisteredType = t;
        }

        #region IEngineConfigurationType Members

        public Type RegisteredType
        {
            get { return mRegisteredType; }
        }

        public void RegisterMember(EngineTypeMember member)
        {
            if (mRegisteredMembers.Find(x => x.Member == member) != null)
            {
                throw new ArgumentException("Member has already been registered", "member");
            }
            mRegisteredMembers.Add(new EngineConfigurationTypeMember(member));
        }

        public IEngineConfigurationTypeMember GetRegisteredMember(EngineTypeMember member)
        {
            return mRegisteredMembers.Find(x => x.Member == member);
        }

        public IEnumerable<IEngineConfigurationTypeMember> GetRegisteredMembers()
        {
            return mRegisteredMembers.ConvertAll(x => (IEngineConfigurationTypeMember) x);
        }

        public void SetFactory(IEngineConfigurationDatasource factory)
        {
            mFactory = factory;
        }

        public IEngineConfigurationDatasource GetFactory()
        {
            return mFactory;
        }

        #endregion
    }
}