[System.Serializable]
public class ShirtJSON
{
    public string assetName;
    public string packName;

    public SDescriptor infoDescriptor;
    public SConfig infoConfig;
}

[System.Serializable]
public class SDescriptor
{
    public string shirtName;
    public string shirtAuthor;
    public string shirtDescription;
}

[System.Serializable]
public class SConfig
{
    public bool customColors;
    public bool invisibility;
    public bool wobbleLoose;
    public bool wobbleLockHorizontal;
    public bool wobbleLockVertical;
}
