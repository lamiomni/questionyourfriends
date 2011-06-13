using AutoPoco.Configuration;

namespace AutoPoco.Engine
{
    public class ObjectPropertySetFromSourceAction : IObjectAction
    {
        private readonly IDatasource mDatasource;
        private readonly EngineTypePropertyMember mMember;

        public ObjectPropertySetFromSourceAction(EngineTypePropertyMember member, IDatasource source)
        {
            mMember = member;
            mDatasource = source;
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            var propertyContext = new GenerationContext(context.Builders, new TypePropertyGenerationContextNode(
                                                                              (TypeGenerationContextNode) context.Node,
                                                                              mMember));
            mMember.PropertyInfo.SetValue(target, mDatasource.Next(propertyContext), null);
        }

        #endregion
    }
}