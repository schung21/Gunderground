using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniBossController : MonoBehaviour
{
  
    public static MiniBossController instance;
    public Rigidbody2D theRB;
    public float moveSpeed;
    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvinc = .5f;

    public Transform critPoint, muzzlePoint;
    public GameObject critEffect, muzzleFlash, Shadow;
    public GameObject levelExit;

    public bool shouldWander, shouldJump, shouldRunAway, shouldChasePlayer, shouldShoot, unlimitedMove, isFireAll, isSlime, isMimic, hasGun, turnFirepoint;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public float runawayRange;

    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [HideInInspector]
    public float oldPos;
    [HideInInspector]
    public float dashCounter;
    private float dashCoolCounter;
    [HideInInspector]
    public float invincCount;

    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject bloodEffect;
    public GameObject[] Coins;
    public GameObject[] Gems;
    public GameObject bulletFire;
    //public Transform Target;
    public Transform gunArm;

    public Transform firePoint, droolPoint;
    public Transform[] firePoints;

    public float timeBetweenShots;
    private float shotCounter;

    public float rangeToShootPlayer;

    public SpriteRenderer theBody;
    public float flashTime;
    Color originColor;

    public bool visible;

    public Vector2 gunDirection;

    public int expPoints;

    [Header("Clone")]
    public bool isClone;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        originColor = theBody.color;

        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }

        InvokeRepeating("OldPosition", 0.2f, 0.2f);

        activeMoveSpeed = moveSpeed;
    }

    private void OldPosition()
    {
        oldPos = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        

        if (hasGun)
        {
            Vector2 targetPos = PlayerController.instance.transform.position;
            gunDirection = targetPos - (Vector2)transform.position;
            
            
            if (PlayerController.instance.transform.position.x > transform.position.x)
            {
                transform.localScale = Vector3.one;
                muzzleFlash.transform.localScale = Vector3.one;

                if (turnFirepoint)
                {
                   
                    firePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                    
                }
                

            }
            else if (PlayerController.instance.transform.position.x <= transform.position.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                muzzleFlash.transform.localScale = new Vector3(-1f, 1f, 1f);


                if (turnFirepoint)
                {
                    if (firePoint.transform.localRotation.z != -180f)
                    {
                        firePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);
                    }
                }
            }
            if (gunDirection != null)
            {

                gunArm.transform.up = gunDirection;
               

            }
        }

        if (theBody.isVisible)
        {
            visible = true;
        }
        else
        {
            visible = false;
        }

        if (PlayerController.instance.gameObject.activeInHierarchy)
        {

            moveDirection = Vector3.zero;

            if (unlimitedMove)
            {
                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
                {

                    moveDirection = PlayerController.instance.transform.position - transform.position;

                }
            }
            else if (unlimitedMove == false && shouldChasePlayer == true && shouldWander == false)
            {
                if (theBody.isVisible && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
                {

                    moveDirection = PlayerController.instance.transform.position - transform.position;

                }
            }

            else if (shouldChasePlayer == true && shouldWander == true)
            {
                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= rangeToChasePlayer)
                {

                    moveDirection = PlayerController.instance.transform.position - transform.position;

                }                
                else
                {
                    moveDirection = Vector3.zero;
                    shouldChasePlayer = false;
                    
                }
            }
           
            else
            {
                if (shouldWander)
                {

                    if (wanderCounter > 0)

                    {

                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;

                        if (wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }

                        if (shouldJump)
                        {

                            if (PlayerController.instance.transform.position.x >= transform.position.x)
                            {

                                if (dashCoolCounter <= 0 && dashCounter <= 0 && transform.position.x < oldPos)
                                {
                                    activeMoveSpeed = dashSpeed;
                                    dashCounter = wanderLength;
                                    //PlayerHealth.instance.invincCount = 1f;
                                    MakeInvincible(dashInvinc);

                                    Physics2D.IgnoreLayerCollision(17, 8);
                                    Physics2D.IgnoreLayerCollision(17, 7);
                                    Physics2D.IgnoreLayerCollision(17, 16);
                                    //Physics2D.IgnoreLayerCollision(17, 12);
                                    Physics2D.IgnoreLayerCollision(17, 15);


                                    anim.SetTrigger("dash2");


                                }
                                else if (dashCoolCounter <= 0 && dashCounter <= 0)
                                {
                                    activeMoveSpeed = dashSpeed;
                                    dashCounter = wanderLength;
                                    //PlayerHealth.instance.invincCount = 1f;
                                    MakeInvincible(dashInvinc);

                                    Physics2D.IgnoreLayerCollision(17, 8);
                                    Physics2D.IgnoreLayerCollision(17, 7);
                                    Physics2D.IgnoreLayerCollision(17, 16);
                                    //Physics2D.IgnoreLayerCollision(17, 12);
                                    Physics2D.IgnoreLayerCollision(17, 15);

                                    anim.SetTrigger("dash");



                                }
                            }

                            else if (PlayerController.instance.transform.position.x < transform.position.x)
                            {
                                if (dashCoolCounter <= 0 && dashCounter <= 0 && transform.position.x < oldPos)
                                {

                                    activeMoveSpeed = dashSpeed;
                                    dashCounter = wanderLength;
                                    //PlayerHealth.instance.invincCount = 1f;
                                    MakeInvincible(dashInvinc);

                                    Physics2D.IgnoreLayerCollision(17, 8);
                                    Physics2D.IgnoreLayerCollision(17, 7);
                                    Physics2D.IgnoreLayerCollision(17, 16);
                                    //Physics2D.IgnoreLayerCollision(17, 12);
                                    Physics2D.IgnoreLayerCollision(17, 15);

                                    anim.SetTrigger("dash");


                                }
                                else if (dashCoolCounter <= 0 && dashCounter <= 0)
                                {
                                    activeMoveSpeed = dashSpeed;
                                    dashCounter = wanderLength;
                                    //PlayerHealth.instance.invincCount = 1f;
                                    MakeInvincible(dashInvinc);

                                    Physics2D.IgnoreLayerCollision(17, 8);
                                    Physics2D.IgnoreLayerCollision(17, 7);
                                    Physics2D.IgnoreLayerCollision(17, 16);
                                    //Physics2D.IgnoreLayerCollision(17, 12);
                                    Physics2D.IgnoreLayerCollision(17, 15);

                                    anim.SetTrigger("dash2");


                                }
                            }

                            if (dashCounter > 0)
                            {
                                dashCounter -= Time.deltaTime;
                                if (dashCounter <= 0)
                                {
                                    Invoke("collideAgain", 0.1f);

                                    activeMoveSpeed = moveSpeed;
                                    dashCoolCounter = dashCooldown;

                                }
                            }

                            if (dashCoolCounter > 0)
                            {
                                dashCoolCounter -= Time.deltaTime;
                               
                            }

                        }
                    }

                    if (pauseCounter > 0)
                    {
                        if (theBody.isVisible && shouldShoot && 
                            Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToShootPlayer)
                        {
                            shotCounter -= Time.deltaTime;

                            if (shotCounter <= 0)
                            {
                                
                                shotCounter = timeBetweenShots;

                                Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                            }
                        }

                            StartCoroutine(stopForSec());

                            pauseCounter -= Time.deltaTime;

                            if (isFireAll)
                            {
                                rangeToShootPlayer = 0;
                            }


                            if (pauseCounter <= 0)
                            {
                                if (isFireAll)
                                {
                                    rangeToShootPlayer = 7;
                                }

                                wanderCounter = wanderLength;

                                wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                            }
                        
                    }
                }
            }
        


            if (theBody.isVisible && shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {

                moveDirection = transform.position - PlayerController.instance.transform.position;

                if (isMimic)
                {
                    shouldShoot = true;
                }

            }
            else
            {
                if (isMimic)
                {
                    shouldShoot = false;
                }
            }

            /*else
            {
                moveDirection = Vector3.zero;
            }*/

            moveDirection.Normalize();

            theRB.velocity = moveDirection * activeMoveSpeed;



            /*if (theBody.isVisible && shouldShoot && PlayerController.instance.isShooting == true && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToShootPlayer
                && Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToChasePlayer)
            {

                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    if (isFireAll)
                    {
                        shotCounter = timeBetweenShots;
                        foreach (Transform t in firePoints)
                        {
                            Instantiate(bulletFire, t.position, t.rotation);
                            anim.SetTrigger("Firing");

                        }
                    }
                    else
                    {
                        shotCounter = timeBetweenShots;
                        Instantiate(bulletFire, firePoint.position, firePoint.rotation);

                        if (hasGun)
                        {
                            Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);
                        
                        }
                    }

                }

            }*/
        }


        else
        {
            theRB.velocity = Vector2.zero;
        }

        //moving animation

        if (moveDirection != Vector3.zero)
        {
            anim.SetBool("isMoving", true);

            if (isSlime)
            {
                Instantiate(bloodEffect, droolPoint.position, droolPoint.rotation);

            }
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (invincCount > 0)
        {


            Physics2D.IgnoreLayerCollision(17, 7);
            Physics2D.IgnoreLayerCollision(17, 15);
            invincCount -= Time.deltaTime;

            if (invincCount <= 0)
            {
                theBody.color = new Color(255, 255, 255, 255);
                Physics2D.IgnoreLayerCollision(17, 7, false);
                Physics2D.IgnoreLayerCollision(17, 15, false);
            }
        }


    }

    //bullet hits enemy trigger function
    public void DamageEnemy(int damage)
    {

        if (invincCount <= 0)
        {
            if (isMimic)
            {
                StartCoroutine("RunWhenHit");
            }
            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            //int selectedSplatter2 = Random.Range(0, deathSplatters2.Length);


            theBody.color = Color.red;
            Invoke("ResetColor", flashTime);


            health -= damage;

            int rotation = Random.Range(0, 4);


            if (bloodEffect != null)
            {
                Instantiate(bloodEffect, transform.position, transform.rotation);
            }

            if (health <= 0)
            {

                if (expPoints > 0)
                {
                    //if exp boost active (expPoints*2)
                    ExpManager.instance.CollectExp(expPoints);

                    if (PlayerController.instance.expBuff)
                    {
                        if (SkinManager.instance.currentSkinCode != 0)
                        {
                            var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                            expPop.GetComponentInChildren<Text>().text = "+" + Mathf.RoundToInt(expPoints * 1.4f).ToString() + "xp";
                        }
                        else
                        {
                            var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                            expPop.GetComponentInChildren<Text>().text = "+" + Mathf.RoundToInt(expPoints * 1.3f).ToString() + "xp";
                        }
                    }
                    else
                    {
                        if (SkinManager.instance.currentSkinCode != 0)
                        {
                            var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                            expPop.GetComponentInChildren<Text>().text = "+" + Mathf.RoundToInt(expPoints * 1.1f).ToString() + "xp";
                        }
                        else
                        {
                            var expPop = Instantiate(PlayerController.instance.expText, transform.position, Quaternion.Euler(0f, 0f, 0f));
                            expPop.GetComponentInChildren<Text>().text = "+" + expPoints.ToString() + "xp";
                        }

                    }

                }

                Destroy(gameObject);

                foreach (var c in Coins)
                {
                    var randomPos = Random.insideUnitSphere * 0.5f;
                    //randomPos.y = 0;
                    Instantiate(c, transform.position + randomPos, transform.rotation);

                }

                foreach (var c in Gems)
                {

                    var randomPos = Random.insideUnitSphere * 0.5f;
                    //randomPos.y = 0;
                    Instantiate(c, transform.position + randomPos, transform.rotation);
                }

                if (deathSplatters.Length != 0)
                {

                    Instantiate(deathSplatters[selectedSplatter], transform.position, transform.rotation /*Quaternion.Euler(0f, 0f, rotation * 90)*/);

                }

              
                if (levelExit.GetComponent<LevelExit>().isCenter)
                {
                    if (Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
                    {

                        PlayerController.instance.transform.position += new Vector3(4f, 0f, 0f);


                    }
                }

                levelExit.SetActive(true);

            }
        }

    }

    void ResetColor()
    {
        theBody.color = originColor;
    }

    public void DestroyEnemy()
    {
        int selectedSplatter = Random.Range(0, deathSplatters.Length);

        int rotation = Random.Range(0, 4);

        Destroy(gameObject);
        Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
    }

    public IEnumerator RunWhenHit()
    {
        runawayRange += 5f;
        yield return new WaitForSeconds(1f);
        runawayRange -= 5f;
    }

    public void MakeInvincible(float length)
    {
        invincCount = length;

    }

    public IEnumerator stopForSec()
    {
        yield return new WaitForSeconds(0.5f);
        shouldChasePlayer = true;
    }

    public void collideAgain()
    {

        Physics2D.IgnoreLayerCollision(17, 8, false);
        Physics2D.IgnoreLayerCollision(17, 7, false);
        Physics2D.IgnoreLayerCollision(17, 16, false);
        Physics2D.IgnoreLayerCollision(17, 12, false);
        Physics2D.IgnoreLayerCollision(17, 15, false);
    }

    public void DamagePop(int number)
    {
        if (number != 0)
        {
            var randomPos = Random.insideUnitSphere * 0.5f;
            var damagePop = Instantiate(PlayerController.instance.damageUI, transform.position + randomPos, Quaternion.Euler(0f, 0f, 0f));
            damagePop.GetComponent<DamageUI>().damageNumber.text = number.ToString();
        }
    }

    public void DamagePop2(int number)
    {
        if (number != 0)
        {
            var randomPos = Random.insideUnitSphere * 0.5f;
            var damagePop = Instantiate(PlayerController.instance.damageUI, transform.position + randomPos, Quaternion.Euler(0f, 0f, 0f));
            damagePop.GetComponent<DamageUI>().critNumber.text = number.ToString();
        }
    }


    /*public IEnumerator Knockback(float knockbackDuration, float knockbackForce, Transform obj)
    {

       GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float timer = 0;

            while (knockbackDuration > timer)
            {
                timer += Time.deltaTime;
                Vector2 direction = (obj.transform.position - enemy.transform.position).normalized;
                enemy.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
            }

        }

        yield return 0;
    }*/

}


