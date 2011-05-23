using System;
using System.Linq;
using System.Reflection;
using AutoPoco.Util;

namespace AutoPoco.Configuration
{
    public class TypeConventionContext : ITypeConventionContext
    {
        private readonly IEngineConfigurationType mType;

        public TypeConventionContext(IEngineConfigurationType type)
        {
            mType = type;
        }

        #region ITypeConventionContext Members

        public Type Target
        {
            get { return mType.RegisteredType; }
        }

        public void SetFactory(Type factory)
        {
            mType.SetFactory(new DatasourceFactory(factory));
        }

        public void SetFactory(Type factory, params object[] ctorArgs)
        {
            var sourceFactory = new DatasourceFactory(factory);
            sourceFactory.SetParams(ctorArgs);
            mType.SetFactory(sourceFactory);
        }

        public void RegisterField(FieldInfo field)
        {
            EngineTypeMember member = ReflectionHelper.GetMember(field);
            if (mType.GetRegisteredMember(member) == null)
            {
                mType.RegisterMember(member);
            }
        }

        public void RegisterProperty(PropertyInfo property)
        {
            EngineTypeMember member = ReflectionHelper.GetMember(property);
            if (mType.GetRegisteredMember(member) == null)
            {
                mType.RegisterMember(ReflectionHelper.GetMember(property));
            }
        }

        public void RegisterMethod(MethodInfo method, MethodInvocationContext context)
        {
            EngineTypeMember member = ReflectionHelper.GetMember(method);
            if (mType.GetRegisteredMember(member) == null)
            {
                mType.RegisterMember(member);
            }
            IEngineConfigurationTypeMember registeredMember = mType.GetRegisteredMember(member);
            registeredMember.SetDatasources(context.GetArguments().Cast<IEngineConfigurationDatasource>());
        }

        #endregion
    }
}