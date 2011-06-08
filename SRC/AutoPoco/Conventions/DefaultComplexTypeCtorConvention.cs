using System;
using System.Linq;
using System.Reflection;
using AutoPoco.Configuration;
using AutoPoco.DataSources;

namespace AutoPoco.Conventions
{
    public class DefaultComplexTypeCtorConvention : ITypeConvention
    {
        #region ITypeConvention Members

        public void Apply(ITypeConventionContext context)
        {
            Type type = context.Target;
            if (type.IsPrimitive || type == typeof (Decimal) || type == typeof (string))
            {
                return;
            }

            ConstructorInfo ctor =
                type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .OrderBy(x => x.GetParameters().Count())
                    .FirstOrDefault();

            if (ctor == null)
            {
                return;
            }

            Type ctorSourceType = typeof (CtorSource<>).MakeGenericType(type);

            context.SetFactory(ctorSourceType, ctor);
        }

        #endregion
    }
}