using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TraitManager : MonoBehaviour
{    
    public PlayerController Player;
    public static TraitManager instance;
    public bool isBunny, isMole, isRaccoon, isCaptain, isFox, isRanger, isCoyote;
    public List<int> Bunny, Mole, Raccoon, Captain, Fox, Ranger, Coyote;

    // Start is called before the first frame update

    private void Awake()
    {
        Raccoon = CharTracker.instance.raccTrait;
        Bunny = CharTracker.instance.bunnyTrait;
        Mole = CharTracker.instance.moleTrait;
        Raccoon = CharTracker.instance.raccTrait;

    }
    void Start()
    {
  
        instance = this;

        if (isBunny)
        {
            ExpManager.instance.ApplyExp(0, 0);
            ExpManager.instance.ApplyDamage(0);
            //PlayerController.instance.playerDamage += ExpManager.instance.Levels[0];
            if (Bunny[0] == 1) { Player.moveSpeed += 0.5f; }
            if (Bunny[1] == 1) { Player.gunCrit1 += 0.05f; }
            if (Bunny[2] == 1) { Player.critDmg1 += 0.25f; }
            if (Bunny[3] == 1) { }//move dmg
            if (Bunny[4] == 1) { }//ult crit buff
            if (Bunny[5] == 1) { Player.dashLength = 1f; Player.dashInvinc += 0.7f; Player.dashCooldown += 0.4f; }
        }
        if (isMole)
        {
            ExpManager.instance.ApplyExp(0, 1);
            ExpManager.instance.ApplyDamage(1);
            //PlayerController.instance.playerDamage += ExpManager.instance.Levels[1];
            if (Mole[0] == 1) { Player.moveSpeed += 0.5f; }
            if (Mole[1] == 1) { }//stun duration
            if (Mole[2] == 1) { }//ultdmg+
            if (Mole[3] == 1) { }//skill upgrade
            if (Mole[4] == 1) { Player.ultLength += 2; }
            if (Mole[5] == 1) { }//ultradius
        }
        if (isRaccoon)
        {
            ExpManager.instance.ApplyExp(0, 2);
            ExpManager.instance.ApplyDamage(2);
            //PlayerController.instance.playerDamage += ExpManager.instance.Levels[1];
            if (Raccoon[0] == 1) { LevelManager.instance.currentCoins += 50; }
            if (Raccoon[1] == 1) { Player.longerDecoy = true; }
            if (Raccoon[2] == 1) { Player.ultCooldown -= 3; }
            if (Raccoon[3] == 1) { }//skill upgrade
            if (Raccoon[4] == 1) { Player.playerHealthBonus += 1; }
            if (Raccoon[5] == 1) { }//summon+1
        }

        if (SceneManager.GetActiveScene().name == "0")
        {
            PlayerHealth.instance.ChangeHealth(Player.playerHealthBonus);

        }
    }

    public void ApplyStat(int skillNumber)
    {
        if (skillNumber == 1)
        {
            if (isBunny) { Player.moveSpeed += 0.5f; }
            if (isMole) { Player.moveSpeed += 0.5f; }
            if (isRaccoon) { LevelManager.instance.currentCoins += 50; }

        }
        if (skillNumber == 2)
        {
            if (isBunny) { Player.gunCrit1 += 0.05f; }
            if (isMole) { }
            if (isRaccoon) { Player.longerDecoy = true; }
        }
        if (skillNumber == 3)
        {
            if (isBunny) { Player.critDmg1 += 0.15f; }
            if (isMole) { }
            if (isRaccoon) { Player.ultCooldown -= 3; }

        }
        if (skillNumber == 4)
        {
            if (isBunny) { }
            if (isMole) { Player.ultLength += 2; }
            if(isRaccoon) { }

        }
        if (skillNumber == 5)
        {
            if (isBunny) { }
            if (isMole) { }
            if(isRaccoon) { Player.playerHealthBonus += 1; PlayerHealth.instance.HealthUp(); }
        }
        if(skillNumber == 6)
        {
            if (isBunny) { Player.dashLength = 1f; Player.dashInvinc += 0.4f; Player.dashCooldown += 0.4f; }
            if (isMole) { }
            if (isRaccoon) { }
        }
    }
}
