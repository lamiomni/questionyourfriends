using System;
using System.Linq;
using System.Reflection;
using AutoPoco.Engine;

namespace AutoPoco.DataSources
{
    public class CtorSource<T> : DatasourceBase<T>
    {
        private readonly ConstructorInfo mConstructorInfo;

        public CtorSource(ConstructorInfo ctor)
        {
            mConstructorInfo = ctor;
        }

        public override T Next(IGenerationContext context)
        {
            // TODO: May actually create a parallel set of interfaces for doing non-generic requests to session
            // This would negate the need for that awful reflection
            object[] args = mConstructorInfo
                .GetParameters()
                .Select(x =>
                            {
                                MethodInfo method = context.GetType().GetMethod("Next", Type.EmptyTypes);
                                MethodInfo target = method.MakeGenericMethod(x.ParameterType);
                                return target.Invoke(context, null);
                            })
                .ToArray();

            return (T) mConstructorInfo.Invoke(args);
        }
    }
}