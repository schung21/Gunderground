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
using UnityEngine.UI;
using GooglePlayGames.BasicApi.SavedGame;


public class GoogleSaveManager : MonoBehaviour
{

    #region SavedGames

    public static GoogleSaveManager instance;
    public bool isMain;
    private bool isSaving;
    public Text success1, success2, fail1, fail2, fail3;
    public GameObject cloudNotice, notLoggedIn, cover;

    private void Awake()
    {
        instance = this;

    }
   /* private void Start()
    {
        GetSaveString();
    }*/
    public void OpenSave(bool saving)
    {

        if (isMain)
        {
            success1.gameObject.SetActive(false);
            fail1.gameObject.SetActive(false);
            fail2.gameObject.SetActive(false);
            fail3.gameObject.SetActive(false);
        }

        if (Social.localUser.authenticated)
        {
            if (cover != null)
            {
                cover.gameObject.SetActive(true);
            }
            isSaving = saving;
            ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution("MyFile", DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SaveGameOpen);
        }
        else
        {
            if (cloudNotice != null)
            {
                cloudNotice.SetActive(false);

                success1.gameObject.SetActive(false);
               
            }
            if (notLoggedIn != null)
            {
                notLoggedIn.SetActive(true);
            }

        }

    }

    private void SaveGameOpen(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (isSaving)
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
        else
        {
            if (isMain)
            {
                if (isSaving)
                {
                    cover.gameObject.SetActive(false);
                    fail3.gameObject.SetActive(true);
                }
                else
                {
                    cover.gameObject.SetActive(false);
                    cloudNotice.SetActive(false);
                    fail3.gameObject.SetActive(true);
                }
            }
        }
    }

    private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata metadata)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Successfully saved to the cloud");
            if (isMain)
            {
                success1.gameObject.SetActive(true);
                cover.gameObject.SetActive(false);
            }
            Invoke("CloseText", 2f);
        }
        else
        {
            Debug.Log("Failed to save to cloud");
            if (isMain)
            {
                fail1.gameObject.SetActive(true);
                cover.gameObject.SetActive(false);
            }
        }
    }

    private void LoadCallBack(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            if (isMain)
            {
                cover.gameObject.SetActive(false);
                success2.gameObject.SetActive(true);
            }

            string loadedData = System.Text.ASCIIEncoding.ASCII.GetString(data);

            LoadSavedString(loadedData);

        }
        else
        {
            if (isMain)
            {
                cover.gameObject.SetActive(false);
                fail2.gameObject.SetActive(true);
            }
        }
    }

    private void LoadSavedString(string cloudData)
    {
        string[] cloudStringArr = cloudData.Split('|');

        LevelManager.instance.currentGems = int.Parse(cloudStringArr[0]);
        LevelManager.instance.currentParts = int.Parse(cloudStringArr[1]);
        LevelManager.instance.tutorialCode = int.Parse(cloudStringArr[2]);
        LevelManager.instance.storyCode = int.Parse(cloudStringArr[3]);

        //Characters
        ContentManager.instance.unlockedChars[0] = int.Parse(cloudStringArr[4]);
        ContentManager.instance.unlockedChars[1] = int.Parse(cloudStringArr[5]);
        ContentManager.instance.unlockedChars[2] = int.Parse(cloudStringArr[6]);
        ContentManager.instance.unlockedChars[3] = int.Parse(cloudStringArr[7]);
        ContentManager.instance.unlockedChars[4] = int.Parse(cloudStringArr[8]);
        ContentManager.instance.unlockedChars[5] = int.Parse(cloudStringArr[9]);

        //Guns
        for (int i = 0; i < ContentManager.instance.unlockedGuns.Count; i++)
        {
            int a = 10 + i;
            ContentManager.instance.unlockedGuns[i] = int.Parse(cloudStringArr[a]);

        }

        //EXP
        for (int i = 0; i < ExpManager.instance.charExp.Count; i++)
        {
            int a = 72 + i;
            ExpManager.instance.charExp[i] = int.Parse(cloudStringArr[a]);
        }

        //Levels
        for (int i = 0; i < ExpManager.instance.Levels.Count; i++)
        {
            int a = 78 + i;
            ExpManager.instance.Levels[i] = int.Parse(cloudStringArr[a]);
        }

        //MaxExp
        for (int i = 0; i < ExpManager.instance.maxExp.Count; i++)
        {
            int a = 84 + i;
            ExpManager.instance.maxExp[i] = int.Parse(cloudStringArr[a]);
        }

        //BunnyTrait
        for (int i = 0; i < TraitManager.instance.Bunny.Count; i++)
        {
            int a = 90 + i;
            TraitManager.instance.Bunny[i] = int.Parse(cloudStringArr[a]);
        }

        //MoleTrait
        for (int i = 0; i < TraitManager.instance.Mole.Count; i++)
        {
            int a = 96 + i;
            TraitManager.instance.Mole[i] = int.Parse(cloudStringArr[a]);
        }

        //RaccTrait
        for (int i = 0; i < TraitManager.instance.Raccoon.Count; i++)
        {
            int a = 102 + i;
            TraitManager.instance.Raccoon[i] = int.Parse(cloudStringArr[a]);
        }

        //CptTrait
        for (int i = 0; i < TraitManager.instance.Captain.Count; i++)
        {
            int a = 108 + i;
            TraitManager.instance.Captain[i] = int.Parse(cloudStringArr[a]);
        }

        //BunnySkin
        for (int i = 0; i < SkinManager.instance.Bunny.Count; i++)
        {
            int a = 114 + i;
            SkinManager.instance.Bunny[i] = int.Parse(cloudStringArr[a]);
        }

        //MoleSkin
        for (int i = 0; i < SkinManager.instance.Mole.Count; i++)
        {
            int a = 119 + i;
            SkinManager.instance.Mole[i] = int.Parse(cloudStringArr[a]);
        }

        //RaccSkin
        for (int i = 0; i < SkinManager.instance.Raccoon.Count; i++)
        {
            int a = 124 + i;
            SkinManager.instance.Raccoon[i] = int.Parse(cloudStringArr[a]);
        }

        //CptSkin
        for (int i = 0; i < SkinManager.instance.Captain.Count; i++)
        {
            int a = 129 + i;
            SkinManager.instance.Captain[i] = int.Parse(cloudStringArr[a]);
        }

        //SkinCode
        for (int i = 0; i < SkinManager.instance.skinCode.Count; i++)
        {
            int a = 134 + i;
            SkinManager.instance.skinCode[i] = int.Parse(cloudStringArr[a]);
        }

        Debug.Log("11-27-2024");
        //Data Array 11-27-2024************
        //Runes

        LevelManager.instance.currentRunes = int.Parse(cloudStringArr[138]);


        for (int i = 0; i < RuneController.instance.stats.Count; i++)
        {
            int a = 139 + i;
            RuneController.instance.stats[i] = int.Parse(cloudStringArr[a]);
        }


        CharTracker.instance.SavePlayer();
        UIController.instance.Quit();

    }

    public string GetSaveString()
    {
        UIController.instance.autosaveText.gameObject.SetActive(true);

        string dataToSave = "";

        dataToSave += CharTracker.instance.currentGems;
        dataToSave += "|";
        dataToSave += CharTracker.instance.currentParts;
        dataToSave += "|";
        dataToSave += CharTracker.instance.tutorialCode;
        dataToSave += "|";
        dataToSave += CharTracker.instance.storyCode;
        dataToSave += "|";

        for (int i = 0; i < CharTracker.instance.unlockedChars.Count; i++)
        {
            dataToSave += CharTracker.instance.unlockedChars[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.unlockedGuns.Count; i++)
        {
            dataToSave += CharTracker.instance.unlockedGuns[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.charExp.Count; i++)
        {
            dataToSave += CharTracker.instance.charExp[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.Levels.Count; i++)
        {
            dataToSave += CharTracker.instance.Levels[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.maxExp.Count; i++)
        {
            dataToSave += CharTracker.instance.maxExp[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.bunnyTrait.Count; i++)
        {
            dataToSave += CharTracker.instance.bunnyTrait[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.moleTrait.Count; i++)
        {
            dataToSave += CharTracker.instance.moleTrait[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.raccTrait.Count; i++)
        {
            dataToSave += CharTracker.instance.raccTrait[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.cptTrait.Count; i++)
        {
            dataToSave += CharTracker.instance.cptTrait[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.bunnySkin.Count; i++)
        {
            dataToSave += CharTracker.instance.bunnySkin[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.moleSkin.Count; i++)
        {
            dataToSave += CharTracker.instance.moleSkin[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.raccSkin.Count; i++)
        {
            dataToSave += CharTracker.instance.raccSkin[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.cptSkin.Count; i++)
        {
            dataToSave += CharTracker.instance.cptSkin[i];
            dataToSave += "|";
        }
        for (int i = 0; i < CharTracker.instance.skinCode.Count; i++)
        {
            dataToSave += CharTracker.instance.skinCode[i];
            dataToSave += "|";
        }

        //runes

        dataToSave += CharTracker.instance.currentRunes;
        dataToSave += "|";


        for (int i = 0; i < CharTracker.instance.runeStats.Count; i++)
        {
            dataToSave += CharTracker.instance.runeStats[i];
            dataToSave += "|";
        }

        int count = dataToSave.Split('|').Length - 1;
        Debug.Log(count);

        return dataToSave;
    }

    public void CloseText()
    {
        UIController.instance.autosaveText.gameObject.SetActive(false);
    }

    #endregion

}
