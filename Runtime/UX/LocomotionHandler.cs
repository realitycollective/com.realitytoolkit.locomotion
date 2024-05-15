// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Services;
using UnityEngine;

namespace RealityToolkit.Locomotion.UX
{
    /// <summary>
    /// Abstract base implementation for handling locomotion events in a component.
    /// </summary>
    public abstract class LocomotionHandler : MonoBehaviour, ILocomotionServiceHandler
    {
        private ILocomotionService locomotionService;

        /// <inheritdoc/>
        protected virtual async void OnEnable()
        {
            await ServiceManager.WaitUntilInitializedAsync();

            if (!ServiceManager.Instance.TryGetService(out locomotionService))
            {
                Debug.LogError($"{GetType().Name} reuires the {nameof(ILocomotionService)} to work.");
                return;
            }

            locomotionService.Register(gameObject);
        }

        /// <inheritdoc/>
        protected virtual void OnDisable()
        {
            if (locomotionService != null)
            {
                locomotionService.Unregister(gameObject);
            }
        }

        /// <inheritdoc/>
        public virtual void OnMoving(LocomotionEventData eventData) { }

        /// <inheritdoc/>
        public virtual void OnTeleportTargetRequested(LocomotionEventData eventData) { }

        /// <inheritdoc/>
        public virtual void OnTeleportStarted(LocomotionEventData eventData) { }

        /// <inheritdoc/>
        public virtual void OnTeleportCompleted(LocomotionEventData eventData) { }

        /// <inheritdoc/>
        public virtual void OnTeleportCanceled(LocomotionEventData eventData) { }
    }
}
