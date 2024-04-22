// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.EventDatum;
using RealityToolkit.Input.Interfaces;
using RealityToolkit.Locomotion.Teleportation;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RealityToolkit.Locomotion
{
    /// <summary>
    /// Describes a locomotion event raised by the <see cref="ILocomotionService"/>.
    /// </summary>
    public class LocomotionEventData : GenericBaseEventData
    {
        /// <summary>
        /// The locomotion provider the event was raised for or raised by.
        /// </summary>
        public ILocomotionProvider LocomotionProvider { get; private set; }

        /// <summary>
        /// The teleport target pose, if any.
        /// </summary>
        public Pose? Pose { get; private set; }

        /// <summary>
        /// The teleport anchor, if any.
        /// </summary>
        public ITeleportAnchor Anchor { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="eventSystem">Typically will be <see cref="EventSystem.current"/></param>
        public LocomotionEventData(EventSystem eventSystem) : base(eventSystem) { }

        /// <summary>
        /// Used to initialize/reset the event and populate the data.
        /// </summary>
        /// <param name="locomotionProvider">The <see cref="ILocomotionProvider"/> the event data is addressed at or coming from.</param>
        /// /// <param name="inputSource">The <see cref="IInputSource"/> the event originated from.</param>
        /// <param name="pose">Optional <see cref="Pose"/> providing a teleport target.</param>
        /// <param name="anchor">Optional <see cref="ITeleportAnchor"/> at the teleport target location.</param>
        public void Initialize(ILocomotionProvider locomotionProvider, IInputSource inputSource, Pose pose, ITeleportAnchor anchor)
        {
            BaseInitialize(inputSource);
            LocomotionProvider = locomotionProvider;
            Pose = pose;
            Anchor = anchor;
        }

        /// <summary>
        /// Used to initialize/reset the event and populate the data.
        /// </summary>
        /// <param name="locomotionProvider">The <see cref="ILocomotionProvider"/> the event data is addressed at or coming from.</param>
        /// <param name="inputSource">The <see cref="IInputSource"/> the event originated from.</param>
        public void Initialize(ILocomotionProvider locomotionProvider, IInputSource inputSource)
        {
            BaseInitialize(inputSource);
            LocomotionProvider = locomotionProvider;
            Pose = null;
            Anchor = null;
        }
    }
}
