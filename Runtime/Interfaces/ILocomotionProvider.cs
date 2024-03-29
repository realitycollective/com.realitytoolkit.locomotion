// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.InputSystem.Definitions;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using UnityEngine;

namespace RealityToolkit.Locomotion.Interfaces
{
    /// <summary>
    /// The base interface to define locomotion providers for the <see cref="ILocomotionService"/>.
    /// </summary>
    public interface ILocomotionProvider : ILocomotionServiceModule,
        ILocomotionServiceHandler,
        IMixedRealityInputHandler,
        IMixedRealityInputHandler<float>,
        IMixedRealityInputHandler<Vector2>
    {
        /// <summary>
        /// Gets whether this <see cref="ILocomotionProvider"/> is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// The input action used to perform locomotion using this provider.
        /// </summary>
        MixedRealityInputAction InputAction { get; }
    }
}
