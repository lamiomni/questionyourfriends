namespace AutoPoco.Configuration.TypeRegistrationActions
{
    public abstract class TypeRegistrationAction : ITypeRegistrationAction
    {
        public ITypeRegistrationAction NextAction { get; set; }

        #region ITypeRegistrationAction Members

        void ITypeRegistrationAction.Apply(IEngineConfigurationType type)
        {
            Apply(type);
            if (NextAction != null)
            {
                NextAction.Apply(type);
            }
        }

        #endregion

        public abstract void Apply(IEngineConfigurationType type);
    }
}