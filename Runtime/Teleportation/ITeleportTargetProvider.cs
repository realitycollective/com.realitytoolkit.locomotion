﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Input.Interfaces;
using UnityEngine;

namespace RealityToolkit.Locomotion.Teleportation
{
    /// <summary>
    /// Interface definition for components and services providing teleportation targets for
    /// <see cref="ITeleportLocomotionProvider"/>s. Whenever an <see cref="ITeleportLocomotionProvider"/>
    /// requests a teleportation target, the target provider will answer the request, if both share
    /// the same <see cref="IInputSource"/> connectinon.
    /// </summary>
    public interface ITeleportTargetProvider : ILocomotionServiceHandler
    {
        /// <summary>
        /// Gets the <see cref="ILocomotionProvider"/> that is currently requesting a teleport target
        /// from this provider, if any.
        /// </summary>
        /// <remarks>Can be <see cref="null"/> if this provider has not received a target request at the time of evaluation.</remarks>
        ILocomotionProvider RequestingLocomotionProvider { get; }

        /// <summary>
        /// Gets whether the <see cref="ITeleportTargetProvider"/> is currently
        /// targeting / looking for a target to answer a teleport target request made by a <see cref="ITeleportLocomotionProvider"/>.
        /// </summary>
        bool IsTargeting { get; }

        /// <summary>
        /// Gets the <see cref="IInputSource"/> this provider is
        /// connected with. Each <see cref="ITeleportTargetProvider"/> must be
        /// assigned an input source so targets can be mapped to the
        /// <see cref="RequestingLocomotionProvider"/>'s input source.
        /// </summary>
        IInputSource InputSource { get; }

        /// <summary>
        /// Gets the target pose provided, if any.
        /// </summary>
        Pose? TargetPose { get; }

        /// <summary>
        /// Gets the target anchor, if any.
        /// </summary>
        ITeleportAnchor Anchor { get; }

        /// <summary>
        /// Gets the validation result for the current target location. The provider should only
        /// provide valid teleportation results to <see cref="ITeleportLocomotionProvider"/>s.
        /// </summary>
        TeleportValidationResult ValidationResult { get; }
    }
}
