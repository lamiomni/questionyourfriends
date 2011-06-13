using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoPoco.Configuration;

namespace AutoPoco.Conventions
{
    public class DefaultTypeConvention : ITypeConvention
    {
        #region ITypeConvention Members

        public void Apply(ITypeConventionContext context)
        {
            // Register every public property on this type
            foreach (PropertyInfo property in context.Target
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.PropertyType.ContainsGenericParameters && IsDefinedOnType(x, context.Target)))
            {
                if (PropertyHasPublicSetter(property))
                {
                    context.RegisterProperty(property);
                }
            }

            // Register every public field on this type
            foreach (FieldInfo field in context.Target
                .GetFields(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => !x.FieldType.ContainsGenericParameters && IsDefinedOnType(x, context.Target)))
            {
                context.RegisterField(field);
            }
        }

        #endregion

        private bool PropertyHasPublicSetter(PropertyInfo property)
        {
            MethodInfo setter = property.GetSetMethod();
            return setter != null && setter.IsPublic;
        }

        private bool IsDefinedOnType(MemberInfo member, Type type)
        {
            if (member.DeclaringType != type)
            {
                return false;
            }

            if (member.MemberType == MemberTypes.Property && !type.IsInterface)
            {
                var property = (PropertyInfo) member;

                IEnumerable<MethodInfo> interfaceMethods =
                    (from i in type.GetInterfaces()
                     from method in type.GetInterfaceMap(i).TargetMethods
                     select method);

                bool exists = (from method in interfaceMethods
                               where property.GetAccessors().Contains(method)
                               select 1).Count() > 0;


                if (exists) return false;
            }

            return true;
        }
    }
}