using System.Threading.Tasks;
using Unity.Services.RemoteConfig;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class RemoteConfig : MonoBehaviour
{
    public static RemoteConfig instance { get; private set; }
    public struct userAttributes { }
    public struct appAttributes { }

    public string newVersion;
    async Task InitializeRemoteConfigAsync()
    {
        // initialize handlers for unity game services
        await UnityServices.InitializeAsync();

        // remote config requires authentication for managing environment information
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    async Task Start()
    {
        
        // initialize Unity's authentication and core services, however check for internet connection
        // in order to fail gracefully without throwing exception if connection does not exist
        if (Utilities.CheckForInternetConnection())
        {
            await InitializeRemoteConfigAsync();
        }

        RemoteConfigService.Instance.FetchCompleted += ApplyRemoteSettings;
        RemoteConfigService.Instance.FetchConfigs(new userAttributes(), new appAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        Debug.Log("RemoteConfigService.Instance.appConfig fetched: " + RemoteConfigService.Instance.appConfig.config.ToString());
        newVersion = RemoteConfigService.Instance.appConfig.GetString("Version Number", "");

        if(MainMenu.instance.currentVer != newVersion)
        {
            Debug.Log("Update Available");
            MainMenu.instance.updateNotice.SetActive(true);
            MainMenu.instance.langButton.SetActive(false);
        }
        else
        {
            MainMenu.instance.LoadVersion();
        }
        
    }
   
}
