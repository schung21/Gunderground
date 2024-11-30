using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class TextSpacing : MonoBehaviour
{
    public bool gunsJP, hasNumber, newAlign, midAlign, rePosY, resize, resize2;
    public Font newFont, oldFont;
    public float newY;
    private int newFontSize;

    // Update is called once per frame

    private void Awake()
    {
        if (LocalizationSettings.SelectedLocale != LocalizationSettings.AvailableLocales.Locales[0])
        {

            if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
            {
                if (rePosY)
                {
                    GetComponent<RectTransform>().anchoredPosition = new Vector3(GetComponent<RectTransform>().anchoredPosition.x,
                       newY, 0f);
                }

            }

            if (newFont == null)
            {
                GetComponent<Text>().lineSpacing = 1.2f;
            }

            if (gunsJP)
            {
                if (GetComponent<Text>().fontSize >= 80)
                {
                    GetComponent<Text>().fontSize -= 20;
                    GetComponent<RectTransform>().anchoredPosition = new Vector3(GetComponent<RectTransform>().anchoredPosition.x,
                        GetComponent<RectTransform>().anchoredPosition.y + 5f, 0f);
                }
              

            }

            if (hasNumber)
            {
                GetComponent<Text>().font = newFont;
            }

            if (newAlign)
            {
                GetComponent<Text>().alignment = TextAnchor.LowerCenter;
            }
            if (midAlign)
            {
                GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            }


            if (resize)
            {
                GetComponent<Text>().resizeTextMaxSize = 90;
            }
            
    
        }
    }

    private void Start()
    {
        newFontSize = GetComponent<Text>().fontSize - 20;
    }
    void Update()
    {
        if(LocalizationSettings.SelectedLocale != LocalizationSettings.AvailableLocales.Locales[0])
        {
            if(LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[1])
            {
                if (rePosY)
                {
                    GetComponent<RectTransform>().anchoredPosition = new Vector3(GetComponent<RectTransform>().anchoredPosition.x,
                       newY, 0f);
                }
         
            }

            if (newFont == null)
            {
                GetComponent<Text>().lineSpacing = 1.2f;
            }

            if (gunsJP)
            {
                if (GetComponent<Text>().fontSize >= 80)
                {
                    GetComponent<Text>().fontSize -= 20;
                    GetComponent<RectTransform>().anchoredPosition = new Vector3(GetComponent<RectTransform>().anchoredPosition.x,
                        GetComponent<RectTransform>().anchoredPosition.y + 5f, 0f);
                }
           
            }

            if (hasNumber)
            {
                GetComponent<Text>().font = newFont;
            }

            if (newAlign)
            {
                GetComponent<Text>().alignment = TextAnchor.LowerCenter;
            }

            if (midAlign)
            {
                GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            }

            if (resize)
            {
                GetComponent<Text>().resizeTextMaxSize = 90;
            }

            if (resize2)
            {
                GetComponent<Text>().fontSize = newFontSize;
            }

        }
        else
        {
            GetComponent<Text>().lineSpacing = 1f;

            if (gunsJP)
            {
                GetComponent<Text>().fontSize = 80;
            }
            
           /* if (newAlign)
            {
                GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            }*/
        }
    }
}
