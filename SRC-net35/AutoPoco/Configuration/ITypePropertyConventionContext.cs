﻿using System;
using AutoPoco.Engine;

namespace AutoPoco.Configuration
{
    public interface ITypePropertyConventionContext
    {
        /// <summary>
        /// Gets the configuration created so far
        /// </summary>
        IEngineConfiguration Configuration { get; }

        /// <summary>
        /// Gets the member being processed
        /// </summary>
        EngineTypePropertyMember Member { get; }

        /// <summary>
        /// Sets the value of the member directly on instantiation
        /// </summary>
        /// <param name="value"></param>
        void SetValue(Object value);

        /// <summary>
        /// Sets the data source where this member will get data from
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void SetSource<T>() where T : IDatasource;

        /// <summary>
        /// Sets the data source where this member will get data from
        /// </summary>
        /// <param name="t"></param>
        void SetSource(Type type);
    }
}