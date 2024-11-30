using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.UI;

public class ObjectAnimation : MonoBehaviour
{
    public Animator anim;
    public GameObject Message, mainUI, fadeScreen, leavingMsg, startButton;
    public string sceneToLoad;
    public Text text1;
    private double timeRecord;
    public Image player;
    public Image[] guns;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            PlayerController.instance.canMove = false;
            anim.SetBool("Triggered", true);
            LevelManager.instance.storyCode = 1;

            int a = 0;

            for (int i = 1; i < PlayerController.instance.availableGuns.Count; i++)
            {
                guns[a].sprite = PlayerController.instance.availableGuns[i].gunUI;
                a++;
            }


            /*   if (ContentManager.instance.unlockedChars[2] != 1)
               {
                   ContentManager.instance.unlockedChars[2] = 2;
                   Message.SetActive(true);
               }*/
            timeRecord = PlayerController.instance.playTime;
            var time = TimeSpan.FromSeconds(timeRecord);
            text1.text = string.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);

            CharTracker.instance.SavePlayer();

            long clearTime = (long)timeRecord;
            Debug.Log(clearTime * 1000);
            Social.ReportScore(clearTime * 1000, GPGSIds.leaderboard_clear_time_story_part_1, LeaderboardUpdate);

           

        }
    }

    public void ShakeCamera()
    {
        PlayerController.instance.mainCam.GetComponent<CameraController>().camShake();
    }

    public void FadeScreen() 
    {
        anim.SetTrigger("Fade");
    
    }
    public void NextScene()
    {
        LevelManager.instance.savedLevel = "";
        CharTracker.instance.SaveLevel();
        CharTracker.instance.ClearTemp();

        SceneManager.LoadScene(sceneToLoad);

        Destroy(PlayerController.instance.gameObject);

    }

    public void DisableUI()
    {
        mainUI.SetActive(false);
        //UIController.instance.HideUI();
        
    }

    public void EnableUI()
    {
       
        mainUI.SetActive(true);
    }


    public void LeaderboardUpdate(bool success)
    {
        if (success)
        { Debug.Log("Updated Leaderboard"); }
        else
        { Debug.Log("Unable to update leaderboard"); }

    }

    public void StopCharAnim()
    {
        if(SkinManager.instance != null)
        {
            if(SkinManager.instance.currentSkinCode != 0)
            {
                player.sprite = SkinManager.instance.ogSkinSprite[SkinManager.instance.currentSkinCode];
            }
            else
            {
                player.sprite = PlayerController.instance.ogSprite;
            }
        }
       
    }

    public void ActiveCursor()
    {
        if (PlayerController.instance.isGamepad)
        {
            startButton.SetActive(false);
            leavingMsg.SetActive(true);

            Invoke("NextScene", 10f);
        }
    }
}
