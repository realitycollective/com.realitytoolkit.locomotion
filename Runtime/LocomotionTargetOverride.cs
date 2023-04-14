﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Locomotion.Interfaces;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Attach this component to any <see cref="GameObject"/> to make that object's
    /// <see cref="Transform"/> the target for locomotion when the component is enabled,
    /// that is the object being translated in space when locomotion occurs.
    ///
    /// When no enabled instance of <see cref="LocomotionTargetOverride"/> is found the
    /// <see cref="Services.LocomotionSystem.LocomotionSystem"/> will target the active
    /// <see cref="Interfaces.CameraSystem.IMixedRealityCameraRig.RigTransform"/> if the
    /// <see cref="Interfaces.CameraSystem.IMixedRealityCameraSystem"/> is active or fallback to
    /// <see cref="Utilities.CameraCache.Main"/> parent ultimately.
    /// </summary>
    public class LocomotionTargetOverride : MonoBehaviour
    {
        private LocomotionService locomotionSystem = null;
        /// <summary>
        /// Gets the currently active <see cref="Services.LocomotionSystem.LocomotionSystem"/> instance.
        /// </summary>
        protected LocomotionService LocomotionSystem
            => locomotionSystem ?? (locomotionSystem = ServiceManager.Instance.GetService<ILocomotionService>() as LocomotionService);

        /// <summary>
        /// This method is called just before any of the update methods is
        /// called for the fist time on this behaviour.
        /// </summary>
        private void Start()
        {
            if (LocomotionSystem == null)
            {
                Debug.LogError($"No active {nameof(LocomotionSystem)} found. {nameof(LocomotionTargetOverride)} can only work with the system enabled.");
                this.Destroy();
                return;
            }

            if (LocomotionSystem.LocomotionTargetOverride != null)
            {
                Debug.LogError($"There can only be one instance of {nameof(LocomotionTargetOverride)} in the scene!");
                this.Destroy();
                return;
            }

            LocomotionSystem.LocomotionTargetOverride = this;
        }

        /// <summary>
        /// This method is called when the behaviour will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (LocomotionSystem != null)
            {
                LocomotionSystem.LocomotionTargetOverride = null;
            }
        }
    }
}
