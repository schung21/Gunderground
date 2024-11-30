using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchButton : Button
{
    public static SwitchButton instance;


    protected override void Awake()
    {
        instance = this;
    }

    protected override void Start()
    {
        onClick.AddListener(Switch);
    }

    public void Switch()
    {
     
            PlayerController.instance.changeGun = true;
        


    }

}
