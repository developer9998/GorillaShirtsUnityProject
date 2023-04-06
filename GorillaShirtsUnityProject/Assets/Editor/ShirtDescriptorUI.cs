using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GorillaShirts.Data 
{
    [CustomEditor(typeof(ShirtDescriptor))]
    public class ShirtDescriptorUI : Editor
    {
        private readonly List<SerializedProperty> properties = new List<SerializedProperty>();

        private GUIStyle titleLabel;
        private GUIStyle creditLabel;
        private GUIStyle boldLabel;

        protected void OnEnable()
        {
            if (properties.Count != 0) properties.Clear();
            properties.Add(serializedObject.FindProperty("Name")); // 0
            properties.Add(serializedObject.FindProperty("Author"));
            properties.Add(serializedObject.FindProperty("Info")); // 2
            properties.Add(serializedObject.FindProperty("Body"));
            properties.Add(serializedObject.FindProperty("LeftUpperArm")); // 4
            properties.Add(serializedObject.FindProperty("RightUpperArm"));
            properties.Add(serializedObject.FindProperty("LeftLowerArm")); // 6
            properties.Add(serializedObject.FindProperty("RightLowerArm"));
            properties.Add(serializedObject.FindProperty("Boobs")); // 8
            properties.Add(serializedObject.FindProperty("customColors"));
            properties.Add(serializedObject.FindProperty("FurTextures")); // 10
            properties.Add(serializedObject.FindProperty("isCreator"));
            properties.Add(serializedObject.FindProperty("SillyNSteady")); // 12
            properties.Add(serializedObject.FindProperty("Head"));
            properties.Add(serializedObject.FindProperty("LeftHand")); // 14
            properties.Add(serializedObject.FindProperty("RightHand"));

            // GUIStyles
            Font utopium = Resources.Load("Utopium-Regular") as Font;

            titleLabel = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                font = utopium,
                fontSize = 20
            };

            creditLabel = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleLeft,
                font = utopium,
                fontSize = 11
            };

            boldLabel = new GUIStyle
            {
                fontStyle = FontStyle.Normal,
                alignment = TextAnchor.MiddleCenter,
                font = utopium,
                fontSize = 15
            };
        }

        public override void OnInspectorGUI()
        {
            // Intro
            GUILayout.Space(14);
            GUILayout.Label("GorillaShirts\n[Descriptor]".ToUpper(), titleLabel);
            GUILayout.Space(6);

            // Options
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Basic Information <color=#D1282Fff>(All Required)</color>".ToUpper(), boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(properties[0], new GUIContent("Name"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  the name of your shirt.  ".ToUpper(), creditLabel);
            GUILayout.Space(6);

            EditorGUILayout.PropertyField(properties[1], new GUIContent("Author"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  that's your name!  ".ToUpper(), creditLabel);
            GUILayout.Space(6);

            EditorGUILayout.PropertyField(properties[2], new GUIContent("Description"));
            GUILayout.Space(2.2f); 
            GUILayout.Label("  just type something out.  ".ToUpper(), creditLabel);
            GUILayout.Space(6);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Objects (Body & Head)".ToUpper(), boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(properties[3], new GUIContent("Body"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  your main shirt object.  ".ToUpper(), creditLabel);
            GUILayout.Space(6);

            EditorGUILayout.PropertyField(properties[13], new GUIContent("Head"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  your head slot.  ".ToUpper(), creditLabel);
            GUILayout.Space(6);

            EditorGUILayout.PropertyField(properties[8], new GUIContent("Boobs"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  not too necessary..  ".ToUpper(), creditLabel);
            GUILayout.Space(12);

            GUILayout.Label("Objects (Left Arm)".ToUpper(), boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(properties[4], new GUIContent("Left Arm (Upper)"));
            GUILayout.Space(2.2f);

            EditorGUILayout.PropertyField(properties[6], new GUIContent("Left Arm (Lower)"));
            GUILayout.Space(2.2f);

            EditorGUILayout.PropertyField(properties[14], new GUIContent("Left Hand"));
            GUILayout.Space(12);

            GUILayout.Label("Objects (Right Arm)".ToUpper(), boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(properties[5], new GUIContent("Right Arm (Upper)"));
            GUILayout.Space(2.2f);

            EditorGUILayout.PropertyField(properties[7], new GUIContent("Right Arm (Lower)"));
            GUILayout.Space(2.2f);

            EditorGUILayout.PropertyField(properties[15], new GUIContent("Right Hand"));
            GUILayout.Space(5f);

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Optional Objects".ToUpper(), boldLabel);
            GUILayout.Space(8);
            EditorGUILayout.PropertyField(properties[9], new GUIContent("Custom Colour"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  if material colours should match\n   with the player's colour.".ToUpper(), creditLabel);
            GUILayout.Space(12);

            EditorGUILayout.PropertyField(properties[10], new GUIContent("Fur Objects"));
            GUILayout.Space(2.2f);
            GUILayout.Label("  objects that will have their material set\n   to the player's main fur material.".ToUpper(), creditLabel);
            GUILayout.Space(12);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
