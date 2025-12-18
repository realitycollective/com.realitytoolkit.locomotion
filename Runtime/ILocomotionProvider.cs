// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.InputSystem;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// The base interface to define locomotion providers for the <see cref="ILocomotionService"/>.
    /// </summary>
    public interface ILocomotionProvider : ILocomotionServiceModule, ILocomotionServiceHandler
    {
        /// <summary>
        /// Gets whether this <see cref="ILocomotionProvider"/> is currently active.
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// The <see cref="UnityEngine.InputSystem.InputAction"/> used to perform locomotion using this provider.
        /// </summary>
        InputActionReference InputAction { get; }
    }
}
