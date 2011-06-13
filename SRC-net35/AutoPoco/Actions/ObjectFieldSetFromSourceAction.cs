using AutoPoco.Configuration;
using AutoPoco.Engine;

namespace AutoPoco.Actions
{
    public class ObjectFieldSetFromSourceAction : IObjectAction
    {
        private readonly IDatasource _datasource;
        private readonly EngineTypeFieldMember _member;

        public ObjectFieldSetFromSourceAction(EngineTypeFieldMember member, IDatasource source)
        {
            _member = member;
            _datasource = source;
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            var fieldContext = new GenerationContext(context.Builders, new TypeFieldGenerationContextNode(
                                                                           (TypeGenerationContextNode) context.Node,
                                                                           _member));
            _member.FieldInfo.SetValue(target, _datasource.Next(fieldContext));
        }

        #endregion
    }
}