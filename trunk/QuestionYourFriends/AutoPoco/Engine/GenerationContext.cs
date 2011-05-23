using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoPoco.Engine
{
    public class GenerationContext : IGenerationContext
    {
        private readonly IGenerationContextNode mNode;
        private readonly IGenerationConfiguration mObjectBuilders;
        private readonly int mRecursionLimit;

        public GenerationContext(IGenerationConfiguration objectBuilders)
            : this(objectBuilders, null)
        {
        }

        public GenerationContext(IGenerationConfiguration objectBuilders, IGenerationContextNode node)
        {
            mObjectBuilders = objectBuilders;
            mNode = node;
            CalculateDepth();
        }

        #region IGenerationContext Members

        public IGenerationContextNode Node
        {
            get { return mNode; }
        }

        public int Depth { get; private set; }

        public IGenerationConfiguration Builders
        {
            get { return mObjectBuilders; }
        }

        public virtual IObjectGenerator<TPoco> Single<TPoco>()
        {
            Type searchType = typeof (TPoco);
            IObjectBuilder foundType = mObjectBuilders.GetBuilderForType(searchType);
            return new ObjectGenerator<TPoco>(this, foundType);
        }

        public ICollectionContext<TPoco, IList<TPoco>> List<TPoco>(int count)
        {
            return new CollectionContext<TPoco, IList<TPoco>>(
                Enumerable.Range(0, count)
                    .Select(x => Single<TPoco>()).ToArray()
                    .AsEnumerable());
        }

        public TPoco Next<TPoco>()
        {
            return Single<TPoco>().Get();
        }

        public TPoco Next<TPoco>(Action<IObjectGenerator<TPoco>> cfg)
        {
            IObjectGenerator<TPoco> generator = Single<TPoco>();
            cfg.Invoke(generator);
            return generator.Get();
        }

        public IEnumerable<TPoco> Collection<TPoco>(int count)
        {
            ICollectionContext<TPoco, IList<TPoco>> generator = List<TPoco>(count);
            return generator.Get();
        }

        public IEnumerable<TPoco> Collection<TPoco>(int count, Action<ICollectionContext<TPoco, IList<TPoco>>> cfg)
        {
            ICollectionContext<TPoco, IList<TPoco>> generator = List<TPoco>(count);
            cfg.Invoke(generator);
            return generator.Get();
        }

        #endregion

        private void CalculateDepth()
        {
            IGenerationContextNode currentNode = mNode;
            int depth = 0;
            while (currentNode != null)
            {
                currentNode = FindNextTypeNode(currentNode);
                depth++;
            }
            Depth = depth;
        }

        private IGenerationContextNode FindNextTypeNode(IGenerationContextNode currentNode)
        {
            while (true)
            {
                currentNode = currentNode.Parent;
                if (currentNode == null || currentNode.ContextType == GenerationTargetTypes.Object)
                {
                    break;
                }
            }
            return currentNode;
        }
    }
}