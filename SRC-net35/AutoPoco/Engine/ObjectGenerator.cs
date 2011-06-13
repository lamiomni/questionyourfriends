using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoPoco.Actions;
using AutoPoco.Configuration;
using AutoPoco.Util;

namespace AutoPoco.Engine
{
    public class ObjectGenerator<T> : IObjectGenerator<T>
    {
        private readonly IGenerationContext mContext;
        private readonly List<IObjectAction> mOverrides = new List<IObjectAction>();
        private readonly IObjectBuilder mType;

        public ObjectGenerator(IGenerationContext session, IObjectBuilder type)
        {
            mContext = session;
            mType = type;
        }

        #region IObjectGenerator<T> Members

        public T Get()
        {
            // Create the object     
            object createdObject = mType.CreateObject(mContext);

            // And overrides
            var typeContext = new GenerationContext(mContext.Builders,
                                                    new TypeGenerationContextNode(mContext.Node, createdObject));
            foreach (IObjectAction action in mOverrides)
            {
                action.Enact(typeContext, createdObject);
            }

            // And return the created object
            return (T) createdObject;
        }

        public IObjectGenerator<T> Impose<TMember>(Expression<Func<T, TMember>> propertyExpr,
                                                   TMember value)
        {
            EngineTypeMember member = ReflectionHelper.GetMember(propertyExpr);
            if (member.IsField)
            {
                AddAction(new ObjectFieldSetFromValueAction((EngineTypeFieldMember) member, value));
            }
            else if (member.IsProperty)
            {
                AddAction(new ObjectPropertySetFromValueAction((EngineTypePropertyMember) member, value));
            }

            return this;
        }

        public IObjectGenerator<T> Source<TMember>(Expression<Func<T, TMember>> propertyExpr, IDatasource dataSource)
        {
            EngineTypeMember member = ReflectionHelper.GetMember(propertyExpr);
            if (member.IsField)
            {
                AddAction(new ObjectFieldSetFromSourceAction((EngineTypeFieldMember) member, dataSource));
            }
            else if (member.IsProperty)
            {
                AddAction(new ObjectPropertySetFromSourceAction((EngineTypePropertyMember) member, dataSource));
            }

            return this;
        }

        public IObjectGenerator<T> Invoke(Expression<Action<T>> methodExpr)
        {
            var invoker = new ObjectMethodInvokeActionAction<T>(methodExpr.Compile());
            mOverrides.Add(invoker);
            return this;
        }

        public IObjectGenerator<T> Invoke<TMember>(Expression<Func<T, TMember>> methodExpr)
        {
            var invoker =
                new ObjectMethodInvokeFuncAction<T, TMember>(methodExpr.Compile());
            mOverrides.Add(invoker);
            return this;
        }

        #endregion

        public void AddAction(IObjectAction action)
        {
            mOverrides.Add(action);
        }
    }
}