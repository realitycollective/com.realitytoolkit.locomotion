// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Definitions.Utilities;
using RealityCollective.ServiceFramework.Definitions;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Configuration profile settings for <see cref="LocomotionService"/>.
    /// </summary>
    public class LocomotionServiceProfile : BaseServiceProfile<ILocomotionServiceModule>
    {
        [SerializeField]
        [Tooltip("Sets startup behaviour for locomotion.")]
        private AutoStartBehavior startupBehaviour = AutoStartBehavior.AutoStart;

        /// <summary>
        /// Gets startup behaviour for locomotion.
        /// </summary>
        public AutoStartBehavior StartupBehaviour
        {
            get => startupBehaviour;
            internal set => startupBehaviour = value;
        }

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
