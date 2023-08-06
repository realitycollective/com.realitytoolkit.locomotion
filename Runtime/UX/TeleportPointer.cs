// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Definitions.Physics;
using RealityToolkit.EventDatum.Input;
using RealityToolkit.Input.Interfaces;
using RealityToolkit.Input.Pointers;
using RealityToolkit.Locomotion.Definitions;
using RealityToolkit.Locomotion.Interfaces;
using RealityToolkit.Utilities.Physics;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RealityToolkit.Locomotion.UX
{
    public class TeleportPointer : LinePointer, ILocomotionServiceHandler, ITeleportTargetProvider
    {
        [SerializeField]
        [Tooltip("Gradient color to apply when targeting an anchor.")]
        [FormerlySerializedAs("lineColorHotSpot")]
        private Gradient lineColorAnchor = new Gradient();

        private bool lateRegisterTeleport = true;

        /// <summary>
        /// Gradient color to apply when targeting an <see cref="ITeleportAnchor"/>.
        /// </summary>
        protected Gradient LineColorAnchor
        {
            get => lineColorAnchor;
            set => lineColorAnchor = value;
        }

        private ITeleportValidationServiceModule validationDataProvider;
        private ITeleportValidationServiceModule ValidationDataProvider => validationDataProvider ?? (validationDataProvider = ServiceManager.Instance.GetService<ITeleportValidationServiceModule>());

        private ILocomotionService locomotionService = null;

        protected ILocomotionService LocomotionService
            => locomotionService ?? (locomotionService = ServiceManager.Instance.GetService<ILocomotionService>());

        /// <inheritdoc />
        public ILocomotionProvider RequestingLocomotionProvider { get; private set; }

        /// <inheritdoc />
        public IInputSource InputSource => base.InputSource;

        /// <inheritdoc />
        public Pose? TargetPose { get; private set; }

        /// <inheritdoc />
        public ITeleportAnchor Anchor { get; private set; }

        /// <inheritdoc />
        public TeleportValidationResult ValidationResult { get; private set; } = TeleportValidationResult.None;

        /// <inheritdoc />
        public bool IsTargeting { get; private set; }

        /// <summary>
        /// Gets the gradient color for the teleport parabolic line depending on a the validation result
        /// for the current teleport target.
        /// </summary>
        /// <param name="targetResult">Validation result for current target.</param>
        /// <returns></returns>
        protected Gradient GetLineGradient(TeleportValidationResult targetResult)
        {
            switch (targetResult)
            {
                case TeleportValidationResult.None:
                    return LineColorNoTarget;
                case TeleportValidationResult.Valid:
                    return LineColorValid;
                case TeleportValidationResult.Invalid:
                    return LineColorInvalid;
                case TeleportValidationResult.Anchor:
                    return LineColorAnchor;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetResult), targetResult, null);
            }
        }

        /// <summary>
        /// Resets the pointer / target provider to defaults
        /// so a new target request can be served.
        /// </summary>
        protected void ResetToDefaults()
        {
            IsTargeting = false;
            IsTeleportRequestActive = false;

            if (BaseCursor != null)
            {
                BaseCursor.IsVisible = false;
            }

            RequestingLocomotionProvider = null;
            PointerOrientation = 0f;
        }

        #region IPointer Implementation

        /// <inheritdoc />
        public override bool IsInteractionEnabled => !IsTeleportRequestActive && IsTargeting;

        /// <inheritdoc />
        public override float PointerOrientation
        {
            get
            {
                if (Anchor != null &&
                    Anchor.OverrideTargetOrientation &&
                    ValidationResult == TeleportValidationResult.Anchor)
                {
                    return Anchor.TargetOrientation;
                }

                return base.PointerOrientation;
            }
            set => base.PointerOrientation = value;
        }

        /// <inheritdoc />
        protected override void OnEnable()
        {
            base.OnEnable();

            if (!lateRegisterTeleport &&
                ServiceManager.Instance.TryGetService(out locomotionService))
            {
                locomotionService.Register(gameObject);
            }
        }

        /// <inheritdoc />
        protected override async void Start()
        {
            base.Start();

            if (lateRegisterTeleport)
            {
                try
                {
                    locomotionService = await ServiceManager.Instance.GetServiceAsync<ILocomotionService>();
                    LocomotionService?.Register(gameObject);
                }
                catch
                {
                    return;
                }
                finally
                {
                    lateRegisterTeleport = false;
                }
            }
        }

        /// <inheritdoc />
        protected override void OnDisable()
        {
            LocomotionService?.Unregister(gameObject);
            base.OnDisable();
        }

        public override void OnPreRaycast()
        {
            if (LineBase == null)
            {
                return;
            }

            // Make sure our array will hold
            if (Rays == null || Rays.Length != LineCastResolution)
            {
                Rays = new RayStep[LineCastResolution];
            }

            float stepSize = 1f / Rays.Length;
            Vector3 lastPoint = LineBase.GetUnClampedPoint(0f);

            for (int i = 0; i < Rays.Length; i++)
            {
                Vector3 currentPoint = LineBase.GetUnClampedPoint(stepSize * (i + 1));
                Rays[i].UpdateRayStep(ref lastPoint, ref currentPoint);
                lastPoint = currentPoint;
            }
        }

        public override void OnPostRaycast()
        {
            // Use the results from the last update to set our NavigationResult
            float clearWorldLength = 0f;
            ValidationResult = TeleportValidationResult.None;
            TargetPose = null;

            if (IsInteractionEnabled)
            {
                LineBase.enabled = true;

                // If we hit something
                if (Result.CurrentPointerTarget != null)
                {
                    // Check for anchor hit.
                    Anchor = Result.CurrentPointerTarget.GetComponent<ITeleportAnchor>();

                    // Validate whether hit target is a valid teleportation target.
                    ValidationResult = ValidationDataProvider.IsValid(Result, Anchor);

                    // Set target pose if we have a valid target.
                    if (ValidationResult == TeleportValidationResult.Valid ||
                        ValidationResult == TeleportValidationResult.Anchor)
                    {
                        TargetPose = new Pose(Result.EndPoint, Quaternion.Euler(0f, PointerOrientation, 0f));
                    }

                    // Use the step index to determine the length of the hit
                    for (int i = 0; i <= Result.RayStepIndex; i++)
                    {
                        if (i == Result.RayStepIndex)
                        {
                            if (Raycaster.DebugEnabled)
                            {
                                Color debugColor = ValidationResult != TeleportValidationResult.None
                                    ? Color.yellow
                                    : Color.cyan;

                                Debug.DrawLine(Result.StartPoint + Vector3.up * 0.1f, Result.StartPoint + Vector3.up * 0.1f, debugColor);
                            }

                            // Only add the distance between the start point and the hit
                            clearWorldLength += Vector3.Distance(Result.StartPoint, Result.EndPoint);
                        }
                        else if (i < Result.RayStepIndex)
                        {
                            // Add the full length of the step to our total distance
                            clearWorldLength += Rays[i].Length;
                        }
                    }

                    // Clamp the end of the parabola to the result hit's point
                    LineBase.LineEndClamp = LineBase.GetNormalizedLengthFromWorldLength(clearWorldLength, LineCastResolution);

                    if (BaseCursor != null)
                    {
                        BaseCursor.IsVisible = ValidationResult == TeleportValidationResult.Valid || ValidationResult == TeleportValidationResult.Anchor;
                    }
                }
                else
                {
                    if (BaseCursor != null)
                    {
                        BaseCursor.IsVisible = false;
                    }

                    LineBase.LineEndClamp = 1f;
                }

                // Set the line color
                for (int i = 0; i < LineRenderers.Length; i++)
                {
                    LineRenderers[i].LineColor = GetLineGradient(ValidationResult);
                }
            }
            else
            {
                LineBase.enabled = false;
            }
        }

        #endregion IPointer Implementation

        #region IInputHandler Implementation

        /// <inheritdoc />
        public override void OnInputChanged(InputEventData<Vector2> eventData)
        {
            // Don't process input if we've got an active teleport request in progress.
            if (eventData.used || IsTeleportRequestActive)
            {
                return;
            }

            // Only if we are currently answering to a teleport target
            // request, we care for input change to reorient the pointer if needed.
            if (RequestingLocomotionProvider != null &&
                eventData.SourceId == InputSource.SourceId &&
                eventData.Handedness == Handedness &&
                eventData.InputAction == RequestingLocomotionProvider.InputAction)
            {
                PointerOrientation = Mathf.Atan2(eventData.InputData.x, eventData.InputData.y) * Mathf.Rad2Deg;
            }
        }

        #endregion IInputHandler Implementation

        #region ITeleportHandler Implementation

        /// <inheritdoc />
        public void OnTeleportTargetRequested(LocomotionEventData eventData)
        {
            // Only enable teleport if the request is addressed at our input source.
            if (eventData.EventSource.SourceId == InputSource.SourceId)
            {
                // This teleport target provider is able to provide a target
                // for the requested input source.
                ((ITeleportLocomotionProvider)eventData.LocomotionProvider).SetTargetProvider(this);

                IsTargeting = true;
                IsTeleportRequestActive = false;
                RequestingLocomotionProvider = eventData.LocomotionProvider;
            }
        }

        /// <inheritdoc />
        public void OnTeleportStarted(LocomotionEventData eventData) { }

        /// <inheritdoc />
        public void OnTeleportCompleted(LocomotionEventData eventData)
        {
            // We could be checking here whether the completed teleport
            // is this teleport provider's own teleport operation and act differently
            // depending on whether yes or not. But for now we'll make any teleport completion
            // basically cancel out any other teleport pointer as well.
            ResetToDefaults();
        }

        /// <inheritdoc />
        public void OnTeleportCanceled(LocomotionEventData eventData)
        {
            // Only cancel teleport if this target provider's teleport was canceled.
            if (eventData.EventSource.SourceId == InputSource.SourceId)
            {
                ResetToDefaults();
            }
        }

        #endregion ITeleportHandler Implementation
    }
}
