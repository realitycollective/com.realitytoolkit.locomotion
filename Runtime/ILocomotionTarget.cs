// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// A <see cref="ILocomotionTarget"/> is the target object being moved by the
    /// <see cref="ILocomotionService"/>.
    /// </summary>
    public interface ILocomotionTarget
    {
        /// <summary>
        /// The <see cref="ILocomotionTarget"/>'s pose in world space.
        /// </summary>
        Pose Pose { get; }

        /// <summary>
        /// Rotates the <see cref="ILocomotionTarget"/> at its current position
        /// around <paramref name="axis"/> by <paramref name="angle"/>.
        /// </summary>
        /// <param name="axis">Axis to rotate around.</param>
        /// <param name="angle">Angle to rotate.</param>
        void RotateAround(Vector3 axis, float angle);

        /// <summary>
        /// Sets the world space position and rotation of the <see cref="ILocomotionTarget"/>.
        /// </summary>
        /// <param name="position">The world space position.</param>
        /// <param name="rotation">The world space rotation.</param>
        void SetPositionAndRotation(Vector3 position, Quaternion rotation);

        /// <summary>
        /// Sets the world space position and rotation of the <see cref="ILocomotionTarget"/>.
        /// </summary>
        /// <param name="position">The world space position.</param>
        /// <param name="rotation">The world space rotation.</param>
        void SetPositionAndRotation(Vector3 position, Vector3 rotation);

        /// <summary>
        /// Moves the <see cref="ILocomotionTarget"/> in <paramref name="direction"/> on the (X,Z) plane..
        /// </summary>
        /// <param name="direction">The direction <see cref="Vector2"/>.</param>
        /// <param name="speed">The speed multiplier for the movement. Defaults to <c>1f</c>.</param>
        void Move(Vector2 direction, float speed = 1f);

        /// <summary>
        /// Moves the <see cref="ILocomotionTarget"/> in <paramref name="direction"/>.
        /// </summary>
        /// <param name="direction">The direction <see cref="Vector3"/>.</param>
        /// <param name="speed">The speed multiplier for the movement. Defaults to <c>1f</c>.</param>
        void Move(Vector3 direction, float speed = 1f);
    }
}
