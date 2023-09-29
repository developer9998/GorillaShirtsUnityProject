[System.Serializable]
public class ShirtJSON
{
    public string fileName;
    public SDescriptor descriptor;
    public SConfig config;
}

[System.Serializable]
public class SDescriptor
{
    public string shirtName;
    public string shirtAuthor;
    public string shirtInfo;
}

[System.Serializable]
public class SConfig
{
    public bool customColors;
    public bool invisibility;
    public bool isCreator;
    public bool SillyNSteady;
}
