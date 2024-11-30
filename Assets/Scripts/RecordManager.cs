using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class RecordManager : MonoBehaviour
{
    public static RecordManager instance;
    public double timeRecorded;
    public Text text1;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //timeRecorded = CharTracker.instance.timeRecord;
        var time = TimeSpan.FromSeconds(timeRecorded);
        text1.text = string.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);

    }

   
}
