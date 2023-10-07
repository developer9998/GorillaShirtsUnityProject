using UnityEditor;

public class PrefWindow : SettingsProvider
{
    public static bool DeveloperMode { get; private set; }

    public PrefWindow() 
        : base("Project/GorillaShirts", SettingsScope.Project) { }

    [SettingsProvider] public static SettingsProvider CreateCustomSettingsProvider()
        => new PrefWindow();

    public override void OnGUI(string search)
    {
        EditorGUI.BeginChangeCheck();
        bool _DeveloperModeTemp = EditorGUILayout.Toggle("Developer Mode", DeveloperMode);

        if (EditorGUI.EndChangeCheck())
        {
            DeveloperMode = _DeveloperModeTemp;
        }
    }
}
