// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Interfaces;
using RealityToolkit.Input.Interfaces;
using RealityToolkit.Locomotion.Teleporting;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// A service implementing locomotion for immersive experiences.
    /// </summary>
    public interface ILocomotionService : IEventService
    {
        /// <summary>
        /// The active <see cref="ILocomotionTarget"/>.
        /// </summary>
        ILocomotionTarget LocomotionTarget { get; set; }

        /// <summary>
        /// Gets whether teleport locomotion is currently in cooldown. While in cooldown,
        /// no new teleport can be requested.
        /// </summary>
        bool IsTeleportCoolingDown { get; }

        /// <summary>
        /// Gets a list of currently enabled <see cref="ILocomotionProvider"/>s.
        /// </summary>
        IReadOnlyList<ILocomotionProvider> EnabledLocomotionProviders { get; }

        /// <summary>
        /// Enables a locomotion provider of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="ILocomotionProvider"/> to enable.</typeparam>
        void EnableLocomotionProvider<T>() where T : ILocomotionProvider;

        /// <summary>
        /// Enables a locomotion provider of type <paramref name="locomotionProviderType"/>.
        /// </summary>
        /// <paramref name="locomotionProviderType">Type of the <see cref="ILocomotionProvider"/> to enable.</typeparam>
        void EnableLocomotionProvider(Type locomotionProviderType);

        /// <summary>
        /// Disables a locomotion provider of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="ILocomotionProvider"/> to disable.</typeparam>
        void DisableLocomotionProvider<T>() where T : ILocomotionProvider;

        /// <summary>
        /// Disables a locomotion provider of type <paramref name="locomotionProviderType"/>.
        /// </summary>
        /// <paramref name="locomotionProviderType">Type of the <see cref="ILocomotionProvider"/> to disable.</typeparam>
        void DisableLocomotionProvider(Type locomotionProviderType);

        /// <summary>
        /// A <see cref="ILocomotionProvider"/> was enabled.
        /// </summary>
        /// <param name="locomotionProvider">The enabled <see cref="ILocomotionProvider"/>.</param>
        void OnLocomotionProviderEnabled(ILocomotionProvider locomotionProvider);

        /// <summary>
        /// A <see cref="ILocomotionProvider"/> was disabled.
        /// </summary>
        /// <param name="locomotionProvider">The disabled <see cref="ILocomotionProvider"/>.</param>
        void OnLocomotionProviderDisabled(ILocomotionProvider locomotionProvider);

        /// <summary>
        /// Raise a teleportation target request event.
        /// </summary>
        /// <param name="teleportLocomotionProvider">The <see cref="ITeleportLocomotionProvider"/> that requests a teleport target.</param>
        /// <param name="inputSource">The <see cref="IInputSource"/> the <paramref name="teleportLocomotionProvider"/> requests the teleport location from.</param>
        void RaiseTeleportTargetRequest(ITeleportLocomotionProvider teleportLocomotionProvider, IInputSource inputSource);

        /// <summary>
        /// Raises a teleportation started event for <see cref="ILocomotionServiceHandler"/>s.
        /// </summary>
        /// <param name="locomotionProvider">The <see cref="ITeleportLocomotionProvider"/> that started teleportation.</param>
        /// <param name="inputSource">The <see cref="IInputSource"/> the <paramref name="locomotionProvider"/>'s teleport request originated from.</param>
        /// <param name="pose">The target <see cref="Pose"/> the teleportation is going for.</param>
        /// <param name="anchor">The teleport target anchor, if any.</param>
        void RaiseTeleportStarted(ITeleportLocomotionProvider locomotionProvider, IInputSource inputSource, Pose pose, ITeleportAnchor anchor);

        /// <summary>
        /// Raises a teleportation completed event for <see cref="ILocomotionServiceHandler"/>s.
        /// </summary>
        /// <param name="locomotionProvider">The <see cref="ITeleportLocomotionProvider"/> whose teleportation has completed.</param>
        /// <param name="inputSource">The <see cref="IInputSource"/> the <paramref name="locomotionProvider"/>'s teleport request originated from.</param>
        /// <param name="pose">The target <see cref="Pose"/> the teleportation was going for.</param>
        /// <param name="anchor">The teleport target anchor, if any.</param>
        void RaiseTeleportCompleted(ITeleportLocomotionProvider locomotionProvider, IInputSource inputSource, Pose pose, ITeleportAnchor anchor);

        /// <summary>
        /// Raises a teleportation canceled event for <see cref="ILocomotionServiceHandler"/>s.
        /// </summary>
        /// <param name="locomotionProvider">The <see cref="ITeleportLocomotionProvider"/> that canceled a previously started teleport.</param>
        /// <param name="inputSource">The <see cref="IInputSource"/> the <paramref name="locomotionProvider"/>'s teleport request originated from.</param>
        void RaiseTeleportCanceled(ITeleportLocomotionProvider locomotionProvider, IInputSource inputSource);
    }
}
