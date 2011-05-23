using AutoPoco.Configuration;

namespace AutoPoco.Engine
{
    public class TypeFieldGenerationContextNode : IGenerationContextNode
    {
        private readonly EngineTypeFieldMember mField;
        private readonly TypeGenerationContextNode mParent;

        public TypeFieldGenerationContextNode(TypeGenerationContextNode parent, EngineTypeFieldMember field)
        {
            mParent = parent;
            mField = field;
        }

        public virtual EngineTypeFieldMember Field
        {
            get { return mField; }
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
            get { return GenerationTargetTypes.Field; }
        }

        #endregion
    }
}