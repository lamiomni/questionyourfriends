using System.Reflection;
using AutoPoco.Util;

namespace AutoPoco.Configuration
{
    public class EngineTypeMethodMember : EngineTypeMember
    {
        private readonly MethodInfo mMethodInfo;

        public EngineTypeMethodMember(MethodInfo methodInfo)
        {
            mMethodInfo = methodInfo;
        }

        public override string Name
        {
            get { return mMethodInfo.Name; }
        }

        public override bool IsMethod
        {
            get { return true; }
        }

        public override bool IsField
        {
            get { return false; }
        }

        public override bool IsProperty
        {
            get { return false; }
        }

        public MethodInfo MethodInfo
        {
            get { return mMethodInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherMember = obj as EngineTypeMethodMember;
            if (otherMember != null)
            {
                return (otherMember.MethodInfo.Name == MethodInfo.Name) &&
                       otherMember.MethodInfo.ArgumentsAreEqualTo(MethodInfo);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return mMethodInfo.GetHashCode();
        }
    }
}