// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Definitions.Utilities;
using RealityToolkit.Input.Attributes;
using RealityToolkit.Input.Definitions;
using UnityEngine;

namespace RealityToolkit.Locomotion.Movement
{
    /// <summary>
    /// Configuration profile for the <see cref="SmoothLocomotionProvider"/>.
    /// </summary>
    public class SmoothLocomotionProviderProfile : BaseLocomotionProviderProfile
    {
        [SerializeField, Range(1f, 100f), Tooltip("Speed in meters per second for movement.")]
        private float speed = 3f;

        /// <summary>
        /// Speed in meters per second for movement.
        /// </summary>
        public float Speed => speed;

        [SerializeField, AxisConstraint(AxisType.Digital), Tooltip("Input action to listen for to enable running speed.")]
        private InputAction runInputAction = InputAction.None;

        /// <summary>
        /// Input action to listen for to enable <see cref="RunningSpeed"/>.
        /// </summary>
        public InputAction RunInputAction => runInputAction;

        [SerializeField, Range(1f, 100f), Tooltip("Speed applied when running.")]
        private float runningSpeed = 5f;

        /// <summary>
        /// Speed applied when the <see cref="RunInputAction"/> is active.
        /// </summary>
        public float RunningSpeed => runningSpeed;
    }
}
