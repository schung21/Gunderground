using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{

    public int healAmount = 1, ammoCount, bombCount, shieldCount;
    public float waitToBeCollected = .1f, dualLength;
    public bool isHealth, isAmmo, isDual, isUpgrade, isFR, 
                isSpeed, isCrit, isHealthUp, isShield, isMag, 
                isUltCD, isMaxGun, isCritDmg, isDmg, isDrill, isDrone, isIceStar;
    public bool isExp, isPartsBuff;
    [Header("Keys")]
    public bool isKey;
    public bool isKey1;
    public bool isKey2;
    public bool isKey3;
    public bool isKey4;

    public GameObject statUpMsg;
    public GameObject theBody;
    public GameObject upgradeEffect, healthEffect;


    [Header("Grenade")]
    public GameObject[] bombs;
    public bool isBomb;
    public int bombCode;



    [Header("Audio")]
    public GameObject ammoPickup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }

        if (upgradeEffect != null)
        {
            if (upgradeEffect.activeInHierarchy)
            {
                upgradeEffect.transform.position = PlayerController.instance.transform.position;
                upgradeEffect.transform.localScale = PlayerController.instance.transform.localScale;
            }
        }

        if (healthEffect != null)
        {
            if (healthEffect.activeInHierarchy)
            {
                healthEffect.transform.position = PlayerController.instance.transform.position;
                healthEffect.transform.localScale = PlayerController.instance.transform.localScale;
            }
        
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player" && waitToBeCollected <= 0)
        {
            //if(adventureMode)
            if (isShield)
            {
                PlayerHealth.instance.shieldActive = true;
                PlayerHealth.instance.currentShield = shieldCount;

                UIController.instance.shieldSlider.maxValue = shieldCount;
                UIController.instance.shieldSlider.value = shieldCount;
                UIController.instance.shieldText.text = shieldCount.ToString();

                UIController.instance.shieldBar.gameObject.SetActive(true);
                UIController.instance.shieldSlider.gameObject.SetActive(true);
                UIController.instance.shieldText.gameObject.SetActive(true);
                UIController.instance.healthText.gameObject.SetActive(false);

                upgradeEffect.SetActive(true);
                statUpMsg.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = false;
                theBody.SetActive(false);
                Invoke("Destroy", 2f);
            }
            if (isHealth)
            {
                healthEffect.SetActive(true);
                PlayerHealth.instance.HealPlayer(healAmount);
                GetComponent<BoxCollider2D>().enabled = false;
                theBody.SetActive(false);
                Invoke("Destroy", 2f);
            }
            if (isMag)
            {
                if (ArtifactManager.instance.hasBandolier == false)
                {
                    upgradeEffect.SetActive(true);
                    ArtifactManager.instance.hasBandolier = true;
                    ArtifactManager.instance.artifacts[0] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isMaxGun)
            {
                if (ArtifactManager.instance.hasGunStrap == false)
                {
                    upgradeEffect.SetActive(true);

                    ArtifactManager.instance.hasGunStrap = true;
                    ArtifactManager.instance.artifacts[6] = 1;

                    PlayerController.instance.gunsHeld += 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isUltCD)
            {
                if (ArtifactManager.instance.hasUltCD == false)
                {
                    upgradeEffect.SetActive(true);

                    ArtifactManager.instance.hasUltCD = true;
                    ArtifactManager.instance.artifacts[7] = 1;

                    PlayerController.instance.ultCooldown -= 2;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isAmmo)
            {
                Instantiate(ammoPickup, transform.position, transform.rotation);

                PlayerController.instance.AddAmmo(ammoCount);


                if (PlayerController.instance.bomb1 < 5)
                {
                    PlayerController.instance.bomb1 += bombCount;
                }

                /*     UIController.instance.ammo1.text = "x" + Gun.instance.Ammo.ToString();
                     //UIController.instance.ammo2.text = "x" + PlayerController.instance.ammo2.ToString();*/
                UIController.instance.bomb1.text = "x" + PlayerController.instance.bomb1.ToString();
                Destroy(gameObject);
            }

            if (isDual)
            {

                if (PlayerController.instance.isDual == true)
                {

                    PlayerController.instance.buffDuration = 30f;
                }
                else
                {
                    PlayerController.instance.isDual = true;
                }

                upgradeEffect.SetActive(true);
                statUpMsg.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = false;
                theBody.SetActive(false);
                Invoke("Destroy", 2f);

                Gun.instance.AddHealth();

            }
            if (isFR)
            {
                if (!ArtifactManager.instance.hasFR)
                {
                    PlayerController.instance.fireRate += 0.1f;
                    PlayerController.instance.AddFR();
                    upgradeEffect.SetActive(true);

                    ArtifactManager.instance.hasFR = true;
                    ArtifactManager.instance.artifacts[13] = 1;
                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }

            }
            if (isSpeed)
            {
                if (ArtifactManager.instance.hasSneakers == false)
                {
                    upgradeEffect.SetActive(true);

                    PlayerController.instance.activeMoveSpeed += 1.5f;
                    PlayerController.instance.moveSpeed += 1.5f;

                    ArtifactManager.instance.hasSneakers = true;
                    ArtifactManager.instance.artifacts[2] = 1;
                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);

                }
            }
            if (isCrit)
            {
                if (ArtifactManager.instance.hasSilverBlt == false)
                {
                    upgradeEffect.SetActive(true);

                    PlayerController.instance.playerCritRate += 0.1f;

                    ArtifactManager.instance.hasSilverBlt = true;
                    ArtifactManager.instance.artifacts[1] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);

                }
            }
            if (isHealthUp)
            {
                healthEffect.SetActive(true);

                PlayerHealth.instance.HealthUp();

                GetComponent<BoxCollider2D>().enabled = false;
                statUpMsg.SetActive(true);
                theBody.SetActive(false);
                Invoke("Disable", 2f);
            }
            if (isCritDmg)
            {
                if (ArtifactManager.instance.hasCritDmg == false)
                {
                    upgradeEffect.SetActive(true);

                    PlayerController.instance.critDmg1 += 0.2f;
                    PlayerController.instance.critDmg2 += 0.2f;

                    ArtifactManager.instance.hasCritDmg = true;
                    ArtifactManager.instance.artifacts[8] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isDmg)
            {
                if (ArtifactManager.instance.hasDmg == false)
                {
                    upgradeEffect.SetActive(true);

                    PlayerController.instance.playerDamage += 0.15f;


                    ArtifactManager.instance.hasDmg = true;
                    ArtifactManager.instance.artifacts[9] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isDrill)
            {
                if (ArtifactManager.instance.hasDrill == false && !PlayerController.instance.isMole)
                {
                    upgradeEffect.SetActive(true);

                    ArtifactManager.instance.hasDrill = true;
                    ArtifactManager.instance.artifacts[4] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isDrone)
            {
                if (ArtifactManager.instance.hasDrone == false)
                {
                    upgradeEffect.SetActive(true);

                    ArtifactManager.instance.hasDrone = true;
                    ArtifactManager.instance.artifacts[10] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isIceStar)
            {
                if(ArtifactManager.instance.hasIceStar == false)
                {
                    upgradeEffect.SetActive(true);

                    ArtifactManager.instance.hasIceStar = true;
                    ArtifactManager.instance.artifacts[14] = 1;

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isKey)
            {
                if (isKey1)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
                if (isKey2)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
                if (isKey3)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
                if (isKey4)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isExp)
            {
                if (!PlayerController.instance.expBuff)
                {
                    PlayerController.instance.expBuff = true;

                    if (PlayerController.instance.partsBuff)
                    {
                        UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition =
                        new Vector3(UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.x, UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.y - 32f, 0f);
                    }

                    UIController.instance.expPlusText.gameObject.SetActive(true);
                    ArtifactManager.instance.artifacts[12] = 1;

                    upgradeEffect.SetActive(true);

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isPartsBuff)
            {
                if (!PlayerController.instance.partsBuff)
                {
                    PlayerController.instance.partsBuff = true;

                    if (PlayerController.instance.expBuff)
                    {
                        UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition =
                        new Vector3(UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.x, UIController.instance.partsPlusText.GetComponent<RectTransform>().anchoredPosition.y - 32f, 0f);
                    }

                    UIController.instance.partsPlusText.gameObject.SetActive(true);
                    ArtifactManager.instance.artifacts[11] = 1;

                    upgradeEffect.SetActive(true);

                    GetComponent<BoxCollider2D>().enabled = false;
                    statUpMsg.SetActive(true);
                    theBody.SetActive(false);
                    Invoke("Destroy", 2f);
                }
            }
            if (isBomb)
            {
                if(bombCode == 1)
                {
                    PlayerController.instance.Grenade = bombs[0];
                }
                if (bombCode == 2)
                {
                    PlayerController.instance.Grenade = bombs[1];
                }
                if (bombCode == 3)
                {
                    PlayerController.instance.Grenade = bombs[2];
                }

                Instantiate(ammoPickup, transform.position, transform.rotation);

                GetComponent<BoxCollider2D>().enabled = false;
                statUpMsg.SetActive(true);
                theBody.SetActive(false);
                Invoke("Destroy", 2f);
            }
        }


    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Disable()
    {
        statUpMsg.SetActive(false);
    }



}
