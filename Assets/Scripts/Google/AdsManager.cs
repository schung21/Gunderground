using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System.Runtime.CompilerServices;
using GoogleMobileAds.Ump.Api;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;
    RewardedAd rewardedAd;
    public string adUnitId;
    public UIController uiScript;
    public bool isRevive, isBuff, isGun;
    private bool isActive;

    private void Awake()
    {
        if (isGun)
        {
            if(instance == null)
            {
                isActive = true;
            }

            if (!isActive)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        uiScript = UIController.instance;

        if (ConsentInformation.CanRequestAds())
        {

            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(HandleInitCompleteAction);

        }

    }

    public void InitializeLoad()
    {
        if (ConsentInformation.CanRequestAds())
        {
            MobileAds.RaiseAdEventsOnUnityMainThread = true;
            MobileAds.Initialize(HandleInitCompleteAction);
        }
    }

    public void InitializeEmptyLoad()
    {
        if (ConsentInformation.CanRequestAds())
        {
            if (rewardedAd == null)
            {
                MobileAds.RaiseAdEventsOnUnityMainThread = true;
                MobileAds.Initialize(HandleInitCompleteAction);
            }
        }
    }

    private void HandleInitCompleteAction(InitializationStatus initializationStatus)
    {

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {

            if (!isRevive && !isBuff && LevelManager.instance.isCamp)
            {
                LoadEmptyRewardedAd();
            }
            else if (isRevive && !LevelManager.instance.isCamp)
            {
                LoadEmptyRewardedAd();
            }

        });

    }

    private void Update()
    {
        if (isRevive || isGun)
        {
            uiScript = UIController.instance;
        }

        if (SceneManager.GetActiveScene().name == "Title Scene" || uiScript.destroyAds)
        {
            Destroy(gameObject);
        }
    }
    /*    private void Update()
        {
            if (rewardedAd == null && !rewardedAd.CanShowAd())
            {
                if (LevelManager.instance.isCamp)
                {
                    uiScript.adButton1.enabled = false;
                    uiScript.adButton2.enabled = false;
                }
                else if (!LevelManager.instance.isCamp && !LevelManager.instance.isCutscene) 
                {
                    uiScript.adButton3.enabled = false;
                }
            }
            else
            {
                if (LevelManager.instance.isCamp)
                {
                    uiScript.adButton1.enabled = true;
                    uiScript.adButton2.enabled = true;
                }
                else if (!LevelManager.instance.isCamp && !LevelManager.instance.isCutscene)
                {
                    uiScript.adButton3.enabled = true;
                }
            }
        }*/

    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(adUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
              // if error is not null, the load request failed.
              if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
                RegisterEventHandlers(rewardedAd);
            });

    }

    public void LoadEmptyRewardedAd()
    {
       
        if (rewardedAd == null)
        {
            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            RewardedAd.Load(adUnitId, adRequest,
                (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                       "with error : " + error);
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                              + ad.GetResponseInfo());

                    rewardedAd = ad;
                    RegisterEventHandlers(rewardedAd);
                });
        }
    }

    public void ShowRewardedAdGun()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                uiScript.adRewardGun();
            });
        }
        else
        {
            uiScript.NoAdWarning();
            //LoadEmptyRewardedAd();
        }
    }
    public void ShowRewardedAdBuff()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                uiScript.adRewardBuff();
            });
        }
        else
        {
            uiScript.NoAdWarning();
           //LoadEmptyRewardedAd();
        }
    }
  
    public void ShowRewardedAdLife()
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                uiScript.adRewardLife();
            });
        }
        else
        {
            uiScript.NoAdWarning();
            //LoadEmptyRewardedAd();
        }
    }


    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log(string.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
            });
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            });
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("Rewarded ad was clicked.");
            });
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() => 
            { Debug.Log("Rewarded ad full screen content opened."); });
        
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("Rewarded ad full screen content closed.");

                if (!isBuff)
                {
                    LoadRewardedAd();
                }
                else if (isBuff)
                {

                    rewardedAd.Destroy();
                    rewardedAd = null;

                }
            });
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
                //uiScript.NoAdWarning();
                LoadRewardedAd();
            });
        };
    }
}
