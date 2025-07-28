using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GorillaShirts.Behaviours.Cosmetic;
using System.Linq;

namespace GorillaShirtsUnityProject
{
    [CustomEditor(typeof(ShirtDescriptor))]
    public class DescriptorEditor : Editor
    {
        private string[] allProperties;

        public void OnEnable()
        {
            allProperties = typeof(ShirtDescriptor).GetFields().Select(x => x.Name).Append("m_Script").ToArray();
        }

        private void DrawProperties(params string[] properties)
        {
            DrawPropertiesExcluding(serializedObject, allProperties.Except(properties).ToArray());
        }

        public override void OnInspectorGUI()
        {
            serializedObject?.Update();
            ShirtDescriptor descriptor = (ShirtDescriptor)target;

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.PackName)), new GUIContent("Shirt Pack"));

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.ShirtName)), new GUIContent("Name"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.Author)), new GUIContent("Author"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.Description)), new GUIContent("Description"));

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.Body)), new GUIContent("Body"));
            GUILayout.Space(6f);
            DrawProperties(nameof(ShirtDescriptor.LeftUpperArm), nameof(ShirtDescriptor.LeftLowerArm), nameof(ShirtDescriptor.LeftHand));
            GUILayout.Space(6f);
            DrawProperties(nameof(ShirtDescriptor.RightUpperArm), nameof(ShirtDescriptor.RightLowerArm), nameof(ShirtDescriptor.RightHand));
            GUILayout.Space(6f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.Head)), new GUIContent("Head"));

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.Fallback)), new GUIContent("Fallback"));
            GUILayout.Space(6f);
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(ShirtDescriptor.BodyType)), new GUIContent("Body Type"));
            GUILayout.Space(6f);
            DrawProperties(nameof(ShirtDescriptor.WearSound), nameof(ShirtDescriptor.RemoveSound));
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            if (GUILayout.Button("Export GorillaShirt"))
            {
                string path = EditorUtility.SaveFilePanel("Export GorillaShirt", "", descriptor.ShirtName + ".gshirt", "gshirt");
                if (!string.IsNullOrEmpty(path) && !string.IsNullOrWhiteSpace(path))
                {
                    EditorUtility.SetDirty(descriptor);
                    ExporterUtils.ExportPackage(descriptor, path);
                }
            }

            serializedObject?.ApplyModifiedProperties();
        }
    }
}