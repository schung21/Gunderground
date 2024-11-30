using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinManager : MonoBehaviour
{

    public static SkinManager instance;

    public bool isBunny, isMole, isRaccoon, isCaptain;
    public GameObject defaultSkin;
    public Sprite defaultDeathSkin;
    public GameObject[] skins;
    public Sprite[] deathSkins;
    public List<int> skinCode;
    [SerializeField]
    public int currentSkinCode;

    //saved code
    public List<int> Bunny, Mole, Raccoon, Captain;


    [Header("Skin Sprite Reference")]
    public Sprite[] ogSkinSprite;



    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Bunny = CharTracker.instance.bunnySkin;
        Mole = CharTracker.instance.moleSkin;
        Raccoon = CharTracker.instance.raccSkin;
        Captain = CharTracker.instance.cptSkin;


        skinCode = CharTracker.instance.skinCode;

       /* if (skinCode.Count < 5)
        {
            for (int i = 4; i < 10; i++)
            {
                skinCode.Insert(i, 0);
            }
        }*/

        LoadSkins();
  
    }

    public void LoadSkins()
    {
        if (isBunny)
        {
            currentSkinCode = skinCode[0];
        }
        if (isMole)
        {
            currentSkinCode = skinCode[1];
        }
        if (isRaccoon)
        {
            currentSkinCode = skinCode[2];
        }
        if (isCaptain)
        {
            currentSkinCode = skinCode[3];
        }


        if (currentSkinCode != 0)
        {
            equipSkin(currentSkinCode);
        }
    }

    // Update is called once per frame
 /*   void Update()
    {
        
    }*/

    public void equipDefault()
    {
        foreach(var a in skins)
        {
            a.SetActive(false);
        }

        currentSkinCode = 0;
        defaultSkin.SetActive(true);
        PlayerController.instance.theBody = defaultSkin.GetComponent<SpriteRenderer>();
        PlayerController.instance.deathSprite = defaultDeathSkin;

        if (isBunny)
        {
            skinCode[0] = 0;
        }
        if (isMole)
        {
            skinCode[1] = 0;
        }
        if (isRaccoon)
        {
            skinCode[2] = 0;
        }
        if (isCaptain)
        {
            skinCode[3] = 0;
        }

        CharTracker.instance.SavePlayer();
    }

    public void equipSkin(int code)
    {
        if (code == 0)
        {
            equipDefault();
        }
        else
        {
            currentSkinCode = code;
            defaultSkin.SetActive(false);
            skins[code].SetActive(true);
            PlayerController.instance.theBody = skins[code].GetComponent<SpriteRenderer>();
            PlayerController.instance.deathSprite = deathSkins[code];

            if (isBunny)
            {
                skinCode[0] = code;
            }
            if (isMole)
            {
                skinCode[1] = code;
            }
            if (isRaccoon)
            {
                skinCode[2] = code;
            }
            if (isCaptain)
            {
                skinCode[3] = code;
            }

            //Save code
            CharTracker.instance.SavePlayer();
        }
       
    }
}
