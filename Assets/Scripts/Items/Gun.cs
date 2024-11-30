using System.Collections;
using System.Collections.Generic;
//using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public static Gun instance;

    public int costPerShot;

    public Animator anim;
    public GameObject mainGun, gun2;

    public GameObject bulletFire;
    public GameObject explosive;
    public GameObject muzzleFlash, smokeEffect, specialEffect, specialBullet;
    public Transform firePoint, firePoint2, firePoint3;
    public Transform muzzlePoint, smokePoint, smokePoint2;

    public Transform firePointDual, firePointDual2, firePointDual3;
    public Transform muzzlePointDual;

    //public float explosiveDuration;
    public float timeBetweenShots, coolDown;
 
    public float shotCounter;
    public float maxFR;
    public bool doSpin = false;
    public bool infiniteAmmo;
    public bool isSpread, isRapidFire, canShoot, isPenetrate, isExplode, isRifle, isCharged, isRevolver, isLasting, isRapidCharged;
    public bool disableGun;

    public Sprite gunUI;

    public string weaponName;

    public int counter = 0;
    public int maxCounter, stacksCounter, maxStacks;
    public float bulletHealth, initBulletHealth, chargeTime;

    public bool isDual, isHoming, dualDelay, smallDualDelay, isStacks, isAddFR;

    public Color newColor;

    [HideInInspector]
    public bool isAgainstWall;

    [HideInInspector]
    public float ogFRValue;
    //public bool tapShoot;

    public GameObject Crosshair;

    private float timeNotFiring;
    private float notFiringCounter = 0;

    [Header("Audio")]
    public bool playSound;
    public GameObject audio1, audio2, audio3;

    [Header("Sub Gun")]
    public SpriteRenderer subBody;
    public SpriteRenderer subBody2;
    public SpriteRenderer subBody3;
    public GameObject subGun;
    public bool isSub, isSecondary;

    [Header("Animation")]
    public bool initialAnim;

    //for rapid fire guns
    [HideInInspector]
    public bool rapidFire;
    private float preBufftbs;


    private void Awake()
    {
        preBufftbs = timeBetweenShots;
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (gun2 != null)
        {
            gun2.SetActive(false);
        }

        if (isRapidFire)
        {
            counter = 0;
            anim.SetBool("isSpin", false);
            canShoot = true;

        }

        if (isAddFR)
        {
            ogFRValue = timeBetweenShots;
        }

        timeNotFiring = 1.5f;

        isAgainstWall = false;
       
        rapidFire = false;
    }

    // Update is called once per frame
    void Update()
    {
      
        if (!isSecondary)
        {
            if (FireButton.instance.pressed && PlayerController.instance.Ammo[PlayerController.instance.currentGun] < costPerShot && canShoot)
            {
                PlayerController.instance.noBullet.SetActive(true);

                if (isRapidFire)
                {
                    rapidFire = false;
                    PlayerController.instance.gunCounter = 0;
                }
            }
            else
            {
                //PlayerController.instance.noBullet.SetActive(false);
            }
        }
     

        if (initialAnim)
        {
            if (FireButton.instance.pressed && disableGun)
            {
                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo)
                {
                    anim.SetTrigger("PreFire");
                }
              
            }
            else
            {
                anim.ResetTrigger("PreFire");
            }
        }
       
        if (isRapidCharged)
        {
            if (PlayerController.instance.isMole)
            {
                if (PlayerController.instance.activeMoveSpeed == PlayerController.instance.dashSpeed)
                {
                    canShoot = false;
                   
                }
               
            }
        }

        if (isHoming)
        {
            if (!PlayerController.instance.isGamepad)
            {
                float angle = Mathf.Atan2(UIController.instance.joyStick2.Direction.y, UIController.instance.joyStick2.Direction.x) * Mathf.Rad2Deg;
                firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
            }
            else if (PlayerController.instance.isGamepad)
            {
                if (Mathf.Abs(PlayerController.instance.aimVector.x) > 0.5f || Mathf.Abs(PlayerController.instance.aimVector.y) > 0.5f)
                {
                    float angle = Mathf.Atan2(PlayerController.instance.aimVector.y, PlayerController.instance.aimVector.x) * Mathf.Rad2Deg;
                    firePoint.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
                }
            }

        }

        if (isDual)
        {
            if (gun2 != null)
            {
                gun2.SetActive(true); 
            }
        }

        if (SceneManager.GetActiveScene().name != "0" && SceneManager.GetActiveScene().name != "Tutorial" && SceneManager.GetActiveScene().name != "Title Scene")
        {
            if (PlayerController.instance.availableGuns[PlayerController.instance.currentGun].infiniteAmmo)
            {
                UIController.instance.Infinite.gameObject.SetActive(true);
      /*          UIController.instance.bigAmmo.gameObject.SetActive(false);*/
                UIController.instance.ammo1.text = "";
             /*   UIController.instance.ammo1.text = "";*/
            }
            else
            {
                UIController.instance.Infinite.gameObject.SetActive(false);
               /* UIController.instance.bigAmmo.gameObject.SetActive(true);*/
                UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
              /*  UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();*/
            }
        }

        if (mainGun != null)
        {


            if ((mainGun.GetComponent<Gun>().shotCounter > 0) && (mainGun.GetComponent<Gun>().shotCounter < mainGun.GetComponent<Gun>().timeBetweenShots))
            {
                if (shotCounter > 0)
                {
                    anim.SetBool("isFiring", false);
                    shotCounter -= Time.deltaTime;

                }
                else
                {

                    FireSubWeapon();



                    if (!FireButton.instance.pressed)
                    {
                        anim.SetBool("isFiring", false);
                    }
                }
            }
            
           
        }
        else
        {
            if (isRapidFire)
            {

                if (shotCounter > 0)
                {

                    shotCounter -= Time.deltaTime;
                    //canShoot = false;
                    anim.SetBool("isFiring", false);


                    if (PlayerController.instance.gunCounter == maxCounter)
                    {
                        /*UIController.instance.switchGunButton.GetComponent<SwitchButton>().enabled = false;
                        UIController.instance.Disabled.gameObject.SetActive(true);*/
                        rapidFire = false;
                        canShoot = false;
                        PlayerController.instance.gunCounter = 0;
                        StartCoroutine("delayFire");

                    }
                }

                else

                {
                    if (PlayerController.instance.canMove && !disableGun)
                    {

                        if (isRevolver)
                        {
                            if (/*Input.GetMouseButtonDown(0)*/FireButton.instance.pressed == true)
                            {
                                rapidFire = true;
                                /*PlayerController.instance.gunCounter++;

                                *//*anim.SetTrigger("Fire");*//*
                                anim.SetBool("isFiring", true);
                                Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                if (gun2 != null && gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                }

                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }


                                shotCounter = timeBetweenShots;*/


                            }

                            if (rapidFire && (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo)
                                && canShoot == true && gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("Still")) 
                            {
                                if (PlayerController.instance.gunCounter <= maxCounter)
                                {
                                    PlayerController.instance.gunCounter++;

                                    /*anim.SetTrigger("Fire");*/
                                    anim.SetBool("isFiring", true);
                                    Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                    Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                    if (gun2 != null && gun2.activeInHierarchy == true)
                                    {

                                        if (!isHoming)
                                        {
                                            if (dualDelay)
                                            {
                                                Invoke("SecondFire", 0.5f);
                                            }
                                            else
                                            {
                                                Invoke("SecondFire", 0.05f);
                                            }
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }


                                    }

                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                    if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                    {
                                        UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                    }
                                    if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                    {
                                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                        PlayerController.instance.noBullet.SetActive(true);

                                    }


                                    shotCounter = timeBetweenShots;
                                }
                            }

                        }
                        else if (isRifle)
                        {
                            if (/*Input.GetMouseButtonDown(0)*/FireButton.instance.pressed == true)
                            {
                                rapidFire = true;
                                /*PlayerController.instance.gunCounter++;

                                *//*anim.SetTrigger("Fire");*//*
                                anim.SetBool("isFiring", true);
                                Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                if (gun2 != null && gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                }

                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }


                                shotCounter = timeBetweenShots;*/


                            }

                            if (/*Input.GetMouseButtonDown(0)*/rapidFire && canShoot &&
                                 (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo))
                            {
                                //InvokeRepeating("RapidFire", 0.1f, 1f);
                                if (PlayerController.instance.gunCounter <= maxCounter)
                                {
                                    PlayerController.instance.gunCounter++;

                                    anim.SetTrigger("Fire");
                                    anim.SetBool("isFiring", true);

                                    if (isLasting)
                                    {
                                        Instantiate(bulletFire, firePoint.transform);
                                        Instantiate(muzzleFlash, muzzlePoint.transform);
                                    }
                                    else
                                    {
                                        Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                        Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                    }
                                    if (smokeEffect != null)
                                    {
                                        Instantiate(smokeEffect, smokePoint.position, Quaternion.Euler(0f, 0f, 0f));
                                        if (smokePoint2.gameObject.activeInHierarchy)
                                        {
                                            if (!dualDelay)
                                            {
                                                Instantiate(smokeEffect, smokePoint2.position, Quaternion.Euler(0f, 0f, 0f));
                                            }
                                        }
                                    }
                                    if (gun2 != null && gun2.activeInHierarchy == true)
                                    {

                                        if (!isHoming)
                                        {
                                            if (dualDelay)
                                            {
                                                Invoke("SecondFire", 0.5f);
                                            }
                                            else if (smallDualDelay)
                                            {
                                                Invoke("SecondFire", 0.15f);
                                            }
                                            else
                                            {
                                                Invoke("SecondFire", 0.05f);
                                            }
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }


                                    }

                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                    if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                    {
                                        UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                    }
                                    if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                    {
                                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                    }

                                    shotCounter = timeBetweenShots;


                                }
                            }
                        }
                    }
                }
            }


            /* if (Input.GetMouseButton(0))
             {
                 if (infiniteAmmo || PlayerController.instance.ammo >= costPerShot)
                 {
                     //fire a bullet everytime shotcounter reaches 0 or less
                     shotCounter -= Time.deltaTime;

                     if (shotCounter <= 0)
                     {
                         anim.SetBool("isFiring", true);

                         Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                         Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                         if (gun2.activeInHierarchy == true)
                         {
                             Instantiate(muzzleFlash, muzzlePointDual.position, muzzlePointDual.rotation);
                             Instantiate(bulletFire, firePointDual.position, firePoint.rotation);

                         }

                         shotCounter = timeBetweenShots;
                         PlayerController.instance.ammo -= costPerShot;
                         UIController.instance.ammo.text = "x" + PlayerController.instance.ammo.ToString();
                         if (PlayerController.instance.ammo <= 0)
                         {
                             PlayerController.instance.ammo = 0;

                         }
                     }
                     else
                     {
                         anim.SetBool("isFiring", false);

                     }
                 }
             }*/
            else if (isCharged)
            {
                if (PlayerController.instance.canMove)
                {

                    if (/*(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))*/ FireButton.instance.pressed == true)
                    {
                        notFiringCounter = timeNotFiring;

                        if (!isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot) && !PlayerController.instance.gunDisabled)
                        {
                            anim.SetBool("isFiring", true);

                            {

                                //InvokeRepeating("ReleaseCharge", 1f, 1f);
                                StartCoroutine("ReleaseCharge");

                            }

                        }
                        if (isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot) && !PlayerController.instance.gunDisabled)
                        {

                            anim.SetBool("isFiring", true);

                            {
                                if (playSound)
                                {
                                    audio1.SetActive(true);
                                    audio2.SetActive(false);
                                    audio3.SetActive(false);
                                }
                                //InvokeRepeating("ReleaseCharge", 1f, 1f);
                                StartCoroutine("ReleaseCharge");
                                
                            }
                        }
                    }
                    else if (FireButton.instance.pressed == false && notFiringCounter > 0)
                    {

                        notFiringCounter -= Time.deltaTime;

                        if (!isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot) && !PlayerController.instance.gunDisabled)
                        {
                            anim.SetBool("isFiring", true);

                            {

                                //InvokeRepeating("ReleaseCharge", 1f, 1f);
                                StartCoroutine("ReleaseCharge");

                            }

                        }
                        if (isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot) && !PlayerController.instance.gunDisabled)
                        {
                            if (playSound)
                            {
                                audio1.SetActive(false);
                                audio2.SetActive(true);
                                audio3.SetActive(false);
                            }

                            anim.SetBool("isFiring", true);

                            {

                                //InvokeRepeating("ReleaseCharge", 1f, 1f);
                                StartCoroutine("ReleaseCharge");

                            }
                        }
                    }

                    else
                    {

                        if (notFiringCounter <= 0)
                        {
                            ResetCharge();

                            if (playSound)
                            {
                                audio1.SetActive(false);
                                audio2.SetActive(false);
                                audio3.SetActive(true);
                            }
                        }
                      
                    }
                }


            }

            else if (!isRapidFire || !isCharged) //should be &&
            {
                if (PlayerController.instance.canMove)
                {
                    //anim.SetBool("isSpin", false);
                 

                    if (shotCounter > 0)
                    {
                        anim.SetBool("isFiring", false);
                        shotCounter -= Time.deltaTime;

                    }
                    else
                    {

                        if (/*(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))*/ FireButton.instance.pressed == true)
                        {
                            if (isPenetrate || isExplode)
                            {
                                if (bulletHealth < initBulletHealth)
                                {
                                    bulletHealth = initBulletHealth;

                                }

                                //Invoke("BulletRecharge", 0.5f);
                            }
                            if (!isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot))
                            {


                                //fire a bullet everytime shotcounter reaches 0 or less
                                //shotCounter -= Time.deltaTime;

                                if (isSpread)
                                {

                                    Instantiate(bulletFire, firePoint2.position, firePoint2.rotation);
                                    Instantiate(bulletFire, firePoint3.position, firePoint3.rotation);

                                }

                                if (isLasting)
                                {
                                    Instantiate(bulletFire, firePoint.transform);
                                    Instantiate(muzzleFlash, muzzlePoint.transform);
                                }
                                else
                                {
                                    Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                    Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                }


                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;

                             
                                anim.SetBool("isFiring", true);
                                


                                if (doSpin)
                                {
                                    anim.SetBool("isSpin", true);

                                }
                                else if (doSpin == false)
                                {
                                    
                                    anim.SetBool("isSpin", false);
                                }



                                if (gun2 != null && gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                    if (isSpread)
                                    {

                                        Instantiate(bulletFire, firePointDual2.position, firePointDual2.rotation);
                                        Instantiate(bulletFire, firePointDual3.position, firePointDual3.rotation);
                                    }
                                }

                                shotCounter = timeBetweenShots;

                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }

                            }
                            if (isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot))
                            {
                                if (playSound)
                                {
                                    audio1.SetActive(true);
                                }
                                if (isAddFR)
                                {
                                    AddFR();
                                }
                                //fire a bullet everytime shotcounter reaches 0 or less
                                //shotCounter -= Time.deltaTime;

                                if (isStacks)
                                {
                                    stacksCounter += 1;

                                    if (stacksCounter == maxStacks)
                                    {
                                        stacksCounter = 0;
                                        Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                        Instantiate(specialBullet, firePoint.position, firePoint.rotation);

                                    }
                                    else
                                    {
                                        Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                        Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                    }

                                    if (stacksCounter == maxStacks - 1)
                                    {
                                        if (muzzlePoint.childCount != 0)
                                        {
                                            muzzlePoint.GetComponentInChildren<SpriteRenderer>().color = newColor;
                                        }
                                    }
                                    else
                                    {
                                        if (muzzlePoint.childCount != 0)
                                        {
                                            muzzlePoint.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                                        }
                                    }
                                }
                                else
                                {
                                  
                                        Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                        Instantiate(bulletFire, firePoint.position, firePoint.rotation);

                                }

                                if (smokeEffect != null)
                                {
                                    Instantiate(smokeEffect, smokePoint.position, Quaternion.Euler(0f, 0f, 0f));
                                    if (smokePoint2.gameObject.activeInHierarchy)
                                    {
                                        if (!dualDelay)
                                        {
                                            Instantiate(smokeEffect, smokePoint2.position, Quaternion.Euler(0f, 0f, 0f));
                                        }
                                    }
                                }

                                if (isSpread)
                                {

                                    Instantiate(bulletFire, firePoint2.position, firePoint2.rotation);
                                    Instantiate(bulletFire, firePoint3.position, firePoint3.rotation);

                                }


                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;

                                anim.SetBool("isFiring", true);

                      
                                if (doSpin)
                                {
                                    anim.SetBool("isSpin", true);

                                }
                                else if (doSpin == false)
                                {
                                    anim.SetBool("isSpin", false);
                                }



                                if (gun2 != null && gun2.activeInHierarchy == true)
                                {
                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }

                                    if (isSpread)
                                    {

                                        Instantiate(bulletFire, firePointDual2.position, firePointDual2.rotation);
                                        Instantiate(bulletFire, firePointDual3.position, firePointDual3.rotation);
                                    }
                                }

                                shotCounter = timeBetweenShots;

                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }

                            }
                        }

                        else
                        {
                            anim.SetBool("isFiring", false);

                            if (playSound)
                            {
                                audio1.SetActive(false);
                            }

                            if (isAddFR)
                            {

                                ResetFR();
                            }
                        }
                    }
                }
            }
        }
     }



    /* public IEnumerator RapidShoot()
     {

         yield return new WaitForSeconds(coolDown);
         if (anim.GetBool("isSpin") == false)
         {
             anim.SetBool("isSpin", true);
         }
         yield return new WaitForSeconds(0.1f);
         anim.SetBool("isSpin", false);
         counter = 0;
         canShoot = true;

     }*/



    public void SecondFire()
    {
        if (isLasting)
        {
            Instantiate(bulletFire, firePointDual.transform);
            Instantiate(muzzleFlash, muzzlePointDual.transform);
        }
        else
        {
            Instantiate(bulletFire, firePointDual.position, firePoint.rotation);
            Instantiate(muzzleFlash, muzzlePointDual.position, muzzlePointDual.rotation);

            if (smokeEffect != null)
            {
                if (smokePoint2 != null)
                {
                    if (smokePoint2.gameObject.activeInHierarchy)
                    {
                        Instantiate(smokeEffect, smokePoint2.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }
            
        }
    }

    /*    public void MinigunFire()
        {


        }*/

    public void BulletRecharge()
    {
        if (bulletHealth < initBulletHealth)
        {
            bulletHealth = initBulletHealth;
        }
    }

    public IEnumerator resetCount()
    {
        yield return new WaitForSeconds(0.1f);
        counter = 0;

    }

    public void stopShoot()
    {
        canShoot = false;
    }

    public IEnumerator ReleaseCharge()
    {
        yield return new WaitForSeconds(chargeTime);

        if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] < costPerShot)
        {
            if (playSound)
            {
                audio1.SetActive(false);
                audio2.SetActive(false);
                audio3.SetActive(true);
            }
            anim.SetBool("isFiring", false);
            StopCoroutine("ReleaseCharge");
        }

        if (isRapidCharged)
        {

            if (shotCounter > 0)
            {

                shotCounter -= Time.deltaTime;
                //canShoot = false;

                if (PlayerController.instance.gunCounter == maxCounter)
                {
                    /*UIController.instance.switchGunButton.GetComponent<SwitchButton>().enabled = false;
                    UIController.instance.Disabled.gameObject.SetActive(true);*/
                    canShoot = false;
                    rapidFire = false;
                    PlayerController.instance.gunCounter = 0;
                    StartCoroutine("delayFire");

                }

            }

            else

            {
               
                if (PlayerController.instance.canMove)
                {
                 
                    if (isRevolver)
                    {
                        if (/*Input.GetMouseButtonDown(0)*/FireButton.instance.pressed == true)
                        {
                            rapidFire = true;

                        }

                        if (/*Input.GetMouseButtonDown(0)*/rapidFire && canShoot == true &&
                             (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo) && gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("Still"))
                        {
                            if (PlayerController.instance.gunCounter <= maxCounter)
                            {

                                PlayerController.instance.gunCounter++;

                                /*anim.SetTrigger("Fire");*/
                                anim.SetBool("isFiring", true);
                                Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                if (gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                }

                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }


                                shotCounter = timeBetweenShots;


                            }
                        }
                    }
                    else if (isRifle)
                    {
                        if (/*Input.GetMouseButtonDown(0)*/FireButton.instance.pressed == true)
                        {
                            rapidFire = true;

                        }
                        if (/*Input.GetMouseButtonDown(0)*/rapidFire && canShoot &&
                             (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo))
                        {
                            //InvokeRepeating("RapidFire", 0.1f, 1f);

                            if (PlayerController.instance.gunCounter <= maxCounter)
                            {
                                PlayerController.instance.gunCounter++;
                                anim.SetTrigger("Fire");
                                if (isLasting)
                                {
                                    Instantiate(bulletFire, firePoint.transform);
                                    Instantiate(muzzleFlash, muzzlePoint.transform);
                                }
                                else
                                {
                                    Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                    Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                }
                                if (smokeEffect != null)
                                {
                                    Instantiate(smokeEffect, smokePoint.position, Quaternion.Euler(0f, 0f, 0f));
                                    if (smokePoint2.gameObject.activeInHierarchy)
                                    {
                                        if (!dualDelay)
                                        {
                                            Instantiate(smokeEffect, smokePoint2.position, Quaternion.Euler(0f, 0f, 0f));
                                        }
                                    }
                                }
                                if (gun2 != null && gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {

                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                }

                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }

                                shotCounter = timeBetweenShots;


                            }
                        }
                    }
                }
            }
        }
        else
        {

            if (shotCounter > 0)
            {

                shotCounter -= Time.deltaTime;

                if (timeBetweenShots > 2)
                {
                    if (shotCounter < 2)
                    {
                        anim.SetBool("isFiring2", false);
                    }
                }
            }
            else
            {

                if (/*(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))*/ FireButton.instance.pressed == true && !PlayerController.instance.gunDisabled)
                {

                    if (isStacks)
                    {
                        stacksCounter += 1;

                        if (stacksCounter == maxStacks)
                        {
                            stacksCounter = 0;
                            Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                            Instantiate(specialBullet, firePoint.position, firePoint.rotation);

                        }
                        else
                        {
                            Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                            Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                        }

                        if (stacksCounter == maxStacks - 1)
                        {
                            if (muzzlePoint.childCount != 0)
                            {
                                muzzlePoint.GetComponentInChildren<SpriteRenderer>().color = newColor;
                            }
                        }
                        else
                        {
                            if (muzzlePoint.childCount != 0)
                            {
                                muzzlePoint.GetComponentInChildren<SpriteRenderer>().color = Color.white;
                            }
                        }
                    }
                    else
                    {

                        Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                        Instantiate(bulletFire, firePoint.position, firePoint.rotation);

                    }

                    if (isSpread)
                    {

                        Instantiate(bulletFire, firePoint2.position, firePoint2.rotation);
                        Instantiate(bulletFire, firePoint3.position, firePoint3.rotation);

                    }


                    if (isRifle)
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                    }
                    if (!isRifle)
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                    }

                    if (timeBetweenShots > 2)
                    {
                        anim.SetBool("isFiring2", true);
                    }

                    anim.SetBool("isFiring", true);





                    if (doSpin)
                    {
                        anim.SetBool("isSpin", true);

                    }
                    else if (doSpin == false)
                    {
                        anim.SetBool("isSpin", false);
                    }



                    if (gun2 != null && gun2.activeInHierarchy == true)
                    {

                        Invoke("SecondFire", 0.03f);

                        if (isSpread)
                        {

                            Instantiate(bulletFire, firePointDual2.position, firePointDual2.rotation);
                            Instantiate(bulletFire, firePointDual3.position, firePointDual3.rotation);
                        }
                    }

                    shotCounter = timeBetweenShots;

                    if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                    {
                        if (isRifle)
                        {
                            UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                        }
                        if (!isRifle)
                        {
                            UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                        }
                    }
                    if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                    }
                    if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                    {
                        PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                    }
                }
            }
        }
    }

    public IEnumerator delayFire()
    {

        anim.SetBool("isSpin", true);

        yield return new WaitForSeconds(coolDown);

        /*UIController.instance.switchGunButton.GetComponent<SwitchButton>().enabled = true;
        UIController.instance.Disabled.gameObject.SetActive(false);*/
        canShoot = true;
        anim.SetBool("isSpin", false);

    }

    public void effectFixed()
    {

        Instantiate(specialEffect, muzzlePoint.transform);

    }

    public void StopCharge()
    {
        shotCounter = 0;
    
    }


    public void FireSubWeapon()
    {
        if (mainGun != null)
        {
            if (isRapidFire)
            {

                if (shotCounter > 0)
                {

                    shotCounter -= Time.deltaTime;
                    //canShoot = false;
                    anim.SetBool("isFiring", false);


                    if (PlayerController.instance.gunCounter == maxCounter)
                    {
                        /*UIController.instance.switchGunButton.GetComponent<SwitchButton>().enabled = false;
                        UIController.instance.Disabled.gameObject.SetActive(true);*/
                        canShoot = false;
                        PlayerController.instance.gunCounter = 0;
                        StartCoroutine("delayFire");

                    }

                }

                else

                {
                    if (PlayerController.instance.canMove)
                    {

                        if (isRevolver)
                        {
                            if (/*Input.GetMouseButtonDown(0)*/FireButton.instance.pressed == true && canShoot == true &&
                                 (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo) && gameObject.GetComponent<SpriteRenderer>().sprite.name.Contains("Still"))
                            {

                                PlayerController.instance.gunCounter++;

                                /*anim.SetTrigger("Fire");*/
                                anim.SetBool("isFiring", true);
                                Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                                if (gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                }

                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }


                                shotCounter = timeBetweenShots;


                            }
                        }
                        else if (isRifle)
                        {
                            if (/*Input.GetMouseButtonDown(0)*/FireButton.instance.pressed == true && canShoot &&
                                 (PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot || infiniteAmmo))
                            {
                                //InvokeRepeating("RapidFire", 0.1f, 1f);

                                PlayerController.instance.gunCounter++;

                                anim.SetTrigger("Fire");
                                Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);


                                if (gun2.activeInHierarchy == true)
                                {

                                    if (!isHoming)
                                    {
                                        if (dualDelay)
                                        {
                                            Invoke("SecondFire", 0.5f);
                                        }
                                        else
                                        {
                                            Invoke("SecondFire", 0.05f);
                                        }
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }


                                }

                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;
                                if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                                {
                                    UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                                }
                                if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                                {
                                    PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                                }

                                shotCounter = timeBetweenShots;


                            }
                        }
                    }
                }
            }


            else if (isCharged)
            {
                if (PlayerController.instance.canMove)
                {


                    if (/*(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))*/ FireButton.instance.pressed == true)
                    {

                        if (!isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot))
                        {
                            anim.SetBool("isFiring", true);

                            {

                                //InvokeRepeating("ReleaseCharge", 1f, 1f);
                                StartCoroutine("ReleaseCharge");

                            }

                        }
                        if (isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot))
                        {

                            anim.SetBool("isFiring", true);

                            {

                                //InvokeRepeating("ReleaseCharge", 1f, 1f);
                                StartCoroutine("ReleaseCharge");

                            }
                        }
                    }


                    else
                    {
                        StopCoroutine("ReleaseCharge");
                        anim.SetBool("isFiring", false);

                    }
                }


            }

            else if (!isRapidFire || !isCharged) //should be &&
            {
                if (PlayerController.instance.canMove)
                {
                    //anim.SetBool("isSpin", false);

                    /* if (shotCounter > 0)
                     {
                         anim.SetBool("isFiring", false);
                         shotCounter -= Time.deltaTime;

                     }
                     else
                     {*/

                    if (/*(Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))*/ FireButton.instance.pressed == true)
                    {
                        if (isPenetrate || isExplode)
                        {
                            if (bulletHealth < initBulletHealth)
                            {
                                bulletHealth = initBulletHealth;
                            }

                            //Invoke("BulletRecharge", 0.5f);
                        }
                        if (!isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot))
                        {


                            //fire a bullet everytime shotcounter reaches 0 or less
                            //shotCounter -= Time.deltaTime;


                            Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                            Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                            if (isSpread)
                            {

                                Instantiate(bulletFire, firePoint2.position, firePoint2.rotation);
                                Instantiate(bulletFire, firePoint3.position, firePoint3.rotation);

                            }


                            PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;

                            anim.SetBool("isFiring", true);


                            if (doSpin)
                            {
                                anim.SetBool("isSpin", true);

                            }
                            else if (doSpin == false)
                            {
                                anim.SetBool("isSpin", false);
                            }



                            if (gun2 != null && gun2.activeInHierarchy == true)
                            {

                                if (!isHoming)
                                {
                                    if (dualDelay)
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.05f);
                                    }
                                }
                                else
                                {
                                    Invoke("SecondFire", 0.5f);
                                }


                                if (isSpread)
                                {

                                    Instantiate(bulletFire, firePointDual2.position, firePointDual2.rotation);
                                    Instantiate(bulletFire, firePointDual3.position, firePointDual3.rotation);
                                }
                            }

                            shotCounter = timeBetweenShots;

                            if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                            {
                                UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                            }
                            if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                            {
                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                            }

                        }
                        if (isRifle && (infiniteAmmo || PlayerController.instance.Ammo[PlayerController.instance.currentGun] >= costPerShot))
                        {

                            //fire a bullet everytime shotcounter reaches 0 or less
                            //shotCounter -= Time.deltaTime;


                            Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                            Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                            if (smokeEffect != null)
                            {
                                Instantiate(smokeEffect, smokePoint.position, Quaternion.Euler(0f, 0f, 0f));
                                if (smokePoint2.gameObject.activeInHierarchy)
                                {
                                    if (!dualDelay)
                                    {
                                        Instantiate(smokeEffect, smokePoint2.position, Quaternion.Euler(0f, 0f, 0f));
                                    }
                                }
                            }

                            if (isSpread)
                            {

                                Instantiate(bulletFire, firePoint2.position, firePoint2.rotation);
                                Instantiate(bulletFire, firePoint3.position, firePoint3.rotation);

                            }


                            PlayerController.instance.Ammo[PlayerController.instance.currentGun] -= costPerShot;

                            anim.SetBool("isFiring", true);


                            if (doSpin)
                            {
                                anim.SetBool("isSpin", true);

                            }
                            else if (doSpin == false)
                            {
                                anim.SetBool("isSpin", false);
                            }



                            if (gun2 != null && gun2.activeInHierarchy == true)
                            {
                                if (!isHoming)
                                {
                                    if (dualDelay)
                                    {
                                        Invoke("SecondFire", 0.5f);
                                    }
                                    else
                                    {
                                        Invoke("SecondFire", 0.05f);
                                    }
                                }
                                else
                                {
                                    Invoke("SecondFire", 0.5f);
                                }

                                if (isSpread)
                                {

                                    Instantiate(bulletFire, firePointDual2.position, firePointDual2.rotation);
                                    Instantiate(bulletFire, firePointDual3.position, firePointDual3.rotation);
                                }
                            }

                            shotCounter = timeBetweenShots;

                            if (UIController.instance.Infinite.gameObject.activeInHierarchy == false)
                            {
                                UIController.instance.ammo1.text = "x" + PlayerController.instance.Ammo[PlayerController.instance.currentGun].ToString();
                            }
                            if (PlayerController.instance.Ammo[PlayerController.instance.currentGun] <= 0)
                            {
                                PlayerController.instance.Ammo[PlayerController.instance.currentGun] = 0;
                            }

                        }
                    }

                    else
                    {
                        anim.SetBool("isFiring", false);
                    }
                }
            }
        }
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
           
            isAgainstWall = true;
            
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            isAgainstWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            isAgainstWall = false;
        }
    }
    public void AddFR()
    {
        
        if (timeBetweenShots >= maxFR)
        {
            timeBetweenShots -= 0.002f;
     
        }
    }
    public void ResetFR()
    {
        timeBetweenShots = ogFRValue;
        
    }

    public void WallBool()
    {
        isAgainstWall = false;
    }

    public void ResetCharge()
    {
        StopCoroutine("ReleaseCharge");
        anim.SetBool("isFiring", false);

        if (timeBetweenShots > 2)
        {
            anim.SetBool("isFiring2", false);
            shotCounter = 0;
        }
    }

    public void AddHealth()
    {
        if (isPenetrate)
        {
            if (PlayerController.instance.isDual)
            {
                initBulletHealth *= 2;

                if (bulletHealth < initBulletHealth)
                {
                    bulletHealth = initBulletHealth;

                }
            }
        }
    }

    public void ResetHealth()
    {
        if (isPenetrate)
        {

            initBulletHealth /= 2;

            if (bulletHealth < initBulletHealth)
            {
                bulletHealth = initBulletHealth;

            }

        }
       
    }

    public void playSound1()
    {
        Instantiate(audio1, transform.position, transform.rotation);
    }
    public void playSound2()
    {
        Instantiate(audio2, transform.position, transform.rotation);
    }

    public void DisableGun()
    {
        if (disableGun)
        {
            disableGun = false;
        }
        else if(!disableGun)
        {
            disableGun = true;
        }
    }

    public void BuffFR()
    {
        if (PlayerController.instance.fireRate > 0)
        {
            if (!isRapidFire && !isCharged && !isRapidCharged && !isLasting)
            {
                float fireRate = PlayerController.instance.fireRate;
                float finalRate = preBufftbs - (preBufftbs * fireRate);

                if (timeBetweenShots > finalRate)
                {
                    timeBetweenShots = finalRate;
                }
            }
        }
        
    }
    public void FireGun()
    {

    }


}









