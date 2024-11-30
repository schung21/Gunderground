using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class PlayerHealth : MonoBehaviour
{

    public static PlayerHealth instance;

    public int currentHealth, currentShield;
    public int maxHealth;

    public float flashTime;

    public float damageInvincLength = 1f;
/*    [HideInInspector]*/
    public float invincCount;

    public bool shieldActive, justRevived;
    //public int reviveCount;


    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Physics2D.IgnoreLayerCollision(6, 8, false);
        Physics2D.IgnoreLayerCollision(6, 9, false);

        if (SceneManager.GetActiveScene().name == "0")
        {

            maxHealth = CharTracker.instance.maxHealth += PlayerController.instance.playerHealthBonus;
            currentHealth = CharTracker.instance.currentHealth += PlayerController.instance.playerHealthBonus;
        }
        else
        {

            maxHealth = CharTracker.instance.maxHealth;
            currentHealth = CharTracker.instance.currentHealth;
        }

        //currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        PlayerController.instance.theBody.color = Color.white;


    }

    // Update is called once per frame
    void Update()
    {

        if (currentHealth < (maxHealth / 2))
        {
            UIController.instance.healthBar.color = Color.red;
        }
        else
        {
            UIController.instance.healthBar.color = Color.white;
        }

        if (invincCount > 0)
        {

            Physics2D.IgnoreLayerCollision(6, 9);
            Physics2D.IgnoreLayerCollision(6, 15);
            invincCount -= Time.deltaTime;

            if (invincCount <= 0)
            {
                if (PlayerController.instance != null)
                {
                    PlayerController.instance.theBody.color = new Color(255, 255, 255, 255);
                }



                PlayerController.instance.CollideAgain();
            
                invincCount = 0;

            }
        }

        if (currentHealth <= 0)
        {

            PlayerController.instance.canMove = false;
        }
    


    }

    public void DamagePlayer()
    {

        if (invincCount <= 0)
        {
            if (justRevived)
            {
                justRevived = false;
            }

            PlayerController.instance.theBody.material = PlayerController.instance.flashEffect;
            PlayerController.instance.redScreen.SetActive(true);
            UIController.instance.healthBar.color = Color.red;
            Invoke("Transparent", flashTime);

            if (shieldActive)
            {
                if (currentShield > 0)
                {
                    currentShield--;
                    UIController.instance.shieldSlider.value = currentShield;
                    UIController.instance.shieldText.text = currentShield.ToString();
                }
                if (currentShield <= 0)
                {
                    UIController.instance.shieldBar.gameObject.SetActive(false);
                    UIController.instance.shieldSlider.gameObject.SetActive(false);
                    UIController.instance.shieldText.gameObject.SetActive(false);
                    UIController.instance.healthText.gameObject.SetActive(true);

                    shieldActive = false;
                }
                
            }
            else if (currentHealth > 0)
            {
                currentHealth--;
            }

            invincCount = damageInvincLength;

            if (currentHealth <= 0)
            {

                if (LevelManager.instance.isRaid || LevelManager.instance.isDefense)
                {
                    if(TimerController.instance.timerGoing == true)
                    {
                        TimerController.instance.timerGoing = false;

                    }
                }
                if (LightController.instance != null)
                {
                    LightController.instance.gameObject.SetActive(false);
                }
                
                //play death animation + invoke death
                PlayerController.instance.canMove = false;
                PlayerController.instance.GetComponent<Rigidbody2D>().simulated = false;
                PlayerController.instance.gameObject.GetComponent<Animator>().enabled = false;
                PlayerController.instance.gameObject.GetComponent<CircleCollider2D>().enabled = false;

                if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].isCharged)
                {

                    PlayerController.instance.StopGunAnim();
                    PlayerController.instance.availableGuns[PlayerController.instance.currentGun].Crosshair.SetActive(false);

                }
                else
                {
                    PlayerController.instance.availableGuns[PlayerController.instance.currentGun].canShoot = false;

                    PlayerController.instance.availableGuns[PlayerController.instance.currentGun].GetComponentInChildren<SpriteRenderer>().enabled = false;
                    Gun.instance.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gun2 != null)
                    {
                        PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gun2.GetComponentInChildren<SpriteRenderer>().enabled = false;
                    }
                    PlayerController.instance.availableGuns[PlayerController.instance.currentGun].Crosshair.SetActive(false);
                }

                UIController.instance.dualGun.gameObject.SetActive(false);
                UIController.instance.grenadeButton.GetComponent<Button>().enabled = false;

                PlayerController.instance.theBody.sprite = PlayerController.instance.deathSprite;
                UIController.instance.DisableButtons();

                StartCoroutine(DeathScreen());

                if (PlayerController.instance.reviveCount > 0)
                {
                    if (UIController.instance.adsScript != null)
                    {
                       // UIController.instance.adsScript.LoadEmptyRewardedAd();
                    }
                }

            }

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        }
    }

    
    void Transparent()
    {
        PlayerController.instance.redScreen.SetActive(false);
        PlayerController.instance.theBody.material = PlayerController.instance.currentMaterial;
        PlayerController.instance.theBody.color = new Color(255, 255, 255, .5f);
        UIController.instance.healthBar.color = new Color(255, 255, 255, 1f);
    }

    public void MakeInvincible(float length)
    {
        invincCount = length;

    }

    public void HealPlayer(int healAmount)
    {

        currentHealth += healAmount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

    public void HealthUp()
    {
        maxHealth += 1;
        currentHealth += (maxHealth - currentHealth);

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();


    }

    public IEnumerator DeathScreen()
    {
        AnalyticsManager.instance.PlayerDeaths(SceneManager.GetActiveScene().name);
      
        yield return new WaitForSeconds(3f);
        PlayerController.instance.theBody.sprite = PlayerController.instance.ogSprite;
       // PlayerController.instance.gameObject.SetActive(false);

        UIController.instance.deathScreen.SetActive(true);

        if (PlayerController.instance.reviveCount == 1)
        {

            UIController.instance.reviveButtons[1].SetActive(false);
            UIController.instance.reviveButtons[2].SetActive(true);
         
        }
        if (PlayerController.instance.reviveCount == 0)
        {
            UIController.instance.reviveButtons[0].SetActive(false);
            UIController.instance.reviveButtons[1].SetActive(false);
            UIController.instance.reviveButtons[2].SetActive(false);

            LevelManager.instance.savedLevel = "";
            CharTracker.instance.SaveLevel();
            CharTracker.instance.ClearTemp();

        }

        CharTracker.instance.SavePlayer();
       
    }

    public void Revive()
    {

        //PlayerController.instance.gameObject.SetActive(true);
      
        PlayerController.instance.reviveCount -= 1;

        if (LightController.instance != null)
        {
            LightController.instance.gameObject.SetActive(true);
        }
        PlayerController.instance.theBody.sprite = PlayerController.instance.deathSprite;

        Invoke("EnablePlayer", 1f);

    }

    public void EnablePlayer()
    {
        
        UIController.instance.EnableButtons();
        currentHealth += maxHealth;
        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        PlayerController.instance.canMove = true;
        PlayerController.instance.GetComponent<Rigidbody2D>().simulated = true;

        if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].isCharged)
        {
            PlayerController.instance.StartGunAnim();
            PlayerController.instance.availableGuns[PlayerController.instance.currentGun].Crosshair.SetActive(true);
        }
        else
        {

            PlayerController.instance.availableGuns[PlayerController.instance.currentGun].GetComponentInChildren<SpriteRenderer>().enabled = true;
            Gun.instance.GetComponentInChildren<SpriteRenderer>().enabled = true;

            if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gun2 != null)
            {
                PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gun2.GetComponentInChildren<SpriteRenderer>().enabled = true;
            }

            PlayerController.instance.availableGuns[PlayerController.instance.currentGun].canShoot = true;
            PlayerController.instance.availableGuns[PlayerController.instance.currentGun].Crosshair.SetActive(true);
        }

        PlayerController.instance.gameObject.GetComponent<Animator>().enabled = true;
        PlayerController.instance.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        UIController.instance.grenadeButton.GetComponent<Button>().enabled = true;

        if (LevelManager.instance.isRaid)
        {
            if (TimerController.instance.timerGoing == false)
            {
                TimerController.instance.timerGoing = true;

            }
        }

        Invoke("Transparent", flashTime);
        MakeInvincible(4f);
        justRevived = true;

    }

    public void ChangeHealth(int hp)
    {
        if (SceneManager.GetActiveScene().name == "0")
        {
            maxHealth = 5;
            maxHealth += hp;
            currentHealth += (maxHealth - currentHealth);

            UIController.instance.healthSlider.maxValue = maxHealth;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

        }
    }
}
