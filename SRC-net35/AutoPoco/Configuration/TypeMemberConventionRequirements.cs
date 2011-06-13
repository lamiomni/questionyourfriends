﻿using System;
using System.Linq.Expressions;

namespace AutoPoco.Configuration
{
    public class TypeMemberConventionRequirements : ITypeMemberConventionRequirements
    {
        private Func<string, bool> mNameRule;
        private Func<Type, bool> mTypeRule;

        #region ITypeMemberConventionRequirements Members

        public void Name(Expression<Func<string, bool>> rule)
        {
            mNameRule = rule.Compile();
        }

        public void Type(Expression<Func<Type, bool>> rule)
        {
            mTypeRule = rule.Compile();
        }

        #endregion

        public bool IsValidType(Type type)
        {
            if (mTypeRule == null)
            {
                return true;
            }
            return mTypeRule.Invoke(type);
        }

        public bool IsValidName(String name)
        {
            if (mNameRule == null)
            {
                return true;
            }
            return mNameRule.Invoke(name);
        }

        internal bool HasNameRule()
        {
            return mNameRule != null;
        }

        internal bool HasTypeRule()
        {
            return mTypeRule != null;
        }
    }
}