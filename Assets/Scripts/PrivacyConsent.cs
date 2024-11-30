using GoogleMobileAds.Api;
using GoogleMobileAds.Api.Mediation.LiftoffMonetize;
using GoogleMobileAds.Api.Mediation.UnityAds;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrivacyConsent : MonoBehaviour
{

    private bool isNoAd;
    // Start is called before the first frame update
    void Start()
    {
     
        MobileAds.RaiseAdEventsOnUnityMainThread = true;

            #if !UNITY_EDITOR // 에디터에선 GDPR 동의 홀드
                        Debug.Log("GDPR 동의 프로세스 스타트");
            #if DEBUGBUILD // 테스트 빌드에서만 테스트모드 활성화
                            Debug.Log("테스트 버전 - 초기화 후 체크 개시!");
                            ConsentInformation.Reset(); // 테스트를 위해 기존 GDPR 정보 초기화
                            var debugSettings = new ConsentDebugSettings
                            {
                                DebugGeography = DebugGeography.EEA, // 일시적으로 유럽인척
            #if UNITY_IOS
                                    TestDeviceHashedIds = new List<string> {"여기에 아이폰 해시 ID"}
            #else
                                    TestDeviceHashedIds = new List<string> {"여기에 안드로이드 해시 ID"}
            #endif
                            };
                            ConsentRequestParameters request = new ConsentRequestParameters {ConsentDebugSettings = debugSettings};
                            ConsentInformation.Update(request, OnConsentInfoUpdated);
            #else // 릴리즈 & 디스트리뷰트에선 GDPR 테스트 모드 끄기
                            Debug.Log("릴리즈 버전 - 체크 개시!");
                            ConsentRequestParameters request = new ConsentRequestParameters();
                            ConsentInformation.Update(request, OnConsentInfoUpdated);
            #endif
            #else
                    // 구글 애즈 초기화
                    Debug.Log("애드몹 초기화 시도!");
    
            #endif

    }

    void OnConsentInfoUpdated(FormError consentError)
    {
        if (consentError != null)
        {
            // Handle the error.
            UnityEngine.Debug.LogError(consentError);
            return;
        }

        // If the error is null, the consent information state was updated.
        // You are now ready to check if a form is available.
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError formError) =>
        {
            if (formError != null)
            {
                // Consent gathering failed.
                UnityEngine.Debug.LogError(consentError);
                return;
            }

            if (GDPR.IsGDPR())
            {
                isNoAd = !GDPR.CanAdShow();

                if (GDPR.IsPartnerConsent("3234"))
                {
                    GoogleMobileAds.Mediation.UnityAds.Api.UnityAds.SetConsentMetaData("gdpr.consent", true);

                }

                if (GDPR.IsPartnerConsent("1423"))
                {
                    GoogleMobileAds.Mediation.LiftoffMonetize.Api.LiftoffMonetize.SetGDPRStatus(true, "v1.0.0");
                }
            }

            // Consent has been gathered.

        /*    if (ConsentInformation.CanRequestAds())
            {
                MobileAds.Initialize((InitializationStatus initstatus) =>
                {
                    // TODO: Request an ad.
                });
            }*/
        });
    }
}
