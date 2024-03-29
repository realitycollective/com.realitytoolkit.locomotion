﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Utilities.Lines.DataProviders;
using UnityEngine;

namespace RealityToolkit.Locomotion.UX
{
    [RequireComponent(typeof(ParabolaPhysicalLineDataProvider))]
    public class ParabolicTeleportPointer : TeleportPointer
    {
        [SerializeField]
        private float minParabolaVelocity = 1f;

        [SerializeField]
        private float maxParabolaVelocity = 5f;

        [SerializeField]
        private float minDistanceModifier = 1f;

        [SerializeField]
        private float maxDistanceModifier = 5f;

        [SerializeField]
        private ParabolaPhysicalLineDataProvider parabolicLineData;

        #region MonoBehaviour Implementation

        protected override void OnValidate()
        {
            base.OnValidate();
            EnsureSetup();

            if (Application.isPlaying && parabolicLineData.LineTransform == transform)
            {
                Debug.LogWarning("Missing Parabolic line helper.\nThe Parabolic Teleport Pointer requires an empty GameObject child for calculating the parabola arc.");
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EnsureSetup();

            if (parabolicLineData.LineTransform == transform)
            {
                var pointerHelper = new GameObject("ParabolicLinePointerHelper");
                pointerHelper.transform.SetParent(transform);
                pointerHelper.transform.localPosition = Vector3.zero;
                parabolicLineData.LineTransform = pointerHelper.transform;
            }
        }

        private void EnsureSetup()
        {
            if (parabolicLineData == null)
            {
                parabolicLineData = GetComponent<ParabolaPhysicalLineDataProvider>();
            }
        }

        #endregion MonoBehaviour Implementation

        #region IMixedRealityPointer Implementation

        /// <inheritdoc />
        public override void OnPreRaycast()
        {
            var transformForward = transform.forward;
            parabolicLineData.LineTransform.rotation = Quaternion.identity;
            parabolicLineData.Direction = transformForward;

            // when pointing straight up, upDot should be close to 1.
            // when pointing straight down, upDot should be close to -1.
            // when pointing straight forward in any direction, upDot should be 0.
            var upDot = Vector3.Dot(transformForward, Vector3.up);

            var velocity = minParabolaVelocity;
            var distance = minDistanceModifier;

            // If we're pointing below the horizon, always use the minimum modifiers.
            if (upDot > 0f)
            {
                // Increase the modifier multipliers the higher we point.
                velocity = Mathf.Lerp(minParabolaVelocity, maxParabolaVelocity, upDot);
                distance = Mathf.Lerp(minDistanceModifier, maxDistanceModifier, upDot);
            }

            parabolicLineData.Velocity = velocity;
            parabolicLineData.DistanceMultiplier = distance;
            base.OnPreRaycast();
        }

        #endregion IMixedRealityPointer Implementation
    }
}