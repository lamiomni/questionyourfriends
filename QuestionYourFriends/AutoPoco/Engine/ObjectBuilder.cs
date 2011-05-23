using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Actions;
using AutoPoco.Configuration;

namespace AutoPoco.Engine
{
    public class ObjectBuilder : IObjectBuilder
    {
        private readonly List<IObjectAction> mActions = new List<IObjectAction>();
        private readonly IDatasource mFactory;

        /// <summary>
        /// Creates this object builder
        /// </summary>
        /// <param name="type"></param>
        public ObjectBuilder(IEngineConfigurationType type)
        {
            InnerType = type.RegisteredType;

            if (type.GetFactory() != null)
            {
                mFactory = type.GetFactory().Build();
            }

            type.GetRegisteredMembers()
                .ToList()
                .ForEach(x =>
                             {
                                 List<IDatasource> sources = x.GetDatasources().Select(s => s.Build()).ToList();

                                 if (x.Member.IsField)
                                 {
                                     if (sources.Count == 0)
                                     {
                                         return;
                                     }

                                     AddAction(new ObjectFieldSetFromSourceAction(
                                                   (EngineTypeFieldMember) x.Member,
                                                   sources.First()));
                                 }
                                 else if (x.Member.IsProperty)
                                 {
                                     if (sources.Count == 0)
                                     {
                                         return;
                                     }

                                     AddAction(new ObjectPropertySetFromSourceAction(
                                                   (EngineTypePropertyMember) x.Member,
                                                   sources.First()));
                                 }
                                 else if (x.Member.IsMethod)
                                 {
                                     AddAction(new ObjectMethodInvokeFromSourceAction(
                                                   (EngineTypeMethodMember) x.Member,
                                                   sources
                                                   ));
                                 }
                             });
        }

        #region IObjectBuilder Members

        public Type InnerType { get; private set; }

        public IEnumerable<IObjectAction> Actions
        {
            get { return mActions; }
        }

        public void ClearActions()
        {
            mActions.Clear();
        }

        public void AddAction(IObjectAction action)
        {
            mActions.Add(action);
        }

        public void RemoveAction(IObjectAction action)
        {
            mActions.Remove(action);
        }

        public Object CreateObject(IGenerationContext context)
        {
            Object createdObject = null;

            if (mFactory != null)
            {
                createdObject = mFactory.Next(context);
            }
            else
            {
                createdObject = Activator.CreateInstance(InnerType);
            }

            // Don't set it up if we've reached recursion limit
            if (context.Depth < context.Builders.RecursionLimit)
            {
                EnactActionsOnObject(context, createdObject);
            }
            return createdObject;
        }

        #endregion

        private void EnactActionsOnObject(IGenerationContext context, object createdObject)
        {
            var typeContext = new GenerationContext(context.Builders,
                                                    new TypeGenerationContextNode(context.Node, createdObject));
            foreach (IObjectAction action in mActions)
            {
                action.Enact(typeContext, createdObject);
            }
        }
    }
}