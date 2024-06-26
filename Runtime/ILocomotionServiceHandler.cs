﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine.EventSystems;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Interface to implement for handling locomotion events by the <see cref="ILocomotionService"/>.
    /// </summary>
    public interface ILocomotionServiceHandler : IEventSystemHandler
    {
        /// <summary>
        /// Raised when a <see cref="Movement.IFreeLocomotionProvider"/> is executing movement.
        /// </summary>
        /// <param name="eventData"><see cref="LocomotionEventData"/> provided.</param>
        void OnMoving(LocomotionEventData eventData);

        /// <summary>
        /// Raised when a <see cref="Teleportation.ITeleportLocomotionProvider"/> requests a
        /// target location for teleport, but no teleportation has started.
        /// </summary>
        /// <param name="eventData"><see cref="LocomotionEventData"/> provided.</param>
        void OnTeleportTargetRequested(LocomotionEventData eventData);

        /// <summary>
        /// Raised when a <see cref="Teleportation.ITeleportLocomotionProvider"/> has started
        /// teleportation to a target location that was provided by a <see cref="Teleportation.ITeleportTargetProvider"/>.
        /// </summary>
        /// <param name="eventData"><see cref="LocomotionEventData"/> provided.</param>
        void OnTeleportStarted(LocomotionEventData eventData);

        /// <summary>
        /// Raised when a <see cref="Teleportation.ITeleportLocomotionProvider"/> has successfully
        /// completed teleportation.
        /// </summary>
        /// <param name="eventData"><see cref="LocomotionEventData"/> provided.</param>
        void OnTeleportCompleted(LocomotionEventData eventData);

        /// <summary>
        /// Raised when a <see cref="Teleportation.ITeleportLocomotionProvider"/> has canceled
        /// teleportation.
        /// </summary>
        /// <param name="eventData"><see cref="LocomotionEventData"/> provided.</param>
        void OnTeleportCanceled(LocomotionEventData eventData);
    }
}
