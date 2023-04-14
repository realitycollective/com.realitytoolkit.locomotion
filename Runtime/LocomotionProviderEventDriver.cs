// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

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
    /// This component is attached to the main <see cref="Camera"/> by the <see cref="ILocomotionService"/>
    /// and provides an event bridge to active <see cref="ILocomotionProvider"/> implementations.
    /// It has a hard dependency on the <see cref="ILocomotionService"/> as well as the <see cref="IMixedRealityInputSystem"/>
    /// and cannot work without both being active and enabled in the application.
    /// Furthermore it expects that the <see cref="GameObject"/> it is attached to is a global <see cref="IMixedRealityInputSystem"/> listener. It will
    /// not take care of registration itself.
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

            // We've been destroyed during the await.
            if (this == null) { return; }

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
                TogglePointers(false, inputSource);
            }

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportTargetRequested(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnTeleportStarted(LocomotionEventData eventData)
        {
            TogglePointers(false);

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportStarted(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnTeleportCompleted(LocomotionEventData eventData)
        {
            TogglePointers(true);

            for (int i = 0; i < LocomotionService.EnabledLocomotionProviders.Count; i++)
            {
                LocomotionService.EnabledLocomotionProviders[i].OnTeleportCompleted(eventData);
            }
        }

        /// <inheritdoc />
        public virtual void OnTeleportCanceled(LocomotionEventData eventData)
        {
            TogglePointers(true);

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

        private void TogglePointers(bool isOn, IMixedRealityInputSource targetInputSource = null)
        {
            if (targetInputSource != null)
            {
                foreach (var pointer in targetInputSource.Pointers)
                {
                    pointer.IsTeleportRequestActive = !isOn;
                    pointer.BaseCursor?.SetVisibility(isOn);
                }

                return;
            }

            foreach (var inputSource in InputService.DetectedInputSources)
            {
                foreach (var pointer in inputSource.Pointers)
                {
                    pointer.IsTeleportRequestActive = !isOn;
                    pointer.BaseCursor?.SetVisibility(isOn);
                }
            }
        }
    }
}
