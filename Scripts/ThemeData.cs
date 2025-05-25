using UnityEngine;

[CreateAssetMenu(fileName = "ThemeData", menuName = "ThemeData", order = 1)]
public class ThemeData : ScriptableObject
{
    [Header("Material")]
    public Material grassMaterial;
    public Material groundMaterial;
    [Header("Color")]
    public Color fogColor;
}