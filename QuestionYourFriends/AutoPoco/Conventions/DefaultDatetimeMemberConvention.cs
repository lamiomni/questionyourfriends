using System;
using AutoPoco.Configuration;
using AutoPoco.DataSources;

namespace AutoPoco.Conventions
{
    public class DefaultDatetimeMemberConvention : ITypeFieldConvention, ITypePropertyConvention
    {
        #region ITypeFieldConvention Members

        public void Apply(ITypeFieldConventionContext context)
        {
            context.SetValue(DateTime.MinValue);
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Type(x => x == typeof (DateTime));
        }

        #endregion

        #region ITypePropertyConvention Members

        public void Apply(ITypePropertyConventionContext context)
        {
            context.SetValue(DateTime.MinValue);
        }

        #endregion
    }

    public class EmailAddressPropertyConvention : ITypePropertyConvention
    {
        #region ITypePropertyConvention Members

        public void Apply(ITypePropertyConventionContext context)
        {
            context.SetSource<EmailAddressSource>();
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Name(x => String.Compare(x, "EmailAddress", true) == 0);
            requirements.Type(x => x == typeof (String));
        }

        #endregion
    }
}