using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using SmokeTest;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;

public class CharTracker : MonoBehaviour
{

    public static CharTracker instance;
    public List<int> unlockedGuns;
    public List<int> unlockedChars;
    [Header("Levels/EXP")]
    public List<int> charExp;
    public List<int> Levels;
    public List<int> maxExp;
    [Header("Characters/Skills")]
    public List<int> bunnyTrait;
    public List<int> moleTrait;
    public List<int> raccTrait;
    public List<int> cptTrait;
    public List<int> foxTrait;
    public List<int> coyoteTrait;
    public List<int> skinCode;
    public List<int> bunnySkin, moleSkin, raccSkin, cptSkin;
    public List<int> runeStats;

    public int currentHealth, maxHealth, currentGems, currentKeys, currentParts, currentBuys, tutorialCode, storyCode, currentCoins, currentRunes;
    public int buyAmt;
    public double timeRecord;

    public string vendingTime1, vendingTime2, vendingTime3, storeItemTime1, dailyGiftTime, savedLevel;

    [Header("Store Items")]
    public List<GameObject> storeItems;

    [Header("Temp Data")]
    public List<string> guns;
    public List<int> artifacts;
    public int savedCoins;
    public int revives;
    public int character;
    public int maxhealth;
    public int playTime;


    private void Awake()
    {
        instance = this;
        string path = Application.persistentDataPath + "/player.data";

        if (File.Exists(path))
        {
           LoadPlayer();
        }

    }


    public void SavePlayer()
    {

        currentGems = LevelManager.instance.currentGems;
        currentKeys = LevelManager.instance.currentKeys;
        currentParts = LevelManager.instance.currentParts;
        currentBuys = LevelManager.instance.currentBuys;
        currentRunes = LevelManager.instance.currentRunes;
        tutorialCode = LevelManager.instance.tutorialCode;
        storyCode = LevelManager.instance.storyCode;
        unlockedChars = ContentManager.instance.unlockedChars;
        charExp = ExpManager.instance.charExp;
        Levels = ExpManager.instance.Levels;
        maxExp = ExpManager.instance.maxExp;

        bunnyTrait = TraitManager.instance.Bunny;
        moleTrait = TraitManager.instance.Mole;
        raccTrait = TraitManager.instance.Raccoon;
        cptTrait = TraitManager.instance.Captain;

        bunnySkin = SkinManager.instance.Bunny;
        moleSkin = SkinManager.instance.Mole;
        raccSkin = SkinManager.instance.Raccoon;
        cptSkin = SkinManager.instance.Captain;

        skinCode = SkinManager.instance.skinCode;

        if (LevelManager.instance.isCamp)
        {
            vendingTime1 = VendingMachine.instance.lastVendTime;
            vendingTime2 = VendingMachine2.instance.lastVendTime;
            vendingTime3 = VendingMachine3.instance.lastVendTime;
            storeItemTime1 = storeItems[0].GetComponent<ShopItemTimer>().lastBuyTime;
            dailyGiftTime = DailyGemManager.instance.lastGiftTime;

            runeStats = RuneController.instance.stats;

        }

        /* if (LevelManager.instance.isLastLvl)
         {
             if (timeRecord == 0 || (timeRecord > PlayerController.instance.playTime))
             {
                 timeRecord = PlayerController.instance.playTime;
                *//* long clearTime = (long)timeRecord;
                 Debug.Log(clearTime * 1000);
                 Social.ReportScore(clearTime * 1000, GPGSIds.leaderboard_clear_time_story_part_1, LeaderboardUpdate);*//*

             }
         }*/

        SaveSystem.SaveProgress(this);


        /*  if (SceneManager.GetActiveScene().name != "Tutorial")
          {
              GoogleSaveManager.instance.OpenSave(true);
          }*/

    }


    public void LoadPlayer()
    {
        SaveSystem.LoadPlayer();
        PlayerData data = SaveSystem.LoadPlayer();
     
        currentGems = data.gems;
        currentKeys = data.keys;
        currentParts = data.parts;
        currentBuys = data.buys;
        currentRunes = data.runes;
        unlockedGuns = data.gunCodes;
        tutorialCode = data.tutorialDone;
        unlockedChars = data.charCodes;
        charExp = data.charExp;
        Levels = data.Levels;
        maxExp = data.maxExp;
        storyCode = data.storyDone;

        bunnyTrait = data.bunnyTrait;
        moleTrait = data.moleTrait;
        raccTrait = data.raccTrait;
        cptTrait = data.cptTrait;

        bunnySkin = data.bunnySkin;
        moleSkin = data.moleSkin;
        raccSkin = data.raccSkin;
        cptSkin = data.cptSkin;

        skinCode = data.skinCodes;

        vendingTime1 = data.vendingTime1;
        vendingTime2 = data.vendingTime2;
        vendingTime3 = data.vendingTime3;
        storeItemTime1 = data.storeItemTime1;
        dailyGiftTime = data.dailyGiftTime;

        savedLevel = data.savedLevel;
        //timeRecord = data.timeRecord;
        savedCoins = data.savedCoins;
        revives = data.revives;
        maxhealth = data.maxHealth;
        character = data.character;
        if (data.artifacts != null)
        {
            artifacts = data.artifacts;
        }
        if (data.guns != null)
        {
            guns = data.guns;
        }
        playTime = data.playTime;

        //new list data
        if (data.runeStats != null)
        {
            runeStats = data.runeStats;
        }

    }

    public void SaveLevel()
    {
        savedLevel = LevelManager.instance.savedLevel;

        if (!LevelManager.instance.isCamp)
        {

            guns.Clear();

            savedCoins = LevelManager.instance.currentCoins;
            revives = PlayerController.instance.reviveCount;
            artifacts = ArtifactManager.instance.artifacts;
            maxhealth = PlayerHealth.instance.maxHealth;

            foreach (var a in PlayerController.instance.availableGuns)
            {

                guns.Add(a.weaponName);

            }

            if (PlayerController.instance.isBunny)
            {
                character = 1;
            }

            if (PlayerController.instance.isMole)
            {
                character = 2;
            }

            if (PlayerController.instance.isRaccoon)
            {
                character = 3;
            }

            playTime = (int)Math.Round(LevelManager.instance.playTime);
        }
    }
    public void ClearTemp()
    {
        savedCoins = 0;
        revives = 0;
        maxhealth = 0;
        character = 0;

        guns.Clear();

        for (int i = 0; i < artifacts.Count; i++)
        {
            artifacts[i] = 0;
        }

        playTime = 0;

        SaveSystem.SaveProgress(this);
    }
  /*  public void LoadTempData()
    {
        SaveSystem.LoadPlayer();
        PlayerData data = SaveSystem.LoadPlayer();

        currentCoins = data.currentCoins;
        revives = data.revives;
        artifacts = data.artifacts;
        maxhealth = data.maxhealth;
        character = data.character;
        guns = data.guns;


    }*/

    public void LeaderboardUpdate(bool success)
    {
        if (success)
        { Debug.Log("Updated Leaderboard"); }
        else
        { Debug.Log("Unable to update leaderboard"); }

    }
}
