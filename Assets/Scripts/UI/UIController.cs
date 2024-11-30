using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using GoogleMobileAds.Api;
using Newtonsoft.Json.Converters;
using System.Xml.Linq;
using UnityEngine.InputSystem.UI;
using System.Data;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;


public class UIController : MonoBehaviour
{

    public Slider healthSlider, bossHealthSlider, expSlider, shieldSlider;
    public Text ammo1, ammo2, bomb1, coins, gems, parts, runes;
    public Text healthText, levelText, charLvlText, expPlusText, partsPlusText, shieldText, autosaveText;
    public static UIController instance;
    public GameObject deathScreen, pauseMenu, gunLogMenu, statMenu, craftMenu, selectScreen, partsUI, gemShop, runeMenu;
    public Text timer, buffTimer, ultTimer, ultCooldown, dashTimer, comboText, comboNumber, comboEXP;
    public Image bigAmmo, fadeScreen, Infinite, dualGun, Disabled, healthBar, Ult, expBar, shieldBar;
    public float fadeSpeed;
    public bool fadeToBlack, fadeOutBlack, fadeText;
    public Joystick joyStick, joyStick2;
    public Joystick[] joysticks;
    public Image currentGun, nextGun;
    public GameObject interactButton, switchGunButton, defaultButton, statButton, grenadeButton, ultButton, ultButtonGlow, dashButton, pauseButton, shopButton, joyStickHandle1, joyStickHandle2;
    public GameObject notEnoughWindow, notEnoughWindow2, noAdWindow;
    public List<GameObject> reviveButtons;

    [HideInInspector]
    public bool isPaused = false;

    [HideInInspector]
    public bool quitting = false;

    public Animator anim;

    [Header("GunRack")]
    public GameObject commonGuns;
    public GameObject rareGuns;
    public GameObject uniqueGuns;
    public GameObject epicGuns;
    public Image gunRackColor;

    [Header("Traits")]
    public GameObject charTraitsMenu;
    public GameObject statsMenu, skinsMenu;
    public GameObject[] characterInfo, characterSkin;
    public Image charPicture;
    public Image defaultGun;
    public GameObject confirmWindow, confirmWindow2, confirmWindow3, confirmWindow4, warningWindow, payFailedWindow, confirmAdWindow;
    public Text Active, lvlText, skillInfo, skillInfo2, skillInfo3, costText, partsInfo1, charStats;

    private GameObject skillLock;
    public int charLvl, uiskillLvl, uiskillNumber, uiCost, skinNumber, skinCost;
    private int craftCost;
    private string gunName;

    public GameObject craftLockedMsg;

    [Header("Audio")]
    public GameObject buttonConfirm, revive;

    [Header("Fade Countdown")]
    public float fadeCountdown;
    [SerializeField]
    public float fadeCount;

    [Header("Ads Handler")]
    public AdsManager adsScript;
    public AdsManager adsScript2;


    [Header("Gem Shop Price")]
    public Text price1;
    public Text price2;
    public Text price3;

    [Header("Cost Texts")]
    public Text cost1;
    public Text cost2;
    public Text cost3;

    [Header("Confirm Texts")]
    public Text craftConfirmText;
    public Text skinActiveText;

    [Header("No Dual Icon")]
    public Image noDual;

    [Header("PlayGames")]
    public GameObject noLoginWindow;

    [Header("Admob Ad")]
    public Button adButton1;
    public Button adButton2;
    public Button adButton3;
    public GameObject screenCover;

    [Header("Cutscene")]
    public GameObject skipButton;
    public GameObject skipButton2;

    [Header("Joystick Type")]
    public int stickType;
    public int moveType;
    public GameObject chooseTypeWindow;
    public GameObject type1;
    public GameObject type2;

    [Header("Load Saved Level")]
    public GameObject continueWindow;
    public GameObject continueButton;

    [Header("Gamepad/Touch Controls")]
    public GameObject cursor;
    public GameObject touchMenu;
    public GameObject gamepadMenu;
    public GameObject gamepadUI;
    public GameObject dynamicButton;
    public GameObject fixedButton;
    private PlayerInput input;
    [HideInInspector]
    public bool cursorOn, gamepadOn;

    private int screenPosition;

    [Header("Canvas Scaling")]
    CanvasScaler canvasScaler;

    [Header("RuneStats")]
    public List<Text> runePoints;
    public List<Text> runeStats;
    public List<Text> runeStatInfo;
    public int runeCount;
    public Button resetButton;

    [HideInInspector]
    public bool joystickOn, destroyAds;

    private void Awake()
    {
        instance = this;

        input = new PlayerInput();

        Application.targetFrameRate = 60;
        Screen.SetResolution(Screen.width, Screen.height, true);
        //canvasScaler = GetComponent<CanvasScaler>();
        //Screen.SetResolution(Screen.width, Screen.height, true);

    }

    // Start is called before the first frame update
    void Start()
    {
        //SetResolution();


        if (selectScreen != null)
        {
            selectScreen.GetComponentInChildren<Scrollbar>().value = 1;
        }

        gamepadOn = false;
        fadeText = true;
        fadeOutBlack = true;
        fadeToBlack = false;

        fadeCount = fadeCountdown;

        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunUI;

        if (PlayerController.instance.availableGuns.Count > 1)
        {
            if (PlayerController.instance.currentGun == 0)
            {
                nextGun.sprite = PlayerController.instance.availableGuns[1].gunUI;
            }
            else
            {
                nextGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.nextGun].gunUI;
            }
        }

        if (PlayerController.instance != null)
        {
            if (PlayerController.instance.expBuff)
            {
                expPlusText.gameObject.SetActive(true);
            }
            else
            {
                expPlusText.gameObject.SetActive(false);
            }

            if (PlayerController.instance.partsBuff)
            {
                partsPlusText.gameObject.SetActive(true);

                if (PlayerController.instance.expBuff)
                {
                    partsPlusText.GetComponent<RectTransform>().anchoredPosition =
                        new Vector3(partsPlusText.GetComponent<RectTransform>().anchoredPosition.x, partsPlusText.GetComponent<RectTransform>().anchoredPosition.y - 32f, 0f);
                }
            }
            else
            {
                partsPlusText.gameObject.SetActive(false);
            }

        }

        if (PlayerController.instance.ultCoolCounter <= 0)
        {
            if (ultButtonGlow != null)
            {
                ultButtonGlow.SetActive(true);
            }
        }


        if (LevelManager.instance.isCamp && !LevelManager.instance.isCutscene)
        {
            LoadMove();

            if (PlayerController.instance.isGamepad)
            {
                //cursor.GetComponent<VirtualMouseInput>().enabled = true;
                cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 1100;
                cursor.GetComponentInChildren<Image>().raycastTarget = false;
                cursor.GetComponentInChildren<Image>().enabled = true;

            }
            else
            {
                cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
                cursor.GetComponentInChildren<Image>().raycastTarget = true;
                cursor.GetComponentInChildren<Image>().enabled = false;

            }

            if (adsScript == null)
            {

                adsScript = AdsManager.instance;
                //adsScript.InitializeLoad();


            }
        }

        if (!LevelManager.instance.isCamp && !LevelManager.instance.isCutscene && SceneManager.GetActiveScene().name != "Tutorial")
        {
            LoadMove();
            Load();

            Invoke("FillReviveAd", 1f);
        }


        Invoke("CheckSavedLevel", 1f);

    }

    public void FillReviveAd()
    {

        if (adsScript == null)
        {
            if (PlayerController.instance.GetComponentInChildren<AdsManager>() != null)
            {
                adsScript = PlayerController.instance.GetComponentInChildren<AdsManager>();
                adsScript.InitializeLoad();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOutBlack)
        {
            fadeCount -= Time.deltaTime;

            if (!LevelManager.instance.isCutscene && SceneManager.GetActiveScene().name != "Tutorial")
            {
                DisableButtons();
            }

            if (fadeCount <= 0)
            {

                fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
                if (fadeScreen.color.a <= 0.5f)
                {
                    if (!LevelManager.instance.isCamp && SceneManager.GetActiveScene().name != "Tutorial")
                    {
                        EnableButtons();
                    }

                }
                if (fadeScreen.color.a == 0f)
                {

                    fadeOutBlack = false;
                    fadeCount = fadeCountdown;
                    fadeScreen.gameObject.SetActive(false);

                }
            }
        }

        if (fadeToBlack)
        {

            DisableButtons();
            fadeScreen.gameObject.SetActive(true);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeText)
        {
            levelText.color = new Color(levelText.color.r, levelText.color.g, levelText.color.b, Mathf.MoveTowards(levelText.color.a, 0f, 0.4f * Time.deltaTime));
            if (levelText.color.a == 0f)
            {
                fadeText = false;
            }
        }


        if (quitting)
        {
            if (ultCooldown.gameObject.activeInHierarchy)
            {
                ultCooldown.gameObject.SetActive(false);
            }
        }


        /* if (deathScreen != null)
         {
             if (deathScreen.activeInHierarchy)
             {
                 //Time.timeScale = 0f;
             }
         }*/

        if (PlayerController.instance != null)
        {
            if (PlayerController.instance.availableGuns.Count == 1)
            {
                switchGunButton.GetComponent<Button>().interactable = false;
                defaultButton.GetComponent<Button>().interactable = false;
            }
            else
            {
                switchGunButton.GetComponent<Button>().interactable = true;
                defaultButton.GetComponent<Button>().interactable = true;
            }

            if (LevelManager.instance.isCamp)
            {
                if (selectScreen.activeInHierarchy)
                {
                    if (PlayerController.instance.isGamepad)
                    {
                        PlayerController.instance.canMove = false;

                    }
                }
            }

            if (!LevelManager.instance.isCutscene)
            {
                if (PlayerController.instance.isGamepad)
                {
                    if (!joyStick2.gameObject.activeInHierarchy && !fadeScreen.gameObject.activeInHierarchy)
                    {
                        //cursor.GetComponent<VirtualMouseInput>().enabled = true;
                        cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 1100;
                        cursor.GetComponentInChildren<Image>().raycastTarget = false;
                        cursor.GetComponentInChildren<Image>().enabled = true;

                    }
                    else
                    {
                        cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
                        cursor.GetComponentInChildren<Image>().raycastTarget = true;
                        cursor.GetComponentInChildren<Image>().enabled = false;

                    }
                }
                else
                {
                    cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
                    cursor.GetComponentInChildren<Image>().raycastTarget = true;
                    cursor.GetComponentInChildren<Image>().enabled = false;
                }
            }

            else if (LevelManager.instance.isCutscene)
            {
                if (PlayerController.instance.isGamepad || CutsceneController.instance.player.isGamepad)
                {
                    if (cursorOn)
                    {
                        //cursor.GetComponent<VirtualMouseInput>().enabled = true;
                        cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 1100;
                        cursor.GetComponentInChildren<Image>().raycastTarget = false;
                        cursor.GetComponentInChildren<Image>().enabled = true;

                    }
                    else
                    {
                        cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
                        cursor.GetComponentInChildren<Image>().raycastTarget = true;
                        cursor.GetComponentInChildren<Image>().enabled = false;

                    }
                }
                else
                {
                    cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
                    cursor.GetComponentInChildren<Image>().raycastTarget = true;
                    cursor.GetComponentInChildren<Image>().enabled = false;
                }

                if (joystickOn)
                {
                    joyStick.gameObject.SetActive(true);
                }
                else if (!joystickOn)
                {
                    joyStick.gameObject.SetActive(false);
                }
            }
        }
    }
    /*
        public void SetResolution()
        {
            canvasScaler.referenceResolution *= new Vector2((canvasScaler.referenceResolution.x / Screen.width), (Screen.height / canvasScaler.referenceResolution.y));
        }*/

    public void startFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }
    public void DisableJoystick1()
    {

        if (joyStick.GetComponent<RectTransform>().anchoredPosition.x >= 0f)
        {
            joyStick.GetComponent<RectTransform>().anchoredPosition = new Vector3(joyStick.GetComponent<RectTransform>().anchoredPosition.x - 1000f,
                joyStick.GetComponent<RectTransform>().anchoredPosition.y, 0f);

            joyStick.GetComponent<Image>().raycastTarget = false;
        }


    }
    public void EnableJoystick1()
    {
        if (joyStick.GetComponent<RectTransform>().anchoredPosition.x < 0f && !PlayerController.instance.isGamepad)
        {
            joyStick.GetComponent<RectTransform>().anchoredPosition = new Vector3(joyStick.GetComponent<RectTransform>().anchoredPosition.x + 1000f,
                joyStick.GetComponent<RectTransform>().anchoredPosition.y, 0f);

            joyStick.GetComponent<Image>().raycastTarget = true;
        }

    }

    public void EnableJoystickCutscene()
    {
        if (joyStick.GetComponent<RectTransform>().anchoredPosition.x < 0f)
        {
            joyStick.GetComponent<RectTransform>().anchoredPosition = new Vector3(joyStick.GetComponent<RectTransform>().anchoredPosition.x + 1000f,
                joyStick.GetComponent<RectTransform>().anchoredPosition.y, 0f);

            joyStick.GetComponent<Image>().raycastTarget = true;
        }

    }

    public void DisableButtons()
    {
        if (LevelManager.instance.isCamp)
        {
            switchGunButton.gameObject.SetActive(false);
            defaultButton.gameObject.SetActive(false);
            joyStick2.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
            shopButton.gameObject.SetActive(false);
            currentGun.gameObject.SetActive(false);
            nextGun.gameObject.SetActive(false);

            DisableJoystick1();

            if (interactButton.gameObject.activeInHierarchy)
            {
                interactButton.GetComponentsInChildren<Image>()[1].enabled = false;
                interactButton.gameObject.GetComponent<Image>().enabled = false;
                interactButton.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            switchGunButton.gameObject.SetActive(false);
            defaultButton.gameObject.SetActive(false);
            joyStick2.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(false);
            currentGun.gameObject.SetActive(false);
            nextGun.gameObject.SetActive(false);
            ultButton.gameObject.SetActive(false);
            dashButton.gameObject.SetActive(false);
            grenadeButton.gameObject.SetActive(false);

            DisableJoystick1();

            if (ultButtonGlow.gameObject.activeInHierarchy)
            {
                ultButtonGlow.gameObject.GetComponent<Image>().enabled = false;
            }

            if (ultCooldown.gameObject.activeInHierarchy)
            {
                ultCooldown.enabled = false;
            }

            if (dashTimer.gameObject.activeInHierarchy)
            {
                dashTimer.enabled = false;
            }

            if (interactButton.gameObject.activeInHierarchy)
            {
                interactButton.gameObject.GetComponent<Image>().enabled = false;
                interactButton.gameObject.GetComponentInChildren<Image>().enabled = false;
                interactButton.gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }

    public void EnableButtons()
    {
        if (LevelManager.instance.isCamp)
        {
            switchGunButton.gameObject.SetActive(true);
            defaultButton.gameObject.SetActive(true);
            joyStick2.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(true);
            shopButton.gameObject.SetActive(true);
            currentGun.gameObject.SetActive(true);
            nextGun.gameObject.SetActive(true);

            EnableJoystick1();

            interactButton.gameObject.GetComponent<Image>().enabled = true;
            interactButton.GetComponentsInChildren<Image>()[1].enabled = true;
            interactButton.gameObject.GetComponent<Button>().interactable = true;

        }
        else
        {
            switchGunButton.gameObject.SetActive(true);
            defaultButton.gameObject.SetActive(true);
            joyStick2.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(true);
            currentGun.gameObject.SetActive(true);
            nextGun.gameObject.SetActive(true);
            ultButton.gameObject.SetActive(true);
            dashButton.gameObject.SetActive(true);
            grenadeButton.gameObject.SetActive(true);

            EnableJoystick1();

            if (ultButtonGlow.activeInHierarchy)
            {
                ultButtonGlow.gameObject.GetComponent<Image>().enabled = true;
            }

            if (ultCooldown.gameObject.activeInHierarchy)
            {
                ultCooldown.enabled = true;
            }

            if (dashTimer.gameObject.activeInHierarchy)
            {
                dashTimer.enabled = true;
            }

            interactButton.gameObject.GetComponent<Image>().enabled = true;
            interactButton.gameObject.GetComponentInChildren<Image>().enabled = true;
            interactButton.gameObject.GetComponent<Button>().interactable = true;

        }
    }


    public void Resume()
    {
        if (isPaused == true)
        {
            if (PlayerController.instance.isGamepad)
            {
                //cursor.GetComponent<VirtualMouseInput>().enabled = false;
                cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
                cursor.GetComponentInChildren<Image>().raycastTarget = true;
                cursor.GetComponentInChildren<Image>().enabled = false;
                //cursor.GetComponentInChildren<Image>().enabled = false;

            }

            pauseMenu.SetActive(false);

            isPaused = false;

            Time.timeScale = 1f;

            if (LevelManager.instance.isCamp)
            {
                if (GoogleSaveManager.instance != null)
                {
                    GoogleSaveManager.instance.success1.gameObject.SetActive(false);
                    GoogleSaveManager.instance.fail1.gameObject.SetActive(false);
                    GoogleSaveManager.instance.fail2.gameObject.SetActive(false);
                    GoogleSaveManager.instance.fail3.gameObject.SetActive(false);
                }
            }

            EnableButtons();

        }
    }
    public void Pause()
    {
        if (!isPaused)
        {
            pauseMenu.SetActive(true);

            isPaused = true;

            Time.timeScale = 0f;

            DisableButtons();

            if (PlayerController.instance.isGamepad)
            {
                //cursor.GetComponent<VirtualMouseInput>().enabled = true;
                cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 1100;
                cursor.GetComponentInChildren<Image>().raycastTarget = false;
                cursor.GetComponentInChildren<Image>().enabled = true;
            }
        }

    }

    public void Quit()
    {
        quitting = true;
        DisableButtons();
        //fadeScreen.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        startFadeToBlack();
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
        LevelManager.instance.savedLevel = "";
        CharTracker.instance.SaveLevel();
        CharTracker.instance.ClearTemp();
        Invoke("Menu", 2f);

        if (PlayerController.instance.isGamepad)
        {
            //cursor.GetComponent<VirtualMouseInput>().enabled = false;
            cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
            cursor.GetComponentInChildren<Image>().raycastTarget = true;
            cursor.GetComponentInChildren<Image>().enabled = false;
        }
    }

    public void Restart()
    {
        LevelManager.instance.savedLevel = "";
        CharTracker.instance.SaveLevel();
        CharTracker.instance.ClearTemp();

        Time.timeScale = 1f;

        SceneManager.LoadScene("0");
        //Load exp UI/scene
        Destroy(PlayerController.instance.gameObject);
        //destroyAds = true;

    }


    public void Menu()
    {

        SceneManager.LoadScene("Title Scene");
    }

    public void GemShop()
    {
        gemShop.SetActive(true);
        PlayerController.instance.canMove = false;
        DisableButtons();
    }
    public void GunLog()
    {
        anim.SetTrigger("Open");
        gunLogMenu.SetActive(true);
        //Time.timeScale = 0f;
        gunRackColor.color = new Color32(255, 255, 255, 255);
        rareGuns.SetActive(true);
        commonGuns.SetActive(true);
        uniqueGuns.SetActive(true);
        epicGuns.SetActive(true);
        Invoke("RefreshList", 0.15f);

        SaveNewGun(0);
        PlayerController.instance.canMove = false;
        DisableButtons();

    }

    public void RefreshList()
    {
        rareGuns.SetActive(false);
        uniqueGuns.SetActive(false);
        epicGuns.SetActive(false);
        gunRackColor.color = new Color32(255, 255, 255, 255);
    }

    public void openCommon()
    {
        commonGuns.SetActive(true);
        rareGuns.SetActive(false);
        uniqueGuns.SetActive(false);
        epicGuns.SetActive(false);

        gunRackColor.color = new Color32(255, 255, 255, 255);
        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void openRare()
    {
        commonGuns.SetActive(false);
        rareGuns.SetActive(true);
        uniqueGuns.SetActive(false);
        epicGuns.SetActive(false);

        gunRackColor.color = new Color32(85, 165, 255, 255);
        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void openUnique()
    {
        commonGuns.SetActive(false);
        rareGuns.SetActive(false);
        uniqueGuns.SetActive(true);
        epicGuns.SetActive(false);

        gunRackColor.color = new Color32(255, 244, 107, 255);
        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void openEpic()
    {
        commonGuns.SetActive(false);
        rareGuns.SetActive(false);
        uniqueGuns.SetActive(false);
        epicGuns.SetActive(true);

        gunRackColor.color = new Color32(255, 98, 93, 255);
        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void CheckCharacter()
    {
        anim.SetTrigger("Open");
        charTraitsMenu.SetActive(true);
        charStats.text = "HP : " + PlayerHealth.instance.maxHealth + "\n" + "DMG : " + PlayerController.instance.playerDamage * 100 + "%" + "\n" +
            "SPD : " + PlayerController.instance.moveSpeed + "\n" + "CRIT : " + PlayerController.instance.gunCrit1 * 100 + "% / " +
            PlayerController.instance.gunCrit2 * 100 + "%";

        if (TraitManager.instance.isBunny)
        {
            charLvl = ExpManager.instance.Levels[0];
            lvlText.text = "Lvl " + charLvl;
            openBunny();

            for (int i = 0; i < TraitManager.instance.Bunny.Count; i++)
            {
                if (TraitManager.instance.Bunny[i] == 1)
                {
                    //characterInfo[0].GetComponent<SkillList>().Skills[i].enabled = false;
                    characterInfo[0].GetComponent<SkillList>().Skills[i].GetComponent<SkillUnlock>().Lock.SetActive(false);
                }
            }
        }
        if (TraitManager.instance.isMole)
        {
            charLvl = ExpManager.instance.Levels[1];
            lvlText.text = "Lvl " + charLvl;
            openMole();

            for (int i = 0; i < TraitManager.instance.Mole.Count; i++)
            {
                if (TraitManager.instance.Mole[i] == 1)
                {
                    //characterInfo[1].GetComponent<SkillList>().Skills[i].enabled = false;
                    characterInfo[1].GetComponent<SkillList>().Skills[i].GetComponent<SkillUnlock>().Lock.SetActive(false);
                }
            }
        }
        if (TraitManager.instance.isRaccoon)
        {
            charLvl = ExpManager.instance.Levels[2];
            lvlText.text = "Lvl " + charLvl;
            openCoon();

            for (int i = 0; i < TraitManager.instance.Raccoon.Count; i++)
            {
                if (TraitManager.instance.Raccoon[i] == 1)
                {
                    //characterInfo[2].GetComponent<SkillList>().Skills[i].enabled = false;
                    characterInfo[2].GetComponent<SkillList>().Skills[i].GetComponent<SkillUnlock>().Lock.SetActive(false);
                }
            }
        }

        DisableButtons();

        //Time.timeScale = 0f;

    }
    public void closeMenu()
    {
        if (gunLogMenu.activeInHierarchy)
        {
            gunLogMenu.SetActive(false);
            commonGuns.SetActive(true);
            rareGuns.SetActive(false);
            uniqueGuns.SetActive(false);
            epicGuns.SetActive(false);
            Time.timeScale = 1f;
        }

        if (charTraitsMenu.activeInHierarchy)
        {
            charTraitsMenu.SetActive(false);
            skinsMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        if (runeMenu.activeInHierarchy)
        {
            resetButton.interactable = false;
            runeMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        PlayerController.instance.canMove = true;
        EnableButtons();

    }

    public void closeGemShop()
    {
        if (gemShop.activeInHierarchy)
        {
            gemShop.SetActive(false);

            if (LevelManager.instance.isCamp)
            {
                if (!deathScreen.activeInHierarchy && !skinsMenu.activeInHierarchy && !confirmWindow4.activeInHierarchy)
                {
                    Time.timeScale = 1f;
                    PlayerController.instance.canMove = true;
                    EnableButtons();
                }

            }

        }

    }



    public void openBunny()
    {
        statsMenu.SetActive(true);
        skinsMenu.SetActive(false);

        characterInfo[0].SetActive(true);
        characterInfo[1].SetActive(false);
        characterInfo[2].SetActive(false);
        /*characterInfo[3].SetActive(false);
        characterInfo[4].SetActive(false);*/

        characterSkin[0].SetActive(false);
        characterSkin[1].SetActive(false);
        characterSkin[2].SetActive(false);

        PlayerController.instance.canMove = false;

    }
    public void openMole()
    {
        statsMenu.SetActive(true);
        skinsMenu.SetActive(false);

        characterInfo[0].SetActive(false);
        characterInfo[1].SetActive(true);
        characterInfo[2].SetActive(false);
        /*characterInfo[3].SetActive(false);
        characterInfo[4].SetActive(false);*/
        characterSkin[0].SetActive(false);
        characterSkin[1].SetActive(false);
        characterSkin[2].SetActive(false);

        PlayerController.instance.canMove = false;

    }
    public void openCoon()
    {
        statsMenu.SetActive(true);
        skinsMenu.SetActive(false);

        characterInfo[0].SetActive(false);
        characterInfo[1].SetActive(false);
        characterInfo[2].SetActive(true);
        /*characterInfo[3].SetActive(false);
        characterInfo[4].SetActive(false);*/
        characterSkin[0].SetActive(false);
        characterSkin[1].SetActive(false);
        characterSkin[2].SetActive(false);

        PlayerController.instance.canMove = false;

    }

    //skin menu
    public void openBunnySkin()
    {
        statsMenu.SetActive(false);
        skinsMenu.SetActive(true);

        characterInfo[0].SetActive(false);
        characterInfo[1].SetActive(false);
        characterInfo[2].SetActive(false);

        characterSkin[0].SetActive(true);
        characterSkin[1].SetActive(false);
        characterSkin[2].SetActive(false);
        /*characterInfo[3].SetActive(false);
        characterInfo[4].SetActive(false);*/

        for (int i = 0; i < SkinManager.instance.Bunny.Count; i++)
        {
            if (SkinManager.instance.Bunny[i] == 1)
            {
                characterSkin[0].GetComponent<SkinSelector>().skins[i].GetComponent<SkillUnlock>().Lock.SetActive(false);
            }
        }
    }
    public void openMoleSkin()
    {
        statsMenu.SetActive(false);
        skinsMenu.SetActive(true);

        characterInfo[0].SetActive(false);
        characterInfo[1].SetActive(false);
        characterInfo[2].SetActive(false);

        characterSkin[0].SetActive(false);
        characterSkin[1].SetActive(true);
        characterSkin[2].SetActive(false);
        /*characterInfo[3].SetActive(false);
        characterInfo[4].SetActive(false);*/

        for (int i = 0; i < SkinManager.instance.Bunny.Count; i++)
        {
            if (SkinManager.instance.Mole[i] == 1)
            {
                characterSkin[1].GetComponent<SkinSelector>().skins[i].GetComponent<SkillUnlock>().Lock.SetActive(false);
            }
        }

    }
    public void openCoonSkin()
    {
        statsMenu.SetActive(false);
        skinsMenu.SetActive(true);

        characterInfo[0].SetActive(false);
        characterInfo[1].SetActive(false);
        characterInfo[2].SetActive(false);

        characterSkin[0].SetActive(false);
        characterSkin[1].SetActive(false);
        characterSkin[2].SetActive(true);
        /*characterInfo[3].SetActive(false);
        characterInfo[4].SetActive(false);*/

        for (int i = 0; i < SkinManager.instance.Bunny.Count; i++)
        {
            if (SkinManager.instance.Raccoon[i] == 1)
            {
                characterSkin[2].GetComponent<SkinSelector>().skins[i].GetComponent<SkillUnlock>().Lock.SetActive(false);
            }
        }

    }


    public void throwGrenade()
    {
        if (PlayerController.instance.bomb1 > 0)
        {
            Vector3 bombPos = Gun.instance.transform.position + new Vector3(0f, 0.3f, 0f);

            Instantiate(PlayerController.instance.grenadePin, bombPos, Gun.instance.transform.rotation);
            Instantiate(PlayerController.instance.Grenade, bombPos, Gun.instance.transform.rotation);
            PlayerController.instance.bomb1 -= 1;
            bomb1.text = "x" + PlayerController.instance.bomb1.ToString();
        }
        else
        {
            PlayerController.instance.noBomb.GetComponent<DestroyOnTime>().enabled = false;
            PlayerController.instance.noBomb.SetActive(true);
        }
    }

    public void activeUlt()
    {

        PlayerController.instance.isUltActive = true;

    }

    public void skillParam(GameObject Lock, int skillNumber, int Cost, int skillLvl, string Info)
    {
        skillLock = Lock;
        uiskillNumber = skillNumber;
        uiskillLvl = skillLvl;
        uiCost = Cost;

        if (uiskillLvl > charLvl)
        {
            skillInfo2.text = Info;
            warningWindow.gameObject.SetActive(true);

        }
        else if (!Lock.activeInHierarchy)
        {
            //costText.text = "Effect :"; //+ Cost;
            skillInfo3.text = Info;
            confirmWindow2.gameObject.SetActive(true);
        }
        else
        {
            costText.text = cost1.text; //+ Cost;
            skillInfo.text = Info;
            confirmWindow.gameObject.SetActive(true);
        }

        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void skinParam(GameObject Lock, int Cost, int skinNum)
    {
        skillLock = Lock;
        skinCost = Cost;
        skinNumber = skinNum;
        uiskillNumber = 0;

        if (!Lock.activeInHierarchy)
        {
            skillInfo3.text = skinActiveText.text;
            confirmWindow2.gameObject.SetActive(true);
            equipSkin(skinNumber);
        }
        else
        {
            costText.text = cost2.text;
            skillInfo.text = cost3.text + Cost;
            confirmWindow.gameObject.SetActive(true);
        }

        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void equipSkin(int skinNum)
    {
        SkinManager.instance.equipSkin(skinNum);
    }

    public void confirmRevive()
    {
        Revive();
    }

    public void closeWindow()
    {
        if (warningWindow != null && confirmWindow != null && confirmWindow2 != null && confirmWindow3 != null && confirmWindow4 != null)
        {
            warningWindow.gameObject.SetActive(false);
            confirmWindow.gameObject.SetActive(false);
            confirmWindow2.gameObject.SetActive(false);
            confirmWindow3.gameObject.SetActive(false);
            notEnoughWindow2.SetActive(false);

            if (confirmWindow4.activeInHierarchy)
            {
                Time.timeScale = 1f;
                confirmWindow4.gameObject.SetActive(false);
                PlayerController.instance.canMove = true;
                EnableButtons();
            }
        }
        else
        {
            notEnoughWindow.SetActive(false);
            noAdWindow.SetActive(false);

        }

        Instantiate(buttonConfirm, transform.position, transform.rotation);

    }

    public void closeWindow2()
    {
        if (notEnoughWindow.activeInHierarchy)
        {
            notEnoughWindow.SetActive(false);
        }

        if (noAdWindow.activeInHierarchy)
        {
            noAdWindow.SetActive(false);

            if (LevelManager.instance.isCamp)
            {
                if (!confirmWindow4.activeInHierarchy && !confirmAdWindow.activeInHierarchy)
                {
                    EnableButtons();
                }
            }
        }

        if (LevelManager.instance.isCamp)
        {
            Time.timeScale = 1f;
            PlayerController.instance.canMove = true;
        }

        Instantiate(buttonConfirm, transform.position, transform.rotation);

    }

    public void closeWindow3()
    {
        payFailedWindow.SetActive(false);

        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }


    public void buySkill()
    {

        if (uiskillNumber == 0)
        {
            if (LevelManager.instance.currentGems < skinCost)
            {
                notEnoughWindow.SetActive(true);
                /* skillInfo2.text = "Not enough gems.";
                 warningWindow.gameObject.SetActive(true);*/
                Instantiate(buttonConfirm, transform.position, transform.rotation);
            }
            else
            {
                skillLock.SetActive(false);

                LevelManager.instance.currentGems -= skinCost;
                gems.text = "x" + LevelManager.instance.currentGems.ToString();

                if (SkinManager.instance.isBunny)
                {
                    SkinManager.instance.Bunny[skinNumber] = 1;
                    SkinManager.instance.currentSkinCode = skinNumber;
                }
                if (SkinManager.instance.isMole)
                {
                    SkinManager.instance.Mole[skinNumber] = 1;
                    SkinManager.instance.currentSkinCode = skinNumber;
                }
                if (SkinManager.instance.isRaccoon)
                {
                    SkinManager.instance.Raccoon[skinNumber] = 1;
                    SkinManager.instance.currentSkinCode = skinNumber;
                }

                closeWindow();
            }
        }
        else
        {
            skillLock.SetActive(false);

            //LevelManager.instance.currentGems -= uiCost;
            //gems.text = "x" + LevelManager.instance.currentGems.ToString();

            if (uiskillNumber == 1)
            {
                TraitManager.instance.ApplyStat(uiskillNumber);

                if (TraitManager.instance.isBunny)
                {
                    TraitManager.instance.Bunny[0] = 1;
                    //characterInfo[0].GetComponent<SkillList>().Skills[0].enabled = false;
                }
                if (TraitManager.instance.isMole)
                {
                    TraitManager.instance.Mole[0] = 1;
                    //characterInfo[1].GetComponent<SkillList>().Skills[0].enabled = false;
                }
                if (TraitManager.instance.isRaccoon)
                {
                    TraitManager.instance.Raccoon[0] = 1;
                    //characterInfo[2].GetComponent<SkillList>().Skills[0].enabled = false;
                }
            }
            if (uiskillNumber == 2)
            {
                TraitManager.instance.ApplyStat(uiskillNumber);

                if (TraitManager.instance.isBunny)
                {
                    TraitManager.instance.Bunny[1] = 1;
                    //characterInfo[0].GetComponent<SkillList>().Skills[1].enabled = false;
                }
                if (TraitManager.instance.isMole)
                {
                    TraitManager.instance.Mole[1] = 1;
                    //characterInfo[1].GetComponent<SkillList>().Skills[1].enabled = false;
                }
                if (TraitManager.instance.isRaccoon)
                {
                    TraitManager.instance.Raccoon[1] = 1;
                    //characterInfo[2].GetComponent<SkillList>().Skills[1].enabled = false;
                }

            }
            if (uiskillNumber == 3)
            {
                TraitManager.instance.ApplyStat(uiskillNumber);

                if (TraitManager.instance.isBunny)
                {
                    TraitManager.instance.Bunny[2] = 1;
                    //characterInfo[0].GetComponent<SkillList>().Skills[2].enabled = false;
                }
                if (TraitManager.instance.isMole)
                {
                    TraitManager.instance.Mole[2] = 1;
                    //characterInfo[1].GetComponent<SkillList>().Skills[2].enabled = false;
                }
                if (TraitManager.instance.isRaccoon)
                {
                    TraitManager.instance.Raccoon[2] = 1;
                    //characterInfo[2].GetComponent<SkillList>().Skills[2].enabled = false;
                }

            }
            if (uiskillNumber == 4)
            {
                TraitManager.instance.ApplyStat(uiskillNumber);

                if (TraitManager.instance.isBunny)
                {
                    TraitManager.instance.Bunny[3] = 1;
                    //characterInfo[0].GetComponent<SkillList>().Skills[3].enabled = false;
                }
                if (TraitManager.instance.isMole)
                {
                    TraitManager.instance.Mole[3] = 1;
                    //characterInfo[1].GetComponent<SkillList>().Skills[3].enabled = false;
                }
                if (TraitManager.instance.isRaccoon)
                {
                    TraitManager.instance.Raccoon[3] = 1;
                    //characterInfo[2].GetComponent<SkillList>().Skills[3].enabled = false;
                }

            }
            if (uiskillNumber == 5)
            {
                TraitManager.instance.ApplyStat(uiskillNumber);

                if (TraitManager.instance.isBunny)
                {
                    TraitManager.instance.Bunny[4] = 1;
                    //characterInfo[0].GetComponent<SkillList>().Skills[4].enabled = false;
                }
                if (TraitManager.instance.isMole)
                {
                    TraitManager.instance.Mole[4] = 1;
                    //characterInfo[1].GetComponent<SkillList>().Skills[4].enabled = false;
                }
                if (TraitManager.instance.isRaccoon)
                {
                    TraitManager.instance.Raccoon[4] = 1;
                    //characterInfo[2].GetComponent<SkillList>().Skills[4].enabled = false;
                }

            }
            if (uiskillNumber == 6)
            {
                TraitManager.instance.ApplyStat(uiskillNumber);

                if (TraitManager.instance.isBunny)
                {
                    TraitManager.instance.Bunny[5] = 1;
                    //characterInfo[0].GetComponent<SkillList>().Skills[5].enabled = false;
                }
                if (TraitManager.instance.isMole)
                {
                    TraitManager.instance.Mole[5] = 1;
                    //characterInfo[1].GetComponent<SkillList>().Skills[5].enabled = false;
                }
                if (TraitManager.instance.isRaccoon)
                {
                    TraitManager.instance.Raccoon[5] = 1;
                    //characterInfo[2].GetComponent<SkillList>().Skills[5].enabled = false;
                }

            }

            charStats.text = "HP : " + PlayerHealth.instance.maxHealth + "\n" + "DMG : " + PlayerController.instance.playerDamage * 100 + "%" + "\n" +
           "SPD : " + PlayerController.instance.moveSpeed + "\n" + "CRIT : " + PlayerController.instance.gunCrit1 * 100 + "% / " +
           PlayerController.instance.gunCrit2 * 100 + "%";

            closeWindow();
        }

        CharTracker.instance.SavePlayer();

        //Instantiate(buttonConfirm, transform.position, transform.rotation);

    }

    public void craftGun(int Cost, string Name)
    {
        gunName = Name;
        craftCost = Cost;


        partsInfo1.text = craftConfirmText.text + Cost;
        confirmWindow3.gameObject.SetActive(true);

        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void confirmCraft()
    {
        if (LevelManager.instance.currentParts < craftCost)
        {
            //skillInfo2.text = "Not enough parts!";
            notEnoughWindow2.gameObject.SetActive(true);
        }
        else
        {
            LevelManager.instance.currentParts -= craftCost;
            parts.text = "x" + LevelManager.instance.currentParts.ToString();

            for (int i = 0; i < PickupManager.instance.gunPickups.Count; i++)
            {
                if (PickupManager.instance.gunPickups[i].name == gunName)
                {
                    Instantiate(PickupManager.instance.spawnEffect, PickupManager.instance.pickupSpawn1.position, PickupManager.instance.pickupSpawn1.rotation);
                    Instantiate(PickupManager.instance.gunPickups[i], PickupManager.instance.pickupSpawn1.position, PickupManager.instance.pickupSpawn1.rotation);
                }
            }

            closeWindow();
            gunLogMenu.SetActive(false);
            commonGuns.SetActive(false);
            rareGuns.SetActive(false);
            uniqueGuns.SetActive(false);
            epicGuns.SetActive(false);
            PlayerController.instance.canMove = true;
            EnableButtons();

            CharTracker.instance.SavePlayer();
        }

        Instantiate(buttonConfirm, transform.position, transform.rotation);
    }

    public void Revive()
    {
        if (PlayerController.instance.reviveCount == 2)
        {
            if (LevelManager.instance.currentGems >= 15)
            {
                LevelManager.instance.currentGems -= 15;
                gems.text = "x" + LevelManager.instance.currentGems.ToString();
                PlayerHealth.instance.Revive();
                deathScreen.SetActive(false);

                CharTracker.instance.SavePlayer();

                Time.timeScale = 1f;
            }
            else
            {
                notEnoughWindow.SetActive(true);
            }

        }
        else if (PlayerController.instance.reviveCount == 1)
        {
            if (LevelManager.instance.currentGems >= 20)
            {
                LevelManager.instance.currentGems -= 20;
                gems.text = "x" + LevelManager.instance.currentGems.ToString();
                PlayerHealth.instance.Revive();
                deathScreen.SetActive(false);

                CharTracker.instance.SavePlayer();

                Time.timeScale = 1f;
            }
            else
            {
                notEnoughWindow.SetActive(true);
            }
        }

    }

    public void confirmGemItem()
    {
        confirmWindow4.SetActive(true);
        adsScript2.LoadEmptyRewardedAd();
        //Time.timeScale = 0f;
        PlayerController.instance.canMove = false;
        DisableButtons();
    }

    public void confirmAdItem()
    {
        confirmAdWindow.SetActive(true);
        //adsScript.LoadEmptyRewardedAd();
        PlayerController.instance.canMove = false;
        DisableButtons();
    }

    public void closeAdConfirm()
    {
        confirmAdWindow.SetActive(false);
        PlayerController.instance.canMove = true;
        Time.timeScale = 1f;
        EnableButtons();
    }

    public void buyGemItem()
    {
        PlayerController.instance.confirmBuy = true;

        //closeWindow();

        //Time.timeScale = 1f;
    }
    public void craftLocked()
    {
        if (!craftLockedMsg.activeInHierarchy)
        {
            craftLockedMsg.SetActive(true);
            Invoke("CloseMsg", 6f);
        }
    }

    public void CloseMsg()
    {
        craftLockedMsg.SetActive(false);
    }

    public void DeActiveSelect()
    {
        selectScreen.SetActive(false);
        PlayerController.instance.canMove = true;

        if (PlayerController.instance.isGamepad)
        {
            //cursor.GetComponent<VirtualMouseInput>().enabled = false;
            cursor.GetComponent<VirtualMouseInput>().cursorSpeed = 0;
            cursor.GetComponentInChildren<Image>().raycastTarget = true;
            cursor.GetComponentInChildren<Image>().enabled = false;
        }

        if (!PlayerPrefs.HasKey("joystick"))
        {
            StickMenu();
            DisableButtons();
            PlayerPrefs.SetInt("joystick", 1);
            Load();
        }
        else
        {
            Load();
        }

        healthSlider.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);
        expSlider.gameObject.SetActive(true);
        charLvlText.gameObject.SetActive(true);

        DailyGemManager.instance.GiveGem();
        adsScript.InitializeEmptyLoad();

    }


    //Ads
    public void playAdGun()
    {
        //NoAdWarning();
        adsScript.ShowRewardedAdGun();

        //if(adcooldown > 0){play ad}
        //if(ad done){revive)
        //noAdWindow.SetActive(true);

    }

    public void playAdBuff()
    {
        //NoAdWarning();
        screenCover.gameObject.SetActive(true);

        Invoke("AdBuffInitiate", 3f);
        //adsScript2.ShowRewardedAdBuff();

        //if(adcooldown > 0){play ad}
        //if(ad done){PlayerController.instance.confirmBuy = true; closeWindow(); Time.timeScale = 1f;)
        //noAdWindow.SetActive(true);

    }

    public void AdBuffInitiate()
    {
        screenCover.gameObject.SetActive(false);
        adsScript2.ShowRewardedAdBuff();
    }

    public void playAdLife()
    {
        //NoAdWarning();
        adsScript.ShowRewardedAdLife();

        //if(adcooldown > 0){play ad}
        //if(ad done){PlayerController.instance.confirmBuy = true; closeWindow(); Time.timeScale = 1f;)
        //noAdWindow.SetActive(true);

    }

    public void adRewardGun()
    {
        VendingMachine2.instance.PlayAd();

        closeAdConfirm();
        EnableButtons();
    }
    public void adRewardBuff()
    {

        PlayerController.instance.confirmAdBuy = true;

        closeWindow();

        Time.timeScale = 1f;
        EnableButtons();
    }

    public void adRewardLife()
    {
        PlayerHealth.instance.Revive();
        deathScreen.SetActive(false);

        Time.timeScale = 1f;
    }

    public void NoAdWarning()
    {

        noAdWindow.SetActive(true);
        if (LevelManager.instance.isCamp)
        {
            Time.timeScale = 0f;
            PlayerController.instance.canMove = false;
            DisableButtons();
        }


    }
    public void PayFailedWarning()
    {
        payFailedWindow.SetActive(true);

    }

    public void CloudWarning()
    {
        GoogleSaveManager.instance.cloudNotice.gameObject.SetActive(true);
    }

    //cloud warning
    public void CloseWarning()
    {
        GoogleSaveManager.instance.cloudNotice.gameObject.SetActive(false);
        GoogleSaveManager.instance.notLoggedIn.gameObject.SetActive(false);
        GoogleSaveManager.instance.cover.gameObject.SetActive(false);
        GoogleSaveManager.instance.success2.gameObject.SetActive(false);
    }

    public void CloseCloudWarning()
    {
        if (PlayerController.instance.isGamepad)
        {
            PlayerController.instance.canMove = true;
        }
        noLoginWindow.SetActive(false);
        EnableButtons();
    }


    public void HideUI()
    {

        healthBar.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        expBar.gameObject.SetActive(false);
        expPlusText.gameObject.SetActive(false);
        expSlider.gameObject.SetActive(false);
        ammo1.gameObject.SetActive(false);
        ammo2.gameObject.SetActive(false);
        Infinite.GetComponent<Image>().enabled = false;
        bigAmmo.gameObject.SetActive(false);
        bomb1.gameObject.SetActive(false);

        coins.gameObject.SetActive(false);
        gems.gameObject.SetActive(false);

        joyStick.gameObject.SetActive(false);
        joyStick2.gameObject.SetActive(false);

        dashButton.gameObject.SetActive(false);
        Ult.gameObject.SetActive(false);
        ultButton.gameObject.SetActive(false);

        grenadeButton.gameObject.SetActive(false);
        switchGunButton.gameObject.SetActive(false);
        currentGun.gameObject.SetActive(false);
        nextGun.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        ultTimer.gameObject.SetActive(false);
        ultCooldown.gameObject.SetActive(false);
        dashTimer.gameObject.SetActive(false);


    }

    public void DisableSkip(int buttonNo)
    {

        if (buttonNo == 1)
        {
            if (skipButton.activeInHierarchy)
            {
                skipButton.gameObject.SetActive(false);
                cursorOn = false;
            }
            else
            {
                skipButton.gameObject.SetActive(true);
                cursorOn = true;
            }
        }
        else if (buttonNo == 2)
        {
            if (skipButton2.activeInHierarchy)
            {
                skipButton2.gameObject.SetActive(false);
                cursorOn = false;
            }
            else
            {
                skipButton2.gameObject.SetActive(true);
                cursorOn = true;
            }
        }
    }

    //joystick type change
    public void ChangeType(int type)
    {
        stickType = type;
        Save();
        chooseTypeWindow.SetActive(false);
        joyStickHandle2.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);

        if (!pauseMenu.activeInHierarchy)
        {
            Time.timeScale = 1f;
            EnableButtons();
        }

    }

    private void Load()
    {
        stickType = PlayerPrefs.GetInt("joystick");
    }

    private void LoadMove()
    {
        if (!PlayerPrefs.HasKey("moveStick"))
        {
            PlayerPrefs.SetInt("moveStick", 1);
            moveType = 1;
        }
        else
        {
            moveType = PlayerPrefs.GetInt("moveStick");
        }

        SetMove();
    }

    private void SetMove()
    {
        if (moveType == 1)
        {
            joyStick = joysticks[0];
            joysticks[1].gameObject.SetActive(false);
            joysticks[0].gameObject.SetActive(true);

            fixedButton.GetComponent<Image>().color = new Color32(255, 90, 86, 255);
            dynamicButton.GetComponent<Image>().color = new Color32(85, 167, 255, 255);
        }
        else if (moveType == 2)
        {
            joyStick = joysticks[1];
            joysticks[0].gameObject.SetActive(false);
            joysticks[1].gameObject.SetActive(true);

            fixedButton.GetComponent<Image>().color = new Color32(85, 167, 255, 255);
            dynamicButton.GetComponent<Image>().color = new Color32(255, 90, 86, 255);
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt("joystick", stickType);
    }

    public void StickMenu()
    {
        chooseTypeWindow.SetActive(true);
        Time.timeScale = 0f;

    }

    public void SaveNewGun(int value)
    {
        PlayerPrefs.SetInt("newgun", value);
    }

    //Control Options

    public void OpenTouchMenu()
    {
        touchMenu.SetActive(true);
        gamepadMenu.SetActive(false);
    }

    public void OpenPadMenu()
    {
        touchMenu.SetActive(false);
        gamepadMenu.SetActive(true);
    }

    public void CloseControlMenu()
    {
        chooseTypeWindow.SetActive(false);
        touchMenu.SetActive(true);
        gamepadMenu.SetActive(false);

        if (!pauseMenu.activeInHierarchy)
        {
            EnableButtons();
            Time.timeScale = 1f;
        }

    }

    //Runes
    public void OpenRuneMenu()
    {
        anim.SetTrigger("Open");
        runeMenu.SetActive(true);

        runeCount = LevelManager.instance.currentRunes;
        runes.text = "x" + Convert.ToString(runeCount);

        for (int i = 0; i < runePoints.Count; i++)
        {
            runePoints[i].text = Convert.ToString(RuneController.instance.stats[i]);

            if (runePoints[i].text == "10")
            {
                runePoints[i].text = "MAX";
            }
        }

        RuneController.instance.ApplyStatsUI();

        PlayerController.instance.canMove = false;
        DisableButtons();
        Time.timeScale = 0f;
    }

    public void SaveRunes()
    {
        RuneController.instance.ResetPlayer();

        for (int i = 0; i < runePoints.Count; i++)
        {
            if (runePoints[i].text == "MAX")
            {
                RuneController.instance.stats[i] = 10;
            }
            else
            {
                RuneController.instance.stats[i] = int.Parse(runePoints[i].text);
            }
        }

        LevelManager.instance.currentRunes = runeCount;
        RuneController.instance.ApplyStats();
        RuneController.instance.ApplyStatsUI();
        CharTracker.instance.SavePlayer();

        RuneController.instance.ApplyPlayer();

        resetButton.interactable = false;

        PlayerController.instance.RoundCrits();

        Instantiate(buttonConfirm, transform.position, transform.rotation);

    }

    public void ResetRunes()
    {
        for (int i = 0; i < runePoints.Count; i++)
        {
            runePoints[i].text = Convert.ToString(RuneController.instance.stats[i]);

            if (runePoints[i].text == "10")
            {
                runePoints[i].text = "MAX";
            }
        }

        runeCount = LevelManager.instance.currentRunes;
        runes.text = "x" + Convert.ToString(runeCount);
        RuneController.instance.ApplyStatsUI();

        resetButton.interactable = false;

        Instantiate(buttonConfirm, transform.position, transform.rotation);

    }

    public void AddRuneStat(int type)
    {
        if (runePoints[type].text != "MAX")
        {
            int statNum = int.Parse(runePoints[type].text);

            if (int.Parse(runePoints[type].text) < 10)
            {
                if (runeCount >= 1)
                {
                    resetButton.interactable = true;

                    runeCount -= 1;

                    runePoints[type].text = Convert.ToString(statNum + 1);

                    if (statNum == 9)
                    {
                        runePoints[type].text = "MAX";
                    }

                    runes.text = "x" + Convert.ToString(runeCount);

                    if (type == 0)
                    {
                        if (statNum + 1 == 10)
                        {
                            runeStats[0].text = runeStatInfo[0].text + " +1";
                        }
                    }
                    if (type == 1)
                    { 
                        runeStats[1].text = runeStatInfo[1].text + " +" + Convert.ToString((0.005f * (statNum + 1)) * 100) + "%";

                    }
                    if (type == 2)
                    {                 
                        if (Convert.ToString(0.02f * (statNum + 1)).Contains("0.0999"))
                        {
                            runeStats[2].text = runeStatInfo[2].text + " +10%";
                        }
                        else
                        {
                            runeStats[2].text = runeStatInfo[2].text + " +" + Convert.ToString((0.02f * (statNum + 1)) * 100) + "%";
                        }
                    }
                    if (type == 3)
                    {
                        runeStats[3].text = runeStatInfo[3].text + " +" + Convert.ToString((0.01f * (statNum + 1)) * 100) + "%";

                        if(statNum + 1 == 10)
                        {
                            runeStats[3].text = runeStatInfo[3].text + " +10%";
                        }
                    }
                    if (type == 4)
                    {
                        runeStats[4].text = runeStatInfo[4].text + " +" + Convert.ToString(0.1f * (statNum + 1));
                    }
                    if (type == 5)
                    {
                        runeStats[5].text = runeStatInfo[5].text + " +" + Convert.ToString((0.005f * (statNum + 1)) * 100) + "%";
                    }
                    if (type == 6)
                    {
                        runeStats[6].text = runeStatInfo[6].text + " +" + Convert.ToString((0.003f * (statNum + 1)) * 100) + "%";
                    }
                }
            }
          
        }

    }

    //Load Saved Level
    public void CheckSavedLevel()
    {
        if (LevelManager.instance.isCamp && !LevelManager.instance.isCutscene)
        {
            if (LevelManager.instance.isCamp)
            {
                if (!string.IsNullOrEmpty(CharTracker.instance.savedLevel))
                {

                    ContinueWindow();
                    //Time.timeScale = 0f;
                }
                else
                {
                    
                    CharTracker.instance.ClearTemp();
                }
            }
        }
    }

    public void LoadSavedLevel()
    {
        //load
        string levelToLoad = CharTracker.instance.savedLevel;

        if (string.IsNullOrEmpty(levelToLoad))
        {
            CancelSave();
        }
        else
        {

            if (CharTracker.instance.character == 1)
            {
                SelectionManager.instance.BunnySelect();
            }
            if (CharTracker.instance.character == 2)
            {
                SelectionManager.instance.MoleSelect();
            }
            if (CharTracker.instance.character == 3)
            {
                SelectionManager.instance.MercSelect();
            }

            if (CharTracker.instance.guns.Count > 1)
            {
                if (CharTracker.instance.guns.Count > 1)
                {

                    for (int a = 0; a < PickupManager.instance.gunPickups.Count; a++)
                    {
                        if (PickupManager.instance.gunPickups[a].name == CharTracker.instance.guns[1])
                        {
                            TransferGuns(PickupManager.instance.gunPickups[a].GetComponent<GunPickup>().theGun);
                            PlayerController.instance.availableGuns[1].gameObject.SetActive(false);
                        }

                    }

                    if (CharTracker.instance.guns.Count > 2)
                    {
                        for (int a = 0; a < PickupManager.instance.gunPickups.Count; a++)
                        {
                            if (PickupManager.instance.gunPickups[a].name == CharTracker.instance.guns[2])
                            {
                                TransferGuns(PickupManager.instance.gunPickups[a].GetComponent<GunPickup>().theGun);
                                PlayerController.instance.availableGuns[2].gameObject.SetActive(false);
                            }

                        }
                    }
                    if (CharTracker.instance.guns.Count > 3)
                    {
                        for (int a = 0; a < PickupManager.instance.gunPickups.Count; a++)
                        {
                            if (PickupManager.instance.gunPickups[a].name == CharTracker.instance.guns[3])
                            {
                                TransferGuns(PickupManager.instance.gunPickups[a].GetComponent<GunPickup>().theGun);
                                PlayerController.instance.availableGuns[3].gameObject.SetActive(false);
                            }

                        }
                    }
                    if (CharTracker.instance.guns.Count > 4)
                    {
                        for (int a = 0; a < PickupManager.instance.gunPickups.Count; a++)
                        {
                            if (PickupManager.instance.gunPickups[a].name == CharTracker.instance.guns[4])
                            {
                                TransferGuns(PickupManager.instance.gunPickups[a].GetComponent<GunPickup>().theGun);
                                PlayerController.instance.availableGuns[4].gameObject.SetActive(false);
                            }

                        }
                    }
                }
            }
            continueWindow.SetActive(false);
            selectScreen.SetActive(false);
            joyStick.GetComponent<RectTransform>().anchoredPosition = new Vector3(joyStick.GetComponent<RectTransform>().anchoredPosition.x - 1000f,
                joyStick.GetComponent<RectTransform>().anchoredPosition.y, 0f);

            Invoke("LoadSaveLevel", 1f);
        }

    }

    public void LoadSaveLevel()
    {
        string levelToLoad = CharTracker.instance.savedLevel;

        ArtifactManager.instance.artifacts = CharTracker.instance.artifacts;
        LevelManager.instance.currentCoins = CharTracker.instance.savedCoins;
        PlayerController.instance.playTime = CharTracker.instance.playTime;
        PlayerController.instance.reviveCount = CharTracker.instance.revives;
        PlayerHealth.instance.maxHealth = CharTracker.instance.maxhealth;
        PlayerHealth.instance.currentHealth = CharTracker.instance.maxhealth;
        PlayerController.instance.dontSaveLvl = true;

        LevelManager.instance.savedLevel = "";

        CharTracker.instance.SaveLevel();

        ArtifactManager.instance.ApplyArtifact();

        startFadeToBlack();
        StartCoroutine(LevelManager.instance.LoadSavedLevel(levelToLoad));
        continueButton.GetComponent<Button>().enabled = false;

        CharTracker.instance.ClearTemp();
    }


    public void ContinueWindow()
    {
        continueWindow.SetActive(true);
    }
    public void CancelSave()
    {
        LevelManager.instance.nextLevel = Convert.ToString(1);
        LevelManager.instance.savedLevel = "";
        CharTracker.instance.SaveLevel();
        continueWindow.SetActive(false);
        Time.timeScale = 1f;

        CharTracker.instance.ClearTemp();
       
    }
    public void TransferGuns(Gun gun)
    {
        Gun gunClone = Instantiate(gun);
        gunClone.transform.parent = PlayerController.instance.gunArm;
        gunClone.transform.position = PlayerController.instance.gunArm.position;
        gunClone.transform.localRotation = Quaternion.Euler(Vector3.zero);
        gunClone.transform.localScale = Vector3.one;

        PlayerController.instance.availableGuns.Add(gunClone);
    }

    public void changeDynamic()
    {
        if (joyStick == joysticks[1])
        {
            joysticks[1].gameObject.SetActive(false);
            joysticks[0].gameObject.SetActive(true);
            joyStick = joysticks[0];

            dynamicButton.GetComponent<Image>().color = new Color32(85, 167, 255, 255);
            fixedButton.GetComponent<Image>().color = new Color32(255, 90, 86, 255);

            PlayerPrefs.SetInt("moveStick", 1);
        }
    }

    public void changeFixed()
    {
        if (joyStick == joysticks[0])
        {
         
            joysticks[0].gameObject.SetActive(false);
            joysticks[1].gameObject.SetActive(true);
            joyStick = joysticks[1];

            fixedButton.GetComponent<Image>().color = new Color32(85, 167, 255, 255);
            dynamicButton.GetComponent<Image>().color = new Color32(255, 90, 86, 255);

            PlayerPrefs.SetInt("moveStick", 2);
        }
    }

}


