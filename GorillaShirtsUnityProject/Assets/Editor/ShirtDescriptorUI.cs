using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GorillaShirts.Data 
{
    [CustomEditor(typeof(ShirtDescriptor))]
    public class ShirtDescriptorUI : Editor
    {
        private readonly List<SerializedProperty> properties = new List<SerializedProperty>();

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
        }

        public override void OnInspectorGUI()
        {
            Font utopium = Resources.Load("Utopium-Regular") as Font;   

            // GUIStyles
            GUIStyle titleLabel = new GUIStyle();
            titleLabel.fontStyle = FontStyle.Normal;
            titleLabel.alignment = TextAnchor.MiddleCenter;
            titleLabel.font = utopium;
            titleLabel.fontSize = 20;

            GUIStyle creditLabel = new GUIStyle();
            creditLabel.alignment = TextAnchor.MiddleCenter;
            creditLabel.font = utopium;
            creditLabel.fontSize = 14;

            GUIStyle boldLabel = new GUIStyle();
            boldLabel.alignment = TextAnchor.MiddleCenter;
            boldLabel.font = utopium;
            boldLabel.fontSize = 16;

            // Intro
            GUILayout.Space(14);
            GUILayout.Label("GorillaShirts Descriptor".ToUpper(), titleLabel);
            GUILayout.Space(3);
            GUILayout.Label("A mod by dev9998".ToUpper(), creditLabel);
            GUILayout.Space(12);

            // Options
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Basic Information <color=#D1282Fff>(All Required)</color>".ToUpper(), boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(properties[0], new GUIContent("Name"));
            EditorGUILayout.PropertyField(properties[1], new GUIContent("Author"));
            EditorGUILayout.PropertyField(properties[2], new GUIContent("Info"));

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Optional Data".ToUpper(), boldLabel);
            GUILayout.Space(8);
            EditorGUILayout.PropertyField(properties[9], new GUIContent("Custom Colors"));
            //EditorGUILayout.PropertyField(properties[11], new GUIContent("isbydev"));
            //EditorGUILayout.PropertyField(properties[12], new GUIContent("issillyandsteady"));

            GUILayout.Space(10);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Objects <color=#D1282Fff>(Body Required)</color>".ToUpper(), boldLabel);
            GUILayout.Space(8);
            EditorGUILayout.PropertyField(properties[3], new GUIContent("Body"));

            if (properties[3].objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                GUILayout.Space(1);
                EditorGUILayout.PropertyField(properties[13], new GUIContent("Head"));
                EditorGUILayout.PropertyField(properties[8], new GUIContent("Boobs"));
                GUILayout.Space(1);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(properties[4], new GUIContent("Left Upper Arm"));
            if (properties[4].objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                GUILayout.Space(1);
                EditorGUILayout.PropertyField(properties[6], new GUIContent("Left Lower Arm"));
                if (properties[6].objectReferenceValue != null)
                {
                    EditorGUI.indentLevel++;
                    GUILayout.Space(1);
                    EditorGUILayout.PropertyField(properties[14], new GUIContent("Left Hand"));
                    GUILayout.Space(1);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(1);
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(properties[5], new GUIContent("Right Upper Arm"));
            if (properties[5].objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                GUILayout.Space(1);
                EditorGUILayout.PropertyField(properties[7], new GUIContent("Right Lower Arm"));
                if (properties[7].objectReferenceValue != null)
                {
                    EditorGUI.indentLevel++;
                    GUILayout.Space(1);
                    EditorGUILayout.PropertyField(properties[15], new GUIContent("Right Hand"));
                    GUILayout.Space(1);
                    EditorGUI.indentLevel--;
                }
                GUILayout.Space(1);
                EditorGUI.indentLevel--;
            }

            GUILayout.Space(5);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Optional Objects".ToUpper(), boldLabel);
            GUILayout.Space(8);
            EditorGUILayout.PropertyField(properties[10], new GUIContent("Fur Objects"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
