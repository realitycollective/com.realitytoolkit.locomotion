// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;
using UnityEngine.Events;

namespace RealityToolkit.Locomotion.UX
{
    /// <summary>
    /// Exposes locomotion events in the inspector.
    /// </summary>
    [AddComponentMenu(RealityToolkitRuntimePreferences.Toolkit_AddComponentMenu + "/Locomotion/" + nameof(LocomotionEvents))]
    public class LocomotionEvents : LocomotionHandler
    {
        [SerializeField]
        private UnityEvent<LocomotionEventData> onMoving = null;

        [Space, SerializeField]
        private UnityEvent<LocomotionEventData> onTeleportTargetRequested = null;

        [Space, SerializeField]
        private UnityEvent<LocomotionEventData> onTeleportStarted = null;

        [Space, SerializeField]
        private UnityEvent<LocomotionEventData> onTeleportCompleted = null;

        [Space, SerializeField]
        private UnityEvent<LocomotionEventData> onTeleportCanceled = null;

        /// <inheritdoc/>
        public override void OnMoving(LocomotionEventData eventData) => onMoving?.Invoke(eventData);

        /// <inheritdoc/>
        public override void OnTeleportTargetRequested(LocomotionEventData eventData) => onTeleportTargetRequested?.Invoke(eventData);

        /// <inheritdoc/>
        public override void OnTeleportStarted(LocomotionEventData eventData) => onTeleportStarted?.Invoke(eventData);

        /// <inheritdoc/>
        public override void OnTeleportCompleted(LocomotionEventData eventData) => onTeleportCompleted?.Invoke(eventData);

        /// <inheritdoc/>
        public override void OnTeleportCanceled(LocomotionEventData eventData) => onTeleportCanceled?.Invoke(eventData);
    }
}
