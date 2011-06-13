﻿using System;

namespace AutoPoco.Engine
{
    public interface IGenerationConfiguration
    {
        /// <summary>
        /// Gets the recursion limit for this configuration
        /// </summary>
        int RecursionLimit { get; }

        /// <summary>
        /// Gets the object builder for a certain type
        /// </summary>
        /// <param name="searchType"></param>
        /// <returns></returns>
        IObjectBuilder GetBuilderForType(Type searchType);
    }
}