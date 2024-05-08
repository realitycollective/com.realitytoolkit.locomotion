// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityCollective.ServiceFramework.Modules;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.Input.Definitions;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Base implementation for any kind of <see cref="ILocomotionProvider"/>.
    /// </summary>
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

        private bool isActive;
        /// <inheritdoc />
        public bool IsActive
        {
            get => isActive;
            set
            {
                if (value == isActive)
                {
                    return;
                }

                isActive = value;
                if (isActive)
                {
                    OnActivated();
                    LocomotionService.OnLocomotionProviderEnabled(this);
                    return;
                }

                OnDeactivated();
                LocomotionService.OnLocomotionProviderDisabled(this);
            }
        }

        /// <inheritdoc />
        public InputAction InputAction { get; }

        /// <summary>
        /// Gets the active <see cref="ILocomotionService"/> instance.
        /// </summary>
        protected ILocomotionService LocomotionService { get; }

        /// <inheritdoc />
        public override void Start() => IsActive = startupBehaviour == AutoStartBehavior.AutoStart;

        /// <summary>
        /// This <see cref="ILocomotionProvider"/> was activated.
        /// </summary>
        protected virtual void OnActivated() { }

        /// <summary>
        /// This <see cref="ILocomotionProvider"/> was deactivated.
        /// </summary>
        protected virtual void OnDeactivated() { }

        /// <inheritdoc />
        public virtual void OnMoving(LocomotionEventData eventData) { }

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
