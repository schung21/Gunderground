using System.Collections;
using System.Collections.Generic;
/*using UnityEditor.Localization.Plugins.XLIFF.V12;*/
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    public Image fadeScreen;
    public float fadeSpeed;
    public bool fadeToBlack, fadeOutBlack;
    public string levelToLoad;
    public bool isFade = false;
    public Text verNum;
    public string currentVer;
    public int langCode;

    public GameObject updateNotice, selectLang, cloudNotice, notLoggedIn, langButton;

    [Header("Continue Save")]
    public GameObject continueWindow;

    [Header("Audio")]
    public GameObject startSound;

    [Header("Gamepad")]
    public GameObject cursor;

    [Header("Changes Notice")]
    public GameObject changesNotice;
    public Text changes;

    private int noticeCode;

    private void Awake()
    {
        instance = this;
        currentVer = Application.version;
        Application.targetFrameRate = 60;
        Screen.SetResolution(Screen.width, Screen.height, true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        if (PlayerController.instance != null)
        {
            if (PlayerController.instance.gameObject.activeInHierarchy)
            {
                Destroy(PlayerController.instance.gameObject);
            }
        }

        fadeOutBlack = true;
        fadeToBlack = false;

        verNum.text = ("Version " + Application.version);

        if (!PlayerPrefs.HasKey("langCode"))
        {
            PlayerPrefs.SetInt("langCode", 0);
            Load();
        }
        else
        {
            Load();
        }

        if (!PlayerPrefs.HasKey("noticeCheck"))
        {
            PlayerPrefs.SetInt("noticeCheck", 0);
            noticeCode = PlayerPrefs.GetInt("noticeCheck");
        }
        else
        {
            noticeCode = PlayerPrefs.GetInt("noticeCheck");
        }


        if (!PlayerPrefs.HasKey("verNum"))
        {
            PlayerPrefs.SetString("verNum", currentVer);

        }

        Debug.Log(noticeCode + PlayerPrefs.GetString("verNum"));

    }

    public void LoadVersion()
    {
      
        if ((currentVer != PlayerPrefs.GetString("verNum")) && noticeCode == 1)
        {
            PlayerPrefs.SetString("verNum", currentVer);
            PlayerPrefs.SetInt("noticeCheck", 0);
            noticeCode = PlayerPrefs.GetInt("noticeCheck");
        }

        LoadNotice();

    }

    private void LoadNotice()
    {
        if(noticeCode == 0)
        {
            if (!string.IsNullOrEmpty(changes.text))
            {
                changesNotice.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("noticeCheck", 1);
            }
        }
    }

    public void CloseNotice()
    {
        changesNotice.SetActive(false);
        PlayerPrefs.SetInt("noticeCheck", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Gamepad.all.Count > 0 && !Gamepad.current.displayName.Contains("uinput") && !Gamepad.current.description.product.Contains("uinput"))
        {
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

        if (fadeOutBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, 0.5f * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                fadeOutBlack = false;
            }
        }

        if (fadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                fadeToBlack = false;
            }
        }

        if (fadeScreen.color.a == 0f && isFade == false)
        {
            isFade = true;
            fadeScreen.gameObject.SetActive(false);
        }
      
    
    }

    public void StartGame()
    {

        fadeScreen.gameObject.SetActive(true);
        startFadeToBlack();
        StartCoroutine(LoadScene());
        //Instantiate(startSound, transform.position, transform.rotation);
    }

    public void CloudWarning()
    {
        cloudNotice.gameObject.SetActive(true);
    }

    public void CloseWarning()
    {
        cloudNotice.gameObject.SetActive(false);
        notLoggedIn.gameObject.SetActive(false);
    }

    public void Options()
    {

    }

    public IEnumerator LoadScene()
    {

        yield return new WaitForSeconds(2f);

        if (CharTracker.instance.tutorialCode == 0)
        {
            SceneManager.LoadScene("Cutscene1");
        }
        else
        {

            SceneManager.LoadScene(levelToLoad);

        }

    }

    public void startFadeToBlack()
    {
        fadeToBlack = true;
        fadeOutBlack = false;
    }

    private void Load()
    {

        langCode = PlayerPrefs.GetInt("langCode");

        if(langCode == 1)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        }
        if (langCode == 2)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        }
        if (langCode == 3)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
        }
    }

    private void Save()
    {
        PlayerPrefs.SetInt("langCode", langCode);
    }

    public void SelectLanguage()
    {
        selectLang.SetActive(true);
    }

    public void CheckLanguage()
    {
        if(langCode == 0)
        {
            selectLang.SetActive(true);
        }
    }

    public void English()
    {
        selectLang.SetActive(false);
        langCode = 1;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
        Save();
    }
    public void Japanese()
    {
        selectLang.SetActive(false);
        langCode = 2;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
        Save();
    }
    public void Korean()
    {
        selectLang.SetActive(false);
        langCode = 3;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
        Save();
    }

    public void StorePage()
    {
        Application.OpenURL("market://details?id=com.StarKyu.Gunderground");
    }

    public void DiscordLink()
    {
        Application.OpenURL("https://discord.gg/DCzGg5pKhM");
    }

    public void OpenNotice()
    {
        changesNotice.SetActive(true);
    }





}
