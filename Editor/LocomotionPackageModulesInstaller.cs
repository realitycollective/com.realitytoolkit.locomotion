// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Editor.Packages;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Locomotion.Definitions;
using RealityToolkit.Locomotion.Interfaces;
using System.Linq;
using UnityEditor;

namespace RealityToolkit.Locomotion.Editor
{
    /// <summary>
    /// Installs <see cref="ILocomotionServiceModule"/>s coming from a third party package
    /// into the <see cref="LocomotionServiceProfile"/> in the <see cref="ServiceManager.ActiveProfile"/>.
    /// </summary>
    [InitializeOnLoad]
    public sealed class LocomotionPackageModulesInstaller : IPackageModulesInstaller
    {
        /// <summary>
        /// Statis initalizer for the installer instance.
        /// </summary>
        static LocomotionPackageModulesInstaller()
        {
            if (Instance == null)
            {
                Instance = new LocomotionPackageModulesInstaller();
            }

            PackageInstaller.RegisterModulesInstaller(Instance);
        }

        /// <summary>
        /// Internal singleton instance of the installer.
        /// </summary>
        private static LocomotionPackageModulesInstaller Instance { get; }

        /// <inheritdoc/>
        public bool Install(ServiceConfiguration serviceConfiguration)
        {
            if (!typeof(ILocomotionServiceModule).IsAssignableFrom(serviceConfiguration.InstancedType.Type))
            {
                // This module installer does not accept the configuration type.
                return false;
            }

            if (!ServiceManager.IsActiveAndInitialized)
            {
                UnityEngine.Debug.LogWarning($"Could not install {serviceConfiguration.InstancedType.Type.Name}.{nameof(ServiceManager)} is not initialized.");
                return false;
            }

            if (!ServiceManager.Instance.HasActiveProfile)
            {
                UnityEngine.Debug.LogWarning($"Could not install {serviceConfiguration.InstancedType.Type.Name}.{nameof(ServiceManager)} has no active profile.");
                return false;
            }

            if (!ServiceManager.Instance.TryGetServiceProfile<ILocomotionService, LocomotionServiceProfile>(out var locomotionServiceProfile))
            {
                UnityEngine.Debug.LogWarning($"Could not install {serviceConfiguration.InstancedType.Type.Name}.{nameof(LocomotionServiceProfile)} not found.");
                return false;
            }

            // Setup the configuration.
            var typedServiceConfiguration = new ServiceConfiguration<ILocomotionServiceModule>(serviceConfiguration.InstancedType.Type, serviceConfiguration.Name, serviceConfiguration.Priority, serviceConfiguration.RuntimePlatforms, serviceConfiguration.Profile);

            // Make sure it's not already in the target profile.
            if (locomotionServiceProfile.ServiceConfigurations.All(sc => sc.InstancedType.Type != serviceConfiguration.InstancedType.Type))
            {
                locomotionServiceProfile.AddConfiguration(typedServiceConfiguration);
                UnityEngine.Debug.Log($"Successfully installed the {serviceConfiguration.InstancedType.Type.Name} to {locomotionServiceProfile.name}.");
            }
            else
            {
                UnityEngine.Debug.Log($"Skipped installing the {serviceConfiguration.InstancedType.Type.Name} to {locomotionServiceProfile.name}. Already installed.");
            }

            return true;
        }
    }
}
