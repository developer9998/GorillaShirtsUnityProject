using UnityEngine;

[AddComponentMenu("GorillaShirts/Cosmetics/Billboard")]
public class Billboard : ShirtComponent
{
    public enum BillboardMode
    {
        Default, Vertical
    }

    [Tooltip("Default: Object will face the player on all coordinates\n\nLockVertical: Object will face the player on the vertical coordinate")]
    public BillboardMode mode;
}
