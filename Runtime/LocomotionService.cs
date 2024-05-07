// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.ServiceFramework.Attributes;
using RealityCollective.ServiceFramework.Definitions.Platforms;
using RealityCollective.ServiceFramework.Definitions.Utilities;
using RealityCollective.ServiceFramework.Services;
using RealityCollective.Utilities.Extensions;
using RealityToolkit.Input.Interfaces;
using RealityToolkit.Input.Listeners;
using RealityToolkit.Locomotion.Movement;
using RealityToolkit.Locomotion.Teleportation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// The Reality Toolkit's specific implementation of the <see cref="ILocomotionService"/>
    /// </summary>
    [RuntimePlatform(typeof(AllPlatforms))]
    [System.Runtime.InteropServices.Guid("9453c088-285e-47aa-bfbb-dafd9109fdd5")]
    public class LocomotionService : BaseEventService, ILocomotionService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">The service display name.</param>
        /// <param name="priority">The service initialization priority.</param>
        /// <param name="profile">The service configuration profile.</param>
        public LocomotionService(string name, uint priority, LocomotionServiceProfile profile)
            : base(name, priority, profile)
        {
            LocomotionEnabled = profile.LocomotionStartupBehaviour == AutoStartBehavior.AutoStart;
            MovementEnabled = profile.MovementStartupBehaviour == AutoStartBehavior.AutoStart;
            TeleportationEnabled = profile.TeleportationStartupBehaviour == AutoStartBehavior.AutoStart;
            teleportCooldown = profile.TeleportCooldown;
        }

        private GameObject eventDriver;
        private readonly float teleportCooldown;
        private float currentTeleportCooldown;
        private LocomotionEventData teleportEventData;
        private readonly Dictionary<Type, List<ILocomotionProvider>> enabledLocomotionProviders = new Dictionary<Type, List<ILocomotionProvider>>()
        {
            { typeof(IFreeLocomotionProvider), new List<ILocomotionProvider>() },
            { typeof(ITeleportLocomotionProvider), new List<ILocomotionProvider>() }
        };

        /// <inheritdoc />
        public bool LocomotionEnabled { get; set; }

        private bool movementEnabled;
        /// <inheritdoc />
        public bool MovementEnabled
        {
            get => LocomotionEnabled && movementEnabled;
            set => movementEnabled = value;
        }

        private bool teleportationEnabled;
        /// <inheritdoc />
        public bool TeleportationEnabled
        {
            get => LocomotionEnabled && teleportationEnabled;
            set => teleportationEnabled = value;
        }

        /// <inheritdoc />
        public ILocomotionTarget LocomotionTarget { get; set; }

        /// <inheritdoc />
        public bool IsTeleportCoolingDown => currentTeleportCooldown > 0f;

        /// <inheritdoc />
        public IReadOnlyList<ILocomotionProvider> EnabledLocomotionProviders => enabledLocomotionProviders.SelectMany(kv => kv.Value).ToList();

        /// <inheritdoc />
        public event LocomotionEventDelegate TeleportTargetRequested;

        /// <inheritdoc />
        public event LocomotionEventDelegate TeleportStarted;

        /// <inheritdoc />
        public event LocomotionEventDelegate TeleportCompleted;

        /// <inheritdoc />
        public event LocomotionEventDelegate TeleportCanceled;

        /// <inheritdoc />
        public override void Initialize()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            teleportEventData = new LocomotionEventData(EventSystem.current);
            EnsureEventDriver();
        }

        /// <inheritdoc />
        public override void Update()
        {
            if (IsTeleportCoolingDown)
            {
                currentTeleportCooldown -= Time.deltaTime;
            }
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            DestroyEventDriver();
            base.Destroy();
        }

        /// <inheritdoc />
        public void EnableLocomotionProvider<T>() where T : ILocomotionProvider
        {
            var provider = ServiceManager.Instance.GetService<T>();
            if (!provider.IsActive)
            {
                provider.IsActive = true;
            }
        }

        /// <inheritdoc />
        public void EnableLocomotionProvider(Type locomotionProviderType)
        {
            Debug.Assert(typeof(ILocomotionProvider).IsAssignableFrom(locomotionProviderType));
            var locomotionProviders = ServiceManager.Instance.GetServices<ILocomotionProvider>();

            for (var i = 0; i < locomotionProviders.Count; i++)
            {
                var provider = locomotionProviders[i];
                if (provider.GetType() == locomotionProviderType && !provider.IsActive)
                {
                    provider.IsActive = true;
                }
            }
        }

        /// <inheritdoc />
        public void DisableLocomotionProvider<T>() where T : ILocomotionProvider
        {
            var provider = ServiceManager.Instance.GetService<T>();
            if (provider.IsActive)
            {
                provider.IsActive = false;
            }
        }

        /// <inheritdoc />
        public void DisableLocomotionProvider(Type locomotionProviderType)
        {
            Debug.Assert(typeof(ILocomotionProvider).IsAssignableFrom(locomotionProviderType));
            var locomotionProviders = ServiceManager.Instance.GetServices<ILocomotionProvider>();

            for (var i = 0; i < locomotionProviders.Count; i++)
            {
                var provider = locomotionProviders[i];
                if (provider.GetType() == locomotionProviderType && provider.IsActive)
                {
                    provider.IsActive = false;
                }
            }
        }

        /// <inheritdoc />
        public void OnLocomotionProviderEnabled(ILocomotionProvider locomotionProvider)
        {
            var enabledLocomotionProvidersSnapshot = new Dictionary<Type, List<ILocomotionProvider>>(enabledLocomotionProviders);

            if (locomotionProvider is ITeleportLocomotionProvider ||
                locomotionProvider is IFreeLocomotionProvider)
            {
                // Free / Teleport providers behave like a toggle group. There can only
                // ever be one active provider for free locomotion and one active for teleprot locomotion.
                // So all we have to do is disable all other providers of the respective type.
                if (locomotionProvider is ITeleportLocomotionProvider)
                {
                    var teleportLocomotionProviders = enabledLocomotionProvidersSnapshot[typeof(ITeleportLocomotionProvider)];
                    for (var i = 0; i < teleportLocomotionProviders.Count; i++)
                    {
                        var teleportLocomotionProvider = teleportLocomotionProviders[i];

                        // Making sure to not disable the provider that just got enabled,
                        // in case it is already in the list.
                        if (teleportLocomotionProvider != locomotionProvider)
                        {
                            teleportLocomotionProvider.IsActive = false;
                        }
                    }

                    // Ensure the now enabled provider gets added to the managed enabled
                    // providers list.
                    if (!teleportLocomotionProviders.Contains(locomotionProvider))
                    {
                        enabledLocomotionProviders[typeof(ITeleportLocomotionProvider)].Add(locomotionProvider);
                    }
                }
                else
                {
                    var freeLocomotionProviders = enabledLocomotionProvidersSnapshot[typeof(IFreeLocomotionProvider)];
                    for (var i = 0; i < freeLocomotionProviders.Count; i++)
                    {
                        var freeLocomotionProvider = freeLocomotionProviders[i];

                        // Making sure to not disable the provider that just got enabled,
                        // in case it is already in the list.
                        if (freeLocomotionProvider != locomotionProvider)
                        {
                            freeLocomotionProvider.IsActive = false;
                        }
                    }

                    // Ensure the now enabled provider gets added to the managed enabled
                    // providers list.
                    if (!freeLocomotionProviders.Contains(locomotionProvider))
                    {
                        enabledLocomotionProviders[typeof(IFreeLocomotionProvider)].Add(locomotionProvider);
                    }
                }
            }
        }

        /// <inheritdoc />
        public void OnLocomotionProviderDisabled(ILocomotionProvider locomotionProvider)
        {
            Type type;
            if (locomotionProvider is ITeleportLocomotionProvider)
            {
                type = typeof(ITeleportLocomotionProvider);
            }
            else if (locomotionProvider is IFreeLocomotionProvider)
            {
                type = typeof(IFreeLocomotionProvider);
            }
            else
            {
                type = typeof(ILocomotionProvider);
            }

            if (enabledLocomotionProviders.ContainsKey(type) &&
                enabledLocomotionProviders[type].Contains(locomotionProvider))
            {
                enabledLocomotionProviders[type].Remove(locomotionProvider);
            }
        }

        private void EnsureEventDriver()
        {
            if (eventDriver.IsNull())
            {
                eventDriver = new GameObject(nameof(LocomotionProviderEventDriver), typeof(LocomotionProviderEventDriver), typeof(InputServiceGlobalListener));
                UnityEngine.Object.DontDestroyOnLoad(eventDriver);
            }
        }

        private void DestroyEventDriver()
        {
            if (eventDriver.IsNotNull())
            {
                eventDriver.Destroy();
            }
        }

        private static readonly ExecuteEvents.EventFunction<ILocomotionServiceHandler> OnTeleportRequestHandler =
            delegate (ILocomotionServiceHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<LocomotionEventData>(eventData);
                handler.OnTeleportTargetRequested(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportTargetRequest(ITeleportLocomotionProvider teleportLocomotionProvider, IInputSource inputSource)
        {
            if (IsTeleportCoolingDown)
            {
                return;
            }

            teleportEventData.Initialize(teleportLocomotionProvider, inputSource);
            TeleportTargetRequested?.Invoke(teleportEventData);
            HandleEvent(teleportEventData, OnTeleportRequestHandler);
        }

        private static readonly ExecuteEvents.EventFunction<ILocomotionServiceHandler> OnTeleportStartedHandler =
            delegate (ILocomotionServiceHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<LocomotionEventData>(eventData);
                handler.OnTeleportStarted(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportStarted(ITeleportLocomotionProvider locomotionProvider, IInputSource inputSource, Pose pose, ITeleportAnchor anchor)
        {
            teleportEventData.Initialize(locomotionProvider, inputSource, pose, anchor);
            TeleportStarted?.Invoke(teleportEventData);
            HandleEvent(teleportEventData, OnTeleportStartedHandler);
        }

        private static readonly ExecuteEvents.EventFunction<ILocomotionServiceHandler> OnTeleportCompletedHandler =
            delegate (ILocomotionServiceHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<LocomotionEventData>(eventData);
                handler.OnTeleportCompleted(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportCompleted(ITeleportLocomotionProvider locomotionProvider, IInputSource inputSource, Pose pose, ITeleportAnchor anchor)
        {
            currentTeleportCooldown = teleportCooldown;
            teleportEventData.Initialize(locomotionProvider, inputSource, pose, anchor);
            TeleportCompleted?.Invoke(teleportEventData);
            HandleEvent(teleportEventData, OnTeleportCompletedHandler);
        }

        private static readonly ExecuteEvents.EventFunction<ILocomotionServiceHandler> OnTeleportCanceledHandler =
            delegate (ILocomotionServiceHandler handler, BaseEventData eventData)
            {
                var casted = ExecuteEvents.ValidateEventData<LocomotionEventData>(eventData);
                handler.OnTeleportCanceled(casted);
            };

        /// <inheritdoc />
        public void RaiseTeleportCanceled(ITeleportLocomotionProvider locomotionProvider, IInputSource inputSource)
        {
            teleportEventData.Initialize(locomotionProvider, inputSource);
            TeleportCanceled?.Invoke(teleportEventData);
            HandleEvent(teleportEventData, OnTeleportCanceledHandler);
        }

        /// <inheritdoc />
        public override void HandleEvent<T>(BaseEventData eventData, ExecuteEvents.EventFunction<T> eventHandler)
        {
            Debug.Assert(eventData != null);
            var teleportData = ExecuteEvents.ValidateEventData<LocomotionEventData>(eventData);
            Debug.Assert(teleportData != null);
            Debug.Assert(!teleportData.used);

            base.HandleEvent(teleportData, eventHandler);
        }
    }
}
