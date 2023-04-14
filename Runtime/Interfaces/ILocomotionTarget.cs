// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace RealityToolkit.Locomotion.Interfaces
{
    /// <summary>
    /// A <see cref="ILocomotionTarget"/> is the target object being moved by the
    /// <see cref="ILocomotionService"/>.
    /// </summary>
    public interface ILocomotionTarget
    {
        /// <summary>
        /// Gets the target <see cref="Transform"/> for locomotion.
        /// </summary>
        Transform LocomotionTargetTransform { get; }
    }
}
