// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Interactions.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RealityToolkit.Locomotion.Movement
{
    /// <summary>
    /// A simple <see cref="IFreeLocomotionProvider"/> implementation that allows free movement
    /// of the player rig, similar to a classic first person view character controller. Movement is constrained
    /// to the XZ-plane.
    /// </summary>
    [System.Runtime.InteropServices.Guid("1be53dfa-b8ae-4eb8-8459-17a5df87ade5")]
    public class SmoothLocomotionProvider : BaseLocomotionProvider, ISmoothLocomotionProvider
    {
        /// <inheritdoc />
        public SmoothLocomotionProvider(string name, uint priority, SmoothLocomotionProviderProfile profile, ILocomotionService parentService)
            : base(name, priority, profile, parentService)
        {
            runInputAction = profile.RunInputAction;
            Speed = profile.Speed;
            RunningSpeed = profile.RunningSpeed;
        }

        private bool isRunning;
        private readonly InputActionReference runInputAction;

        private float speed;
        /// <inheritdoc />
        public float Speed
        {
            get => speed;
            set
            {
                if (value < 1f)
                {
                    value = 1f;
                    Debug.LogError($"{GetType().Name}.{nameof(Speed)} must be 1 or greater.");
                }

                speed = value;
            }
        }

        private float runningSpeed;
        /// <inheritdoc />
        public float RunningSpeed
        {
            get => runningSpeed;
            set
            {
                if (value < 1f)
                {
                    value = 1f;
                    Debug.LogError($"{GetType().Name}.{nameof(RunningSpeed)} must be 1 or greater.");
                }

                runningSpeed = value;
            }
        }

        /// <inheritdoc />
        protected override void OnActivated()
        {
            base.OnActivated();

            if (InputActionUtilities.TryGetInputAction(runInputAction, out var runAction))
            {
                runAction.started += RunInputAction_Started;
                runAction.canceled += RunInputAction_Canceled;
            }
        }

        /// <inheritdoc />
        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            if (InputActionUtilities.TryGetInputAction(runInputAction, out var inputAction))
            {
                inputAction.started -= RunInputAction_Started;
                inputAction.canceled -= RunInputAction_Canceled;
            }
        }

        private void RunInputAction_Started(InputAction.CallbackContext context)
        {
            isRunning = true;
        }

        private void RunInputAction_Canceled(InputAction.CallbackContext context)
        {
            isRunning = false;
        }

        /// <inheritdoc />
        protected override void OnLocomotionActionPerformed(InputAction.CallbackContext context)
        {
            base.OnLocomotionActionPerformed(context);

            if (IsActive &&
                LocomotionService.MovementEnabled)
            {
                var speed = isRunning ? RunningSpeed : Speed;
                var direction = context.ReadValue<Vector2>();

                LocomotionService.LocomotionTarget.Move(direction, speed);
                LocomotionService.RaiseMoving(this, InteractionService.GetController(context.control.device.deviceId), direction, speed);
            }
        }
    }
}
