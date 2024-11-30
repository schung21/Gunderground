using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager instance;
    private bool isInitialized = false;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
        isInitialized = true;
        
    }

    public void PlayerDeaths(string currentLevel)
    {
        if(!isInitialized)
        {
            return;
        }
        CustomEvent customEvent = new CustomEvent("Level_Died")
        {
            {"level_index", currentLevel }
        };
        AnalyticsService.Instance.RecordEvent(customEvent);
        AnalyticsService.Instance.Flush();
      
    }


  
}
