// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Input.Definitions;
using RealityToolkit.Input.Interfaces.Handlers;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// The base interface to define locomotion providers for the <see cref="ILocomotionService"/>.
    /// </summary>
    public interface ILocomotionProvider : ILocomotionServiceModule,
        ILocomotionServiceHandler,
        IInputHandler,
        IInputHandler<float>,
        IInputHandler<Vector2>
    {
        /// <summary>
        /// Gets whether this <see cref="ILocomotionProvider"/> is currently active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// The input action used to perform locomotion using this provider.
        /// </summary>
        InputAction InputAction { get; }
    }
}
