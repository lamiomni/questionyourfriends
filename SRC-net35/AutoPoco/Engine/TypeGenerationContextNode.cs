﻿namespace AutoPoco.Engine
{
    public class TypeGenerationContextNode : IGenerationContextNode
    {
        private readonly IGenerationContextNode mParent;
        private readonly object mTarget;

        public TypeGenerationContextNode(IGenerationContextNode parent, object target)
        {
            mParent = parent;
            mTarget = target;
        }

        public virtual object Target
        {
            get { return mTarget; }
        }

        #region IGenerationContextNode Members

        public virtual IGenerationContextNode Parent
        {
            get { return mParent; }
        }

        public GenerationTargetTypes ContextType
        {
            get { return GenerationTargetTypes.Object; }
        }

        #endregion
    }
}