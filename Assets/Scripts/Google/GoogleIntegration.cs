using System.Collections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Unity.Services.Core;
using Unity.Services.Authentication;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using GooglePlayGames.BasicApi.SavedGame;

public class GoogleIntegration : MonoBehaviour
{

    public string GooglePlayToken;
    public string GooglePlayError;

    private void Awake()
    {
        PlayGamesPlatform.Activate();
    }

    async void Start()
    {
        await UnityServices.InitializeAsync();
        await Authenticate();
        await SignInWithGoogleAsync(GooglePlayToken);
    }
    public Task Authenticate()
    {

        var tcs = new TaskCompletionSource<object>();
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if(success == SignInStatus.Success)
            {
                Debug.Log("Login with Google was successful.");
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code => {
                    Debug.Log($"Auth code is {code}");
                    GooglePlayToken = code;
                    tcs.SetResult(null);
                });

            }
            else
            {
                GooglePlayError = "Failed to retrieve GPG auth code";
                Debug.LogError("Login Failed");
                tcs.SetException(new Exception("Failed"));

            }
        });

        return tcs.Task;
     
    }

    async Task SignInWithGoogleAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGoogleAsync(authCode);
          
        }
        catch (AuthenticationException x)
        {
            Debug.LogException(x);
            throw;
        }
        catch (RequestFailedException x)
        {
            Debug.LogException(x);
            throw;
        }
    }

    #region SavedGames

    private bool isSaving;
    public void OpenSave(bool saving)
    {
        if(Social.localUser.authenticated)
        {
            isSaving = saving;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("MyFile", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SaveGameOpen);

        }
    }

    private void SaveGameOpen(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            if(isSaving)
            {
                //convert datatypes to a byte array

                byte[] myData = System.Text.ASCIIEncoding.ASCII.GetBytes(GetSaveString());

                //update our metadata

                SavedGameMetadataUpdate updateForMetadata = new SavedGameMetadataUpdate.Builder().WithUpdatedDescription("Game upated at: " + DateTime.Now.ToString()).Build();

                //commit save

                ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metadata, updateForMetadata, myData, SaveCallBack);
            }
            else
            {
                ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metadata, LoadCallBack);
            }
        }
    }

    private void LoadCallBack(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string loadedData = System.Text.ASCIIEncoding.ASCII.GetString(data);

            LoadSavedString(loadedData);
        }
    }

    private void LoadSavedString(string cloudData)
    {
        string[] cloudStringArr = cloudData.Split('|');
    }

    public string GetSaveString()
    {
        string dataToSave = "";

        dataToSave += CharTracker.instance.bunnyTrait;

        return dataToSave;
    }

    private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if(status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Successfully saved to the cloud");
        }
        else
        {
            Debug.Log("Failed to save to cloud");
        }
    }

    #endregion

}
