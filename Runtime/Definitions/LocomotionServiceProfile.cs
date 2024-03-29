﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using RealityToolkit.Locomotion.Interfaces;
using UnityEngine;

namespace RealityToolkit.Locomotion.Definitions
{
    /// <summary>
    /// Configuration profile settings for <see cref="LocomotionService"/>.
    /// </summary>
    public class LocomotionServiceProfile : BaseServiceProfile<ILocomotionServiceModule>
    {
        [SerializeField]
        [Tooltip("The teleportation cooldown defines the time that needs to pass after a successful teleportation for another one to be possible.")]
        [Range(0, 10f)]
        private float teleportCooldown = 1f;

        /// <summary>
        /// The teleportation cooldown defines the time that needs to pass after a successful teleportation for another one to be possible.
        /// </summary>
        public float TeleportCooldown
        {
            get => teleportCooldown;
            internal set => teleportCooldown = value;
        }
    }
}
