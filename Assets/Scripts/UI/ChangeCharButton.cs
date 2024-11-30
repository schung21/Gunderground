using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharButton : Button
{

    public static ChangeCharButton instance;


    protected override void Start()
    {
        onClick.AddListener(changeChar);

    }       

    public void changeChar()
    {

        CharacterSelectManager.instance.change = true;
        

    }
}
