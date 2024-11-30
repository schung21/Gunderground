using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompanionController : MonoBehaviour
{

    public static CompanionController instance;
    public Rigidbody2D theRB;
    public float moveSpeed;
    private float activeMoveSpeed;
    public float dashSpeed = 8f, dashLength = .5f, dashCooldown = 1f, dashInvinc = .5f, skillCooldown, skillLength;

    public Transform critPoint, muzzlePoint, muzzlePoint2, effectPoint;
    public GameObject critEffect, muzzleFlash, LOS;

    public bool shouldWander, shouldJump, shouldRunAway, shouldChasePlayer, shouldShoot, unlimitedMove, isFireAll, isSlime, hasGun, turnFirepoint, isCloseCombat;
    public bool noGunMoving;

    public float rangeToChasePlayer;
    private Vector3 moveDirection;

    public float playerGapRange;

    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter;
    private Vector3 wanderDirection;

    [HideInInspector]
    public float oldPos;
    [HideInInspector]
    public float dashCounter, skillCounter;
    private float dashCoolCounter, skillCoolCounter;
    [HideInInspector]
    public float invincCount;

    public Animator anim;

    public int health, skillChance;

    public GameObject[] deathSplatters;
    public GameObject bloodEffect;
    public GameObject bulletFire, specialAtk;
    public GameObject gunEffect;
    //public Transform Target;
    public Transform gunArm, gunArmSub, skillPoint;


    public Transform firePoint, firePoint2, droolPoint;
    public Transform[] firePoints;

    public float timeBetweenShots;
    private float shotCounter;

    public float rangeToShootEnemy;

    private GameObject nearestEnemy = null;
    private GameObject[] bosses = null;
    private GameObject[] enemies = null;

    private GameObject enemy, boss;

    public SpriteRenderer theBody;
    public GameObject detectCircle;
    public float flashTime;
    Color originColor;

    public bool visible, isMerc, isDual, isUnit, isSkill, isCollide;

    public Vector2 gunDirection;

    [Header("Player Summon")]
    public bool playerSummon;

    [Header("UniqueUnit")]
    public bool isUnique;

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;

        if (!isUnit)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
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
        if (!playerSummon)
        {
            if (SceneManager.GetActiveScene().name == "0" || SceneManager.GetActiveScene().name == "Title Scene" ||
                LevelManager.instance.isBoss || LevelManager.instance.isWideBoss || LevelManager.instance.isTallBoss)
            {
                Destroy(gameObject);
            }
        }
        else if (playerSummon)
        {
            if(SceneManager.GetActiveScene().name == "0" || SceneManager.GetActiveScene().name == "Title Scene")
            {
                Destroy(gameObject);
            }
        }

        if(PlayerHealth.instance.currentHealth <= 0)
        {
            shouldShoot = false;
            
        }
        else if (PlayerHealth.instance.currentHealth > 0)
        {
            shouldShoot = true;
        }

        if (turnFirepoint)
        {
            if(transform.localScale.x == -1)
            {
                gunArmSub.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                gunArmSub.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        if (!hasGun)
        {
            if (isCollide && activeMoveSpeed == 0)
            {

                ResetTarget();
                
            }
        }

        if (hasGun)
        {
            if (isCollide && !isUnique)
            {
                if(nearestEnemy != null)
                {
                  
                    if(Vector3.Distance(nearestEnemy.transform.position, transform.position) < 2)
                    {
                        if (activeMoveSpeed == 0)
                        {
                            activeMoveSpeed = 3;
                            moveSpeed = 3;
                        }
                        
                    }
                    else
                    {
                        activeMoveSpeed = 0;
                        moveSpeed = 0;
                    }
                }
            }
            /* Vector2 targetPos = PlayerController.instance.transform.position;
             gunDirection = targetPos - (Vector2)transform.position;*/

            //****auto aim system*****//

            if (isMerc == true)
            {
                gunArm.gameObject.SetActive(true);
                //enemy + boss autoaim

                float bestAngle = -1f;

                var distance = float.MaxValue;

                /* if (enemies != null)
                 {
                     foreach (GameObject enemy in enemies)
                     {
                         if (enemy != null)
                         {
                             if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                             {
                                 distance = Vector3.Distance(transform.position, enemy.transform.position);
                                 StartCoroutine(TargetEnemy(enemy));
                                 Vector3 vectorToEnemy = enemy.transform.position - transform.position;
                                 vectorToEnemy.Normalize();
                                 float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;


                                 if (isCloseCombat)
                                 {
                                     if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                                     {

                                         nearestEnemy = enemy;
                                         bestAngle = angleToEnemy;


                                         if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                         {

                                             transform.localScale = new Vector3(-1f, 1f, 1f);
                                             gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                             gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                             muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                                         }
                                         else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                         {

                                             transform.localScale = Vector3.one;
                                             gunArm.localScale = Vector3.one;
                                             gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                             muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);


                                         }
                                     }
                                     else if (nearestEnemy == null)
                                     {


                                         if (PlayerController.instance.transform.position.x > transform.position.x)
                                         {
                                             transform.localScale = Vector3.one;

                                         }
                                         else if (PlayerController.instance.transform.position.x <= transform.position.x)
                                         {
                                             transform.localScale = new Vector3(-1f, 1f, 1f);


                                         }

                                         gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                         gunArm.localScale = Vector3.one;


                                     }
                                 }

                                 if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                                 {
                                     StartCoroutine(TargetEnemy(enemy));

                                     bestAngle = angleToEnemy;



                                     if (vectorToEnemy.x < 0f)//&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                                     {

                                         transform.localScale = new Vector3(-1f, 1f, 1f);
                                         gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                         gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                         muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                                     }
                                     else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                                     {

                                         transform.localScale = Vector3.one;
                                         gunArm.localScale = Vector3.one;
                                         gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                         muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);

                                     }
                                 }


                                 else if (nearestEnemy == null)
                                 {

                                     if (PlayerController.instance.transform.position.x > transform.position.x)
                                     {
                                         transform.localScale = Vector3.one;


                                     }
                                     else if (PlayerController.instance.transform.position.x <= transform.position.x)
                                     {
                                         transform.localScale = new Vector3(-1f, 1f, 1f);


                                     }

                                     gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                     gunArm.localScale = Vector3.one;

                                 }
                             }
                         }
                     }
                 }*/
                if (enemy != null)
                {
                    if(enemy.GetComponent<EnemyController>() != null)
                    {
                        if(enemy.GetComponent<EnemyController>().health <= 0)
                        {
                            ResetTarget();
                        }
                        if (Vector3.Distance(transform.position, enemy.transform.position) > rangeToShootEnemy)
                        {
                            ResetTarget();
                        }
                    }

                    if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, enemy.transform.position);
                        StartCoroutine(TargetEnemy(enemy));
                        Vector3 vectorToEnemy = enemy.transform.position - transform.position;
                        vectorToEnemy.Normalize();
                        float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;


                        if (isCloseCombat)
                        {
                            if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                            {

                                nearestEnemy = enemy;
                                bestAngle = angleToEnemy;


                                if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                    muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                                }
                                else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                    muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);


                                }
                            }
                            else if (nearestEnemy == null)
                            {


                                if (PlayerController.instance.transform.position.x > transform.position.x)
                                {
                                    transform.localScale = Vector3.one;

                                }
                                else if (PlayerController.instance.transform.position.x <= transform.position.x)
                                {
                                    transform.localScale = new Vector3(-1f, 1f, 1f);


                                }

                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                gunArm.localScale = Vector3.one;


                            }
                        }

                        if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                        {
                            StartCoroutine(TargetEnemy(enemy));

                            bestAngle = angleToEnemy;



                            if (vectorToEnemy.x < 0f)//&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                               
                            }
                            else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);


                            }
                        }
                     


                        else if (nearestEnemy == null)
                        {

                            if (PlayerController.instance.transform.position.x > transform.position.x)
                            {
                                transform.localScale = Vector3.one;


                            }
                            else if (PlayerController.instance.transform.position.x <= transform.position.x)
                            {
                                transform.localScale = new Vector3(-1f, 1f, 1f);


                            }

                            gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                            gunArm.localScale = Vector3.one;

                        }
                    }
                }

                /*if (bosses != null)
                {

                    foreach (GameObject boss in bosses)
                    {
                        if (boss != null)
                        {
                            if (Vector3.Distance(transform.position, boss.transform.position) < distance)
                            {
                                distance = Vector3.Distance(transform.position, boss.transform.position);
                                nearestEnemy = boss;
                                Vector3 vectorToEnemy = boss.transform.position - transform.position;
                                vectorToEnemy.Normalize();
                                float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;

                                if (isCloseCombat)
                                {
                                    if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                                    {

                                        nearestEnemy = boss;
                                        bestAngle = angleToEnemy;


                                        if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                        {

                                            transform.localScale = new Vector3(-1f, 1f, 1f);
                                            gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                            gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                            muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                                        }
                                        else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                        {

                                            transform.localScale = Vector3.one;
                                            gunArm.localScale = Vector3.one;
                                            gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                            muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);


                                        }
                                    }
                                    else if (nearestEnemy == null)
                                    {


                                        if (PlayerController.instance.transform.position.x > transform.position.x)
                                        {
                                            transform.localScale = Vector3.one;

                                        }
                                        else if (PlayerController.instance.transform.position.x <= transform.position.x)
                                        {
                                            transform.localScale = new Vector3(-1f, 1f, 1f);


                                        }

                                        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                        gunArm.localScale = Vector3.one;


                                    }
                                }
                                if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                                {

                                    nearestEnemy = boss;
                                    bestAngle = angleToEnemy;


                                    if (vectorToEnemy.x < 0f)//&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                                    {

                                        transform.localScale = new Vector3(-1f, 1f, 1f);
                                        gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                        muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                                    }
                                    else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                                    {

                                        transform.localScale = Vector3.one;
                                        gunArm.localScale = Vector3.one;
                                        gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                        muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);

                                    }
                                }

                                else if (nearestEnemy == null)
                                {

                                    if (PlayerController.instance.transform.position.x > transform.position.x)
                                    {
                                        transform.localScale = Vector3.one;


                                    }
                                    else if (PlayerController.instance.transform.position.x <= transform.position.x)
                                    {
                                        transform.localScale = new Vector3(-1f, 1f, 1f);


                                    }

                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                    gunArm.localScale = Vector3.one;

                                }
                            }
                        }
                    }
                }*/
                if (boss != null)
                {
                    if (Vector3.Distance(transform.position, boss.transform.position) < distance)
                    {
                        distance = Vector3.Distance(transform.position, boss.transform.position);
                        nearestEnemy = boss;
                        Vector3 vectorToEnemy = boss.transform.position - transform.position;
                        vectorToEnemy.Normalize();
                        float angleToEnemy = Mathf.Atan2(vectorToEnemy.y, vectorToEnemy.x) * Mathf.Rad2Deg;

                        if (isCloseCombat)
                        {
                            if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                            {

                                nearestEnemy = boss;
                                bestAngle = angleToEnemy;


                                if (vectorToEnemy.x < 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                {

                                    transform.localScale = new Vector3(-1f, 1f, 1f);
                                    gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                    muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                                }
                                else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, enemy.transform.position) <= 7f)
                                {

                                    transform.localScale = Vector3.one;
                                    gunArm.localScale = Vector3.one;
                                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                    muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);


                                }
                            }
                            else if (nearestEnemy == null)
                            {


                                if (PlayerController.instance.transform.position.x > transform.position.x)
                                {
                                    transform.localScale = Vector3.one;

                                }
                                else if (PlayerController.instance.transform.position.x <= transform.position.x)
                                {
                                    transform.localScale = new Vector3(-1f, 1f, 1f);


                                }

                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                                gunArm.localScale = Vector3.one;


                            }
                        }
                        if ((angleToEnemy > bestAngle || angleToEnemy < bestAngle) && distance <= rangeToShootEnemy)
                        {

                            nearestEnemy = boss;
                            bestAngle = angleToEnemy;


                            if (vectorToEnemy.x < 0f)//&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                            {

                                transform.localScale = new Vector3(-1f, 1f, 1f);
                                gunArm.localScale = new Vector3(-1f, -1f, 1f);
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, -180f);

                            }
                            else if (vectorToEnemy.x > 0f) //&& Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 5f)
                            {

                                transform.localScale = Vector3.one;
                                gunArm.localScale = Vector3.one;
                                gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, bestAngle));

                                muzzlePoint.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);

                            }
                        }

                        else if (nearestEnemy == null)
                        {

                            if (PlayerController.instance.transform.position.x > transform.position.x)
                            {
                                transform.localScale = Vector3.one;


                            }
                            else if (PlayerController.instance.transform.position.x <= transform.position.x)
                            {
                                transform.localScale = new Vector3(-1f, 1f, 1f);


                            }

                            gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                            gunArm.localScale = Vector3.one;

                        }
                    }
                }



                if ((nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > rangeToShootEnemy && boss == null) ||
                    (nearestEnemy != null && Vector3.Distance(nearestEnemy.transform.position, transform.position) > rangeToShootEnemy && boss != null))
                {
                    if (PlayerController.instance.transform.position.x > transform.position.x)
                    {
                        transform.localScale = Vector3.one;

                    }
                    else if (PlayerController.instance.transform.position.x <= transform.position.x)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);

                    }


                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    gunArm.localScale = Vector3.one;

                }

                if (enemy == null && boss == null)
                {


                    if (PlayerController.instance.transform.position.x > transform.position.x)
                    {
                        transform.localScale = Vector3.one;

                    }
                    else if (PlayerController.instance.transform.position.x <= transform.position.x)
                    {
                        transform.localScale = new Vector3(-1f, 1f, 1f);

                    }

                    gunArm.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    gunArm.localScale = Vector3.one;


                }

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

        if (isSkill)
        {
            if (skillCounter > 0)
            {
                skillCounter -= Time.deltaTime;

                if (skillCounter <= 0)
                {

                    skillCoolCounter = skillCooldown;

                }
            }

            if (skillCoolCounter > 0)
            {
                skillCoolCounter -= Time.deltaTime;

            }
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

            else if (shouldChasePlayer == true)
            {

                if (nearestEnemy != null)
                {

                    if (Vector3.Distance(transform.position, nearestEnemy.transform.position) > 5f &&
                        Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= playerGapRange)
                    {
                        moveDirection = PlayerController.instance.transform.position - transform.position;

                        if (noGunMoving)
                        {
                            gunArm.gameObject.SetActive(false);
                        }

                    }

                    else
                    {
                        moveDirection = Vector3.zero;
                        shouldChasePlayer = false;

                        if (noGunMoving)
                        {
                            gunArm.gameObject.SetActive(true);
                        }

                        shouldWander = true;

                    }


                }
                else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= playerGapRange)
                {
                    moveDirection = PlayerController.instance.transform.position - transform.position;

                    if (noGunMoving)
                    {
                        gunArm.gameObject.SetActive(false);
                    }
                }
                else
                {
                    moveDirection = Vector3.zero;
                    shouldChasePlayer = false;

                    if (noGunMoving)
                    {
                        gunArm.gameObject.SetActive(true);
                    }

                    shouldWander = true;

                }

            }

 
            else
            {
                if (shouldWander)
                {

                    if (wanderCounter > 0)

                    {
                        if (noGunMoving)
                        {
                            gunArm.gameObject.SetActive(false);
                        }

                        wanderCounter -= Time.deltaTime;

                        //move the object
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
                                    Physics2D.IgnoreLayerCollision(17, 12);
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
                                    Physics2D.IgnoreLayerCollision(17, 12);
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
                                    Physics2D.IgnoreLayerCollision(17, 12);
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
                                    Physics2D.IgnoreLayerCollision(17, 12);
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
                        if (isSkill && nearestEnemy != null)
                        {
                            if ( Vector3.Distance(transform.position, nearestEnemy.transform.position) < rangeToShootEnemy
                                && LOS.GetComponent<LineOfSight>().canSee == true)
                            {
                                if (skillCounter <= 0 && skillCoolCounter <= 0)
                                {
                                    skillCounter = skillLength;
                                    SkillAnim();
                                }
                            }

                        }

                        if (noGunMoving)
                        {
                            gunArm.gameObject.SetActive(true);
                        }

                        if (nearestEnemy != null)
                        {
                            if (theBody.isVisible && shouldShoot &&
                                Vector3.Distance(transform.position, nearestEnemy.transform.position) < rangeToShootEnemy
                                && LOS.GetComponent<LineOfSight>().canSee == true)
                            {
                                shotCounter -= Time.deltaTime;

                                if (shotCounter <= 0)
                                {

                                    shotCounter = timeBetweenShots;

                                    Instantiate(bulletFire, firePoint.position, firePoint.rotation);
                                    Instantiate(muzzleFlash, muzzlePoint.position, muzzlePoint.rotation);

                                    if (isDual)
                                    {
                                        Instantiate(bulletFire, firePoint2.position, firePoint2.rotation);
                                        Instantiate(muzzleFlash, muzzlePoint2.position, muzzlePoint2.rotation);

                                    }

                                    if (gunEffect != null)
                                    {

                                        Instantiate(gunEffect, effectPoint.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
                                    }

                                }
                            }
                        }

                        if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) >= playerGapRange)
                        {
                            StartCoroutine(stopForSec());
                            wanderCounter = 0;
                        }

                        pauseCounter -= Time.deltaTime;

                        if (isFireAll)
                        {
                            rangeToShootEnemy = 0;
                        }


                        if (pauseCounter <= 0)
                        {
                            if (isFireAll)
                            {
                                rangeToShootEnemy = 7;
                            }

                            wanderCounter = wanderLength;

                            wanderDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
                        }

                    }
                }
            }

            if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 15f && !isCollide)
            {
                transform.position = PlayerController.instance.transform.position;
           
            }

            if (nearestEnemy != null)
            {
                if (Vector3.Distance(transform.position, nearestEnemy.transform.position) <= 4f)
                {
                    wanderDirection = (transform.position - nearestEnemy.transform.position);
                    moveDirection = (transform.position - nearestEnemy.transform.position);
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
            if (moveSpeed > 0)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }

          /*  if (isSlime)
            {
                Instantiate(bloodEffect, droolPoint.position, droolPoint.rotation);

            }*/
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (invincCount > 0)
        {
            invincCount -= Time.deltaTime;
            
        }


    }

    //bullet hits enemy trigger function
    public void DamageCompanion()
    {

        if (invincCount <= 0)
        {
            invincCount = 1f;

            int selectedSplatter = Random.Range(0, deathSplatters.Length);
            //int selectedSplatter2 = Random.Range(0, deathSplatters2.Length);


            theBody.color = Color.red;
            Invoke("ResetColor", flashTime);


            health -= 1;

            int rotation = Random.Range(0, 4);


            if (bloodEffect != null)
            {
                Instantiate(bloodEffect, transform.position, transform.rotation);
            }

            if (health <= 0)
            {


                Destroy(gameObject);


                if (deathSplatters.Length != 0)
                {
                    var deathSprite = Instantiate(deathSplatters[selectedSplatter], transform.position, transform.rotation /*Quaternion.Euler(0f, 0f, rotation * 90)*/);
                    deathSprite.transform.localScale = transform.localScale;

                }

               

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

/*    public IEnumerator RunWhenHit()
    {
        runawayRange += 5f;
        yield return new WaitForSeconds(1f);
        runawayRange -= 5f;
    }*/

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

    public IEnumerator TargetEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(0.3f);

        nearestEnemy = enemy;

    }

    public void ResetTarget()
    {
        detectCircle.GetComponent<Animator>().SetTrigger("reset");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(other.gameObject.GetComponent<EnemyController>() != null)
            {
                if (other.gameObject.GetComponent<EnemyController>().health > 0)
                {
                    enemy = other.gameObject;
                }
            }
            if (other.gameObject.GetComponent<MiniBossController>() != null)
            {
                if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                {
                    enemy = other.gameObject;
                }
            }

        }

        if(other.gameObject.CompareTag("Boss"))
        {
            boss = other.gameObject;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isCollide)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                DamageCompanion();
            }
        }
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (isCollide)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                DamageCompanion();
            }
        }
    }

    public void SkillAnim()
    {
        int random = Random.Range(0, 10);

        if (random <= skillChance)
        {
            anim.SetTrigger("Skill");

        }
    }
    public void SkillAtk()
    {

        Instantiate(specialAtk, skillPoint.transform.position, Quaternion.Euler(0f, 0f, 0f));

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


