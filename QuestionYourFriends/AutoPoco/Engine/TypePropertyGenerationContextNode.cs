using AutoPoco.Configuration;

namespace AutoPoco.Engine
{
    public class TypePropertyGenerationContextNode : IGenerationContextNode
    {
        private readonly TypeGenerationContextNode mParent;
        private readonly EngineTypePropertyMember mProperty;

        public TypePropertyGenerationContextNode(TypeGenerationContextNode parent, EngineTypePropertyMember property)
        {
            mParent = parent;
            mProperty = property;
        }

        public virtual EngineTypePropertyMember Property
        {
            get { return mProperty; }
        }

        public virtual object Target
        {
            get { return mParent.Target; }
        }

        #region IGenerationContextNode Members

        public virtual IGenerationContextNode Parent
        {
            get { return mParent; }
        }

        public GenerationTargetTypes ContextType
        {
            get { return GenerationTargetTypes.Property; }
        }

        #endregion
    }
}