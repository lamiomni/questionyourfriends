﻿using System;
using AutoPoco.Configuration;
using AutoPoco.Conventions;
using AutoPoco.Engine;

namespace AutoPoco
{
    public static class AutoPocoContainer
    {
        public static IGenerationSessionFactory Configure(Action<IEngineConfigurationBuilder> setup)
        {
            var config = new EngineConfigurationBuilder();
            config.Conventions(x => x.Register<DefaultPrimitiveCtorConvention>());
            setup.Invoke(config);
            var configFactory = new EngineConfigurationFactory();
            return new GenerationSessionFactory(configFactory.Create(config, config.ConventionProvider),
                                                config.ConventionProvider);
        }

        public static IGenerationSession CreateDefaultSession()
        {
            var config = new EngineConfigurationBuilder();
            var configFactory = new EngineConfigurationFactory();

            config.Conventions(x => x.UseDefaultConventions());

            return new GenerationSessionFactory(
                configFactory.Create(config, config.ConventionProvider), config.ConventionProvider)
                .CreateSession();
        }
    }
}