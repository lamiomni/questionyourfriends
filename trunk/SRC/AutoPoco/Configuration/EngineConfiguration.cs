using System;
using System.Collections.Generic;

namespace AutoPoco.Configuration
{
    public class EngineConfiguration : IEngineConfiguration
    {
        private readonly List<EngineConfigurationType> mRegisteredTypes = new List<EngineConfigurationType>();

        #region IEngineConfiguration Members

        public IEnumerable<IEngineConfigurationType> GetRegisteredTypes()
        {
            return mRegisteredTypes.ConvertAll(x => (IEngineConfigurationType) x);
        }

        public void RegisterType(Type t)
        {
            if (mRegisteredTypes.Find(x => x.RegisteredType == t) != null)
            {
                throw new ArgumentException("Type has already been registered", "t");
            }
            mRegisteredTypes.Add(new EngineConfigurationType(t));
        }

        public IEngineConfigurationType GetRegisteredType(Type t)
        {
            return mRegisteredTypes.Find(x => x.RegisteredType == t);
        }

        #endregion
    }
}