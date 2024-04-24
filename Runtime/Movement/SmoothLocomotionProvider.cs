// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.EventDatum.Input;
using RealityToolkit.Input.Definitions;
using UnityEngine;

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
        private readonly InputAction runInputAction;

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
        public override void OnInputDown(InputEventData eventData)
        {
            base.OnInputDown(eventData);

            if (eventData.InputAction == runInputAction)
            {
                isRunning = true;
            }
        }

        /// <inheritdoc />
        public override void OnInputUp(InputEventData eventData)
        {
            base.OnInputUp(eventData);

            if (eventData.InputAction == runInputAction)
            {
                isRunning = false;
            }
        }

        /// <inheritdoc />
        public override void OnInputChanged(InputEventData<Vector2> eventData)
        {
            base.OnInputChanged(eventData);

            if (IsActive &&
                LocomotionService.MovementEnabled &&
                eventData.InputAction == InputAction)
            {
                LocomotionService.LocomotionTarget.Move(eventData.InputData, isRunning ? RunningSpeed : Speed);
            }
        }
    }
}
