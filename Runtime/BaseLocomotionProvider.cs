// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityCollective.ServiceFramework.Modules;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Interactions;
using RealityToolkit.Interactions.Utilities;
using UnityEngine.InputSystem;

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
        public InputActionReference InputAction { get; }

        /// <summary>
        /// Gets the active <see cref="ILocomotionService"/> instance.
        /// </summary>
        protected ILocomotionService LocomotionService { get; }

        /// <summary>
        /// Gets the active <see cref="IInteractionService"/> instance.
        /// </summary>
        protected IInteractionService InteractionService { get; private set; }

        /// <inheritdoc />
        public override void Start() => IsActive = startupBehaviour == AutoStartBehavior.AutoStart;

        /// <summary>
        /// This <see cref="ILocomotionProvider"/> was activated.
        /// </summary>
        protected virtual void OnActivated()
        {
            if (ServiceManager.Instance.TryGetService<IInteractionService>(out var interactionService))
            {
                InteractionService = interactionService;
            }

            if (InputActionUtilities.TryGetInputAction(InputAction, out var locomotionAction))
            {
                locomotionAction.started += LocomotionAction_Started;
                locomotionAction.performed += LocomotionAction_Performed;
                locomotionAction.canceled += LocomotionAction_Canceled;
            }
        }

        /// <summary>
        /// This <see cref="ILocomotionProvider"/> was deactivated.
        /// </summary>
        protected virtual void OnDeactivated()
        {
            if (InputActionUtilities.TryGetInputAction(InputAction, out var locomotionAction))
            {
                locomotionAction.started -= LocomotionAction_Started;
                locomotionAction.performed -= LocomotionAction_Performed;
                locomotionAction.canceled -= LocomotionAction_Canceled;
            }
        }

        private void LocomotionAction_Started(InputAction.CallbackContext context) => OnLocomotionActionStarted(context);

        /// <summary>
        /// Internal method called when the <see cref="InputAction"/> is started.
        /// </summary>
        /// <param name="context">Input callback context.</param>
        protected virtual void OnLocomotionActionStarted(InputAction.CallbackContext context) { }

        private void LocomotionAction_Performed(InputAction.CallbackContext context) => OnLocomotionActionPerformed(context);

        /// <summary>
        /// Internal method called when the <see cref="InputAction"/> is performed.
        /// </summary>
        /// <param name="context">Input callback context.</param>
        protected virtual void OnLocomotionActionPerformed(InputAction.CallbackContext context) { }

        private void LocomotionAction_Canceled(InputAction.CallbackContext context) => OnLocomotionActionCanceled(context);

        /// <summary>
        /// Internal method called when the <see cref="InputAction"/> is canceled.
        /// </summary>
        /// <param name="context">Input callback context.</param>
        protected virtual void OnLocomotionActionCanceled(InputAction.CallbackContext context) { }

        /// <inheritdoc />
        public override void Destroy()
        {
            OnDeactivated();
            base.Destroy();
        }

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
    }
}
