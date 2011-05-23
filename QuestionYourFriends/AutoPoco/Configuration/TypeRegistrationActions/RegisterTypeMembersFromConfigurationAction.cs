using System.Collections.Generic;
using System.Linq;
using AutoPoco.Configuration.Providers;

namespace AutoPoco.Configuration.TypeRegistrationActions
{
    public class RegisterTypeMembersFromConfigurationAction : TypeRegistrationAction
    {
        private readonly IEngineConfigurationProvider mConfigurationProvider;

        public RegisterTypeMembersFromConfigurationAction(IEngineConfigurationProvider configurationProvider)
        {
            mConfigurationProvider = configurationProvider;
        }

        public override void Apply(IEngineConfigurationType type)
        {
            ApplyToType(type);
        }

        private void ApplyToType(IEngineConfigurationType type)
        {
            IEnumerable<IEngineConfigurationTypeProvider> typeProviders =
                mConfigurationProvider.GetConfigurationTypes().Where(
                    x => x.GetConfigurationType() == type.RegisteredType);
            foreach (IEngineConfigurationTypeProvider typeProvider in typeProviders)
            {
                foreach (IEngineConfigurationTypeMemberProvider member in typeProvider.GetConfigurationMembers())
                {
                    EngineTypeMember typeMember = member.GetConfigurationMember();

                    if (type.GetRegisteredMember(typeMember) == null)
                    {
                        type.RegisterMember(typeMember);
                    }
                }
            }
        }
    }
}