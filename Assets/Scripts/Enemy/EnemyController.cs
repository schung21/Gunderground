using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Pathfinding;
using NUnit.Framework;
public class EnemyController : MonoBehaviour
{
    
    public static EnemyController instance;
    public Rigidbody2D theRB;
    public float moveSpeed;

    public Transform critPoint, muzzlePoint;
    public GameObject critEffect, dizzyEffect, Shadow;
  
    public bool shouldWander, shouldJump, shouldRunAway, shouldChasePlayer, shouldShoot, shouldChaseShoot, shouldTurn, 
        unlimitedMove, isSlime, isMimic, isFloat, isBomb, isAppear;
    public bool isSplitFire, isFireAll, hasGun, specialAtk, stunImmune, global;
  
    public float rangeToChasePlayer;
    private Vector3 moveDirection;
  
    public float runawayRange;

    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    public Animator anim;

    public int health = 150;

    public GameObject[] deathSplatters;
    public GameObject[] specialEffects;
    public GameObject bloodEffect, initialEffect, afterEffect, flameEffect, iceEffect;
    public bool fireOnDeath;
    public GameObject[] Coins;
    public GameObject[] Gems;
    public GameObject Bullet, noAimBullet, shootEffect;

    public Transform firePoint, droolPoint;
    public Transform[] firePoints;
    public Transform[] firePoints2;
    public Transform gunArm, gunArm2;
    public Transform effectOnGun;

    public float timeBetweenShots;
    private float shotCounter;

    public float rangeToShootPlayer;

    public SpriteRenderer theBody, theBody2;
    public SpriteRenderer[] bodyParts;
    public float flashTime;
    Color originColor;

    public bool visible, canMove, isObject, isTwoObjects;

    private Vector2 gunDirection;

    public int expPoints;

    public bool canSee, decoyVisible, hasWeapon, shootWhenPause, isBossSummon;

    //[HideInInspector]
    public bool noAimDecoy, stunned;

    public GameObject decoys;

    [Header("Stop Animation")]
    public bool stopAnim;

    private float stunDuration, stunTimer, stunCooldown;
    private bool startStun;

    [Header("Raid Elite")]
    public bool raidElite;

    [Header("Tailed")]
    public bool isTailed;
    public Transform tailSet;


    // Start is called before the first frame update
    void Start()
    {
     
        instance = this;
        originColor = theBody.color;

        if (shouldWander)
        {
            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
        }

        canMove = true;

        decoyVisible = true;
        noAimDecoy = false;

        if (GetComponent<AIPath>() != null)
        {
            GetComponent<AIPath>().maxSpeed = moveSpeed;
        }

        if (!visible)
        {
            GetComponent<Rigidbody2D>().simulated = false;
        }
    }
  
    // Update is called once per frame
    void Update()
    {
        if (startStun)
        {
            if (stunDuration != 0)
            {
                if (stunTimer > 0)
                {
                    stunTimer -= Time.deltaTime;
                    stunCooldown = stunTimer;

                    canMove = false;
                    dizzyEffect.SetActive(true);

                    if (stunCooldown <= 0)
                    {
                        canMove = true;
                        stunned = false;
                        if (dizzyEffect != null)
                        {
                            dizzyEffect.SetActive(false);
                        }
                        startStun = false;
                        
                    }
                }

            }
            else if(stunDuration == 0 && stunCooldown > 0)
            {
                stunCooldown -= Time.deltaTime;

                if (stunCooldown <= 0)
                {
                    canMove = true;
                    stunned = false;
                    if (dizzyEffect != null)
                    {
                        dizzyEffect.SetActive(false);
                    }
                    startStun = false;

                }
            }
            else if(stunDuration == 0 && stunned)
            {
                canMove = true;
                stunned = false;
                if (dizzyEffect != null)
                {
                    dizzyEffect.SetActive(false);
                }
                startStun = false;
            }
 
        }

        if (isTailed)
        {
            if(tailSet != null)
            {
                if (tailSet.childCount != GetComponentInParent<SnakeManager>().snakeBody.Count - 1)
                {
                    for (int i = 1; GetComponentInParent<SnakeManager>().snakeBody.Count > i; i++)
                    {
                        GetComponentInParent<SnakeManager>().snakeBody[i].transform.SetParent(tailSet, true);
                    }
                }
            }
        }

    
        if (GetComponent<HomingProjectile>() != null)
        {
        
            flameEffect.transform.rotation = transform.rotation;
            iceEffect.transform.rotation = Quaternion.Euler(0f, 0f, 360f);

            if (stunned)
            {
                GetComponent<HomingProjectile>().enabled = false;
            }
            else
            {
                GetComponent<HomingProjectile>().enabled = true;
            }
        }
        if(decoys == null && decoyVisible == false)
        {
            decoyVisible = true;
            noAimDecoy = false;
        }

        if (shouldShoot)
        {
            if (UIController.instance.fadeOutBlack)
            {
                shotCounter = 2;
            }
        }

        if(shouldChasePlayer || shouldChaseShoot)
        {
         
            if (GetComponent<AIDestinationSetter>() != null)
            {
                if (!UIController.instance.fadeOutBlack)
                {
                    if (decoys != null)
                    {
                        GetComponent<AIDestinationSetter>().target = decoys.transform;
                    }
                    else
                    {
                        GetComponent<AIDestinationSetter>().target = PlayerController.instance.transform;
                    }


                    if (shouldChasePlayer)
                    {
                        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToChasePlayer)
                        {
                            GetComponent<AIPath>().maxSpeed = 0;
                        }
                        else
                        {

                            if (stunned || PlayerHealth.instance.currentHealth <= 0)
                            {
                                GetComponent<AIPath>().maxSpeed = 0;
                            }
                            else if (!stunned)
                            {
                                GetComponent<AIPath>().maxSpeed = moveSpeed;
                            }

                        }
                    }
                    if (shouldChaseShoot)
                    {
                        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToChasePlayer && visible)
                        {
                            if (stunned || PlayerHealth.instance.currentHealth <= 0)
                            {
                                GetComponent<AIPath>().maxSpeed = 0;
                            }
                            else if (!stunned)
                            {
                                GetComponent<AIPath>().maxSpeed = moveSpeed;
                            }
                        }
                        else
                        {
                            GetComponent<AIPath>().maxSpeed = 0;

                        }
                    }

                }
               
            }

        }
     
        if (shouldTurn && health > 0 && !stunned)
        {
            if (decoys != null)
            {
   
                if (decoys.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);

                    if(gunArm2 != null)
                    {
                        gunArm2.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
                else if (decoys.transform.position.x <= transform.position.x)
                {
                    transform.localScale = Vector3.one;

                    if (gunArm2 != null)
                    {
                        gunArm2.localScale = new Vector3(1f, -1f, 1f);
                    }
                }
            }
            else
            {
             
                if (PlayerController.instance.transform.position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);

                    if (gunArm2 != null)
                    {
                        gunArm2.localScale = new Vector3(1f, 1f, 1f);
                    }
                }
                else if (PlayerController.instance.transform.position.x <= transform.position.x)
                {
                    transform.localScale = Vector3.one;

                    if (gunArm2 != null)
                    {
                        gunArm2.localScale = new Vector3(1f, -1f, 1f);
                    }

                }
            }
        }

        if (isAppear)
        {
            canMove = false;
            visible = false;

            if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= 7f)
            {
                if(specialEffects.Length != 0)
                {
                    specialEffects[0].SetActive(true);
                    specialEffects[1].SetActive(true);
                    specialEffects[2].SetActive(true);
                    specialEffects[3].SetActive(true);
                }
                Instantiate(initialEffect, transform.position, transform.rotation);
                theBody.gameObject.SetActive(true);
                GetComponent<BoxCollider2D>().enabled = true;
               
                Shadow.SetActive(true);

                canMove = true;
                visible = true;
                isAppear = false;
            }
        }

        if (hasGun && health > 0)
        {
            if (!stunned)
            {
                if (decoys != null && decoyVisible == true)
                {
                    Vector2 targetPos = decoys.transform.position;
                    gunDirection = targetPos - (Vector2)transform.position;
                   
                    if (Vector3.Distance(transform.position, decoys.transform.position) > 4f)
                    {
                        if (gunArm.gameObject.GetComponent<LineOfSight>().canSee == false)
                        {
                            decoyVisible = false;
                            noAimDecoy = true;

                        }
                        else
                        {
                          
                            if (decoys.transform.position.x < transform.position.x)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;

                                if (effectOnGun != null)
                                {
                                    effectOnGun.localScale = Vector3.one;
                                }

                            }
                            else if (decoys.transform.position.x >= transform.position.x)
                            {
                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, 1f, 1f);

                                if (effectOnGun != null)
                                {
                                    effectOnGun.localScale = new Vector3(1f, -1f, 1f);
                                }


                            }
                            if (gunDirection != null)
                            {

                                gunArm.transform.up = gunDirection;

                            }
                        }
                       
                    }
                    else
                    {

                        if (decoys.transform.position.x < transform.position.x)
                        {

                            transform.localScale = Vector3.one;
                            gunArm.localScale = Vector3.one;

                            if (effectOnGun != null)
                            {
                                effectOnGun.localScale = Vector3.one;
                            }
                         

                        }
                        else if (decoys.transform.position.x >= transform.position.x)
                        {
                            transform.localScale = new Vector3(-1f, 1f, 1f);
                            gunArm.localScale = new Vector3(-1f, 1f, 1f);

                            if (effectOnGun != null)
                            {
                                effectOnGun.localScale = new Vector3(1f, -1f, 1f);
                            }
    
                        }
                        if (gunDirection != null)
                        {

                            gunArm.transform.up = gunDirection;

                        }
                    }
                }
                else
                {

                    Vector2 targetPos = PlayerController.instance.transform.position;
                    gunDirection = targetPos - (Vector2)transform.position;

                    if (PlayerController.instance.transform.position.x < transform.position.x)
                    {

                        transform.localScale = Vector3.one;
                        gunArm.localScale = Vector3.one;

                        if (effectOnGun != null)
                        {
                            effectOnGun.localScale = Vector3.one;
                        }
                     
                    }
                    else if (PlayerController.instance.transform.position.x >= transform.position.x)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        gunArm.localScale = new Vector3(-1f, 1f, 1f);

                        if (effectOnGun != null)
                        {
                            effectOnGun.localScale = new Vector3(1f, -1f, 1f);
                        }
                     

                    }
                    if (gunDirection != null)
                    {

                        gunArm.transform.up = gunDirection;

                    }

                    if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 4f)
                    {
                        if (gunArm.gameObject.GetComponent<LineOfSight>().canSee == false)
                        {
                            canMove = false;
                         
                            moveDirection = Vector3.zero;
                            anim.SetBool("isMoving", false);
                        }
                        else
                        {
                            canMove = true;
                           
                        }
                    }
                    else
                    {
                        canMove = true;
                     
                    }
                }
            }

            if (isSplitFire)
            {
                if (transform.localScale.x == 1f)
                {
                    firePoint.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (transform.localScale.x == -1f)
                {
                    firePoint.localScale = new Vector3(-1f, 1f, 1f);
                }
            
            }
        }

        if (LevelManager.instance.isRaid == true)
        {
            if (unlimitedMove == false && !global)
            {
                unlimitedMove = true;
            }

            if (shouldChasePlayer)
            {
                if (rangeToChasePlayer < 80)
                {
                    rangeToChasePlayer = 80;
                }
            }
        }

        if (!global)
        {
            if (theBody.isVisible)
            {

                visible = true;
                GetComponent<Rigidbody2D>().simulated = true;
            }
            else
            {
                visible = false;
            }
        }
        else 
        {
            visible = true;
            GetComponent<Rigidbody2D>().simulated = true;
        }

        if (PlayerController.instance.gameObject.activeInHierarchy && canMove == true && PlayerHealth.instance.currentHealth > 0)
        {
            moveDirection = Vector3.zero;


            if (shouldChasePlayer && GetComponent<AIDestinationSetter>() == null)
            {
                if (unlimitedMove)
                {
                    if (decoys != null)
                    {

                        if (Vector3.Distance(transform.position, decoys.transform.position) < rangeToChasePlayer && shouldChasePlayer)
                        {

                            if (Vector3.Distance(transform.position, decoys.transform.position) > 0.5f)
                            {
                                moveDirection = decoys.transform.position - transform.position;
                            }

                        }
                        else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
                        {

                            moveDirection = PlayerController.instance.transform.position - transform.position;

                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer && shouldChasePlayer)
                        {

                            moveDirection = PlayerController.instance.transform.position - transform.position;

                        }
                    }
                    
                }
                else if (unlimitedMove == false)
                {
                    if (decoys != null && decoyVisible == true)
                    {
                        if (Vector3.Distance(transform.position, decoys.transform.position) < rangeToChasePlayer)
                        {

                            if (Vector3.Distance(transform.position, decoys.transform.position) > 0.5f)
                            {

                                moveDirection = decoys.transform.position - transform.position;

                            }
                           
                        }
                        else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
                        {

                            moveDirection = PlayerController.instance.transform.position - transform.position;

                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
                        {

                            moveDirection = PlayerController.instance.transform.position - transform.position;

                        }
                    }
                    
                }
            }

            else if (shouldChaseShoot)
            {
                if (unlimitedMove)
                {
                    if (decoys != null)
                    {
                        if (Vector3.Distance(transform.position, decoys.transform.position) >= rangeToChasePlayer)
                        {

                            moveDirection = decoys.transform.position - transform.position;

                        }
                        else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= rangeToChasePlayer)
                        {

                            moveDirection = PlayerController.instance.transform.position - transform.position;

                        }
                        else
                        {
                            moveDirection = Vector3.zero;

                            shotCounter -= Time.deltaTime;

                            if (shotCounter <= 0)
                            {
                                shotCounter = timeBetweenShots;
                                anim.SetTrigger("Firing");

                                if (noAimDecoy)
                                {
                                    var bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                    bullet.gameObject.GetComponent<EnemyBullet>().direction = PlayerController.instance.transform.position - transform.position;
                                }
                                else
                                {
                                    Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                }
                                
                            }
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= rangeToChasePlayer)
                        {

                            moveDirection = PlayerController.instance.transform.position - transform.position;

                        }
                        else
                        {
                            moveDirection = Vector3.zero;

                            shotCounter -= Time.deltaTime;

                            if (shotCounter <= 0)
                            {
                                shotCounter = timeBetweenShots;
                                Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                anim.SetTrigger("Firing");
                            }
                        }
                    }
                }
                else 
                {
                    if (visible == true)
                    {
                        if (decoys != null && decoyVisible == true)
                        {

                            if (Vector3.Distance(transform.position, decoys.transform.position) >= rangeToChasePlayer)
                            {

                                moveDirection = decoys.transform.position - transform.position;
                                shotCounter -= Time.deltaTime;
                            }
                            else
                            {
                                if (Vector3.Distance(transform.position, decoys.transform.position) < rangeToShootPlayer)
                                {
                                    moveDirection = Vector3.zero;

                                    shotCounter -= Time.deltaTime;

                                    if (shotCounter <= 0)
                                    {

                                        if (!specialAtk)
                                        {
                                            shotCounter = timeBetweenShots;
                                            anim.SetTrigger("Firing");

                                            if (noAimDecoy)
                                            {
                                                var bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                                bullet.gameObject.GetComponent<EnemyBullet>().aimDecoy = false;
                                            }
                                            else
                                            {
                                                var bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                                bullet.gameObject.GetComponent<EnemyBullet>().aimDecoy = true;
                                                bullet.gameObject.GetComponent<EnemyBullet>().allyDecoy = decoys;
                                            }

                                        }
                                        else
                                        {
                                            shotCounter = timeBetweenShots;
                                            anim.SetTrigger("Firing");

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= rangeToChasePlayer)
                            {

                                moveDirection = PlayerController.instance.transform.position - transform.position;
                                shotCounter -= Time.deltaTime;
                            }
                            else
                            {
                                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToShootPlayer)
                                {
                                    moveDirection = Vector3.zero;

                                    shotCounter -= Time.deltaTime;

                                    if (shotCounter <= 0)
                                    {
                                        if (!specialAtk)
                                        {
                                            shotCounter = timeBetweenShots;
                                            Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                            anim.SetTrigger("Firing");
                                        }
                                        else
                                        {
                                            shotCounter = timeBetweenShots;
                                            anim.SetTrigger("Firing");

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (shouldWander && visible == true)
                {
                    if (wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;
                        anim.SetBool("isMoving", true);

                        if (wanderCounter <= 0)
                        {
                            pauseCounter = Random.Range(pauseLength * .75f, pauseLength * 1.25f);
                        }
                    }

                    if (pauseCounter > 0)
                    {
                        pauseCounter -= Time.deltaTime;
                        anim.SetBool("isMoving", false);

                        if (isFireAll || hasGun)
                        {
                            rangeToShootPlayer = 0;
                        }

                        if (shootWhenPause)
                        {
                            rangeToShootPlayer = 15;
                        }


                        if (pauseCounter <= 0)
                        {
                            if (isFireAll || hasGun)
                            {

                                if (hasWeapon)
                                {
                                    rangeToShootPlayer = 15;
                                }
                                else
                                {
                                    rangeToShootPlayer = 10;
                                }
                            }

                            wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

                        }
                    }
                }
            }

            if(decoys != null && decoyVisible == true)
            {
                if (visible == true && shouldRunAway && Vector3.Distance(transform.position, decoys.transform.position) < runawayRange)
                {

                    moveDirection = transform.position - decoys.transform.position;

                    if (isMimic)
                    {
                        shouldShoot = true;
                    }
                    if (hasWeapon)
                    {
                        shouldShoot = false;

                        if (shouldWander)
                        {
                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }
                    }

                }
                else
                {
                    if (isMimic)
                    {
                        shouldShoot = false;
                    }
                    if (hasWeapon)
                    {
                        shouldShoot = true;
                    }
                }
            }
            else if (visible == true && shouldRunAway && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < runawayRange)
            {

                moveDirection = transform.position - PlayerController.instance.transform.position;

                if (isMimic)
                {
                    shouldShoot = true;
                }
                if (hasWeapon)
                {
                    shouldShoot = false;

                    if (shouldWander)
                    {
                        wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                    }
        
                }
       

            }
            else
            {
                if (isMimic)
                {
                    shouldShoot = false;
                }
                if (hasWeapon)
                {

                    shouldShoot = true;

                }
               
            }




            /*else
            {
                moveDirection = Vector3.zero;
            }*/

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;

            if (decoys != null && decoyVisible == true)
            {
                if (visible == true && shouldShoot && PlayerController.instance.isShooting == true && Vector3.Distance(transform.position, decoys.transform.position) < rangeToShootPlayer
                    && Vector3.Distance(transform.position, decoys.transform.position) > rangeToChasePlayer &&
                    Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToChasePlayer)
                {

                    shotCounter -= Time.deltaTime;

                    if (shotCounter <= 0)
                    {
                        if (isFireAll)
                        {
                            shotCounter = timeBetweenShots;
                            foreach (Transform t in firePoints)
                            {
                                Instantiate(Bullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");

                            }
                        }
                        else if (isSplitFire)
                        {
                            if (noAimDecoy)
                            {
                                Vector2 fireDirection = (Vector2)PlayerController.instance.transform.position - (Vector2)firePoint.transform.position;
                                fireDirection.Normalize();
                                /* Vector2 targetPos = PlayerController.instance.transform.position;
                                 Vector2 fireDirection = targetPos - (Vector2)transform.position;*/
                                firePoint.transform.up = fireDirection;
                            }
                            else
                            {
                                Vector2 fireDirection = (Vector2)decoys.transform.position - (Vector2)firePoint.transform.position;
                                fireDirection.Normalize();
                                /* Vector2 targetPos = PlayerController.instance.transform.position;
                                 Vector2 fireDirection = targetPos - (Vector2)transform.position;*/
                                firePoint.transform.up = fireDirection;
                            }


                            shotCounter = timeBetweenShots;
                            foreach (Transform t in firePoints)
                            {
      
                                anim.SetTrigger("Firing");

                                if (noAimDecoy)
                                {
                                    var bullet = Instantiate(Bullet, t.position, t.rotation);
                                    bullet.gameObject.GetComponent<EnemyBullet>().aimDecoy = false;
                                }
                                else
                                {
                                    var bullet = Instantiate(Bullet, t.position, t.rotation);
                                    bullet.gameObject.GetComponent<EnemyBullet>().aimDecoy = true;
                                    bullet.gameObject.GetComponent<EnemyBullet>().allyDecoy = decoys;
                                }
                            }

                            foreach (Transform t in firePoints2)
                            {
                                Instantiate(noAimBullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");
                            }


                        }
                        else
                        {
                            shotCounter = timeBetweenShots;
                            anim.SetTrigger("Firing");

                            if (shootEffect != null)
                            {
                                Instantiate(shootEffect, muzzlePoint.position, muzzlePoint.rotation);
                            }

                            if (noAimDecoy)
                            {
                                var bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
                                bullet.gameObject.GetComponent<EnemyBullet>().aimDecoy = false;
                            }
                            else
                            {
                                var bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);

                                if (bullet.gameObject.GetComponent<EnemyBullet>() != null)
                                {
                                    bullet.gameObject.GetComponent<EnemyBullet>().aimDecoy = true;
                                    bullet.gameObject.GetComponent<EnemyBullet>().allyDecoy = decoys;

                                }

                            }

                        }

                    }

                }
                else if (visible == true && shouldShoot && !hasWeapon && PlayerController.instance.isShooting == true && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToShootPlayer
                    && Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToChasePlayer &&
                    Vector3.Distance(transform.position, decoys.transform.position) > rangeToShootPlayer)
                {

                    shotCounter -= Time.deltaTime;

                    if (shotCounter <= 0)
                    {
                        if (isFireAll)
                        {
                            shotCounter = timeBetweenShots;
                            foreach (Transform t in firePoints)
                            {
                                Instantiate(Bullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");

                            }
                        }
                        else if (isSplitFire)
                        {
                            Vector2 fireDirection = (Vector2)PlayerController.instance.transform.position - (Vector2)firePoint.transform.position;
                            fireDirection.Normalize();
                            /* Vector2 targetPos = PlayerController.instance.transform.position;
                             Vector2 fireDirection = targetPos - (Vector2)transform.position;*/
                            firePoint.transform.up = fireDirection;


                            shotCounter = timeBetweenShots;
                            foreach (Transform t in firePoints)
                            {
                                Instantiate(Bullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");
                            }

                            foreach (Transform t in firePoints2)
                            {
                                Instantiate(noAimBullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");
                            }


                        }
                        else
                        {
                            shotCounter = timeBetweenShots;
                            Instantiate(Bullet, firePoint.position, firePoint.rotation);
                            anim.SetTrigger("Firing");

                            if (shootEffect != null)
                            {
                                Instantiate(shootEffect, muzzlePoint.position, muzzlePoint.rotation);
                            }
                        }

                    }

                }
            }
            else
            {
                if (visible == true && shouldShoot && PlayerController.instance.isShooting == true && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToShootPlayer
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
                                Instantiate(Bullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");

                            }
                        }
                        else if (isSplitFire)
                        {
                            Vector2 fireDirection = (Vector2)PlayerController.instance.transform.position - (Vector2)firePoint.transform.position;
                            fireDirection.Normalize();
                            /* Vector2 targetPos = PlayerController.instance.transform.position;
                             Vector2 fireDirection = targetPos - (Vector2)transform.position;*/
                            firePoint.transform.up = fireDirection;


                            shotCounter = timeBetweenShots;
                            foreach (Transform t in firePoints)
                            {
                                Instantiate(Bullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");
                            }

                            foreach (Transform t in firePoints2)
                            {
                                Instantiate(noAimBullet, t.position, t.rotation);
                                anim.SetTrigger("Firing");
                            }


                        }
                        else
                        {
                            shotCounter = timeBetweenShots;
                            Instantiate(Bullet, firePoint.position, firePoint.rotation);
                            anim.SetTrigger("Firing");

                            if (shootEffect != null)
                            {
                                Instantiate(shootEffect, muzzlePoint.position, muzzlePoint.rotation);
                            }
                        }

                    }

                }

            }

        }

        else
        {
            theRB.velocity = Vector2.zero;
        }




        //moving animation

        if (moveDirection != Vector3.zero)
        {

            anim.SetBool("isMoving", true);
            

            if (isSlime && health > 0)
            {
                if (Time.timeScale != 0f)
                {
                    Instantiate(bloodEffect, droolPoint.position, droolPoint.rotation);
                }

            }
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (flameEffect != null && flameEffect.activeInHierarchy)
        {

            if (flameEffect.gameObject.GetComponent<TriggerCC>().Duration <= 0)
            {
                //if cc artifact buff exists = 6
                flameEffect.gameObject.GetComponent<TriggerCC>().Duration = 4;
                flameEffect.gameObject.SetActive(false);
            }
            if (health <= 0)
            {
                flameEffect.gameObject.SetActive(false);
            }
        }


        if (iceEffect != null && iceEffect.activeInHierarchy)
        {
            stunned = true;

            GetComponent<Animator>().enabled = false;

            if (specialAtk && theBody2 != null)
            {
                theBody2.enabled = false;
            }

            if (iceEffect.gameObject.GetComponent<TriggerCC>().Duration <= 0)
            {
                //if cc artifact buff exists = 6
                iceEffect.gameObject.GetComponent<TriggerCC>().Duration = 4;
                canMove = true;  
                stunned = false;
                iceEffect.gameObject.SetActive(false);

                GetComponent<Animator>().enabled = true;


                if (specialAtk && theBody2 != null)
                {
                    theBody2.enabled = true;
                }

            }
            if (health <= 0)
            {
                iceEffect.gameObject.SetActive(false);
            }
        }

        if (health <= 0)
        {
            dizzyEffect.SetActive(false);
        }

        if (isBossSummon)
        {
            if (BossController.instance != null)
            {
                if (BossController.instance.currentHealth <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
/*
        if(specialAtk && stunned)
        {
            GetComponent<Animator>().enabled = false;

            if(theBody2 != null)
            {
                theBody2.enabled = false;
            }
        }
        if(specialAtk && !stunned)
        {
            GetComponent<Animator>().enabled = true;


            if (theBody2 != null)
            {
                theBody2.enabled = true;
            }
        }*/
    }


    //bullet hits enemy trigger function
    public void DamageEnemy(int damage)
    {

        if (shouldChasePlayer)
        {
            StartCoroutine("ChaseWhenHit");
        }
        if (isMimic)
        {
            StartCoroutine("RunWhenHit");
        }
        int selectedSplatter = Random.Range(0, deathSplatters.Length);
        //int selectedSplatter2 = Random.Range(0, deathSplatters2.Length);

        if (damage > 0)
        {
            theBody.color = Color.red;

            if (isTailed)
            {
                foreach(var a in GetComponentInChildren<EnemyBodyPart>().bodies)
                {
                    a.color = Color.red;
                }
               
            }
        }
        if (isTwoObjects)
        {
            if (damage > 0)
            {
                theBody2.color = Color.red;
            }
        }

        Invoke("ResetColor", flashTime);



        health -= damage;

        int rotation = Random.Range(0, 4);


        if (bloodEffect != null)
        {

            Instantiate(bloodEffect, transform.position, transform.rotation);

        }


        if (health <= 0)
        {
            if (LevelManager.instance.isRaid || LevelManager.instance.isDefense)
            {
                if (TimerController.instance.timeValue > 1)
                {
                    if (raidElite)
                    {
                        LevelManager.instance.AddCombo(5);
                    }
                    else
                    {
                        LevelManager.instance.AddCombo(1);
                    }
                }
            }

            if (stopAnim)
            {
                GetComponent<Animator>().enabled = false;
                moveSpeed = 0;
            }


            if (fireOnDeath)
            {
                FireBullet();
            }

            canMove = false;
            theBody.gameObject.SetActive(false);
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            dizzyEffect.SetActive(false);
            Shadow.SetActive(false);

            GetComponent<Animator>().enabled = false;

            if (isTwoObjects)
            {
                theBody2.gameObject.SetActive(false);
            }

            if (gunArm != null)
            {
                gunArm.gameObject.SetActive(false);
            }

            if (GetComponent<RotateCircle>() != null)
            {
                GetComponent<RotateCircle>().enabled = false;
            }

            if (!LevelManager.instance.isRaid)
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
            }

            iceEffect.gameObject.SetActive(false);
            flameEffect.gameObject.SetActive(false);


            if (afterEffect != null)
            {
                Instantiate(afterEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

            }


            if (deathSplatters.Length != 0)
            {
                if (isSlime)
                {
                    Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
                }
                if (isBomb)
                {
                    Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
                else
                {
                    if (deathSplatters[selectedSplatter] != null)
                    {
                        deathSplatters[selectedSplatter].SetActive(true);
                    }

                }

            }

            Invoke("Destroy", 1f);

        }

    }


    void ResetColor()
    {
        theBody.color = originColor;

        if (isTwoObjects)
        {
            theBody2.color = originColor;
        }

        if (isTailed)
        {
            foreach (var a in GetComponentInChildren<EnemyBodyPart>().bodies)
            {
                a.color = originColor;
            }
        }
    }

    public void DestroyEnemy()
    {
        //int selectedSplatter = Random.Range(0, deathSplatters.Length);

        int rotation = Random.Range(0, 4);

        Destroy(gameObject);
        Instantiate(deathSplatters[0], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
      
    }

    public IEnumerator RunWhenHit()
    {
        runawayRange += 5f;
        yield return new WaitForSeconds(1f);
        runawayRange -= 5f;
    }
    public IEnumerator ChaseWhenHit()
    {
        rangeToChasePlayer += 10f;
        yield return new WaitForSeconds(2f);
        rangeToChasePlayer -= 10f;
    }

    public void Stunned(float duration)
    {

        stunDuration = duration;
        stunTimer = stunDuration;

        if (!isObject)
        {
            if (!stunImmune)
            {
                startStun = true;
                //EndStun();
                stunned = true;

            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Overlap Box"))
        {
            Destroy(gameObject);
        }
    }
    
    public void EndStun()
    {
        startStun = true;
        /*if (stunDuration != 0)
        {
            canMove = false;

            dizzyEffect.SetActive(true);

            yield return new WaitForSeconds(stunDuration);

            canMove = true;
            stunned = false;
            if (dizzyEffect != null)
            {
                dizzyEffect.SetActive(false);
            }
        }*/
    }


    public IEnumerator Freeze()
    {
        moveSpeed /= 2;

        yield return new WaitForSeconds(2f);

        moveSpeed *= 2;
    }


    public IEnumerator stopForSec()
    {
        yield return new WaitForSeconds(0.5f);
        shouldChaseShoot = true;

    }

    public void FireBullet()
    {
        if (isFireAll)
        {
            foreach (Transform t in firePoints)
            {
                Instantiate(Bullet, t.position, t.rotation);

            }
        }
    }

    public void DoSpecialAtk()
    {
        if (health > 0)
        {
            GameObject bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), bullet.GetComponent<CircleCollider2D>());
            
        }
    }

    public void DoChildSpecialAtk()
    {
        if (health > 0)
        {
            GameObject bullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
            bullet.GetComponent<DamagePlayer>().parentEnemy = gameObject;
            bullet.GetComponent<DamagePlayer>().followEnemy = true;
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), bullet.GetComponent<BoxCollider2D>());

        }
    }

    public void Destroy()
    {
      
        Destroy(gameObject);
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


    public void InvincOn()
    {
        GetComponent<BoxCollider2D>().enabled = false;
    }

    public void InvincOff()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void SelfDestruct()
    {
        DamageEnemy(100000);
    }

    public void DamageEnemyNoBlood(int damage)
    {

        if (isMimic)
        {
            StartCoroutine("RunWhenHit");
        }
        int selectedSplatter = Random.Range(0, deathSplatters.Length);
        //int selectedSplatter2 = Random.Range(0, deathSplatters2.Length);

        if (damage > 0)
        {
            theBody.color = Color.red;
        }
        Invoke("ResetColor", flashTime);


        health -= damage;

        int rotation = Random.Range(0, 4);


        if (health <= 0)
        {
            if (LevelManager.instance.isRaid || LevelManager.instance.isDefense)
            {
                if (TimerController.instance.timeValue > 1)
                {
                    if (raidElite)
                    {
                        LevelManager.instance.AddCombo(5);
                    }
                    else
                    {
                        LevelManager.instance.AddCombo(1);
                    }
                }
            }

            if (fireOnDeath)
            {
                FireBullet();
            }

            canMove = false;
            theBody.gameObject.SetActive(false);
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            dizzyEffect.SetActive(false);
            Shadow.SetActive(false);
            iceEffect.gameObject.SetActive(false);
            flameEffect.gameObject.SetActive(false);

            if (gunArm != null)
            {
                gunArm.gameObject.SetActive(false);
            }

            if (GetComponent<RotateCircle>() != null)
            {
                GetComponent<RotateCircle>().enabled = false;
            }

            if (!LevelManager.instance.isRaid)
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
            }

            if (afterEffect != null)
            {
                Instantiate(afterEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

            }


            if (deathSplatters.Length != 0)
            {
                if (isSlime)
                {
                    Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, rotation * 90));
                }
                if (isBomb)
                {
                    Instantiate(deathSplatters[selectedSplatter], transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
                else
                {
                    if (deathSplatters[selectedSplatter] != null)
                    {
                        deathSplatters[selectedSplatter].SetActive(true);
                    }

                }

            }

            Invoke("Destroy", 1f);
            
        }

    }
}
