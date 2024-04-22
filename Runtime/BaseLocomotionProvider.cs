// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Definitions.Utilities;
using RealityCollective.ServiceFramework.Modules;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.Input.Definitions;
using UnityEngine;

namespace RealityToolkit.Locomotion
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
        public InputAction InputAction { get; }

        /// <summary>
        /// Gets the active <see cref="ILocomotionService"/> instance.
        /// </summary>
        protected ILocomotionService LocomotionService { get; }

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
