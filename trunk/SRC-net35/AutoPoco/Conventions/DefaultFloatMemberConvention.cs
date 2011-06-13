using AutoPoco.Configuration;

namespace AutoPoco.Conventions
{
    public class DefaultFloatMemberConvention : ITypeFieldConvention, ITypePropertyConvention
    {
        #region ITypeFieldConvention Members

        public void Apply(ITypeFieldConventionContext context)
        {
            if (context.Member.FieldInfo.FieldType == typeof (float))
            {
                context.SetValue(0);
            }
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Type(x => x == typeof (float));
        }

        #endregion

        #region ITypePropertyConvention Members

        public void Apply(ITypePropertyConventionContext context)
        {
            if (context.Member.PropertyInfo.PropertyType == typeof (float))
            {
                context.SetValue(0);
            }
        }

        #endregion
    }
}