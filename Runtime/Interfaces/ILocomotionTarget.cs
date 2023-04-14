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
        /// Gets the <see cref="Transform"/> that should be moved by locomotion.
        /// </summary>
        Transform MoveTransform { get; }

        /// <summary>
        /// Gets the <see cref="Transform"/> that should be used to determine orientation when performing locomotion.
        /// May or may not be the same as <see cref="MoveTransform"/>.
        /// </summary>
        Transform OrientationTransform { get; }
    }
}
