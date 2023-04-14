// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Definitions.Utilities;
using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Modules;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.InputSystem.Definitions;
using RealityToolkit.Locomotion.Definitions;
using RealityToolkit.Locomotion.Interfaces;
using UnityEngine;

namespace RealityToolkit.Locomotion.Modules
{
    public abstract class BaseLocomotionProvider : BaseServiceModule, ILocomotionProvider
    {
        /// <inheritdoc />
        public BaseLocomotionProvider(string name, uint priority, BaseLocomotionProviderProfile profile, ILocomotionService parentService)
            : base(name, priority, profile, parentService)
        {
            startupBehaviour = profile.StartupBehaviour;
            InputAction = profile.InputAction;
            LocomotionService = parentService;
        }

        private readonly AutoStartBehavior startupBehaviour;
        private bool isInitialized;

        /// <inheritdoc />
        public bool IsActive { get; protected set; }

        /// <inheritdoc />
        public MixedRealityInputAction InputAction { get; }

        /// <summary>
        /// Gets the active <see cref="ILocomotionService"/> instance.
        /// </summary>
        protected ILocomotionService LocomotionService { get; }

        /// <summary>
        /// Gets the player camera <see cref="Transform"/>.
        /// </summary>
        protected virtual Transform CameraTransform
        {
            get
            {
                return ServiceManager.Instance.TryGetService<ICameraService>(out var cameraSystem)
                    ? cameraSystem.CameraRig.CameraTransform
                    : Camera.main.transform;
            }
        }

        /// <summary>
        /// Gets the target <see cref="Transform"/> for locomotion.
        /// </summary>
        protected virtual Transform LocomotionTargetTransform
        {
            get
            {
                var targetOverride = LocomotionService.LocomotionTarget.LocomotionTargetTransform.GetComponentInParent<LocomotionTargetOverride>();
                if (targetOverride.IsNotNull())
                {
                    return targetOverride.transform;
                }

                return LocomotionService.LocomotionTarget.LocomotionTargetTransform;
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            if (IsActive)
            {
                return;
            }

            if (startupBehaviour == AutoStartBehavior.AutoStart || isInitialized)
            {
                IsActive = true;
                LocomotionService.OnLocomotionProviderEnabled(this);
            }
            else
            {
                Disable();
            }

            isInitialized = true;
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            if (!IsActive)
            {
                return;
            }

            IsActive = false;
            LocomotionService.OnLocomotionProviderDisabled(this);
        }

        /// <inheritdoc />
        public virtual void OnTeleportTargetRequested(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public virtual void OnTeleportStarted(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public virtual void OnTeleportCompleted(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public virtual void OnTeleportCanceled(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public virtual void OnInputChanged(InputEventData<float> eventData) { }

        /// <inheritdoc />
        public virtual void OnInputChanged(InputEventData<Vector2> eventData) { }

        /// <inheritdoc />
        public virtual void OnInputDown(InputEventData eventData) { }

        /// <inheritdoc />
        public virtual void OnInputUp(InputEventData eventData) { }
    }
}
