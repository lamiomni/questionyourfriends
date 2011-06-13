﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoPoco.Configuration;
using AutoPoco.Configuration.Providers;
using AutoPoco.Configuration.TypeRegistrationActions;

namespace AutoPoco.Conventions
{
    // Still a klooge, but leaves me in a better place for later
    public class DefaultEngineConfigurationProviderLoadingConvention : IEngineConfigurationProviderLoader
    {
        #region IEngineConfigurationProviderLoader Members

        public void Apply(IEngineConfigurationProviderLoaderContext context)
        {
            OnPreTypeLoad(context);

            ITypeRegistrationAction typeAction = CreateTypeRegistrationActions(context);
            foreach (IEngineConfigurationType type in context.Configuration.GetRegisteredTypes())
            {
                typeAction.Apply(type);
            }

            OnPostTypeLoad(context);
        }

        #endregion

        protected virtual void OnPreTypeLoad(IEngineConfigurationProviderLoaderContext context)
        {
            FindAndRegisterAllBaseTypes(context);
        }

        protected virtual void OnPostTypeLoad(IEngineConfigurationProviderLoaderContext context)
        {
            foreach (IEngineConfigurationType type in context.Configuration.GetRegisteredTypes())
                ApplyBaseRulesToType(context, type);
        }

        protected virtual void FindAndRegisterAllBaseTypes(IEngineConfigurationProviderLoaderContext context)
        {
            foreach (IEngineConfigurationTypeProvider type in context.ConfigurationProvider.GetConfigurationTypes())
            {
                TryRegisterType(context.Configuration, type.GetConfigurationType());
            }
        }

        protected virtual void TryRegisterType(IEngineConfiguration configuration, Type configType)
        {
            IEngineConfigurationType configuredType = configuration.GetRegisteredType(configType);
            if (configuredType == null)
            {
                configuration.RegisterType(configType);
                configuredType = configuration.GetRegisteredType(configType);
            }

            foreach (Type interfaceType in configType.GetInterfaces())
            {
                TryRegisterType(configuration, interfaceType);
            }

            Type baseType = configType.BaseType;
            if (baseType != null)
            {
                TryRegisterType(configuration, baseType);
            }
        }

        protected virtual void ApplyBaseRulesToType(IEngineConfigurationProviderLoaderContext context,
                                                    IEngineConfigurationType type)
        {
            IEnumerable<IEngineConfigurationTypeMember> membersToApply =
                GetAllTypeHierarchyMembers(context.Configuration, type);

            foreach (IEngineConfigurationTypeMember existingMemberConfig in membersToApply)
            {
                IEngineConfigurationTypeMember currentMemberConfig =
                    type.GetRegisteredMember(existingMemberConfig.Member);
                if (currentMemberConfig == null)
                {
                    type.RegisterMember(existingMemberConfig.Member);
                    currentMemberConfig = type.GetRegisteredMember(existingMemberConfig.Member);
                    currentMemberConfig.SetDatasources(existingMemberConfig.GetDatasources());
                }
            }
        }

        protected virtual IEnumerable<IEngineConfigurationTypeMember> GetAllTypeHierarchyMembers(
            IEngineConfiguration baseConfiguration, IEngineConfigurationType sourceType)
        {
            var configurationStack = new Stack<IEngineConfigurationType>();
            Type currentType = sourceType.RegisteredType;
            IEngineConfigurationType currentTypeConfiguration = null;

            // Get all the base types into a stack, where the base-most type is at the top
            while (currentType != null)
            {
                currentTypeConfiguration = baseConfiguration.GetRegisteredType(currentType);
                if (currentTypeConfiguration != null)
                {
                    configurationStack.Push(currentTypeConfiguration);
                }
                currentType = currentType.BaseType;
            }

            // Put all the implemented interfaces on top of that
            foreach (Type interfaceType in sourceType.RegisteredType.GetInterfaces())
            {
                currentTypeConfiguration = baseConfiguration.GetRegisteredType(interfaceType);
                if (currentTypeConfiguration != null)
                {
                    configurationStack.Push(currentTypeConfiguration);
                }
            }

            IEngineConfigurationTypeMember[] membersToApply = (from typeConfig in configurationStack
                                                               from memberConfig in typeConfig.GetRegisteredMembers()
                                                               select memberConfig).ToArray();

            return membersToApply;
        }

        protected virtual ITypeRegistrationAction CreateTypeRegistrationActions(
            IEngineConfigurationProviderLoaderContext context)
        {
            return new ApplyTypeConventionsAction(context.ConventionProvider)
                       {
                           NextAction = new ApplyTypeFactoryAction(context.ConfigurationProvider)
                                            {
                                                NextAction =
                                                    new RegisterTypeMembersFromConfigurationAction(
                                                    context.ConfigurationProvider)
                                                        {
                                                            NextAction =
                                                                new ApplyTypeMemberConventionsAction(
                                                                context.Configuration, context.ConventionProvider)
                                                                    {
                                                                        NextAction =
                                                                            new ApplyTypeMemberConfigurationAction(
                                                                            context.ConfigurationProvider)
                                                                                {
                                                                                    NextAction =
                                                                                        new CascadeBaseTypeConfigurationAction
                                                                                        (context.Configuration)
                                                                                }
                                                                    }
                                                        }
                                            }
                       };
        }
    }
}