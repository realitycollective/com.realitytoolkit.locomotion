// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework;
using RealityCollective.ServiceFramework.Attributes;
using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityCollective.ServiceFramework.Services;
using RealityToolkit.Locomotion.Movement;
using RealityToolkit.Locomotion.Teleportation;
using UnityEngine;

namespace RealityToolkit.Locomotion.UX
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu(RealityToolkitRuntimePreferences.Toolkit_AddComponentMenu + "/Locomotion/" + nameof(LocomotionConfigurationTrigger))]
    public class LocomotionConfigurationTrigger : MonoBehaviour
    {
        [SerializeField, Tooltip("If set, teleportation is enabled in this room.")]
        private bool locomotionEnabled = true;

        [SerializeField, Tooltip("If set, free movement is enabled in this room.")]
        private bool movementEnabled = true;

        [SerializeField, Tooltip("If set, teleportation is enabled in this room.")]
        private bool teleportationEnabled = true;

        [SerializeField, Implements(typeof(IFreeLocomotionProvider), TypeGrouping.ByNamespaceFlat)]
        private SystemType freeProviderType = null;

        [SerializeField, Implements(typeof(ITeleportLocomotionProvider), TypeGrouping.ByNamespaceFlat)]
        private SystemType teleportProviderType = null;

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        private void Awake()
        {
            var collider = GetComponent<Collider>();
            if (!collider.isTrigger)
            {
                collider.isTrigger = true;
                Debug.LogWarning($"{nameof(LocomotionConfigurationTrigger)} requires the attached {nameof(Collider)} to be a trigger and has auto configured it.", this);
            }
        }

        /// <summary>
        /// See <see cref="MonoBehaviour"/>.
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<ILocomotionTarget>(out _))
            {
                return;
            }

            if (!ServiceManager.Instance.TryGetService<ILocomotionService>(out var locomotionService))
            {
                Debug.LogError($"{nameof(LocomotionConfigurationTrigger)} reuires the {nameof(ILocomotionService)} to work.");
                return;
            }

            locomotionService.LocomotionEnabled = locomotionEnabled;
            locomotionService.MovementEnabled = movementEnabled;
            locomotionService.TeleportationEnabled = teleportationEnabled;

            if (freeProviderType != null)
            {
                locomotionService.EnableLocomotionProvider(freeProviderType.Type);
            }

            if (teleportProviderType != null)
            {
                locomotionService.EnableLocomotionProvider(teleportProviderType.Type);
            }
        }
    }
}
