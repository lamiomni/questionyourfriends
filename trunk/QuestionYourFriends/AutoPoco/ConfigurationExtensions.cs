﻿using System;
using AutoPoco.Configuration;

namespace AutoPoco
{
    public static class ConfigurationExtensions
    {
        public static IEngineConfigurationBuilder AddFromAssemblyContainingType<T>(
            this IEngineConfigurationBuilder builder)
        {
            foreach (Type type in typeof (T).Assembly.GetTypes())
            {
                builder.Include(type);
            }
            return builder;
        }
    }
}