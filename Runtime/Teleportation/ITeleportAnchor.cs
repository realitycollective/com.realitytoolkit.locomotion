// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using UnityEngine;

namespace RealityToolkit.Locomotion.Teleportation
{
    public delegate void OnTargetedChangedDelegate(bool isTargeted);

    /// <summary>
    /// A teleportation anchor supported by the <see cref="ILocomotionService"/>.
    /// A teleport anchor is a predefined teleportation <see cref="Position"/> and optionally <see cref="TargetOrientation"/>.
    /// It is always a valid teleport target when <see cref="IsEnabled"/>.
    /// </summary>
    public interface ITeleportAnchor
    {
        /// <summary>
        /// Is this <see cref="ITeleportAnchor"/> enabled and can be teleported to?
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Is this <see cref="ITeleportAnchor"/> currently being targeted by a <see cref="ITeleportTargetProvider"/>?
        /// </summary>
        bool IsTargeted { get; }

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
        /// The <see cref="ITeleportAnchor"/> is being targeted by a <see cref="ITeleportTargetProvider"/>
        /// or it has stopped targeting it.
        /// </summary>
        event OnTargetedChangedDelegate TargetedChanged;

        /// <summary>
        /// The <see cref="ITeleportAnchor"/> has been teleported to.
        /// </summary>
        event Action Activated;
    }
}
