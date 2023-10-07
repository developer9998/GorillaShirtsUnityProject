using UnityEngine;
using UnityEditor;

namespace GorillaShirts.Data 
{
    [CustomEditor(typeof(ShirtDescriptor))]
    public class DescriptorWindow : Editor
    {
        private GUIStyle boldLabel;

        public SerializedProperty PropertyFromName(string name) => serializedObject.FindProperty(name);

        protected void OnEnable()
        {
            boldLabel = new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 20
            };
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(11);
            GUILayout.Label("Required Data", boldLabel);
            GUILayout.Space(11);

            EditorGUILayout.PropertyField(PropertyFromName("Name"), new GUIContent("Name"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("Author"), new GUIContent("Author"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("Info"), new GUIContent("Description"));
            GUILayout.Space(12);

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Objects", boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("Head"), new GUIContent("Head"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("Body"), new GUIContent("Body"));
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("LeftUpperArm"), new GUIContent("Left Arm (Upper)"));
            GUILayout.Space(2);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(PropertyFromName("LeftLowerArm"), new GUIContent("Left Arm (Lower)"));
            GUILayout.Space(2);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(PropertyFromName("LeftHand"), new GUIContent("Left Hand"));
            GUILayout.Space(8);
            EditorGUI.indentLevel -= 2;

            EditorGUILayout.PropertyField(PropertyFromName("RightUpperArm"), new GUIContent("Right Arm (Upper)"));
            GUILayout.Space(2);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(PropertyFromName("RightLowerArm"), new GUIContent("Right Arm (Lower)"));
            GUILayout.Space(2);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(PropertyFromName("RightHand"), new GUIContent("Right Hand"));
            GUILayout.Space(12);
            EditorGUI.indentLevel -= 2;

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Optional Data", boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("customColors"), new GUIContent("Custom Colour"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("invisibility"), new GUIContent("Invisibility"));
            GUILayout.Space(2);

            if (PrefWindow.DeveloperMode)
            {
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
                GUILayout.Label("Developer Data", boldLabel);
                GUILayout.Space(8);

                EditorGUILayout.PropertyField(PropertyFromName("isCreator"), new GUIContent("Verified"));
                GUILayout.Space(2);
                EditorGUILayout.PropertyField(PropertyFromName("SillyNSteady"), new GUIContent("Silly & Steady"));
                GUILayout.Space(12);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
