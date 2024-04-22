// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Input.Interactors;
using RealityToolkit.Locomotion.Definitions;
using UnityEngine;

namespace RealityToolkit.Locomotion.Teleporting
{
    /// <summary>
    /// Interface to define teleportation validation service modules. A <see cref="ITeleportValidationServiceModule"/>
    /// is responsible for validating whether a given teleportation target position is valid or not.
    /// </summary>
    public interface ITeleportValidationServiceModule : ILocomotionServiceModule
    {
        /// <summary>
        /// Validates a <see cref="IInteractorResult"/> and returns whether the <see cref="RaycastHit"/>
        /// qualifies for teleporation.
        /// </summary>
        /// <param name="pointerResult">The <see cref="IInteractorResult"/> to validate.</param>
        /// <param name="anchor"><see cref="ITeleportAnchor"/> found at the target position, if any.</param>
        /// <returns>The <see cref="TeleportValidationResult"/> for <paramref name="pointerResult"/>.</returns>
        TeleportValidationResult IsValid(IInteractorResult pointerResult, ITeleportAnchor anchor = null);
    }
}
