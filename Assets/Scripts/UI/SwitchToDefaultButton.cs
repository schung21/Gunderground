using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwitchToDefaultButton : Button
{
    public static SwitchToDefaultButton instance;


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

        PlayerController.instance.changeDefaultGun = true;



    }

}
