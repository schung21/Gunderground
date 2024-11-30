using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FireButton : Button
{
    public static FireButton instance;
    public bool pressed;


    protected override void Awake()
    {
        instance = this;
    }

    protected override void Start()
    {
        //onClick.AddListener(TapShoot);
    }

   /* public void TapShoot()
    {
        if (Gun.instance.canShoot == true)
        {
            Gun.instance.tapShoot = true;
        }
    }*/

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        pressed = true;
        //StartCoroutine(Gun.instance.Shoot());
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        pressed = false;
    }




}
