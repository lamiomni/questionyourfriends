using System;
using AutoPoco.Engine;

namespace AutoPoco.Actions
{
    public class ObjectMethodInvokeFuncAction<TPoco, TReturn> : IObjectAction
    {
        private readonly Func<TPoco, TReturn> _action;

        public ObjectMethodInvokeFuncAction(Func<TPoco, TReturn> action)
        {
            _action = action;
        }

        #region IObjectAction Members

        public void Enact(IGenerationContext context, object target)
        {
            _action.Invoke((TPoco) target);
        }

        #endregion
    }
}