﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Core.Definitions.Utilities;
using System;
using System.Collections.Generic;

namespace Microsoft.MixedReality.Toolkit.Core.Interfaces
{
    /// <summary>
    /// Interface for Mixed Reality Toolkit service registration.
    /// </summary>
    public interface IMixedRealityServiceRegistrar
    {
        #region IMixedRealityService registration

        /// <summary>
        /// Registers a service of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the service to be registered (ex: IMixedRealityBoundarySystem).</typeparam>
        /// <param name="serviceInstance">An instance of the service to be registered.</param>
        bool RegisterService<T>(IMixedRealityService serviceInstance) where T : IMixedRealityService;

        /// <summary>
        /// Registers a service of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the service to be registered (ex: IMixedRealityBoundarySystem).</typeparam>
        /// <param name="concreteType">The concrete type to instantiate.</param>
        /// <param name="supportedPlatforms">The platform(s) on which the service is supported.</param>
        /// <param name="args">Optional arguments used when instantiating the concrete type.</param>
        /// <returns>True if the service was successfully registered, false otherwise.</returns>
        bool RegisterService<T>(
            Type concreteType,
            SupportedPlatforms supportedPlatforms = (SupportedPlatforms)(-1),
            params object[] args) where T : IMixedRealityService;

        /// <summary>
        /// Unregisters a service.
        /// </summary>
        /// <param name="name">The name of the service to unregister.</param>
        /// <returns>True if the service was successfully unregistered, false otherwise.</returns>
        bool UnregisterService(string name); // todo: namespace?

        /// <summary>
        /// Unregisters a service of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the service to be unregistered (ex: IMixedRealityBoundarySystem).
        /// <param name="name">The name of the service to unregister.</param>
        /// <returns>True if the service was successfully unregistered, false otherwise.</returns>
        /// <remarks>If the name argument is not especified, the first instance will be unregistered</remarks>
        bool UnregisterService<T>(string name = null) where T : IMixedRealityService; // todo: namespace?

        /// <summary>
        /// Unregisters a service.
        /// </summary>
        /// <param name="service">The specific service instance to unregister.</param>
        /// <returns>True if the service was successfully unregistered, false otherwise.</returns>
        bool UnregisterService(IMixedRealityService serviceInstance);

        /// <summary>
        /// Unregisters all services.
        /// </summary>
        void UnregisterServices();

        /// <summary>
        /// Unregisters all services of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the services to be unregistered (ex: IMixedRealityBoundarySystem).
        void UnregisterServices<T>() where T : IMixedRealityService;

        /// <summary>
        /// Checks to see if a service has been registered.
        /// </summary>
        /// <param name="name">The name of the service.</param>
        /// <returns>True if the service is registered, false otherwise.</returns>
        bool IsServiceRegistered(string name);  // todo: namespace?

        /// <summary>
        /// Checks to see if a service of the specified type has been registered.
        /// </summary>
        /// <typeparam name="T">The interface type of the service (ex: IMixedRealityBoundarySystem).
        /// <param name="name">The name of the service.</param>
        /// <returns>True if the service is registered, false otherwise.</returns>
        bool IsServiceRegistered<T>(string name = null) where T : IMixedRealityService;  // todo: namespace?

        /// <summary>
        /// Gets the instance of the registered service.
        /// </summary>
        /// <typeparam name="T">The interface type of the service (ex: IMixedRealityBoundarySystem).
        /// <param name="name">The name of the service.</param>
        /// <param name="showLogs">Indicates whether or not diagnostic logging should be performed in case of an error</param>
        /// <returns>The registered service instance as the requested type.</returns>
        T GetService<T>(string name = null, bool showLogs = true) where T : IMixedRealityService;  // todo: namespace?

        /// <summary>
        /// Gets the collection of the registered service instances matching the requested type.
        /// </summary>
        /// <returns>Read-only collection of the service instances, as IMixedRealityService.</returns>
        IReadOnlyList<IMixedRealityService> GetServices();

        /// <summary>
        /// Gets the collection of the registered service instances matching the requested type.
        /// </summary>
        /// <typeparam name="T">The interface type of the service (ex: IMixedRealityBoundarySystem).
        /// <returns>Read-only collection of the service instances, as tye requested type.</returns>
        IReadOnlyList<T> GetServices<T>() where T : IMixedRealityService;

        #endregion IMixedRealityServce registration

        #region IMixedRealityDataProvider registration

        /// <summary>
        /// Registers a service of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the data provider to be registered.</typeparam>
        /// <param name="dataProviderInstance">An instance of the data provider to be registered.</param>
        bool RegisterDataProvider<T>(IMixedRealityDataProvider dataProviderInstance) where T : IMixedRealityDataProvider;

        /// <summary>
        /// Registers a data provider of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the data provider to be registered.
        /// <returns>True if the data provider was successfully registered, false otherwise.</returns>
        bool RegisterDataProvider<T>(
            Type concreteType,
            SupportedPlatforms supportedPlatforms = (SupportedPlatforms)(-1),
            params object[] args) where T : IMixedRealityDataProvider;

        /// <summary>
        /// Unregisters a data provider.
        /// </summary>
        /// <param name="name">The name of the data provider to unregister.</param>
        /// <returns>True if the data provider was successfully unregistered, false otherwise.</returns>
        bool UnregisterDataProvider(string name); // todo: namespace?

        /// <summary>
        /// Unregisters a data provider of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the data provider to be unregistered.
        /// <param name="name">The name of the data provider to unregister.</param>
        /// <returns>True if the data provider was successfully unregistered, false otherwise.</returns>
        /// <remarks>If the name argument is not specified, the first instance will be unregistered</remarks>
        bool UnregisterDataProvider<T>(string name = null) where T : IMixedRealityDataProvider; // todo: namespace?

        /// <summary>
        /// Unregisters a data provider.
        /// </summary>
        /// <param name="service">The specific data provider instance to unregister.</param>
        /// <returns>True if the data provider was successfully unregistered, false otherwise.</returns>
        bool UnregisterDataProviderService(IMixedRealityDataProvider dataProviderInstance);

        /// <summary>
        /// Unregisters all data providers.
        /// </summary>
        bool UnregisterDataProviders();

        /// <summary>
        /// Unregisters all data providers.
        /// </summary>
        /// <typeparam name="T">The interface type of the data providers to be unregistered.
        bool UnregisterDataProviders<T>() where T : IMixedRealityDataProvider;

        /// <summary>
        /// Gets the instance of the registered data provider.
        /// </summary>
        /// <param name="name">The name of the data provider.</param>
        /// <returns>The registered data provider instance, as IMixedRealityDataProvider.</returns>
        IMixedRealityDataProvider GetDataProvider(string name); // todo: namespace?

        /// <summary>
        /// Gets the instance of the registered data provider of the specified type.
        /// </summary>
        /// <typeparam name="T">The interface type of the data provider.
        /// <param name="name">The name of the data provider.</param>
        /// <returns>The registered data provider instance as the requested type.</returns>
        T GetDataProvider<T>(string name = null) where T : IMixedRealityDataProvider; // todo: namespace?

        /// <summary>
        /// Gets the collection of the registered data provider instances matching the requested type.
        /// </summary>
        /// <returns>Read-only collection of the data provider instances, as IMixedRealitydata provider.</returns>
        IReadOnlyList<IMixedRealityDataProvider> GetDataProviders();

        /// <summary>
        /// Gets the collection of the registered data provider instances matching the requested type.
        /// </summary>
        /// <typeparam name="T">The interface type of the data provider.
        /// <returns>Read-only collection of the data provider instances, as tye requested type.</returns>
        IReadOnlyList<T> GetDataProviders<T>() where T : IMixedRealityDataProvider;

        #endregion IMixedRealityDataProvider registration
    }
}
