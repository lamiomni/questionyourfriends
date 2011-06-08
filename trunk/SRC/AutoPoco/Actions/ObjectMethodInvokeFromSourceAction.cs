using System.Collections.Generic;
using System.Linq;
using AutoPoco.Configuration;
using AutoPoco.Engine;

namespace AutoPoco.Actions
{
    public class ObjectMethodInvokeFromSourceAction : IObjectAction
    {
        private readonly EngineTypeMethodMember _member;
        private readonly IEnumerable<IDatasource> _sources;

        public ObjectMethodInvokeFromSourceAction(EngineTypeMethodMember member, IEnumerable<IDatasource> sources)
        {
            _member = member;
            _sources = sources.ToArray();
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            var paramList = new List<object>();
            var methodContext = new GenerationContext(context.Builders,
                                                      new TypeMethodGenerationContextNode(
                                                          (TypeGenerationContextNode) context.Node, _member));

            foreach (IDatasource source in _sources)
            {
                paramList.Add(source.Next(methodContext));
            }

            _member.MethodInfo.Invoke(target, paramList.ToArray());
        }

        #endregion
    }
}