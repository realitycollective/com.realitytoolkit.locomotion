// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Editor.Extensions;
using RealityToolkit.Editor.UX.Pointers;
using RealityToolkit.Locomotion.UX;
using UnityEditor;
using UnityEngine;

namespace RealityToolkit.Locomotion.Editor
{
    [CustomEditor(typeof(TeleportPointer))]
    public class TeleportPointerInspector : LinePointerInspector
    {
        private readonly GUIContent teleportFoldoutHeader = new GUIContent("Teleport Pointer Settings");

        private SerializedProperty lineColorAnchor;

        protected override void OnEnable()
        {
            DrawBasePointerActions = false;
            base.OnEnable();

            lineColorAnchor = serializedObject.FindProperty(nameof(lineColorAnchor));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            if (lineColorAnchor.FoldoutWithBoldLabelPropertyField(teleportFoldoutHeader))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(lineColorAnchor);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
