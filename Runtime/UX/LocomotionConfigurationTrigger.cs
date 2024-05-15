// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace RealityToolkit.Locomotion.UX
{
    [RequireComponent(typeof(Collider))]
    [AddComponentMenu(RealityToolkitRuntimePreferences.Toolkit_AddComponentMenu + "/Locomotion/" + nameof(LocomotionConfigurationTrigger))]
    public class LocomotionConfigurationTrigger : MonoBehaviour
    {
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
    }
}
