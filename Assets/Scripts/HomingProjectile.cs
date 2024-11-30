using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{

    public Transform Target;
    private int hitCount;
    public int totalHit;
    private Rigidbody2D rb;
    public float Speed, rotateSpeed, rotateDistance, deActCount;
    public GameObject impactEffect, critEffect, Light;
    public float gunCritRate, critDamage = 1.5f;
    public int bulletDamage = 50, bulletDamage2;
    [HideInInspector]
    public int critChance;
    [HideInInspector]
    public int avgDmg, finalDmg;

    private GameObject nearestEnemy = null;

    public GameObject[] enemies, bosses;

    public GameObject enemyController;

    public bool isEnemy, isDelay, isActive, hasBody, isCollide;

    Gun currentGunData;

    private float startSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (hasBody && isEnemy)
        {
            startSpeed = Speed;
            Speed = 0;

            Invoke("MoveAgain", 1f);
        }

        currentGunData = PlayerController.instance.availableGuns[PlayerController.instance.currentGun];

        rb = GetComponent<Rigidbody2D>();

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

        if (isDelay)
        {
            Invoke("Activate", 0.5f);
            rb.velocity = transform.up * Speed;
        }

        //bulletDamage += PlayerController.instance.playerDamage;

        avgDmg = Mathf.RoundToInt(Random.Range(bulletDamage, bulletDamage2) + (Random.Range(bulletDamage, bulletDamage2) * PlayerController.instance.playerDamage));
        finalDmg = Mathf.RoundToInt(avgDmg * critDamage);

        if (ArtifactManager.instance.hasDmg == true)
        {
            avgDmg += Mathf.RoundToInt(avgDmg * 0.15f);
            finalDmg += Mathf.RoundToInt(finalDmg * 0.15f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemy && isActive)
        {
            if (Time.timeScale > 0)
            {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
                bosses = GameObject.FindGameObjectsWithTag("Boss");

                var distance = float.MaxValue;

                if (enemies != null)
                {
                    if (deActCount > 0)
                    {
                        Invoke("Deactivate", deActCount);
                    }

                    foreach (GameObject enemy in enemies)
                    {
                        if (enemy.GetComponent<EnemyController>() != null)
                        {
                            if (enemy.GetComponent<EnemyController>().visible == true)
                            {
                                if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                                {
                                    if (enemy.GetComponent<EnemyController>().health > 0)
                                    {
                                        distance = Vector3.Distance(transform.position, enemy.transform.position);
                                        nearestEnemy = enemy;
                                        Vector3 vectorToEnemy = enemy.transform.position - transform.position;
                                        vectorToEnemy.Normalize();

                                        Target = nearestEnemy.transform;
                                    }

                                }
                            }
                        }

                        if (enemy.GetComponent<MiniBossController>() != null)
                        {
                            if (enemy.GetComponent<MiniBossController>().visible == true)
                            {
                                if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
                                {
                                    distance = Vector3.Distance(transform.position, enemy.transform.position);
                                    nearestEnemy = enemy;
                                    Vector3 vectorToEnemy = enemy.transform.position - transform.position;
                                    vectorToEnemy.Normalize();

                                    Target = nearestEnemy.transform;
                                }
                            }
                        }
                    }
                }
                if (bosses != null)
                {
                    foreach (GameObject boss in bosses)
                    {
                        if (boss.GetComponent<BossController>() != null)
                        {
                            if (boss.GetComponent<BossController>().theBody.isVisible == true)
                            {
                                if (Vector3.Distance(transform.position, boss.transform.position) < distance)
                                {
                                    distance = Vector3.Distance(transform.position, boss.transform.position);
                                    nearestEnemy = boss;
                                    Vector3 vectorToEnemy = boss.transform.position - transform.position;
                                    vectorToEnemy.Normalize();

                                    Target = nearestEnemy.transform;
                                }
                            }
                        }
                    }
                }


                if (Target != null)
                {
                    if (Target.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (Target.gameObject.GetComponent<EnemyController>().health <= 0)
                        {
                            Target = null;
                        }
                    }
                }


                if (Target == null || Target.gameObject.activeInHierarchy == false)
                {

                    if (GetComponent<GhostEffect>() != null)
                    {
                        rotateSpeed = 0;
                        rb.velocity = transform.up * Speed;
                        GetComponent<GhostEffect>().isActive = false;
                    }
                    else
                    {
                        GetComponent<SpriteRenderer>().enabled = false;
                        Light.gameObject.SetActive(false);
                        GetComponent<BoxCollider2D>().enabled = false;
                        ParticleSystem ps = GetComponent<ParticleSystem>();
                        var main = ps.main;
                        main.loop = false;

                    }
                }
            }
        }
        else if (isEnemy)
        {
            var distance = float.MaxValue;

            if (DecoyController.instance != null)
            {
                if (Vector3.Distance(transform.position, DecoyController.instance.transform.position) < distance)
                {
                    if (PlayerHealth.instance.currentHealth > 0)
                    {

                        Vector3 vectorToEnemy = DecoyController.instance.transform.position - transform.position;
                        vectorToEnemy.Normalize();

                        Target = DecoyController.instance.transform;
                    }
                    else if (Vector3.Distance(transform.position, DecoyController.instance.transform.position) > 20)
                    {

                        Target = null;

                    }

                }
            }
            else if (PlayerController.instance != null)
            {

                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < distance)
                {
                    if (PlayerHealth.instance.currentHealth > 0)
                    {

                        Vector3 vectorToEnemy = PlayerController.instance.transform.position - transform.position;
                        vectorToEnemy.Normalize();

                        Target = PlayerController.instance.transform;
                    }
                    else if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > 25)
                    {

                        Target = null;

                    }

                }

            }


            if (!hasBody)
            {
                if (deActCount > 0)
                {
                    Invoke("Deactivate", deActCount);
                }
                else
                {
                    Invoke("Deactivate", 3f);
                }

            }

            if (hasBody)
            {
                if (Time.timeScale > 0)
                {
                    GameObject[] dirt = GameObject.FindGameObjectsWithTag("Dirt");

                    foreach (var a in dirt)
                    {
                        if (Vector3.Distance(transform.position, a.transform.position) < 1.5f)
                        {
                            a.GetComponent<DestructibleTile>().BulletCollide();
                        }
                    }
                }
            }

        }
        
      
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isEnemy && hasBody)
        {
            if (impactEffect != null)
            {

                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));

            }
        }
        else
        {
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.Euler(0f, 0f, 0f));
               
            }
            Destroy(gameObject);
        }

    }
    private void FixedUpdate()
    {

        if (Target != null && GetComponent<BoxCollider2D>().enabled == true)
        {
            if (!hasBody)
            {
                if (Vector3.Distance(transform.position, Target.position) > rotateDistance)
                {
                    Vector2 direction = (Vector2)Target.position - rb.position;

                    direction.Normalize();

                    float rotateAmount = Vector3.Cross(direction, transform.up).z;

                    rb.angularVelocity = -rotateAmount * rotateSpeed;

                    rb.velocity = transform.up * Speed;
                }
                else
                {
                    //start timer for lingering and make the object move away 
                    rb.velocity = transform.up * Speed;
                }
            }
            else if (hasBody)
            {
                if (Vector3.Distance(transform.position, Target.position) > rotateDistance && PlayerHealth.instance.currentHealth > 0)
                {
                    Vector2 direction = (Vector2)Target.position - rb.position;

                    direction.Normalize();

                    float rotateAmount = Vector3.Cross(direction, transform.up).z;

                    rb.angularVelocity = -rotateAmount * rotateSpeed;

                    rb.velocity = transform.up * Speed;
                }
                else
                {
 
                    rb.velocity = transform.up * Speed;
                    
                }
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemy)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!hasBody)
                {
                    Destroy(gameObject);
                }
            }
        }

        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {

                hitCount += 1;

                if (other.gameObject.GetComponent<MiniBossController>() != null)
                {
                    if (hitCount >= 1)
                    {
                        Light.gameObject.SetActive(false);
                        GetComponent<BoxCollider2D>().enabled = false;
                        GetComponent<SpriteRenderer>().enabled = false;
                        ParticleSystem ps = GetComponent<ParticleSystem>();
                        var main = ps.main;
                        main.loop = false;
                    }
                }
                else
                {

                    if (hitCount == totalHit)
                    {
                        Light.gameObject.SetActive(false);
                        GetComponent<BoxCollider2D>().enabled = false;
                        GetComponent<SpriteRenderer>().enabled = false;
                        ParticleSystem ps = GetComponent<ParticleSystem>();
                        var main = ps.main;
                        main.loop = false;
                    }
                }

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


                if (totalHit == 0)
                {
                    Destroy(gameObject);
                }

            }


            if (other.gameObject.CompareTag("Boss"))
            {
                hitCount += 1;

                if (hitCount >= 1)
                {
                    Light.gameObject.SetActive(false);
                    GetComponent<BoxCollider2D>().enabled = false;
                    GetComponent<SpriteRenderer>().enabled = false;
                    ParticleSystem ps = GetComponent<ParticleSystem>();
                    var main = ps.main;
                    main.loop = false;
                }

                critChance = Random.Range(0, 100);

                if (gunCritRate != 0 && critChance <= (gunCritRate + PlayerController.instance.playerCritRate) * 100)
                {
                    if (other.gameObject.GetComponent<BossController>() != null)
                    {
                        other.gameObject.GetComponent<BossController>().TakeDamage(finalDmg);
                        Instantiate(critEffect, transform.position, transform.rotation);
                        Instantiate(other.gameObject.GetComponent<BossController>().critEffect,
                        other.gameObject.GetComponent<BossController>().critPoint.transform.position,
                        other.gameObject.GetComponent<BossController>().critPoint.transform.rotation);

                        other.gameObject.GetComponent<BossController>().DamagePop2(finalDmg);
                    }

                }
                else
                {
                    if (other.gameObject.GetComponent<BossController>() != null)
                    {
                        other.gameObject.GetComponent<BossController>().TakeDamage(avgDmg);

                        other.gameObject.GetComponent<BossController>().DamagePop(avgDmg);
                    }

                }


                if (totalHit == 0)
                {
                    Destroy(gameObject);
                }
            }
        }
       
        if (other.gameObject.CompareTag("Dirt"))
        {
            if (other.gameObject.GetComponent<DestructibleTile>().isShopItem == false)
            {
                other.gameObject.GetComponent<DestructibleTile>().BulletCollide();
            }

        }

    }

    public void MoveAgain()
    {
        Speed = startSpeed;
    }

    public void Deactivate()
    {
        rb.freezeRotation = true;
        /*GetComponent<BoxCollider2D>().enabled = false;
        ParticleSystem ps = GetComponent<ParticleSystem>();*/
     /*   var main = ps.main;
        main.loop = false;*/

    }

    private void OnBecameInvisible()
    {
        if ((isEnemy && !hasBody) || isCollide)
        {
            Destroy(gameObject);
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void NoTarget()
    {
        Target = null;
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
