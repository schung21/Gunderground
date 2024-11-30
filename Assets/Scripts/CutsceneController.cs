using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    public static CutsceneController instance;
    public Animator anim;
    public GameObject triggerObject1, joystickBg;
    public Camera mainCam1, mainCam2;
    public PlayerController oldPlayer, player;
    public Transform playerPoint1;
    // Start is called before the first frame update
    public Button skipButton1, skipButton2;

    private void Start()
    {
        instance = this;
    }
    // Update is called once per frame
    void Update()
    {
        mainCam2.transform.position = mainCam1.transform.position;
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
       
            anim.SetTrigger("Scene2");
        }
    }

    public void EnableMove()
    {
        UIController.instance.joystickOn = true;
        //UIController.instance.joyStick.gameObject.SetActive(true);
        player.canMove = true;

        UIController.instance.gamepadOn = true;
        
   
    }

    public void DisableMove()
    {
        player.canMove = false;
        Invoke("DisableStick", 0.2f);

        UIController.instance.gamepadOn = false;

    }

    public void EnableCursor()
    {
        if (UIController.instance.cursorOn)
        {
            if (player.isGamepad)
            {
                UIController.instance.cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 1100;
                UIController.instance.cursor.GetComponentInChildren<Image>().raycastTarget = false;
                UIController.instance.cursor.GetComponentInChildren<Image>().enabled = true;
            }
        }
    }
    public void DisableStick()
    {
        //UIController.instance.joyStick.gameObject.SetActive(false);
        UIController.instance.joystickOn = false;
    }

    public void StartTutorial()
    {
        UIController.instance.startFadeToBlack();
        SceneManager.LoadScene("Tutorial");
        //StartCoroutine(LevelManager.instance.LevelEnd());
    }

    public void TurnLeft()
    {
        PlayerController.instance.transform.localScale = new Vector3(-1f, 1f, 1f);
    }

    public void TurnRight()
    {
        PlayerController.instance.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void ResetCam()
    {
        PlayerController.instance.mainCam.transform.position = PlayerController.instance.transform.position;
    }

    public void SwitchCam()
    {
        mainCam1.gameObject.SetActive(false);
        mainCam2.gameObject.SetActive(true);
    }

    public void ResetCharacter()
    {
        if (player.anim.enabled == true)
        {
            player.enabled = false;
        }
        else
        {
            player.enabled = true;
        }
    }

    public void RepositionChar1()
    {
        player.transform.position = playerPoint1.position;
        player.transform.localScale = new Vector3(-1f, 1f, 0f);
    }

    public void SkipScene()
    {
        //skipButton1.enabled = false;
        UIController.instance.DisableSkip(1);
        AudioListener.volume = 0;
        anim.CrossFade("Cutscene1_Anim", 0f, 0, 0.9f);
        //anim.Play("Cutscene1_Anim", 0, 0.9f);
    }

    public void SkipScene2()
    {
        UIController.instance.DisableSkip(2);
        StartTutorial();
        //anim.CrossFade("Cutscene2", 0f, 0, 0.9f);
        //anim.Play("Cutscene1_Anim", 0, 0.9f);
    }

    public void ResetSound()
    {
        AudioListener.volume = 1f;
    }

    public void SkipButton1()
    {
        UIController.instance.DisableSkip(1);
    }
    public void SkipButton2()
    {
        UIController.instance.DisableSkip(2);
    }

    public void ActiveStickBG()
    {
        joystickBg.SetActive(true);
    }



}
