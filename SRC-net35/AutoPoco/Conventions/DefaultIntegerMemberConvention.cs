using AutoPoco.Configuration;

namespace AutoPoco.Conventions
{
    public class DefaultIntegerMemberConvention : ITypeFieldConvention, ITypePropertyConvention
    {
        #region ITypeFieldConvention Members

        public void Apply(ITypeFieldConventionContext context)
        {
            if (context.Member.FieldInfo.FieldType == typeof (int))
            {
                context.SetValue(0);
            }
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Type(x => x == typeof (int));
        }

        #endregion

        #region ITypePropertyConvention Members

        public void Apply(ITypePropertyConventionContext context)
        {
            if (context.Member.PropertyInfo.PropertyType == typeof (int))
            {
                context.SetValue(0);
            }
        }

        #endregion
    }
}