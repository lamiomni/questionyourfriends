using System;
using System.Reflection;

namespace AutoPoco.Configuration
{
    public class EngineTypePropertyMember : EngineTypeMember
    {
        private readonly PropertyInfo mPropertyInfo;

        public EngineTypePropertyMember(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }
            mPropertyInfo = propertyInfo;
        }

        public override string Name
        {
            get { return mPropertyInfo.Name; }
        }

        public override bool IsMethod
        {
            get { return false; }
        }

        public override bool IsField
        {
            get { return false; }
        }

        public override bool IsProperty
        {
            get { return true; }
        }

        public PropertyInfo PropertyInfo
        {
            get { return mPropertyInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherMember = obj as EngineTypePropertyMember;
            if (otherMember != null)
            {
                PropertyInfo propertyOne = PropertyInfo;
                PropertyInfo propertyTwo = otherMember.PropertyInfo;

                if (String.Compare(propertyOne.Name, propertyTwo.Name, false) != 0)
                {
                    return false;
                }
                if (propertyOne.PropertyType != propertyTwo.PropertyType)
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return PropertyInfo.GetHashCode();
        }
    }
}