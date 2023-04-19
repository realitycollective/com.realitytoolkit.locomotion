// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Interfaces;
using RealityToolkit.InputSystem.Interfaces.Handlers;
using RealityToolkit.Locomotion.Definitions;
using RealityToolkit.Locomotion.Interfaces;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// This component is an event bridge to active <see cref="ILocomotionProvider"/> implementations.
    /// It has a hard dependency on the <see cref="ILocomotionService"/> as well as the <see cref="IMixedRealityInputSystem"/>
    /// and cannot work without both being active and enabled in the application. It will additionally manage active <see cref="IMixedRealityPointer"/>s
    /// while a teleport locomotion is active.
    /// The <see cref="ILocomotionService"/> will ensure a <see cref="GameObject"/> with this component attached is created, when the
    /// service is enabled. You do not need to manually place it in the scene.
    /// </summary>
    public class LocomotionProviderEventDriver : MonoBehaviour,
        ILocomotionServiceHandler,
        IMixedRealityInputHandler,
        IMixedRealityInputHandler<float>,
        IMixedRealityInputHandler<Vector2>
    {
        private IMixedRealityInputSystem inputService = null;
        /// <summary>
        /// Gets the currently active <see cref="IMixedRealityInputSystem"/> instance.
        /// </summary>
        protected IMixedRealityInputSystem InputService
            => inputService ??= ServiceManager.Instance.GetService<IMixedRealityInputSystem>();

        private ILocomotionService locomotionService = null;
        /// <summary>
        /// Gets the currently active <see cref="Interfaces.ILocomotionService"/> instance.
        /// </summary>
        protected ILocomotionService LocomotionService
            => locomotionService ??= ServiceManager.Instance.GetService<ILocomotionService>();

        /// <summary>
        /// This method is called just before any of the update methods is called the first time.
        /// </summary>
        protected virtual async void Start()
        {
            try
            {
                locomotionService = await ServiceManager.Instance.GetServiceAsync<ILocomotionService>();
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
                return;
            }

            if (this.IsNull())
            {
                // We've been destroyed during the await.
                return;
            }

            LocomotionService?.Register(gameObject);
        }

        /// <summary>
        /// This method is called when the behaviour will be destroyed.
        /// </summary>
        protected virtual void OnDestroy() => LocomotionService?.Unregister(gameObject);

        /// <inheritdoc />
        public virtual void OnTeleportTargetRequested(LocomotionEventData eventData)
        {
            if (InputService.TryGetInputSource(eventData.EventSource.SourceId, out var inputSource))
            {
                TogglePointers(false, true, inputSource);
            }

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportTargetRequested(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnTeleportStarted(LocomotionEventData eventData)
        {
            if (InputService.TryGetInputSource(eventData.EventSource.SourceId, out var inputSource))
            {
                TogglePointers(false, true, inputSource);
            }

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportStarted(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnTeleportCompleted(LocomotionEventData eventData)
        {
            if (InputService.TryGetInputSource(eventData.EventSource.SourceId, out var inputSource))
            {
                TogglePointers(true, false, inputSource);
            }

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportCompleted(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnTeleportCanceled(LocomotionEventData eventData)
        {
            if (InputService.TryGetInputSource(eventData.EventSource.SourceId, out var inputSource))
            {
                TogglePointers(true, false, inputSource);
            }

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportCanceled(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnInputChanged(InputEventData<float> eventData)
        {
            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnInputChanged(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnInputChanged(InputEventData<Vector2> eventData)
        {
            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnInputChanged(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnInputDown(InputEventData eventData)
        {
            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnInputDown(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnInputUp(InputEventData eventData)
        {
            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnInputUp(eventData);
            }
        }

        private void TogglePointers(bool isOn, bool teleportInProgress, IMixedRealityInputSource teleportInputSource = null)
        {
            foreach (var inputSource in InputService.DetectedInputSources)
            {
                var isTeleportInputSource = inputSource.SourceId == teleportInputSource.SourceId;

                foreach (var pointer in inputSource.Pointers)
                {
                    if (isTeleportInputSource && pointer is ITeleportTargetProvider _)
                    {
                        // If this pointer is the one handling the teleport and providing a target,
                        // we do not want to mess with its state as it will manage it internally.
                        continue;
                    }

                    pointer.IsTeleportRequestActive = teleportInProgress;

                    if (pointer.BaseCursor != null)
                    {
                        // The pointer might be a teleport target provider, in which case we do not want
                        // to enable it in any case.
                        pointer.BaseCursor.IsVisible = isOn && !(pointer is ITeleportTargetProvider);
                    }
                }
            }
        }
    }
}
