// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityToolkit.Editor.Utilities;
using RealityToolkit.Locomotion.Teleportation;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace RealityToolkit.Locomotion.Editor
{
    [CustomEditor(typeof(TeleportAnchor), true)]
    public class TeleportAnchorInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var inspector = new VisualElement();

            var anchorTransformField = new PropertyField()
            {
                label = "Anchor Transform",
                bindingPath = "anchorTransform"
            };

            inspector.Add(anchorTransformField);

            var overrideOrientationField = new Toggle("OverrideTarget Orientation")
            {
                bindingPath = "overrideOrientation"
            };

            inspector.Add(overrideOrientationField);
            inspector.Add(UIElementsUtilities.VerticalSpace());

            var onTargetedChangedField = new PropertyField()
            {
                bindingPath = "onTargetedChanged"
            };

            inspector.Add(onTargetedChangedField);
            inspector.Add(UIElementsUtilities.VerticalSpace());

            var onActivatedField = new PropertyField()
            {
                bindingPath = "onActivated"
            };

            inspector.Add(onActivatedField);

            return inspector;
        }
    }
}
