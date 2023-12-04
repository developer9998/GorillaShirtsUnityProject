using UnityEngine;

[AddComponentMenu("GorillaShirts/Cosmetics/Billboard")]
public class Billboard : MonoBehaviour
{
    public enum BillboardMode
    {
        Default, LockVertical
    }

    [Tooltip("Default: Object will face the player on all coordinates\n\nLockVertical: Object will face the player on X/Z coordinates, with the Y coordinate being untouched")]
    public BillboardMode mode;
}
