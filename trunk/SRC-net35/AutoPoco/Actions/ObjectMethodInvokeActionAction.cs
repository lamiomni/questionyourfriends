using System;
using AutoPoco.Engine;

namespace AutoPoco.Actions
{
    public class ObjectMethodInvokeActionAction<T> : IObjectAction
    {
        private readonly Action<T> _action;

        public ObjectMethodInvokeActionAction(Action<T> action)
        {
            _action = action;
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            _action.Invoke((T) target);
        }

        #endregion
    }
}