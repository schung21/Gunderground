using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneController : MonoBehaviour
{
    public static RuneController instance;
    public int runesQty, hp;
    public float crit, critDmg, dmg, spd, uniqueDrop, legendDrop;
    public List<int> stats;


    void Start()
    {
        instance = this;

        if (CharTracker.instance.runeStats.Count != 0)
        {
            stats = CharTracker.instance.runeStats;
            ApplyStats();
        }

    }

    private void Update()
    {
        runesQty = LevelManager.instance.currentRunes;
    
    }

    public void ApplyStats()
    {
        if (stats[0] == 10)
        {
            hp = 1;
        }
        else
        {
            hp = 0;
        }

        crit = 0.005f * stats[1];
        critDmg = 0.02f * stats[2];
        dmg = 0.01f * stats[3];
        spd = 0.1f * stats[4];
        uniqueDrop = 0.005f * stats[5];
        legendDrop = 0.003f * stats[6];

        if (Convert.ToString(critDmg).Contains("0.09999"))
        {
            critDmg = 0.1f;
        }
        if (Convert.ToString(dmg).Contains("0.09999"))
        {
            dmg = 0.1f;
        }
        

    }

    public void ApplyPlayer()
    {
        if(hp == 1)
        {
            PlayerController.instance.playerHealthBonus = 1;
            Invoke("ApplyHealth", 0.01f); 
        }

        PlayerController.instance.gunCrit1 += crit;
        PlayerController.instance.gunCrit2 += crit;

        PlayerController.instance.critDmg1 += critDmg;
        PlayerController.instance.critDmg2 += critDmg;

        PlayerController.instance.moveSpeed += spd;

        if (PlayerController.instance.isBunny)
        {
            ExpManager.instance.ApplyDamage(0);
        }
        if (PlayerController.instance.isMole)
        {
            ExpManager.instance.ApplyDamage(1);
        }
        if (PlayerController.instance.isRaccoon)
        {
            ExpManager.instance.ApplyDamage(2);
        }

    }

    public void ResetPlayer()
    {
      
        PlayerController.instance.gunCrit1 -= crit;
        PlayerController.instance.gunCrit2 -= crit;

        PlayerController.instance.critDmg1 -= critDmg;
        PlayerController.instance.critDmg2 -= critDmg;

        PlayerController.instance.playerDamage -= dmg;

        PlayerController.instance.moveSpeed -= spd;

    }


    public void ApplyHealth()
    {
        PlayerHealth.instance.ChangeHealth(PlayerController.instance.playerHealthBonus);
    }

    public void ApplyStatsUI()
    {
     
        UIController.instance.runeStats[0].text = UIController.instance.runeStatInfo[0].text + " +" + Convert.ToString(hp);
        UIController.instance.runeStats[1].text = UIController.instance.runeStatInfo[1].text + " +" + Convert.ToString(crit * 100) + "%";
        UIController.instance.runeStats[2].text = UIController.instance.runeStatInfo[2].text + " +" + Convert.ToString(critDmg * 100) + "%";
        UIController.instance.runeStats[3].text = UIController.instance.runeStatInfo[3].text + " +" + Convert.ToString(dmg * 100) + "%";
        UIController.instance.runeStats[4].text = UIController.instance.runeStatInfo[4].text + " +" + Convert.ToString(spd);
        UIController.instance.runeStats[5].text = UIController.instance.runeStatInfo[5].text + " +" + Convert.ToString(uniqueDrop * 100) + "%";
        UIController.instance.runeStats[6].text = UIController.instance.runeStatInfo[6].text + " +" + Convert.ToString(legendDrop * 100) + "%"; 
      
    }

}
