﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions;
using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityToolkit.Input.Definitions;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Base configuration profile for <see cref="ILocomotionProvider"/>s. Use the <see cref="BaseLocomotionProvider"/>
    /// base class to get started implementing your own provider.
    /// </summary>
    public class BaseLocomotionProviderProfile : BaseProfile
    {
        [SerializeField]
        [Tooltip("Sets startup behaviour for this provider.")]
        private AutoStartBehavior startupBehaviour = AutoStartBehavior.ManualStart;

        /// <summary>
        /// Gets startup behaviour for this provider.
        /// </summary>
        public AutoStartBehavior StartupBehaviour
        {
            get => startupBehaviour;
            internal set => startupBehaviour = value;
        }

        [SerializeField]
        [Tooltip("Input action to perform locomotion using this provider.")]
        private InputAction inputAction = InputAction.None;

        /// <summary>
        /// Gets input action to perform locomotion using this provider.
        /// </summary>
        public InputAction InputAction
        {
            get => inputAction;
            internal set => inputAction = value;
        }
    }
}
