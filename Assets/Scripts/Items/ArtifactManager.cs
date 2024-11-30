using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager instance;
    // Start is called before the first frame update

    [Header("Obtained Artifacts")]
    public bool hasBandolier;
    public bool hasSilverBlt;
    public bool hasSneakers;
    public bool hasDrones;
    public bool hasDrill;
    public bool hasKevlar;
    public bool hasGunStrap;
    public bool hasUltCD;
    public bool hasCritDmg;
    public bool hasDmg;
    public bool hasDrone;
    public bool hasFR;
    public bool hasIceStar;

    public List<int> artifacts;
    public List<int> bossArtifacts;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "0")
        {
            hasBandolier = false;
            hasDrones = false;
            hasDrill = false;
            hasKevlar = false;
            hasDrone = false;
            hasIceStar = false;

            if (hasGunStrap)
            {
                hasGunStrap = false;
                PlayerController.instance.gunsHeld -= 1;
            }
            if (hasUltCD)
            {
                hasUltCD = false;
                PlayerController.instance.ultCooldown += 2;
            }
            if (hasSneakers)
            {
                hasSneakers = false;
                PlayerController.instance.moveSpeed -= 1.5f;
            }
            if (hasSilverBlt)
            {
                hasSilverBlt = false;
                PlayerController.instance.playerCritRate -= 0.1f;
            }
            if (hasCritDmg)
            {
                hasCritDmg = false;
                PlayerController.instance.critDmg1 -= 0.2f;
                PlayerController.instance.critDmg2 -= 0.2f;
            }
            if (hasDmg)
            {
                hasDmg = false;
                PlayerController.instance.playerDamage -= 0.15f;
            }
            if(hasFR)
            {
                hasFR = false;
                PlayerController.instance.fireRate -= 0.1f;
            }
            if (PlayerController.instance.expBuff)
            {
                PlayerController.instance.expBuff = false;
                UIController.instance.expPlusText.gameObject.SetActive(false);
            }
            if (PlayerController.instance.partsBuff)
            {
                PlayerController.instance.partsBuff = false;
                UIController.instance.partsPlusText.gameObject.SetActive(false);
            }

        }
    }


    public void ApplyArtifact()
    {

        for (int i = 0; i < artifacts.Count; i++)
        {
            if (artifacts[i] == 1)
            {
                if (i == 0)
                {
                    hasBandolier = true;
                }
                if (i == 1)
                {
                    PlayerController.instance.playerCritRate += 0.1f;

                    hasSilverBlt = true;
                }
                if (i == 2)
                {
                    PlayerController.instance.activeMoveSpeed += 1.5f;
                    PlayerController.instance.moveSpeed += 1.5f;

                    hasSneakers = true;
                }
                if (i == 3)
                {

                }
                if (i == 4)
                {
                    hasDrill = true;
                }
                if (i == 5)
                {

                }
                if (i == 6)
                {
                    hasGunStrap = true;
                }
                if (i == 7)
                {
                    PlayerController.instance.ultCooldown -= 2;
                    hasUltCD = true;
                }
                if (i == 8)
                {
                    PlayerController.instance.critDmg1 += 0.2f;
                    PlayerController.instance.critDmg2 += 0.2f;

                    hasCritDmg = true;
                }
                if (i == 9)
                {
                    PlayerController.instance.playerDamage += 0.15f;

                    hasDmg = true;
                }
                if (i == 10)
                {
                    hasDrone = true;
                }
                if (i == 11)
                {
                    PlayerController.instance.partsBuff = true;

                    if (PlayerController.instance.expBuff)
                    {
                        UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition =
                        new Vector3(UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.x, UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.y - 32f, 0f);
                    }
                }
                if (i == 12)
                {
                    PlayerController.instance.expBuff = true;

                    if (PlayerController.instance.partsBuff)
                    {
                        UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition =
                        new Vector3(UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.x, UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.y - 32f, 0f);
                    }

                }
                if(i == 13)
                {
                    PlayerController.instance.fireRate += 0.1f;
                    PlayerController.instance.AddFR();

                    hasFR = true;
                }
                if(i == 14)
                {
                    hasIceStar = true;
                }
            }
        }

    }

    void Update()
    {
        if (hasBandolier) { artifacts[0] = 1; }
        if (hasSilverBlt) { artifacts[1] = 1; }
        if (hasSneakers) { artifacts[2] = 1; }
        if (hasDrill) { artifacts[4] = 1; }
        if (hasGunStrap) { artifacts[6] = 1; }
        if (hasUltCD) { artifacts[7] = 1; }
        if (hasCritDmg) { artifacts[8] = 1; }
        if (hasDmg) { artifacts[9] = 1; }
        if (hasDrone) { artifacts[10] = 1; }
        if (hasFR) { artifacts[13] = 1; }
        if (hasIceStar) { artifacts[14] = 1; }
        if (PlayerController.instance.partsBuff) { artifacts[11] = 1; }
        if (PlayerController.instance.expBuff) { artifacts[12] = 1; }
    }

 // Update is called once per frame
 } 

