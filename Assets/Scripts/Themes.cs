using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Themes : MonoBehaviour
{
    public Color32[] ColorText;
    public Color32[] Color1, Color2, Color3, Color4, Color5, Color6;
    public Image[] pallette1, pallette2, pallette3, pallette4, pallette5, pallette6;
    public TextMeshProUGUI[] text;
    public int numPallette;

    public void Awake()
    {
        
    }

    public void Start()
    {
        if (!PlayerPrefs.HasKey("tema"))
        {
            PlayerPrefs.SetInt("tema", numPallette);
        }
        numPallette = PlayerPrefs.GetInt("tema");
        Tema();
    }
    public void Tema()
    {
        PlayerPrefs.SetInt("tema", numPallette);

        for (int i = 0; i < text.Length; i++)
        {
            text[i].color = ColorText[numPallette];
        }

        for (int i = 0; i < pallette1.Length; i++)
        {
            pallette1[i].color = Color1[numPallette];
        }
        for (int i = 0; i < pallette2.Length; i++)
        {
            pallette2[i].color = Color2[numPallette];
        }
        for (int i = 0; i < pallette3.Length; i++)
        {
            pallette3[i].color = Color3[numPallette];
        }
        for (int i = 0; i < pallette4.Length; i++)
        {
            pallette4[i].color = Color4[numPallette];
        }
        for (int i = 0; i < pallette5.Length; i++)
        {
            pallette5[i].color = Color5[numPallette];
        }
        for (int i = 0; i < pallette6.Length; i++)
        {
            pallette6[i].color = Color6[numPallette];
        }
    }
}
