using System;
using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.Localization.Plugins.XLIFF.V12;*/
using UnityEditor.Rendering;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public static ShopItem instance;
    public int Cost;
    public bool isInZone;
   
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    
    }

    // Update is called once per frame
/*    void Update()
    {
     
    }*/

   
}
