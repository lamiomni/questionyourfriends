using AutoPoco.Configuration;

namespace AutoPoco.Engine
{
    public class TypeMethodGenerationContextNode : IGenerationContextNode
    {
        private readonly EngineTypeMethodMember mMethod;
        private readonly TypeGenerationContextNode mParent;

        public TypeMethodGenerationContextNode(TypeGenerationContextNode parent, EngineTypeMethodMember method)
        {
            mParent = parent;
            mMethod = method;
        }

        public virtual EngineTypeMethodMember Method
        {
            get { return mMethod; }
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
            get { return GenerationTargetTypes.Method; }
        }

        #endregion
    }
}