using System;
using AutoPoco.Configuration;
using AutoPoco.Engine;

namespace AutoPoco.Actions
{
    public class ObjectFieldSetFromValueAction : IObjectAction
    {
        private readonly EngineTypeFieldMember _member;
        private readonly Object _value;

        public ObjectFieldSetFromValueAction(EngineTypeFieldMember member, Object value)
        {
            _member = member;
            _value = value;
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            _member.FieldInfo.SetValue(target, _value);
        }

        #endregion
    }
}