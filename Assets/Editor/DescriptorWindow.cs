using UnityEngine;
using UnityEditor;

namespace GorillaShirts.Data 
{
    [CustomEditor(typeof(ShirtDescriptor))]
    public class DescriptorWindow : Editor
    {
        private GUIStyle boldLabel, subBoldLabel;

        public SerializedProperty PropertyFromName(string name) => serializedObject.FindProperty(name);

        protected void OnEnable()
        {
            boldLabel ??= new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 16,
            };
            subBoldLabel ??= new GUIStyle
            {
                fontStyle = FontStyle.Bold,
                fontSize = 14
            };
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(PropertyFromName("Pack"), new GUIContent("Shirt Pack", "The pack of your shirt\n[Required]"));

            GUILayout.Space(12);
            GUILayout.Label("Settings", boldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("Name"), new GUIContent("Name", "The name of your shirt\n[Required]"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("Author"), new GUIContent("Author", "The author of your shirt\n[Required]"));
            GUILayout.Space(4f);
            EditorGUILayout.PropertyField(PropertyFromName("Info"), new GUIContent("Description", "The description of your shirt\n[Required]"));

            GUILayout.Space(12);
            EditorGUI.indentLevel++;
            GUILayout.Label("Appearence Factors", subBoldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("customColors"), new GUIContent("Custom Colour", "Materials with the '_BaseColor' property will have that property set to the colour for the player"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("invisibility"), new GUIContent("Invisibility", "The player wearing the shirt will become invisible when wearing it"));

            GUILayout.Space(12);
            GUILayout.Label("Physical Factors", subBoldLabel);
            GUILayout.Space(8);

            EditorGUILayout.PropertyField(PropertyFromName("wobbleLoose"), new GUIContent("Loose", "Wobble physics can move loosely with a reduced amount of constraint"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("wobbleLockHorizontal"), new GUIContent("Lock Horizontal", "Wobble physics will be locked on the X/Z coordinates"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("wobbleLockVertical"), new GUIContent("Lock Vertical", "Wobble physics will be locked on the Y coordinate"));
            GUILayout.Space(2);
            EditorGUILayout.PropertyField(PropertyFromName("wobbleLockRoot"), new GUIContent("Lock Root", "Wobble physics will be locked on the root object (Object with the 'WobbleBone' component)"));

            GUILayout.Space(12);
            EditorGUI.indentLevel--;
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
