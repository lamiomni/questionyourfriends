using System.Collections.Generic;
using System.Linq;
using AutoPoco.Configuration.Providers;

namespace AutoPoco.Configuration.TypeRegistrationActions
{
    public class ApplyTypeMemberConfigurationAction : TypeRegistrationAction
    {
        private readonly IEngineConfigurationProvider mConfigurationProvider;

        public ApplyTypeMemberConfigurationAction(IEngineConfigurationProvider configurationProvider)
        {
            mConfigurationProvider = configurationProvider;
        }

        public override void Apply(IEngineConfigurationType type)
        {
            ApplyToType(type);
        }

        private void ApplyToType(IEngineConfigurationType type)
        {
            IEnumerable<IEngineConfigurationTypeProvider> typeProviders = mConfigurationProvider.GetConfigurationTypes()
                .Where(x => x.GetConfigurationType() == type.RegisteredType);

            foreach (IEngineConfigurationTypeProvider typeProvider in typeProviders)
            {
                foreach (IEngineConfigurationTypeMemberProvider memberProvider in typeProvider.GetConfigurationMembers()
                    )
                {
                    EngineTypeMember typeMember = memberProvider.GetConfigurationMember();

                    // Get the member
                    IEngineConfigurationTypeMember configuredMember = type.GetRegisteredMember(typeMember);

                    // Set the action on that member if a datasource has been set explicitly for this member
                    IEnumerable<IEngineConfigurationDatasource> datasources = memberProvider.GetDatasources();
                    if (datasources.Count() > 0)
                    {
                        configuredMember.SetDatasources(datasources);
                    }
                }
            }
        }
    }
}