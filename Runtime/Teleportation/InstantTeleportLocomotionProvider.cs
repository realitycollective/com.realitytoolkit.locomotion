﻿// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace RealityToolkit.Locomotion.Teleportation
{
    /// <summary>
    /// A simple <see cref="ITeleportLocomotionProvider"/> implementation that teleports
    /// the player rig instantly to a target location within a single frame.
    /// </summary>
    [System.Runtime.InteropServices.Guid("790cdfd8-89c7-41c9-8dab-6b32e1e9d0a9")]
    public class InstantTeleportLocomotionProvider : BaseTeleportLocomotionProvider, IInstantTeleportLocomotionProvider
    {
        /// <inheritdoc />
        public InstantTeleportLocomotionProvider(string name, uint priority, BaseTeleportLocomotionProviderProfile profile, ILocomotionService parentService)
            : base(name, priority, profile, parentService) { }

        /// <inheritdoc />
        public override void OnTeleportStarted(LocomotionEventData eventData)
        {
            // Was this teleport provider's teleport started and did this provider
            // actually expect a teleport to start?
            if (OpenTargetRequests.ContainsKey(eventData.EventSource.SourceId))
            {
                var targetRotation = Vector3.zero;
                var targetPosition = eventData.Pose.Value.position;
                targetRotation.y = eventData.Pose.Value.rotation.eulerAngles.y;

                if (eventData.Anchor != null)
                {
                    targetPosition = eventData.Anchor.Position;
                    if (eventData.Anchor.OverrideTargetOrientation)
                    {
                        targetRotation.y = eventData.Anchor.TargetOrientation;
                    }
                }

                LocomotionService.LocomotionTarget.SetPositionAndRotation(targetPosition, targetRotation);
                var inputSource = OpenTargetRequests[eventData.EventSource.SourceId];
                LocomotionService.RaiseTeleportCompleted(this, inputSource, eventData.Pose.Value, eventData.Anchor);
            }

            base.OnTeleportStarted(eventData);
        }
    }
}
