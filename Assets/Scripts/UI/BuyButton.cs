using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : Button
{
    public static BuyButton instance;
  

    protected override void Start()
    {
        onClick.AddListener(buyItem);

    }

    public void buyItem()
    {  
        PlayerController.instance.buyItem = true;

    }
}
