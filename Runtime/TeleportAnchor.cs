// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Input.Handlers;
using RealityToolkit.Locomotion.Interfaces;
using UnityEngine;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Default implementation for <see cref="ITeleportAnchor"/>.
    /// Place the component on a game object to make it an anchor for teleportation.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BoxCollider))]
    public class TeleportAnchor : BaseFocusHandler, ITeleportAnchor
    {
        [SerializeField]
        [Tooltip("Should the destination orientation be overridden? " +
                 "Useful when you want to orient the user in a specific direction when they teleport to this position. " +
                 "Override orientation is the transform forward of the GameObject this component is attached to.")]
        private bool overrideOrientation = false;

        /// <inheritdoc />
        public bool OverrideTargetOrientation => overrideOrientation;

        /// <inheritdoc />
        public Vector3 Position => transform.position;

        /// <inheritdoc />
        public Vector3 Normal => transform.up;

        /// <inheritdoc />
        public bool IsActive => isActiveAndEnabled;

        /// <inheritdoc />
        public float TargetOrientation => transform.eulerAngles.y;

        private void OnDrawGizmos()
        {
            Gizmos.color = IsActive ? Color.green : Color.red;
            Gizmos.DrawLine(Position + (Vector3.up * 0.1f), Position + (Vector3.up * 0.1f) + (transform.forward * 0.1f));
            Gizmos.DrawSphere(Position + (Vector3.up * 0.1f) + (transform.forward * 0.1f), 0.01f);
        }
    }
}
