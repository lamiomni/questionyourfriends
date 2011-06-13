﻿using System;
using AutoPoco.Configuration;
using AutoPoco.DataSources;

namespace AutoPoco.Conventions
{
    public class DefaultPrimitiveCtorConvention : ITypeConvention
    {
        #region ITypeConvention Members

        public void Apply(ITypeConventionContext context)
        {
            Type type = context.Target;
            if (type.IsPrimitive || type == typeof (Decimal))
            {
                context.SetFactory(typeof (DefaultSource<>).MakeGenericType(type));
            }
            else if (type == typeof (string))
            {
                context.SetFactory(typeof (DefaultStringSource));
            }
        }

        #endregion
    }
}