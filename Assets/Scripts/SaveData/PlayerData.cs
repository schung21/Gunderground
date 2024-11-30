using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    //Player Stats
    //public int health, playerDamage, gunsHeld, charCode;
    //public float fireRate, playerCritRate, moveSpeed;
    //public string gun1, gun2, gun3;
    //Level
    public List<int> charExp;
    public List<int> Levels;
    public List<int> maxExp;
    public List<int> runeStats;
    //Currency
    public int gems, keys, parts, buys, storyDone, tutorialDone, currentCoins, runes;
    public List<int> gunCodes;
    //Characters Unlocked
    public List<int> charCodes;
    public List<int> bunnyTrait, moleTrait, raccTrait, cptTrait, foxTrait, rangerTrait, coyoteTrait;
    public List<int> bunnySkin, moleSkin, raccSkin, cptSkin;
    public List<int> skinCodes;

    public string vendingTime1, vendingTime2, vendingTime3, storeItemTime1, dailyGiftTime, savedLevel;
    public double timeRecord;

    //Temp
    public List<string> guns;
    public List<int> artifacts;
    public int savedCoins;
    public int revives;
    public int character;
    public int maxHealth;
    public int playTime;

    public PlayerData (CharTracker progress)
    {
        //level = SceneManager.GetActiveScene().name;
        vendingTime1 = progress.vendingTime1;
        vendingTime2 = progress.vendingTime2;
        vendingTime3 = progress.vendingTime3;
        storeItemTime1 = progress.storeItemTime1;
        dailyGiftTime = progress.dailyGiftTime;

        //Player Levels,EXP
        charExp = progress.charExp;
        Levels = progress.Levels;
        maxExp = progress.maxExp;
        runeStats = progress.runeStats;

        //Currency
        gems = progress.currentGems;
        parts = progress.currentParts;
        buys = progress.currentBuys;
        runes = progress.currentRunes;

        //Unlocked Guns
        gunCodes = progress.unlockedGuns;

        //Tutorial
        tutorialDone = progress.tutorialCode;
        storyDone = progress.storyCode;

        //Story
        storyDone = progress.storyCode;

        //Unlocked Chars
        charCodes = progress.unlockedChars;
        bunnyTrait = progress.bunnyTrait;
        moleTrait = progress.moleTrait;
        raccTrait = progress.raccTrait;
        cptTrait = progress.cptTrait;

        bunnySkin = progress.bunnySkin;
        moleSkin = progress.moleSkin;
        raccSkin = progress.raccSkin;
        cptSkin = progress.cptSkin;

        skinCodes = progress.skinCode;

        //Saved level
        savedLevel = progress.savedLevel;
        guns = progress.guns;
        artifacts = progress.artifacts;
        savedCoins = progress.savedCoins;
        revives = progress.revives;
        character = progress.character;
        maxHealth = progress.maxHealth;
        playTime = progress.playTime;

        /*      //Player Stats
        health = playerhp.currentHealth;
        playerDamage = player.playerDamage;
        fireRate = player.fireRate;
        playerCritRate = player.playerCritRate;
        moveSpeed = player.moveSpeed;
        gunsHeld = player.gunsHeld;

        if (player.availableGuns[1] != null)
        {
            gun1 = player.availableGuns[1].name;
        }
        if (player.availableGuns[2] != null)
        {
            gun2 = player.availableGuns[2].name;
        }

        if(player.isBunny == true)
        {
            charCode = 1;
        }
        if(player.isMole == true)
        {
            charCode = 2;
        }*/

    }

}
