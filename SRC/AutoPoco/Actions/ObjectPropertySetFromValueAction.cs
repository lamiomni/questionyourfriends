using System;
using AutoPoco.Configuration;
using AutoPoco.Engine;

namespace AutoPoco.Actions
{
    public class ObjectPropertySetFromValueAction : IObjectAction
    {
        private readonly EngineTypePropertyMember mMember;
        private readonly Object mValue;

        public ObjectPropertySetFromValueAction(EngineTypePropertyMember member, Object value)
        {
            mMember = member;
            mValue = value;
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            mMember.PropertyInfo.SetValue(target, mValue, null);
        }

        #endregion
    }
}