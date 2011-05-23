using System.Reflection;

namespace AutoPoco.Configuration
{
    public class EngineTypeFieldMember : EngineTypeMember
    {
        private readonly FieldInfo mFieldInfo;

        public EngineTypeFieldMember(FieldInfo fieldInfo)
        {
            mFieldInfo = fieldInfo;
        }

        public override string Name
        {
            get { return mFieldInfo.Name; }
        }

        public override bool IsMethod
        {
            get { return false; }
        }

        public override bool IsField
        {
            get { return true; }
        }

        public override bool IsProperty
        {
            get { return false; }
        }

        public FieldInfo FieldInfo
        {
            get { return mFieldInfo; }
        }

        public override bool Equals(object obj)
        {
            var otherMember = obj as EngineTypeFieldMember;
            if (otherMember != null)
            {
                return otherMember.FieldInfo == FieldInfo;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return FieldInfo.GetHashCode();
        }
    }
}