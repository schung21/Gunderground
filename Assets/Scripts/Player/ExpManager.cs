using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ExpManager : MonoBehaviour
{

    public static ExpManager instance;
    public List<int> charExp;
    public List<int> Levels;
    public List<int> maxExp;
    private float Seconds, playerDmg;
    public int maxLvl;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        charExp = CharTracker.instance.charExp;
        Levels = CharTracker.instance.Levels;
        maxExp = CharTracker.instance.maxExp;

        if (PlayerController.instance.isBunny)
        {
            ApplyExp(0, 0);
            if (LevelManager.instance.isCamp)
            {
                ApplyDamage(0);
            }
        }
        if (PlayerController.instance.isMole)
        {
            ApplyExp(0, 1);
            if (LevelManager.instance.isCamp)
            {
                ApplyDamage(1);
            }

        }
        if (PlayerController.instance.isRaccoon)
        {
            ApplyExp(0, 2);
            if (LevelManager.instance.isCamp)
            {
                ApplyDamage(2);
            }

        }

    }
    void Update()
    {
        if(Seconds > 0)
        {
            Seconds -= Time.deltaTime;
        }

        if (PlayerController.instance.isBunny)
        {
            if (Levels[0] < maxLvl)
            {
                if (charExp[0] >= maxExp[0])
                {
                    int expLeft = charExp[0] - maxExp[0];
                    LevelUp(0, expLeft);
                }
            }
            else
            {
                charExp[0] = 0;
            }
        }
        if (PlayerController.instance.isMole)
        {

            if (Levels[1] < maxLvl)
            {
                if (charExp[1] >= maxExp[1])
                {
                    int expLeft = charExp[1] - maxExp[1];
                    LevelUp(1, expLeft);
                }
            }
            else
            {
                charExp[1] = 0;
            }

        }
        if (PlayerController.instance.isRaccoon)
        {
            if (Levels[2] < maxLvl)
            {
                if (charExp[2] >= maxExp[2])
                {
                    int expLeft = charExp[2] - maxExp[2];
                    LevelUp(2, expLeft);
                }
            }
            else
            {
                charExp[2] = 0;
            }

        
        }
        if (PlayerController.instance.isCpt)
        {
            if (Levels[3] < maxLvl)
            {
                if (charExp[3] >= maxExp[3])
                {
                    int expLeft = charExp[3] - maxExp[3];
                    LevelUp(3, expLeft);
                }
            }
            else
            {
                charExp[3] = 0;
            }

      
        }
        if (PlayerController.instance.isFox)
        {
      
        }

    }

    public void CollectExp(int expPoints)
    {
        if (PlayerController.instance.isBunny)
        {
            Seconds = 3f;

            ApplyExp(expPoints, 0);

          /*  if (charExp[0] >= maxExp[0])
            {
                int expLeft = charExp[0] - maxExp[0]; 
                LevelUp(0, expLeft);
            }
       */
            Invoke("HideUI", 3f);
        }
        if (PlayerController.instance.isMole)
        {
            Seconds = 3f;

            ApplyExp(expPoints, 1);

       /*     if (charExp[1] >= maxExp[1])
            {
                int expLeft = charExp[1] - maxExp[1];
                LevelUp(1, expLeft);
            }*/

            Invoke("HideUI", 3f);
        }
        if (PlayerController.instance.isRaccoon)
        {
            Seconds = 3f;

            ApplyExp(expPoints, 2);

           /* if (charExp[2] >= maxExp[2])
            {
                int expLeft = charExp[2] - maxExp[2];
                LevelUp(2, expLeft);
            }*/

            Invoke("HideUI", 3f);
        }
        if (PlayerController.instance.isCpt)
        {
            Seconds = 3f;

            ApplyExp(expPoints, 3);
/*
            if (charExp[3] >= maxExp[3])
            {
                int expLeft = charExp[3] - maxExp[3];
                LevelUp(3, expLeft);
            }*/

            Invoke("HideUI", 3f);
        }
        if (PlayerController.instance.isFox)
        {
            charExp[4] += expPoints;
        }
    }


    public void ApplyExp(int expPoints, int charNum)
    {
       
        if (Levels[charNum] < maxLvl)
        {
            if (PlayerController.instance.expBuff)
            {
                if(SkinManager.instance.currentSkinCode != 0)
                {
                    charExp[charNum] += Mathf.RoundToInt(expPoints * 1.4f);
                }
                else
                {
                    charExp[charNum] += Mathf.RoundToInt(expPoints * 1.3f);
                }
            }
            else
            {
                if (SkinManager.instance.currentSkinCode != 0)
                {
                    charExp[charNum] += Mathf.RoundToInt(expPoints * 1.1f);
                }
                else
                {
                    charExp[charNum] += expPoints;
                }
                  
            }

            UIController.instance.expSlider.maxValue = maxExp[charNum];
            /* UIController.instance.expSlider.gameObject.SetActive(true);
             UIController.instance.charLvlText.gameObject.SetActive(true);*/
            UIController.instance.charLvlText.text = "Lvl " + Levels[charNum];
            UIController.instance.expSlider.value = charExp[charNum];

           
        }
        else
        {
            charExp[charNum] = 0; 
            UIController.instance.expSlider.value = charExp[charNum];
            UIController.instance.charLvlText.text = "Lvl " + Levels[charNum];
        }

    }

    public void ApplyDamage(int charNum)
    {
        float bonus = Levels[charNum] * 0.01f;
        bonus += RuneController.instance.dmg;
        PlayerController.instance.playerDamage = bonus;
    }

    public void LevelUp(int charNum, int expLeft)
    {
        //PlayerController.instance.playerDamage += 1;

        charExp[charNum] = expLeft;
        Levels[charNum]++;
        /*    maxExp[charNum] += Mathf.RoundToInt(maxExp[charNum] / 5f);*/
        maxExp[charNum] += 1000;

        UIController.instance.anim.SetTrigger("Lvlup");
        UIController.instance.expSlider.value = charExp[charNum];
        UIController.instance.expSlider.maxValue = maxExp[charNum];
        UIController.instance.charLvlText.text = "Lvl " + Levels[charNum];

        Instantiate(PlayerController.instance.lvlUpText, PlayerController.instance.transform.position, Quaternion.Euler(0f,0f,0f));

        PlayerController.instance.playerDamage += 0.01f;

        CharTracker.instance.SavePlayer();
    }

    public void HideUI()
    {
        if (Seconds <= 0)
        {
           /* UIController.instance.expSlider.gameObject.SetActive(false);
            UIController.instance.charLvlText.gameObject.SetActive(false);*/
        }
    }
}
