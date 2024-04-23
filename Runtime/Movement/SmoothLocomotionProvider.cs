// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.EventDatum.Input;
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
            speed = profile.Speed;
        }

        private readonly float speed;

        /// <inheritdoc />
        public override void OnInputChanged(InputEventData<Vector2> eventData)
        {
            base.OnInputChanged(eventData);

            if (IsActive &&
                LocomotionService.MovementEnabled &&
                eventData.InputAction == InputAction)
            {
                LocomotionService.LocomotionTarget.Move(eventData.InputData, speed);
            }
        }
    }
}
