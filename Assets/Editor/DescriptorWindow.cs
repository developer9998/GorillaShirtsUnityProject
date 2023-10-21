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
            boldLabel ??= new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16
            };
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(PropertyFromName("Pack"), new GUIContent("Shirt Pack", "The pack of your shirt\n[Required]"));

            GUILayout.Space(12);
            GUILayout.Label("Mandatory Data", boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("Name"), new GUIContent("Name", "The name of your shirt\n[Required]"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("Author"), new GUIContent("Author", "The author of your shirt\n[Required]"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("Info"), new GUIContent("Info", "The description of your shirt\n[Required]"));

            GUILayout.Space(12);
            GUILayout.Label("Optional Data", boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("customColors"), new GUIContent("Custom Colour"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("invisibility"), new GUIContent("Invisibility"));

            GUILayout.Space(12);
            GUILayout.Label("Objects", boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("Head"), new GUIContent("Head"));
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(PropertyFromName("Body"), new GUIContent("Body", "[Required]"));
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("LeftUpperArm"), new GUIContent("Left Arm (Upper)"));
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(PropertyFromName("LeftLowerArm"), new GUIContent("Left Arm (Lower)"));
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(PropertyFromName("LeftHand"), new GUIContent("Left Hand"));
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("RightUpperArm"), new GUIContent("Right Arm (Upper)"));
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(PropertyFromName("RightLowerArm"), new GUIContent("Right Arm (Lower)"));
            GUILayout.Space(4);
            EditorGUILayout.PropertyField(PropertyFromName("RightHand"), new GUIContent("Right Hand"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
