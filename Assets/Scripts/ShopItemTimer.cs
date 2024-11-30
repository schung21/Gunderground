using System;
using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.Localization.Plugins.XLIFF.V12;*/
using UnityEditor.Rendering;
using UnityEngine;

public class ShopItemTimer : MonoBehaviour
{
    public static ShopItemTimer instance;
    public int Cost;
    public bool isInZone, timerItem;
    public string lastBuyTime;
    public GameObject itemObject, caseObject, texts;
    public BoxCollider2D box1, box2;

    DateTime currentTime;
    DateTime lastTimeClicked;
    TimeSpan span;


    public bool isBought;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        if (timerItem)
        {
            if (CharTracker.instance.storeItemTime1 != "" || CharTracker.instance.storeItemTime1 != null)
            {
                lastBuyTime = CharTracker.instance.storeItemTime1;

            }
        }

        isBought = false;

    }

    // Update is called once per frame
    void Update()
    {

        if (timerItem)
        {

            currentTime = DateTime.UtcNow;

            if (lastBuyTime != "" && lastBuyTime != null)
            {
                lastTimeClicked = DateTime.Parse(lastBuyTime);
                span = (currentTime - lastTimeClicked);
                box1.enabled = false;
                box2.enabled = false;
                itemObject.SetActive(false);
                caseObject.SetActive(false);
                texts.SetActive(false);
            }

            if (span.TotalMinutes >= 1440 && lastBuyTime != "")
            {

                if (Vector3.Distance(PlayerController.instance.transform.position, transform.position) > 5)
                {
                    if (!isBought)
                    {
                        box1.enabled = true;
                        box2.enabled = true;
                        itemObject.SetActive(true);
                        caseObject.SetActive(true);
                        texts.SetActive(true);
                    }
                }

                lastBuyTime = "";
                CharTracker.instance.SavePlayer();
            }

        }
    }


}
