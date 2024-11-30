using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicochetScript : MonoBehaviour
{
    public static RicochetScript instance;
    public float Speed;

    public Rigidbody2D theRB;

    public bool isSplash, isExplode, isExplodePlus, isPenetrate, noEffects, isShockwave, isBomb, isFire, isIce;
    public bool triggerOnKill, triggerOnCrit, IceOnCrit, FireOnCrit;

    public float explosiveRadius;
    public GameObject impactEffect, critEffect;
    public GameObject impactEnemyEffect;
    public GameObject impactEnemyEffect2;
    public GameObject impactEnemyEffect3;
    public GameObject impactDirtEffect;
    public GameObject invisBlock;
    public GameObject onKillEffect, onCritEffect;

    public float gunCritRate, critDamage = 1.5f;
    public int bulletDamage = 50, bulletDamage2;
    [HideInInspector]
    public int critChance;
    [HideInInspector]
    public int avgDmg, finalDmg;

    public CircleCollider2D revolverCollider;

    public LayerMask whatIsBox;
    public int counter = 0;
    public float knockbackForce, knockbackDuration;

    public float stunDuration;
    public int totalHit;
    private int Hit;

    Gun currentGunData;
    GameObject explosive;

    private CameraController camera;


    private void Start()
    {
        theRB.velocity = transform.right * Speed;

        instance = this;
        explosive = Gun.instance.explosive;

        currentGunData = PlayerController.instance.availableGuns[PlayerController.instance.currentGun];

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        if (bulletDamage <= 50)
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

        avgDmg = Mathf.RoundToInt(Random.Range(bulletDamage, bulletDamage2) + (Random.Range(bulletDamage, bulletDamage2) * PlayerController.instance.playerDamage));
        finalDmg = Mathf.RoundToInt(avgDmg * critDamage);

        if (ArtifactManager.instance.hasDmg == true)
        {
            avgDmg += Mathf.RoundToInt(avgDmg * 0.15f);
            finalDmg += Mathf.RoundToInt(finalDmg * 0.15f);
        }

        if (Gun.instance.isAgainstWall)
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {

        Vector2 dir = theRB.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        theRB.MoveRotation(angle);
       
        if(Vector3.Distance(gameObject.transform.position, PlayerController.instance.transform.position) > 20f)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
      
        Vector3 v = Vector3.Reflect(-other.relativeVelocity, other.contacts[0].normal);
        theRB.velocity = (v).normalized * Speed;

        Hit += 1;

        if (other.gameObject.CompareTag("Enemy"))
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
                            collider.GetComponent<EnemyController>().DamageEnemyNoBlood(finalDmg);
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                            collider.GetComponent<EnemyController>().critPoint.transform.position,
                             Quaternion.Euler(0f, 0f, 0f));

                            collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<EnemyController>().DamageEnemyNoBlood(avgDmg);

                            collider.GetComponent<EnemyController>().DamagePop2(avgDmg);
                        }

                    }
                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        critChance = Random.Range(0, 100);

                        if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(Mathf.RoundToInt(bulletDamage * critDamage));
                            Instantiate(critEffect, transform.position, transform.rotation);
                            Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                            collider.GetComponent<MiniBossController>().critPoint.transform.position,
                            collider.GetComponent<MiniBossController>().critPoint.transform.rotation);

                            collider.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(bulletDamage);

                            collider.GetComponent<MiniBossController>().DamagePop2(avgDmg);
                        }

                    }
                    if (collider.GetComponent<BossController>() != null)
                    {
                        if (collider.GetComponent<BossController>().ghostEffect != null)
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

                                    collider.GetComponent<EnemyController>().DamagePop2(finalDmg);
                                }
                                else
                                {
                                    collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                    collider.GetComponent<EnemyController>().DamagePop(avgDmg);
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

                }
            }
            else
            {

                critChance = Random.Range(0, 100);

                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        other.gameObject.GetComponent<EnemyController>().DamageEnemyNoBlood(finalDmg);
                        Instantiate(critEffect, transform.position, transform.rotation);
                        Instantiate(other.gameObject.GetComponent<EnemyController>().critEffect,
                        other.gameObject.GetComponent<EnemyController>().critPoint.transform.position,
                           Quaternion.Euler(0f, 0f, 0f));

                        other.gameObject.GetComponent<EnemyController>().DamagePop2(finalDmg);
                    }

                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        other.gameObject.GetComponent<MiniBossController>().DamageEnemy(finalDmg);
                        Instantiate(critEffect, transform.position, transform.rotation);
                        Instantiate(other.gameObject.GetComponent<MiniBossController>().critEffect,
                        other.gameObject.GetComponent<MiniBossController>().critPoint.transform.position,
                        other.gameObject.GetComponent<MiniBossController>().critPoint.transform.rotation);

                        other.gameObject.GetComponent<MiniBossController>().DamagePop2(finalDmg);
                    }

                    if (triggerOnCrit)
                    {

                        Instantiate(onCritEffect, transform.position, transform.rotation);

                    }
                }

                else
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        other.gameObject.GetComponent<EnemyController>().DamageEnemyNoBlood(avgDmg);

                        other.gameObject.GetComponent<EnemyController>().DamagePop(avgDmg);
                    }
                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        other.gameObject.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                        other.gameObject.GetComponent<MiniBossController>().DamagePop(avgDmg);

                    }
                }

            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }
        }

        else if (other.gameObject.CompareTag("Boss"))
        {

            if (isSplash)
            {
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<BossController>() != null)
                    {
                        if (collider.GetComponent<BossController>().currentHealth > 0)
                        {
                            critChance = Random.Range(0, 100);

                            if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                            {
                                collider.GetComponent<BossController>().TakeDamage(finalDmg);
                                Instantiate(critEffect, transform.position, transform.rotation);
                                Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                                other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                                collider.GetComponent<BossController>().DamagePop2(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<BossController>().TakeDamage(avgDmg);

                                collider.GetComponent<BossController>().DamagePop(avgDmg);
                            }



                            //other.gameObject.GetComponent<BossController>().TakeDamage(bulletDamage);
                            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
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
                        other.gameObject.GetComponent<BossController>().TakeDamage(Mathf.RoundToInt(bulletDamage * critDamage));
                        Instantiate(critEffect, transform.position, transform.rotation);
                        Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                                other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                                other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                        other.gameObject.GetComponent<BossController>().DamagePop2(finalDmg);

                        if (triggerOnCrit)
                        {

                            Instantiate(onCritEffect, transform.position, transform.rotation);

                        }
                    }
                    else
                    {
                        other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                        other.gameObject.GetComponent<BossController>().DamagePop(avgDmg);
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
                            if (collider.GetComponent<BossController>().ghostEffect != null)
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

                            //other.gameObject.GetComponent<BossController>().TakeDamage(bulletDamage);
                            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));


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

                                collider.GetComponent<MiniBossController>().DamagePop(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }


                        }

                    }

                }



                else
                {

                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();

                }


                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                Instantiate(impactDirtEffect, transform.position, transform.rotation);


            }

        }

        else
        {

            if (isSplash)
            {

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
                        if (collider.GetComponent<BossController>().ghostEffect != null)
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

                        //other.gameObject.GetComponent<BossController>().TakeDamage(bulletDamage);
                        Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));


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

                            collider.GetComponent<MiniBossController>().DamagePop(finalDmg);
                        }
                        else
                        {
                            collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                            collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                        }


                    }

                }

            }

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            }

        }

        if (other.gameObject.CompareTag("Wall"))
        {
            if(Vector3.Distance(transform.position, other.gameObject.transform.position) <= 0.5f)
            {
                GetComponent<Rigidbody2D>().simulated = false;
                Destroy(gameObject);
            }
 
        }

        if (Hit == totalHit)
        {
            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
            Destroy(gameObject);
          
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Dirt")
        {
            if (other.gameObject.GetComponent<DestructibleTile>().isShopItem == false)
            {

                if (isSplash)
                {

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
                            if (collider.GetComponent<BossController>().ghostEffect != null)
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

                            //other.gameObject.GetComponent<BossController>().TakeDamage(bulletDamage);
                            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));


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

                                collider.GetComponent<MiniBossController>().DamagePop(finalDmg);
                            }
                            else
                            {
                                collider.GetComponent<MiniBossController>().DamageEnemy(avgDmg);

                                collider.GetComponent<MiniBossController>().DamagePop(avgDmg);
                            }


                        }

                    }

                }


                else
                {
                 
                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();

                    if (other.gameObject.GetComponent<DestructibleTile>().isBarrier)
                    {
                        Destroy(gameObject);

                    }


                }

                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
                Instantiate(impactDirtEffect, transform.position, transform.rotation);


            }

        }
    }

    public void DestroyBullet()
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
        }
        Destroy(gameObject);

    }
}
