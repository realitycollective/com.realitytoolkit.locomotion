﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Modules;
using RealityToolkit.Input.Interactors;
using UnityEngine;

namespace RealityToolkit.Locomotion.Teleportation
{
    /// <summary>
    /// The Reality Toolkit's specific implementation of the <see cref="ITeleportValidationServiceModule"/>.
    /// </summary>
    [System.Runtime.InteropServices.Guid("14199fd8-1636-4147-bb08-6475e76ed1cd")]
    public class TeleportValidationServiceModule : BaseServiceModule, ITeleportValidationServiceModule
    {
        /// <inheritdoc />
        public TeleportValidationServiceModule(string name, uint priority, TeleportValidationServiceModuleProfile profile, ILocomotionService parentService)
            : base(name, priority, profile, parentService)
        {
            AnchorsOnly = profile.AnchorsOnly;
            validLayers = profile.ValidLayers;
            invalidLayers = profile.InvalidLayers;
            upDirectionThreshold = profile.UpDirectionThreshold;
            maxDistanceSquare = profile.MaxDistance * profile.MaxDistance;
            maxHeightDistance = profile.MaxHeightDistance;
        }

        private readonly LayerMask validLayers;
        private readonly LayerMask invalidLayers;
        private readonly float upDirectionThreshold;
        private readonly float maxDistanceSquare;
        private readonly float maxHeightDistance;

        /// <inheritdoc />
        public bool AnchorsOnly { get; set; }

        /// <inheritdoc />
        public TeleportValidationResult IsValid(IInteractorResult pointerResult, ITeleportAnchor anchor = null)
        {
            TeleportValidationResult teleportValidationResult;

            // Check distance.
            if ((pointerResult.EndPoint - Camera.main.transform.position).sqrMagnitude > maxDistanceSquare ||
                Mathf.Abs(pointerResult.EndPoint.y - Camera.main.transform.position.y) > maxHeightDistance)
            {
                teleportValidationResult = TeleportValidationResult.Invalid;
            }
            // Check anchors only.
            else if (AnchorsOnly && (anchor == null || !anchor.IsEnabled))
            {
                teleportValidationResult = TeleportValidationResult.Invalid;
            }
            // Check if it's in our valid layers
            else if (((1 << pointerResult.CurrentTarget.layer) & validLayers.value) != 0)
            {
                // See if it's a hot spot
                if (anchor != null && anchor.IsEnabled)
                {
                    teleportValidationResult = TeleportValidationResult.Anchor;
                }
                else
                {
                    // If it's NOT an anchor, check if the hit normal is too steep 
                    // (Anchors override dot requirements)
                    teleportValidationResult = Vector3.Dot(pointerResult.LastRaycastHit.normal, Vector3.up) > upDirectionThreshold
                        ? TeleportValidationResult.Valid
                        : TeleportValidationResult.Invalid;
                }
            }
            else if (((1 << pointerResult.CurrentTarget.layer) & invalidLayers) != 0)
            {
                teleportValidationResult = TeleportValidationResult.Invalid;
            }
            else
            {
                teleportValidationResult = TeleportValidationResult.None;
            }

            return teleportValidationResult;
        }
    }
}
