// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace RealityToolkit.Locomotion.Teleportation
{
    /// <summary>
    /// A teleportation anchor supported by the <see cref="ILocomotionService"/>.
    /// A teleport anchor is a predefined teleportation <see cref="Position"/> and optionally <see cref="TargetOrientation"/>.
    /// It is always a valid teleport target when <see cref="IsEnabled"/>.
    /// </summary>
    public interface ITeleportAnchor
    {
        /// <summary>
        /// Gets whether this <see cref="ITeleportAnchor"/> is enabled and
        /// can be teleported to.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// The position the teleport will end at.
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// The normal of the teleport raycast.
        /// </summary>
        Vector3 Normal { get; }

        /// <summary>
        /// Should the target orientation be overridden?
        /// </summary>
        bool OverrideTargetOrientation { get; }

        /// <summary>
        /// If <see cref="OverrideTargetOrientation"/> is set, this will specify the player's target
        /// orientation on the Y-axis.
        /// </summary>
        float TargetOrientation { get; }

        /// <summary>
        /// The <see cref="ITeleportAnchor"/> is being targeted for teleportation but teleportation
        /// has taken place. It may not happen.
        /// </summary>
        event Action Targeted;

        /// <summary>
        /// The <see cref="ITeleportAnchor"/> has been teleported to.
        /// </summary>
        event Action Activated;
    }
}
