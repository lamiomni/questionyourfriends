using AutoPoco.Configuration;

namespace AutoPoco.Conventions
{
    public class DefaultStringMemberConvention : ITypeFieldConvention, ITypePropertyConvention
    {
        #region ITypeFieldConvention Members

        public void Apply(ITypeFieldConventionContext context)
        {
            if (context.Member.FieldInfo.FieldType == typeof (string))
            {
                context.SetValue(string.Empty);
            }
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Type(x => x == typeof (string));
        }

        #endregion

        #region ITypePropertyConvention Members

        public void Apply(ITypePropertyConventionContext context)
        {
            if (context.Member.PropertyInfo.PropertyType == typeof (string))
            {
                context.SetValue(string.Empty);
            }
        }

        #endregion
    }
}