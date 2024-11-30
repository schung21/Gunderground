using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class TimerController : MonoBehaviour
{

    public static TimerController instance;
    public GameObject levelExit;
    public GameObject bonusLevelExit;
    public float timeValue = 180f;
    public bool timerGoing, buffTimer, ultTimer;
    public int comboEXP;
    [HideInInspector]
    public Vector3 newPosition1, newPosition2, oldPosition, oldPosition2;
    
    public float timeDiff;

    private float ogTime;
    //public string levelToLoad;

    //public Text timeCounter;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        timerGoing = true;
        newPosition1 = UIController.instance.dualGun.gameObject.transform.position + new Vector3(0f, -75f, 0f);
        newPosition2 = UIController.instance.buffTimer.gameObject.transform.position + new Vector3(0f, -75f, 0f);
        oldPosition = UIController.instance.dualGun.gameObject.transform.position;
        oldPosition2 = UIController.instance.buffTimer.gameObject.transform.position;

        ogTime = timeValue;
    }

    // Update is called once per frame
    void Update()
    {
     

        /*     Vector3 playerPos = PlayerController.instance.gameObject.transform.position;
             Vector3 playerDirection = PlayerController.instance.transform.forward;
             Quaternion playerRotation = PlayerController.instance.transform.rotation;
             float spawnDistance = 10;
             Vector3 spawnPos = playerPos + playerDirection * spawnDistance;*/

        if (SceneManager.GetActiveScene().name != "0" && !SceneManager.GetActiveScene().name.Contains("Raid"))
        {
            comboEXP = 0;
            /*if (timeValue > 0 && timerGoing == true)
            {
                timeValue -= Time.deltaTime;
            }
            else if (timeValue <= 0 && timerGoing == true)
            {

                Invoke("SpawnHorde", 3f);

            }
            else
            {
                timeValue = 0;
                timerGoing = false;
            }

            DisplayTime(timeValue);*/
        }
        else if(LevelManager.instance.isRaid)
        {
            UIController.instance.timer.gameObject.SetActive(true);

            if (timeValue > 0 && timerGoing == true)
            {
                timeValue -= Time.deltaTime;
            }
            else if (timeValue <= 1 && timerGoing == true)
            {
                OpenExit();
                if (PlayerController.instance.expBuff)
                {
                    comboEXP = Mathf.RoundToInt((LevelManager.instance.comboCount * LevelManager.instance.expBonus) * 1.3f);
                }
                else
                {
                    comboEXP = LevelManager.instance.comboCount * LevelManager.instance.expBonus;
                }
             
                UIController.instance.comboEXP.text = "EXP +" + comboEXP.ToString();
                UIController.instance.comboEXP.gameObject.SetActive(true);
                ExpManager.instance.CollectExp(comboEXP);
            }
         

            DisplayTime(timeValue);

        }
        else if (LevelManager.instance.isDefense)
        {
            UIController.instance.timer.gameObject.SetActive(true);

            if (timeValue > 0 && timerGoing == true)
            {
                timeValue -= Time.deltaTime;

                timeDiff = ogTime - timeValue;

            }
            else if (timeValue <= 1 && timerGoing == true)
            {
                OpenExit();
                if (PlayerController.instance.expBuff)
                {
                    if (SkinManager.instance.currentSkinCode != 0)
                    {
                        comboEXP = Mathf.RoundToInt((LevelManager.instance.comboCount * LevelManager.instance.expBonus) * 1.4f);
                    }
                    else
                    {
                        comboEXP = Mathf.RoundToInt((LevelManager.instance.comboCount * LevelManager.instance.expBonus) * 1.3f);
                    }
                }
                else
                {
                    if (SkinManager.instance.currentSkinCode != 0)
                    {
                        comboEXP = Mathf.RoundToInt((LevelManager.instance.comboCount * LevelManager.instance.expBonus) * 1.1f);
                    }
                    else
                    {
                        comboEXP = LevelManager.instance.comboCount * LevelManager.instance.expBonus;
                    }
                     
                }

                UIController.instance.comboEXP.text = "EXP +" + comboEXP.ToString();
                UIController.instance.comboEXP.gameObject.SetActive(true);
                ExpManager.instance.CollectExp(comboEXP);
            }


            DisplayTime(timeValue);

        }

        if(PlayerController.instance.isDual == true)
        {
            if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gun2 == null && 
                PlayerController.instance.availableGuns[PlayerController.instance.currentGun].subGun == null)
            {
                UIController.instance.noDual.gameObject.SetActive(true);
            }
            else
            {
                UIController.instance.noDual.gameObject.SetActive(false);
            }

            if (UIController.instance.ultTimer.isActiveAndEnabled)
            {
             
                UIController.instance.dualGun.gameObject.transform.position = newPosition1;
                UIController.instance.dualGun.gameObject.SetActive(true);
        
                buffTimer = true;
            }
            else
            {
                UIController.instance.dualGun.gameObject.SetActive(true);

                buffTimer = true;
            }

            if (PlayerController.instance.buffDuration > 0 && buffTimer == true)
            {
                PlayerController.instance.buffDuration -= Time.deltaTime;
            }
            else if (PlayerController.instance.buffDuration <= 0)
            {
                buffTimer = false;
                PlayerController.instance.isDual = false;
                UIController.instance.dualGun.gameObject.SetActive(false);
                Gun.instance.ResetHealth();
                Invoke("resetBuff", 0.5f);
            }

            BuffTime(PlayerController.instance.buffDuration);
        }

        if(PlayerController.instance.isUltActive == true)
        {
               
            UIController.instance.Ult.gameObject.SetActive(true);


            UltTime(PlayerController.instance.ultCounter);
        }
    }


    void DisplayTime(float timeToDisplay)
    {
        if(timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        UIController.instance.timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void BuffTime(float timeToDisplay)
    {
        if (UIController.instance.ultTimer.isActiveAndEnabled)
        {
            UIController.instance.buffTimer.gameObject.transform.position = newPosition2;
            UIController.instance.buffTimer.gameObject.SetActive(true);
        }
        else
        {
            UIController.instance.buffTimer.gameObject.SetActive(true);
        }

        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
            UIController.instance.buffTimer.gameObject.SetActive(false);
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        UIController.instance.buffTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void UltTime(float timeToDisplay)
    {
       
        UIController.instance.ultTimer.gameObject.SetActive(true);
        

        if (timeToDisplay <= 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        UIController.instance.ultTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
      
    }

 
    public void resetBuff()
    {
        PlayerController.instance.buffDuration = 30f;

        //if has artifact, 60f
    }

    public void OpenExit()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            //enemy.gameObject.SetActive(false);
            enemy.gameObject.GetComponent<EnemyController>().DamageEnemy(10000);
        }

        if (levelExit.GetComponent<LevelExit>().isCenter)
        {
            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {

                PlayerController.instance.transform.position += new Vector3(4f, 0f, 0f);


            }
        }
        if (bonusLevelExit.GetComponent<LevelExit>().isCenter)
        {
            if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {

                PlayerController.instance.transform.position += new Vector3(4f, 0f, 0f);


            }
        }
       /* else if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
        {
            levelExit.transform.position += new Vector3(4f, 0f, 0f);
            bonusLevelExit.transform.position += new Vector3(4f, 0f, 0f);
        }*/

        UIController.instance.timer.gameObject.SetActive(false);
        levelExit.SetActive(true);

        int randomNumb = Random.Range(0, 100);

        if (randomNumb <= 40)
        {
            bonusLevelExit.SetActive(true);
        }


        timerGoing = false;

    }
    public void DestroyAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            //enemy.gameObject.SetActive(false);
            enemy.gameObject.GetComponent<EnemyController>().DamageEnemy(10000);
        }

        timerGoing = false;
    }

    /*void SpawnHorde()
    {

        PlayerController.instance.gameObject.SetActive(false);

        UIController.instance.deathScreen.SetActive(true);
    }*/


}
