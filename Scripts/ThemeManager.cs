using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour
{
    public LevelManager levelManager;
    public bool isOnMainMenu = false;

    public ThemeData paleGreenTheme;
    public ThemeData warmBrownTheme;
    public ThemeData pinkTheme;
    public ThemeData cyberBlueTheme;
    public ThemeData scarletTheme;
    public ThemeData lightGreenTheme;
    public ThemeData mintTheme;
    public ThemeData monochromeTheme;
    public ThemeData orangeTheme;
    public ThemeData blueTheme;
    public ThemeData purpleTheme;

    public ThemeType themeType;
    public Transform terrain;

    public enum ThemeType
    {
        paleGreen,
        warmBrown,
        pink,
        cyberBlue,
        scarlet,
        lightGreen,
        mint,
        monochrome,
        orange,
        blue,
        purple
    }

    private void Awake()
    {
        if(!isOnMainMenu)
        {
            themeType = levelManager.level.theme;
        }
        switch (themeType)
        {
            case ThemeType.paleGreen:
                UpdateTheme(paleGreenTheme);
                break;
            case ThemeType.warmBrown:
                UpdateTheme(warmBrownTheme);
                break;
            case ThemeType.pink:
                UpdateTheme(pinkTheme);
                break;
            case ThemeType.cyberBlue:
                UpdateTheme(cyberBlueTheme);
                break;
            case ThemeType.scarlet:
                UpdateTheme(scarletTheme);
                break;
            case ThemeType.lightGreen:
                UpdateTheme(lightGreenTheme);
                break;
            case ThemeType.mint:
                UpdateTheme(mintTheme);
                break;
            case ThemeType.monochrome:
                UpdateTheme(monochromeTheme);
                break;
            case ThemeType.orange:
                UpdateTheme(orangeTheme);
                break;
            case ThemeType.blue:
                UpdateTheme(blueTheme);
                break;
            case ThemeType.purple:
                UpdateTheme(purpleTheme);
                break;
        }
    }

    private void UpdateTheme(ThemeData themeData)
    {
        terrain.GetComponent<ModelGrass>().grassMaterial = themeData.grassMaterial;
        terrain.gameObject.GetComponent<Renderer>().material = themeData.groundMaterial;
        Camera.main.GetComponent<Fog>().fogColor = themeData.fogColor;
    }
}
