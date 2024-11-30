using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerBullet : MonoBehaviour

{
    public bool isSplash, isExplode, isExplodePlus, isPenetrate, noEffects, isShockwave, isBomb, isFire, isIce, isTrigger, isSummon, dotDmg;
    public bool triggerOnKill, triggerOnCrit, IceOnCrit, FireOnCrit;
    public static PlayerBullet instance;

    public float speed = 7.5f;
    public float explosiveRadius;
    public Rigidbody2D theRB;
    public GameObject impactEffect, critEffect;
    public GameObject impactEnemyEffect;
    public GameObject impactEnemyEffect2;
    public GameObject impactEnemyEffect3;
    public GameObject impactDirtEffect;
    public GameObject invisBlock;
    public GameObject onKillEffect, onCritEffect;

    public float gunCritRate, critDamage = 1.5f; //burnRate, freezeRate;
    public int bulletDamage = 50, bulletDamage2, reducedDmg;
    [HideInInspector]
    public int critChance;
    [HideInInspector]
    public int avgDmg, finalDmg;

    public CircleCollider2D revolverCollider;

    public LayerMask whatIsBox;
    public int counter = 0;
    public float knockbackForce, knockbackDuration;

    public float stunDuration, rangeToBlow;

    Gun currentGunData;
    GameObject explosive;

    private CameraController camera;
    private bool detonate;

    [Header("Boomerang")]
    public bool isReturn;
    public float length;
    private bool returnToPlayer;

    [Header("Sub Gun Bullet")]
    public bool isSub;

    [Header("Prevent Double")]
    public bool isSpawn;

    [Header("Limit Penetrate")]
    public bool halfPen;

    [Header("Grenade")]
    public bool isGrenade;

    [Header("Lasting Bullets")]
    public bool isLasting;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        explosive = Gun.instance.explosive;

        if(bulletDamage <= 50)
        {
            bulletDamage2 = Mathf.RoundToInt(bulletDamage / 1.8f);
        }
        if (bulletDamage > 50 && bulletDamage <= 150)
        {
            bulletDamage2 = Mathf.RoundToInt(bulletDamage / 1.5f);
        }
        else if (bulletDamage > 150)
        {
            bulletDamage2 = Mathf.RoundToInt(bulletDamage / 1.2f);
        }


        currentGunData = PlayerController.instance.availableGuns[PlayerController.instance.currentGun];

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        if (isTrigger)
        {
            Invoke("Enable", 0.1f);
        }

        if(PlayerHealth.instance.currentHealth <= 0)
        {
            Destroy(gameObject);
        }

        avgDmg = Mathf.RoundToInt(Random.Range(bulletDamage, bulletDamage2) + (Random.Range(bulletDamage, bulletDamage2) * PlayerController.instance.playerDamage));
        finalDmg = Mathf.RoundToInt(avgDmg * critDamage);

        if(ArtifactManager.instance.hasDmg == true)
        {
            avgDmg += Mathf.RoundToInt(avgDmg * 0.15f);
            finalDmg += Mathf.RoundToInt(finalDmg * 0.15f);
        }

        if (isSub)
        {
            currentGunData = currentGunData.subGun.GetComponent<Gun>();
        }

        if (gunCritRate != 0 && critDamage != 0)
        {
            if (!currentGunData.isRifle)
            {
                gunCritRate += PlayerController.instance.gunCrit1;
                critDamage += PlayerController.instance.critDmg1;
            }
            else if (currentGunData.isRifle)
            {
                gunCritRate += PlayerController.instance.gunCrit2;
                critDamage += PlayerController.instance.critDmg2;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
      
        if (isLasting)
        {
            if (!GetComponentInParent<Gun>().enabled || !GetComponentInParent<Gun>().gameObject.activeInHierarchy)
            {
                Destroy(gameObject);
            }
        }

        if(!isReturn)
        {
            theRB.velocity = transform.right * speed;
        }
        else if (isReturn)
        {
            Invoke("TurnBack", length);

            if (returnToPlayer)
            {
                Vector3 direction = (PlayerController.instance.transform.position - transform.position);
                direction.Normalize();
                theRB.velocity = direction * speed;
               

                if(Vector3.Distance(PlayerController.instance.transform.position, transform.position) <= 1f)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                theRB.velocity = transform.right * speed;
            }
        }

       
        if (currentGunData.bulletHealth <= 0 && 
            (isPenetrate || isExplode) && (currentGunData.isPenetrate || currentGunData.isExplode))
        {

            /* if (LevelManager.instance.isBoss == true) 
             {
                 BossController.instance.theRB.constraints = RigidbodyConstraints2D.FreezeRotation;
             }*/

            if (halfPen)
            {
                Destroy(gameObject);
            }

        
        }

        if (isBomb)
        {
            if(Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToBlow)
            {
                isBomb = false;
                Instantiate(invisBlock, transform.position, transform.rotation);
                
            }
        }
        if (isExplodePlus)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToBlow)
            {
                isExplodePlus = false;
                Instantiate(invisBlock, transform.position, Quaternion.Euler(0f, 0f, 0f));
              

            }
        }
        if (isSummon)
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > rangeToBlow)
            {
                isSummon = false;
                //Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                Instantiate(invisBlock, transform.position, Quaternion.Euler(0f, 0f, 0f));
                //Destroy(gameObject);
            }
        }
        if (dotDmg)
        {
            if (name.Contains("Laser"))
            {
                if(PlayerController.instance.isMole)
                {
                    if (PlayerController.instance.activeMoveSpeed == PlayerController.instance.dashSpeed)
                    {
                        GetComponent<Animator>().enabled = false;
                        Destroy(gameObject);
                    }
                }
            }
        }
   


    }

    //default unity function > trigger collider on another collider 
    /* private void OnTriggerEnter2D(Collider2D other)
     {

         Destroy(gameObject);

         //retrieve enemycontroller function
         if (other.tag == "Skeleton")
         {
             Instantiate(impactEnemyEffect, transform.position, transform.rotation);
             other.GetComponent<EnemyController>().DamageEnemy(avgDmg);
         }
         if (other.tag == "Breakable")
         {
             Instantiate(impactEffect, transform.position, transform.rotation);
             other.GetComponent<Breakable>().BulletCollide();
         }

         else
         {
             Instantiate(impactEffect, transform.position, transform.rotation);
         }
     }*/

    private void OnCollisionEnter2D(Collision2D other)
    {
  
        if (isExplode)
        {
            camera.camShake();
        }

        PlayerController.instance.isShooting = true;


        if (explosive != null && explosive.activeInHierarchy == true)
        {
            Destroy(explosive);

        }

        //retrieve enemycontroller function
        if (other.gameObject.CompareTag("Enemy"))
        {

            if (triggerOnKill)
            {
                if (other.gameObject.GetComponent<EnemyController>() != null)
                {
                    if (other.gameObject.GetComponent<EnemyController>().health <= avgDmg)
                    {
                        Instantiate(onKillEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);
                    }
                }
            }

            if (other.gameObject.GetComponent<EnemyController>() != null)
            {
                if (isFire)
                {
                    if (other.gameObject.GetComponent<EnemyController>().flameEffect != null)
                    {
                        if (!other.gameObject.GetComponent<EnemyController>().iceEffect.activeInHierarchy)
                        {
                            other.gameObject.GetComponent<EnemyController>().flameEffect.SetActive(true);
                        }
                    }
                }
                if (isIce)
                {
                    if (other.gameObject.GetComponent<EnemyController>().iceEffect != null)
                    {
                        if (!other.gameObject.GetComponent<EnemyController>().flameEffect.activeInHierarchy)
                        {
                            other.gameObject.GetComponent<EnemyController>().iceEffect.SetActive(true);
                        }
                    }
                }
            }

            if (impactEffect != null)
            {
                if (isSpawn)
                {
                    if (PlayerController.instance.canSpawnEffect)
                    {
                        PlayerController.instance.canSpawnEffect = false;
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
                else
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
            }

            
            //StartCoroutine(EnemyController.instance.Knockback(knockbackDuration, knockbackForce, PlayerController.instance.transform));
            float timer = 0;

            if (stunDuration != 0 && other.gameObject.GetComponent<EnemyController>() != null)
            {
                other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
            }

            if (knockbackDuration != 0)
            {
                while (knockbackDuration > timer)
                {
                    timer += Time.deltaTime;
                    Vector2 direction = (PlayerController.instance.transform.position - other.gameObject.transform.position).normalized;
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
                }
            }

            if (isSplash)
            {
                isSplash = false;
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();
                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {
                        if (collider.GetComponent<EnemyController>().health > 0)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {

                                collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.GetComponent<EnemyController>().critEffect,
                                collider.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                collider.GetComponent<EnemyController>().DamagePop2(finalDmg);

                            }
                            else
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<EnemyController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                    if (collider.GetComponent<BossController>() != null)
                    {
                        if (collider.GetComponent<BossController>().ghostEffect != null && !collider.GetComponent<BossController>().ghostEffect.noDodge)
                        {
                            if (collider.GetComponent<BossController>().ghostEffect.isActive == false)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                    collider.GetComponent<BossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                    collider.GetComponent<BossController>().DamagePop(avgDmg);
                                }
                            }
                        }
                        else
                        {
                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                collider.GetComponent<BossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                collider.GetComponent<BossController>().DamagePop(avgDmg);
                            }
                        }


                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<BossController>().currentHealth > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            collider.GetComponent<MiniBossController>().critPoint.transform.position,
                            collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                }
             
            }
            else
            {

                critChance = Random.Range(0, 100);

                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                            other.gameObject.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            other.gameObject.GetComponent<EnemyController>().DamagePop2(finalDmg);

                            if (triggerOnCrit)
                            {

                                Instantiate(onCritEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);

                            }
                            if (IceOnCrit)
                            {
                                if (other.gameObject.GetComponent<EnemyController>().iceEffect != null)
                                {
                                    if (!other.gameObject.GetComponent<EnemyController>().flameEffect.activeInHierarchy)
                                    {
                                        other.gameObject.GetComponent<EnemyController>().iceEffect.SetActive(true);
                                    }
                                }
                            }
                            if (FireOnCrit)
                            {
                                if (other.gameObject.GetComponent<EnemyController>().flameEffect != null)
                                {
                                    if (!other.gameObject.GetComponent<EnemyController>().iceEffect.activeInHierarchy)
                                    {
                                        other.gameObject.GetComponent<EnemyController>().flameEffect.SetActive(true);
                                    }
                                }
                            }
                        }

                    }
                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                        {
                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            other.gameObject.GetComponent<MiniBossController>().critPoint.transform.position,
                            other.gameObject.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            other.gameObject.GetComponent<MiniBossController>().DamagePop2(finalDmg);

                            if (triggerOnCrit)
                            {

                                Instantiate(onCritEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);

                            }
                        }
 
                    }
                }

                else
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                            other.gameObject.GetComponent<EnemyController>().DamagePop(avgDmg);

                        }
                    }
                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                        {
                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            other.gameObject.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }
                    }
                }

            }

            if (!isPenetrate && !isExplodePlus)
            {
                Destroy(gameObject);
            }

        }

        else if (other.gameObject.CompareTag("Boss"))
        {

            if (impactEffect != null)
            {
                if (isSpawn)
                {
                    if (PlayerController.instance.canSpawnEffect)
                    {
                        PlayerController.instance.canSpawnEffect = false;
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
                else
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
            }

            if (isSplash)
            {
                isSplash = false;
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();
                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(collider.GetComponent<EnemyController>().critEffect,
                            collider.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                            collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                        }

                    }

                    if (collider.GetComponent<BossController>() != null)
                    {
                        if (collider.GetComponent<BossController>().ghostEffect != null && !collider.GetComponent<BossController>().ghostEffect.noDodge)
                        {
                            if (collider.GetComponent<BossController>().ghostEffect.isActive == false)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                    collider.GetComponent<BossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                    collider.GetComponent<BossController>().DamagePop(avgDmg);
                                }
                            }
                        }
                        else
                        {
                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                collider.GetComponent<BossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                collider.GetComponent<BossController>().DamagePop(avgDmg);
                            }
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<BossController>().currentHealth > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            collider.GetComponent<MiniBossController>().critPoint.transform.position,
                            collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                }
            }
            else
            {
                if (other.gameObject.GetComponent<BossController>().currentHealth > 0)
                {
                    critChance = Random.Range(0, 100);

                    if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                    {
                        other.gameObject.GetComponent<BossController>().TakeDamage(finalDmg);
                        Instantiate(critEffect, transform.position, transform.rotation);
                        Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                                other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                        other.gameObject.GetComponent<BossController>().DamagePop2(finalDmg);

                        if (triggerOnCrit)
                        {

                            Instantiate(onCritEffect, other.gameObject.transform.position, other.gameObject.transform.rotation);

                        }
                    }
                    else
                    {
                        other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                        other.gameObject.GetComponent<BossController>().DamagePop(avgDmg);

                    }
                }

                //Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

            }


            if (!isPenetrate)
            {

                Destroy(gameObject);

                // BossController.instance.theRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

        }

        else if (other.gameObject.CompareTag("Dirt"))
        {
            if (other.gameObject.GetComponent<DestructibleTile>().isShopItem == false)
            {
                if (impactEffect != null)
                {
                    if (isSpawn)
                    {
                        if (PlayerController.instance.canSpawnEffect)
                        {
                            PlayerController.instance.canSpawnEffect = false;
                            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        }
                    }
                    else
                    {
                        if (!isGrenade)
                        {
                            if (!other.gameObject.GetComponent<DestructibleTile>().isDestroyed)
                            {

                                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

                            }
                        }
                        else if (isGrenade)
                        {
                            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        }
                        
                    }
                }

                if (isSplash)
                {
                    isSplash = false;
                    Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                    foreach (var collider in objectToDmg)
                    {

                        if (collider.GetComponent<DestructibleTile>() != null)
                        {
                            collider.GetComponent<DestructibleTile>().BulletCollide();
                        }

                        if (collider.GetComponent<EnemyController>() != null)
                        {
                            if (collider.GetComponent<EnemyController>().health > 0)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(collider.GetComponent<EnemyController>().critEffect,
                                    collider.GetComponent<EnemyController>().critPoint.transform.position,
                                    Quaternion.Euler(0f, 0f, 0f));

                                    collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                    collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                                }
                            }
                        }

                        if (collider.GetComponent<BossController>() != null)
                        {
                            if (collider.GetComponent<BossController>().ghostEffect != null && !collider.GetComponent<BossController>().ghostEffect.noDodge)
                            {
                                if (collider.GetComponent<BossController>().ghostEffect.isActive == false)
                                {
                                    critChance = Random.Range(0, 100);

                                    if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                    {
                                        collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                        Instantiate(critEffect, transform.position, transform.rotation);
                                        Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                        collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                        collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                        collider.GetComponent<BossController>().DamagePop2(finalDmg);
                                    }
                                    else
                                    {
                                        collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                        collider.GetComponent<BossController>().DamagePop(avgDmg);
                                    }
                                }
                            }
                            else
                            {
                                critChance = Random.Range(0, 100);


                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                    collider.GetComponent<BossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                    collider.GetComponent<BossController>().DamagePop(avgDmg);
                                }
                            }



                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<BossController>().currentHealth > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                        if (collider.GetComponent<MiniBossController>() != null)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                    collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                    collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                    collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                    collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                                }
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<MiniBossController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                    }

                }

                else

                {

                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();

                    Instantiate(impactDirtEffect, transform.position, transform.rotation);

                }


                if (!isPenetrate && !isExplodePlus)
                {
                    Destroy(gameObject);
                }
            }

        }

        else
        {

            if (isSplash)
            {
                isSplash = false;

                if (impactEffect != null)
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
                Destroy(gameObject);
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();

                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {
                        if (collider.GetComponent<EnemyController>().health > 0)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.GetComponent<EnemyController>().critEffect,
                                collider.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }
                        }

                    }

                    if (collider.GetComponent<BossController>() != null)
                    {
                        if (collider.GetComponent<BossController>().ghostEffect != null && !collider.GetComponent<BossController>().ghostEffect.noDodge)
                        {
                            if (collider.GetComponent<BossController>().ghostEffect.isActive == false)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                    collider.GetComponent<BossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                    collider.GetComponent<BossController>().DamagePop(avgDmg);
                                }
                            }
                        }
                        else
                        {
                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                collider.GetComponent<BossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                collider.GetComponent<BossController>().DamagePop(avgDmg);
                            }
                        }

                        //other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        if (collider.GetComponent<MiniBossController>().health > 0)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }

                        }
                    }

                }

            }
            else
            {
                if (isSpawn)
                {
                    if (PlayerController.instance.canSpawnEffect)
                    {
                        PlayerController.instance.canSpawnEffect = false;
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    
                    }
                }
                else
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }

                Destroy(gameObject);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController.instance.isShooting = true;

        if (isExplode)
        {
            camera.camShake();
        }

        if (isPenetrate)
        {
            currentGunData.bulletHealth -= 1;

        }

        //retrieve enemycontroller function
        if (other.gameObject.tag == "Enemy")
        {

            if (other.gameObject.GetComponent<EnemyController>() != null)
            {
                if (triggerOnKill)
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health <= avgDmg)
                        {
                            Instantiate(onKillEffect, other.gameObject.transform.position, transform.rotation);
                        }
                    }
                }

                if (isFire)
                {
                    if (other.gameObject.GetComponent<EnemyController>().flameEffect != null)
                    {
                        if (!other.gameObject.GetComponent<EnemyController>().iceEffect.activeInHierarchy)
                        {
                            other.gameObject.GetComponent<EnemyController>().flameEffect.SetActive(true);
                        }
                    }
                }
                if (isIce)
                {
                    if (other.gameObject.GetComponent<EnemyController>().iceEffect != null)
                    {
                        if (!other.gameObject.GetComponent<EnemyController>().flameEffect.activeInHierarchy)
                        {
                            other.gameObject.GetComponent<EnemyController>().iceEffect.SetActive(true);
                        }
                    }
                }
            }

            
            float timer = 0;

            if (stunDuration != 0)
            {
                other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
            }

            if (knockbackDuration != 0)
            {
                while (knockbackDuration > timer)
                {
                    timer += Time.deltaTime;
                    Vector2 direction = (PlayerController.instance.transform.position - other.gameObject.transform.position).normalized;
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
                }
            }

            if (isSplash)
            {

                isSplash = false;
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();

                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {
                        if (collider.GetComponent<EnemyController>().health > 0)
                        {
                            collider.GetComponent<EnemyController>().Stunned(stunDuration);

                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                                collider.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                collider.GetComponent<EnemyController>().DamagePop2(finalDmg);

                                if (triggerOnCrit)
                                {

                                    Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                                }
                            }
                            else
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<EnemyController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }
                        }

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        if (collider.GetComponent<MiniBossController>().health > 0)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);

                                if (triggerOnCrit)
                                {

                                    Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                                }
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                }
            }
            else
            {

                critChance = Random.Range(0, 100);

                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            if (dotDmg)
                            {
                                Instantiate(critEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                            }
                            else
                            {
                                Instantiate(critEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                            }
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                            Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                            other.gameObject.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            other.gameObject.GetComponent<EnemyController>().DamagePop2(finalDmg);

                            if (triggerOnCrit)
                            {

                                Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                            }
                        }

                    }

                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                        {
                            if (dotDmg)
                            {
                                Instantiate(critEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                            }
                            else
                            {
                                Instantiate(critEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                            }
                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            other.gameObject.GetComponent<MiniBossController>().critPoint.transform.position,
                            other.gameObject.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            other.gameObject.GetComponent<MiniBossController>().DamagePop2(finalDmg);

                            if (triggerOnCrit)
                            {

                                Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                            }
                        }
                    }
                }

                else
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                            other.gameObject.GetComponent<EnemyController>().DamagePop(avgDmg);
                        }
                    }
                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                        {
                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            other.gameObject.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }
                    }
                }

            }

            if (impactEffect != null)
            {
                if (isSpawn)
                {
                    if (PlayerController.instance.canSpawnEffect)
                    {
                        PlayerController.instance.canSpawnEffect = false;
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

                    }
                }
                else
                {
                    if (dotDmg)
                    {
                        Instantiate(impactEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }

        }

        else if (other.gameObject.tag == "Boss")
        {
            

            if (isSplash)
            {

                isSplash = false;
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();

                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {

                        collider.GetComponent<EnemyController>().Stunned(stunDuration);

                        critChance = Random.Range(0, 100);


                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                            collider.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            collider.GetComponent<EnemyController>().DamagePop2(finalDmg);

                            if (triggerOnCrit)
                            {

                                Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                            }
                        }
                        else
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                            collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<EnemyController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            collider.GetComponent<MiniBossController>().critPoint.transform.position,
                            collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);

                            if (triggerOnCrit)
                            {

                                Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                            }
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                }
            }
            else
            {
                critChance = Random.Range(0, 100);

                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                {
                    if (dotDmg)
                    {
                        Instantiate(critEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(critEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    other.gameObject.GetComponent<BossController>().TakeDamage(finalDmg);
                    Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                                  other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                  other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                    other.gameObject.GetComponent<BossController>().DamagePop2(finalDmg);

                    if (triggerOnCrit)
                    {

                        Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                    }
                }
                else
                {
                    other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                    other.gameObject.GetComponent<BossController>().DamagePop(avgDmg);
                }
            }


            if (impactEffect != null) 
            {
               
                if (isSpawn)
                {
                    if (PlayerController.instance.canSpawnEffect)
                    {
                        PlayerController.instance.canSpawnEffect = false;
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

                    }
                }
                else
                {
                    if (dotDmg)
                    {
                        Instantiate(impactEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }


        }

        else if (other.gameObject.tag == "Dirt")
        {

            if (other.gameObject.GetComponent<DestructibleTile>().isShopItem == false)
            {

                if (isSplash)
                {

                    isSplash = false;
                    Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                    foreach (var collider in objectToDmg)
                    {

                        if (collider.GetComponent<DestructibleTile>() != null)
                        {
                            collider.GetComponent<DestructibleTile>().BulletCollide();

                        }

                        if (collider.GetComponent<EnemyController>() != null)
                        {

                            collider.GetComponent<EnemyController>().Stunned(stunDuration);

                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.gameObject.GetComponent<EnemyController>().critEffect,
                                collider.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                collider.GetComponent<EnemyController>().DamagePop2(finalDmg);

                                if (triggerOnCrit)
                                {

                                    Instantiate(onCritEffect, other.gameObject.transform.position, transform.rotation);

                                }
                            }
                            else
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<EnemyController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                        if (collider.GetComponent<MiniBossController>() != null)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<MiniBossController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                    }
                }
                else
                {

                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();



                    if (impactDirtEffect != null && impactEffect != null && !dotDmg)
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        Instantiate(impactDirtEffect, transform.position, transform.rotation);
                    }
                }
            }
        }
       
        else
        {
            if (isSplash)
            {
                isSplash = false;

                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();

                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(collider.GetComponent<EnemyController>().critEffect,
                            collider.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                            collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                        }

                    }

                    if (collider.GetComponent<BossController>() != null)
                    {
                        if (collider.GetComponent<BossController>().ghostEffect != null && !collider.GetComponent<BossController>().ghostEffect.noDodge)
                        {
                            if (collider.GetComponent<BossController>().ghostEffect.isActive == false)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                    collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                    collider.GetComponent<BossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                    collider.GetComponent<BossController>().DamagePop(avgDmg);
                                }
                            }
                        }
                        else
                        {
                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(collider.gameObject.GetComponent<BossController>().critEffect,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                collider.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                collider.GetComponent<BossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                collider.GetComponent<BossController>().DamagePop(avgDmg);
                            }
                        }

                        //other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(collider.gameObject.GetComponent<MiniBossController>().critEffect,
                            collider.GetComponent<MiniBossController>().critPoint.transform.position,
                            collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }

                    }

                }

            }
            if (impactEffect != null && !dotDmg)
            {
                if (isSpawn)
                {
                    if (PlayerController.instance.canSpawnEffect)
                    {
                        PlayerController.instance.canSpawnEffect = false;
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

                    }
                }
                else
                {
                    if (dotDmg)
                    {
                        Instantiate(impactEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }
            }
            if (!isShockwave)
            {
                if (GetComponent<IgnoreWall>() != null)
                {
                    
                }
                else
                {
                    Destroy(gameObject);

                    currentGunData.bulletHealth = currentGunData.initBulletHealth;
                }
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (dotDmg)
        {
            PlayerController.instance.isShooting = true;

            if (isExplode)
            {
                camera.camShake();
            }

            if (isPenetrate)
            {
                currentGunData.bulletHealth -= 1;

            }

            //retrieve enemycontroller function
            if (other.gameObject.tag == "Enemy")
            {

                if (other.gameObject.GetComponent<EnemyController>() != null)
                {
                    if (isFire)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().flameEffect != null)
                        {
                            if (!other.gameObject.GetComponent<EnemyController>().iceEffect.activeInHierarchy)
                            {
                                other.gameObject.GetComponent<EnemyController>().flameEffect.SetActive(true);
                            }
                        }
                    }
                    if (isIce)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().iceEffect != null)
                        {
                            if (!other.gameObject.GetComponent<EnemyController>().flameEffect.activeInHierarchy)
                            {
                                other.gameObject.GetComponent<EnemyController>().iceEffect.SetActive(true);
                            }
                        }
                    }
                }


                float timer = 0;

                if (stunDuration != 0)
                {
                    other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
                }

                if (knockbackDuration != 0)
                {
                    while (knockbackDuration > timer)
                    {
                        timer += Time.deltaTime;
                        Vector2 direction = (PlayerController.instance.transform.position - other.gameObject.transform.position).normalized;
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
                    }
                }

                if (isSplash)
                {

                    isSplash = false;
                    Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                    foreach (var collider in objectToDmg)
                    {

                        if (collider.GetComponent<DestructibleTile>() != null)
                        {
                            collider.GetComponent<DestructibleTile>().BulletCollide();

                        }

                        if (collider.GetComponent<EnemyController>() != null)
                        {
                            if (collider.GetComponent<EnemyController>().health > 0)
                            {
                                collider.GetComponent<EnemyController>().Stunned(stunDuration);

                                critChance = Random.Range(0, 100);


                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                                    collider.GetComponent<EnemyController>().critPoint.transform.position,
                                    Quaternion.Euler(0f, 0f, 0f));

                                    collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                    collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                                }
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<EnemyController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                        if (collider.GetComponent<MiniBossController>() != null)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                critChance = Random.Range(0, 100);

                                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                                {
                                    collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                    Instantiate(critEffect, transform.position, transform.rotation);
                                    Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                    collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                    collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                    collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                    collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                                }
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<MiniBossController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }


                        }

                    }
                }
                else
                {

                    critChance = Random.Range(0, 100);

                    if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                    {
                        if (other.gameObject.GetComponent<EnemyController>() != null)
                        {
                            if (other.gameObject.GetComponent<EnemyController>().health > 0)
                            {
                                if (dotDmg)
                                {
                                    Instantiate(critEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                                }
                                else
                                {
                                    Instantiate(critEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                                }
                                other.gameObject.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                                other.gameObject.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                other.gameObject.GetComponent<EnemyController>().DamagePop2(finalDmg);
                            }
                        }

                        if (other.gameObject.GetComponent<MiniBossController>() != null)
                        {
                            if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                            {
                                if (dotDmg)
                                {
                                    Instantiate(critEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                                }
                                else
                                {
                                    Instantiate(critEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                                }
                                other.gameObject.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                other.gameObject.GetComponent<MiniBossController>().critPoint.transform.position,
                                other.gameObject.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                other.gameObject.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                            }
                        }
                    }

                    else
                    {
                        if (other.gameObject.GetComponent<EnemyController>() != null)
                        {
                            if (other.gameObject.GetComponent<EnemyController>().health > 0)
                            {
                                other.gameObject.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                other.gameObject.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }
                        }
                        if (other.gameObject.GetComponent<MiniBossController>() != null)
                        {
                            if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                            {
                                other.gameObject.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                other.gameObject.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }
                        }
                    }

                }

                if (impactEffect != null)
                {
                    if (dotDmg)
                    {
                        Instantiate(impactEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }



            }

            else if (other.gameObject.tag == "Boss")
            {


                if (isSplash)
                {

                    isSplash = false;
                    Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                    foreach (var collider in objectToDmg)
                    {

                        if (collider.GetComponent<DestructibleTile>() != null)
                        {
                            collider.GetComponent<DestructibleTile>().BulletCollide();

                        }

                        if (collider.GetComponent<EnemyController>() != null)
                        {

                            collider.GetComponent<EnemyController>().Stunned(stunDuration);

                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                                collider.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<EnemyController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                        if (collider.GetComponent<MiniBossController>() != null)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<MiniBossController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                    }
                }
                else
                {
                    critChance = Random.Range(0, 100);

                    if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                    {
                        if (dotDmg)
                        {
                            Instantiate(critEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                        }
                        else
                        {
                            Instantiate(critEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        }
                        other.gameObject.GetComponent<BossController>().TakeDamage(finalDmg);
                        Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                                      other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                      other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                        other.gameObject.GetComponent<BossController>().DamagePop2(finalDmg);
                    }
                    else
                    {
                        other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                        other.gameObject.GetComponent<BossController>().DamagePop(avgDmg);
                    }
                }


                if (impactEffect != null)
                {
                    if (dotDmg)
                    {
                        Instantiate(impactEffect, other.gameObject.transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                    else
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                    }
                }


            }


            else
            {
                if (impactEffect != null && !dotDmg)
                {
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                }
                if (!isShockwave)
                {
                    if (GetComponent<IgnoreWall>() != null)
                    {

                    }
                    else
                    {
                        Destroy(gameObject);

                        currentGunData.bulletHealth = currentGunData.initBulletHealth;
                    }
                }

            }
        }
    }


    //this object is no longer visible / offscreen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    /*    public void FreezeRB()
        {
            BossController.instance.theRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        }*/

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Skeleton"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Enemy")
        {

            if (other.gameObject.GetComponent<EnemyController>() != null)
            {
                if (isFire)
                {
                    if (other.gameObject.GetComponent<EnemyController>().flameEffect != null)
                    {
                        if (!other.gameObject.GetComponent<EnemyController>().iceEffect.activeInHierarchy)
                        {
                            other.gameObject.GetComponent<EnemyController>().flameEffect.SetActive(true);
                        }
                    }
                }
                if (isIce)
                {
                    if (other.gameObject.GetComponent<EnemyController>().iceEffect != null)
                    {
                        if (!other.gameObject.GetComponent<EnemyController>().flameEffect.activeInHierarchy)
                        {
                            other.gameObject.GetComponent<EnemyController>().iceEffect.SetActive(true);
                        }
                    }
                }
            }

            float timer = 0;

            if (stunDuration != 0)
            {
                other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
            }

            if (knockbackDuration != 0)
            {
                while (knockbackDuration > timer)
                {
                    timer += Time.deltaTime;
                    Vector2 direction = (PlayerController.instance.transform.position - other.gameObject.transform.position).normalized;
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
                }
            }

            if (isSplash)
            {

                isSplash = false;
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();

                    }

                    if (collider.GetComponent<EnemyController>() != null)
                    {

                        collider.GetComponent<EnemyController>().Stunned(stunDuration);

                        critChance = Random.Range(0, 100);


                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {

                            collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                            collider.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            collider.GetComponent<EnemyController>().DamagePop2(finalDmg);

                        }
                        else
                        {
                            collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                            collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<EnemyController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            collider.GetComponent<MiniBossController>().critPoint.transform.position,
                            collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }

                        if (isExplodePlus)
                        {
                            if (collider.GetComponent<MiniBossController>().health > 0)
                            {
                                Destroy(gameObject);
                            }
                        }

                    }

                }
            }
            else
            {

                critChance = Random.Range(0, 100);

                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                {

                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemyNoBlood(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                            other.gameObject.GetComponent<EnemyController>().critPoint.transform.position,
                            Quaternion.Euler(0f, 0f, 0f));

                            other.gameObject.GetComponent<EnemyController>().DamagePop2(finalDmg);
                        }

                    }


                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                        {
                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            other.gameObject.GetComponent<MiniBossController>().critPoint.transform.position,
                            other.gameObject.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            other.gameObject.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                        }
                    }

                }

                else
                {

                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemyNoBlood(avgDmg);

                            other.gameObject.GetComponent<EnemyController>().DamagePop(avgDmg);
                        }
                    }


                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        if (other.gameObject.GetComponent<MiniBossController>().health > 0)
                        {
                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            other.gameObject.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }
                    }

                }

            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }

        }

        if (other.gameObject.tag == "Dirt")
        {

            if (other.gameObject.GetComponent<DestructibleTile>().isShopItem == false)
            {
                if (isSplash)
                {

                    isSplash = false;
                    Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                    foreach (var collider in objectToDmg)
                    {

                        if (collider.GetComponent<DestructibleTile>() != null)
                        {
                            collider.GetComponent<DestructibleTile>().BulletCollide();

                        }

                        if (collider.GetComponent<EnemyController>() != null)
                        {

                            collider.GetComponent<EnemyController>().Stunned(stunDuration);

                            critChance = Random.Range(0, 100);


                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                                collider.GetComponent<EnemyController>().critPoint.transform.position,
                                Quaternion.Euler(0f, 0f, 0f));

                                collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<EnemyController>().DamageEnemy(avgDmg);

                                collider.GetComponent<EnemyController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<EnemyController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                        if (collider.GetComponent<MiniBossController>() != null)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                                collider.GetComponent<MiniBossController>().critPoint.transform.position,
                                collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                                collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }

                            if (isExplodePlus)
                            {
                                if (collider.GetComponent<MiniBossController>().health > 0)
                                {
                                    Destroy(gameObject);
                                }
                            }

                        }

                    }
                }
                else
                {

                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();


                    if (impactDirtEffect != null && impactEffect != null)
                    {
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                        Instantiate(impactDirtEffect, transform.position, transform.rotation);
                    }
                }
            }
        }

        if (other.gameObject.tag == "Boss")
        {
         
            critChance = Random.Range(0, 100);

            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
            {
                other.gameObject.GetComponent<BossController>().TakeDamage(finalDmg);
                Instantiate(critEffect, transform.position, transform.rotation);
                Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                              other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                              other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                other.gameObject.GetComponent<BossController>().DamagePop2(finalDmg);
            }
            else
            {
                other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                other.gameObject.GetComponent<BossController>().DamagePop(avgDmg);
            }


        }

    }

    public void Enable()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }
    public void DestroyBullet()
    {
        if (impactEffect != null)
        {

            if (isSpawn)
            {
                if (PlayerController.instance.canSpawnEffect)
                {
                    PlayerController.instance.canSpawnEffect = false;
                    Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

                }
            }
            else
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }

        }
   
        Destroy(gameObject);

    }

    public void TurnBack()
    {
        returnToPlayer = true;
    }

}
