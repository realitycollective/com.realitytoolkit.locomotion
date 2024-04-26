﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Extensions;
using RealityToolkit.Input.Handlers;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace RealityToolkit.Locomotion.Teleportation
{
    /// <summary>
    /// Default implementation for <see cref="ITeleportAnchor"/>.
    /// Place the component on a game object to make it an anchor for teleportation.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    [HelpURL("https://www.realitytoolkit.io/docs/locomotion/teleportation-anchor")]
    public class TeleportAnchor : BaseFocusHandler, ITeleportAnchor
    {
        [SerializeField, Tooltip("Defaults to the local transform. Override to specify a different transform to " +
            "serve as the anchor definifing position and orientation.")]
        private Transform anchorTransform = null;

        [SerializeField]
        [Tooltip("Should the destination orientation be overridden? " +
                 "Useful when you want to orient the user in a specific direction when they teleport to this position. " +
                 "Override orientation is the transform forward of the GameObject this component is attached to.")]
        private bool overrideOrientation = false;

        [SerializeField, Tooltip("The anchor is being targeted for teleportation.")]
        private UnityEvent onTargeted = null;

        [SerializeField, Tooltip("The anchor has been teleported to.")]
        private UnityEvent onActivated = null;

        /// <inheritdoc />
        public bool IsEnabled => isActiveAndEnabled;

        /// <inheritdoc />
        public Vector3 Position => anchorTransform.position;

        /// <inheritdoc />
        public Vector3 Normal => anchorTransform.up;

        /// <inheritdoc />
        public bool OverrideTargetOrientation => overrideOrientation;

        /// <inheritdoc />
        public float TargetOrientation => anchorTransform.eulerAngles.y;

        /// <inheritdoc />
        public event Action Activated;

        /// <inheritdoc />
        public event Action Targeted;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        private void OnEnable()
        {
            if (anchorTransform.IsNull())
            {
                anchorTransform = transform;
            }
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            var source = anchorTransform.IsNull() ? transform : anchorTransform;
            var position = source.position;

            Gizmos.color = IsEnabled ? Color.green : Color.red;
            Gizmos.DrawLine(position + (Vector3.up * 0.1f), position + (Vector3.up * 0.1f) + (source.forward * 0.5f));
            Gizmos.DrawSphere(position + (Vector3.up * 0.1f) + (source.forward * 0.5f), 0.01f);
        }
    }
}
