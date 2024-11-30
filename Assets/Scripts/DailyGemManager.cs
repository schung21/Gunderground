using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class DailyGemManager : MonoBehaviour
{
    public static DailyGemManager instance;
    DateTime currentTime;
    DateTime lastTimeClicked;
    TimeSpan span;
    public string lastGiftTime;

    [Header("GemGift")]
    public GameObject dailyGem;
    public Transform gemPoint;

    private bool gemAvailable;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //Debug.Log(CharTracker.instance.dailyGemTime); 
        if (CharTracker.instance.dailyGiftTime != null || CharTracker.instance.dailyGiftTime != "")
        {  
            lastGiftTime = CharTracker.instance.dailyGiftTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = DateTime.UtcNow; 

        if (lastGiftTime != "" && lastGiftTime != null)
        {
            lastTimeClicked = DateTime.Parse(lastGiftTime);
            span = (currentTime - lastTimeClicked);
        }

        if (span.TotalMinutes >= 1440 && lastGiftTime != "")
        {
            lastGiftTime = "";
           
            CharTracker.instance.SavePlayer();
        }
    }

    public void GiveGem()
    {
        if (lastGiftTime == null || lastGiftTime == "")
        {
            Instantiate(dailyGem, gemPoint.position, gemPoint.rotation);
            lastGiftTime = DateTime.UtcNow.ToString();
            CharTracker.instance.SavePlayer();
        }

    }
}
