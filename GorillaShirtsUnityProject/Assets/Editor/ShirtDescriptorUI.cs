using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml.Linq;

namespace GorillaShirts.Data 
{
    [CustomEditor(typeof(ShirtDescriptor))]
    public class ShirtDescriptorUI : Editor
    {
        private List<SerializedProperty> properties = new List<SerializedProperty>();

        protected void OnEnable()
        {
            if (properties.Count != 0) properties.Clear();
            properties.Add(serializedObject.FindProperty("Name"));
            properties.Add(serializedObject.FindProperty("Author"));
            properties.Add(serializedObject.FindProperty("Info"));
            properties.Add(serializedObject.FindProperty("Body"));
            properties.Add(serializedObject.FindProperty("LeftUpperArm"));
            properties.Add(serializedObject.FindProperty("RightUpperArm"));
            properties.Add(serializedObject.FindProperty("LeftLowerArm"));
            properties.Add(serializedObject.FindProperty("RightLowerArm"));
            properties.Add(serializedObject.FindProperty("Boobs"));
            properties.Add(serializedObject.FindProperty("CustomColors"));
            properties.Add(serializedObject.FindProperty("FurTextures"));
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(properties[0], new GUIContent("Name"));
            EditorGUILayout.PropertyField(properties[1], new GUIContent("Author"));
            EditorGUILayout.PropertyField(properties[2], new GUIContent("Info"));
            EditorGUILayout.PropertyField(properties[9], new GUIContent("Custom Colors"));

            GUILayout.Space(10);

            EditorGUILayout.PropertyField(properties[3], new GUIContent("Body"));

            if (properties[3].objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(properties[8], new GUIContent("Boobs"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(properties[4], new GUIContent("Left Upper Arm"));
            if (properties[4].objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(properties[6], new GUIContent("Left Lower Arm"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(properties[5], new GUIContent("Right Upper Arm"));
            if (properties[5].objectReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(properties[7], new GUIContent("Right Lower Arm"));
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.PropertyField(properties[10], new GUIContent("Fur Objects"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
