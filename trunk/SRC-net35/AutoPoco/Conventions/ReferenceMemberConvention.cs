using System.Collections;
using AutoPoco.Configuration;
using AutoPoco.DataSources;

namespace AutoPoco.Conventions
{
    public class ReferenceMemberConvention : ITypeFieldConvention, ITypePropertyConvention
    {
        #region ITypeFieldConvention Members

        public void Apply(ITypeFieldConventionContext context)
        {
            context.SetSource(typeof (AutoSource<>).MakeGenericType(context.Member.FieldInfo.FieldType));
        }

        public void SpecifyRequirements(ITypeMemberConventionRequirements requirements)
        {
            requirements.Type(x =>
                              x.IsClass
                              && x.GetInterface(typeof (IEnumerable).FullName) == null);
        }

        #endregion

        #region ITypePropertyConvention Members

        public void Apply(ITypePropertyConventionContext context)
        {
            context.SetSource(typeof (AutoSource<>).MakeGenericType(context.Member.PropertyInfo.PropertyType));
        }

        #endregion
    }
}