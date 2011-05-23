namespace AutoPoco.Engine
{
    public class RootGenerationContextNode : IGenerationContextNode
    {
        #region IGenerationContextNode Members

        public IGenerationContextNode Parent
        {
            get { return null; }
        }

        public GenerationTargetTypes ContextType
        {
            get { return GenerationTargetTypes.Root; }
        }

        #endregion
    }
}