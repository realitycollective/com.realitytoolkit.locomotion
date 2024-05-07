// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Definitions.Utilities;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Configuration profile settings for <see cref="LocomotionService"/>.
    /// </summary>
    public class LocomotionServiceProfile : BaseServiceProfile<ILocomotionServiceModule>
    {
        [SerializeField]
        [Tooltip("Sets startup behaviour for locomotion in general.")]
        private AutoStartBehavior locomotionStartupBehaviour = AutoStartBehavior.AutoStart;

        /// <summary>
        /// Gets startup behaviour for locomotion in general.
        /// </summary>
        public AutoStartBehavior LocomotionStartupBehaviour
        {
            get => locomotionStartupBehaviour;
            internal set => locomotionStartupBehaviour = value;
        }

        [Header("Movement")]
        [SerializeField]
        [Tooltip("Sets startup behaviour for free movement.")]
        private AutoStartBehavior movementStartupBehaviour = AutoStartBehavior.AutoStart;

        /// <summary>
        /// Gets startup behaviour for free movement.
        /// </summary>
        public AutoStartBehavior MovementStartupBehaviour
        {
            get => movementStartupBehaviour;
            internal set => movementStartupBehaviour = value;
        }

        [Header("Teleportation")]
        [SerializeField]
        [Tooltip("Sets startup behaviour for teleportation.")]
        private AutoStartBehavior teleportationStartupBehaviour = AutoStartBehavior.AutoStart;

        /// <summary>
        /// Gets startup behaviour for teleportation.
        /// </summary>
        public AutoStartBehavior TeleportationStartupBehaviour
        {
            get => teleportationStartupBehaviour;
            internal set => teleportationStartupBehaviour = value;
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
