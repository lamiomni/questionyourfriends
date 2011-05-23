namespace AutoPoco.Configuration
{
    public class TypePropertyConventionContext : TypeMemberConventionContext, ITypePropertyConventionContext
    {
        public TypePropertyConventionContext(IEngineConfiguration config, IEngineConfigurationTypeMember member)
            : base(config, member)
        {
        }

        #region ITypePropertyConventionContext Members

        public new EngineTypePropertyMember Member
        {
            get { return base.Member as EngineTypePropertyMember; }
        }

        #endregion
    }
}