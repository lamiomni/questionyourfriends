namespace AutoPoco.Configuration
{
    public class TypeFieldConventionContext : TypeMemberConventionContext, ITypeFieldConventionContext
    {
        public TypeFieldConventionContext(IEngineConfiguration config, IEngineConfigurationTypeMember member)
            : base(config, member)
        {
        }

        #region ITypeFieldConventionContext Members

        public new EngineTypeFieldMember Member
        {
            get { return base.Member as EngineTypeFieldMember; }
        }

        #endregion
    }
}