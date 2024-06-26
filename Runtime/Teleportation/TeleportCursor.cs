﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Input.Cursors;
using RealityToolkit.Input.Definitions;
using RealityToolkit.Input.Interactors;
using System;
using UnityEngine;

namespace RealityToolkit.Locomotion.Teleportation
{
    [AddComponentMenu("")]
    public class TeleportCursor : AnimatedCursor, ILocomotionServiceHandler
    {
        [SerializeField]
        [Tooltip("Arrow Transform to point in the Teleporting direction.")]
        private Transform arrowTransform = null;

        private Vector3 cursorOrientation = Vector3.zero;

        /// <inheritdoc />
        public override IInteractor Pointer
        {
            get => pointer;
            set
            {
                Debug.Assert(value.GetType() == typeof(TeleportInteractor),
                    "Teleport Cursor's Pointer must derive from a TeleportPointer type.");

                pointer = (TeleportInteractor)value;
                pointer.BaseCursor = this;
                RegisterManagers();
            }
        }

        private TeleportInteractor pointer;

        /// <inheritdoc />
        public override Vector3 Position => PrimaryCursorVisual.position;

        /// <inheritdoc />
        public override Quaternion Rotation => arrowTransform.rotation;

        /// <inheritdoc />
        public override Vector3 LocalScale => PrimaryCursorVisual.localScale;

        private ILocomotionService locomotionService;
        /// <summary>
        /// Gets the active <see cref="ILocomotionService"/> instance.
        /// </summary>
        protected ILocomotionService LocomotionService => locomotionService ??= ServiceManager.Instance.GetService<ILocomotionService>();

        /// <inheritdoc />
        public override CursorStateEnum CheckCursorState()
        {
            if (CursorState != CursorStateEnum.Contextual)
            {
                if (pointer.IsInteractionEnabled)
                {
                    switch (pointer.ValidationResult)
                    {
                        case TeleportValidationResult.None:
                            return CursorStateEnum.Release;
                        case TeleportValidationResult.Invalid:
                            return CursorStateEnum.ObserveHover;
                        case TeleportValidationResult.Anchor:
                        case TeleportValidationResult.Valid:
                            return CursorStateEnum.ObserveHover;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                return CursorStateEnum.Release;
            }

            return CursorStateEnum.Contextual;
        }

        /// <inheritdoc />
        protected override void UpdateCursorTransform()
        {
            if (Pointer == null)
            {
                Debug.LogError($"[TeleportCursor.{name}] No Pointer has been assigned!");
                Destroy(gameObject);
                return;
            }

            if (!InputService.FocusProvider.TryGetFocusDetails(Pointer, out var focusDetails))
            {
                if (InputService.FocusProvider.IsPointerRegistered(Pointer))
                {
                    Debug.LogError($"{gameObject.name}: Unable to get focus details for {pointer.GetType().Name}!");
                }
                else
                {
                    Debug.LogError($"{pointer.GetType().Name} has not been registered!");
                    Destroy(gameObject);
                }

                return;
            }

            transform.position = focusDetails.EndPoint;

            var forward = Camera.main.transform.forward;
            forward.y = 0f;

            // Smooth out rotation just a tad to prevent jarring transitions
            PrimaryCursorVisual.rotation = Quaternion.Lerp(PrimaryCursorVisual.rotation, Quaternion.LookRotation(forward.normalized, Vector3.up), 0.5f);

            // Point the arrow towards the target orientation
            cursorOrientation.y = pointer.PointerOrientation;
            arrowTransform.eulerAngles = cursorOrientation;
        }

        /// <inheritdoc />
        public void OnMoving(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public void OnTeleportTargetRequested(LocomotionEventData eventData)
        {
            OnCursorStateChange(CursorStateEnum.Observe);
        }

        /// <inheritdoc />
        public void OnTeleportStarted(LocomotionEventData eventData)
        {
            OnCursorStateChange(CursorStateEnum.Release);
        }

        /// <inheritdoc />
        public void OnTeleportCompleted(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public void OnTeleportCanceled(LocomotionEventData eventData)
        {
            OnCursorStateChange(CursorStateEnum.Release);
        }
    }
}
