// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Editor.Extensions;
using RealityToolkit.Editor.UX.Pointers;
using RealityToolkit.Locomotion.UX;
using UnityEditor;
using UnityEngine;

namespace RealityToolkit.Locomotion.Editor
{
    [CustomEditor(typeof(TeleportInteractor))]
    public class TeleportInteractorInspector : LinePointerInspector
    {
        private readonly GUIContent parabolicFoldoutHeaderContent = new GUIContent("Parabolic Pointer Options");
        private readonly GUIContent teleportFoldoutHeader = new GUIContent("Teleport Pointer Settings");

        private SerializedProperty lineColorAnchor;
        private SerializedProperty minParabolaVelocity;
        private SerializedProperty maxParabolaVelocity;
        private SerializedProperty minDistanceModifier;
        private SerializedProperty maxDistanceModifier;

        protected override void OnEnable()
        {
            DrawBasePointerActions = false;
            base.OnEnable();

            lineColorAnchor = serializedObject.FindProperty(nameof(lineColorAnchor));

            minParabolaVelocity = serializedObject.FindProperty(nameof(minParabolaVelocity));
            maxParabolaVelocity = serializedObject.FindProperty(nameof(maxParabolaVelocity));
            minDistanceModifier = serializedObject.FindProperty(nameof(minDistanceModifier));
            maxDistanceModifier = serializedObject.FindProperty(nameof(maxDistanceModifier));
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

            if (minParabolaVelocity.FoldoutWithBoldLabelPropertyField(parabolicFoldoutHeaderContent))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(maxParabolaVelocity);
                EditorGUILayout.PropertyField(minDistanceModifier);
                EditorGUILayout.PropertyField(maxDistanceModifier);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}