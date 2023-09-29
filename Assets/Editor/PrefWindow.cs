using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefWindow : SettingsProvider
{
    public static bool DeveloperModeTemp { get; private set; }

    public PrefWindow() 
        : base("Project/GorillaShirts", SettingsScope.Project) { }

    [SettingsProvider] public static SettingsProvider CreateCustomSettingsProvider()
        => new PrefWindow();

    public override void OnGUI(string search)
    {
        EditorGUI.BeginChangeCheck();
        bool _DeveloperModeTemp = EditorGUILayout.Toggle("Developer Mode", DeveloperModeTemp);

        if (EditorGUI.EndChangeCheck())
        {
            DeveloperModeTemp = _DeveloperModeTemp;
        }
    }
}
