using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{

    public static DamageArea instance;
    public float explosiveRadius, distanceToBurn;
    public float knockbackForce, knockbackDuration, stunDuration, dmgDuration, Duration;
    private float timer1 = 0, dmgSeconds;
    public bool isPlayer, isShake, isShake2, isShake3, isDot, isEnemy;
    public bool isFireArea, isIceArea, isSkill, isUlt;
    public int areaDmg;
    private CameraController camera;
    private PlayerBullet playerBullet;
    private bool dmgEnemy;
    private GameObject[] enemies;



    // Start is called before the first frame update
   
    void Start()
    {

        instance = this;

        dmgSeconds = dmgDuration;

        //enemies = GameObject.FindGameObjectsWithTag("Enemy");

        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

       
      /*  if (isFireArea)
        {
            Invoke("ChangeCollider", 0.3f);

            if (isPlayer)
            {
                dmgDuration = 0;
                Invoke("delayAction", 0.2f);

            }

        }*/

        if (isShake)
        {
            camera.camShake();
        }
        if (isShake2)
        {
            camera.weakCamShake();
        }
        if (isShake3)
        {
            camera.weakestCamShake();
        }

        if (!isDot)
        {
            Invoke("Destroy", 5f);
        }

        if (isEnemy)
        {
        /*
            foreach (var enemy in enemies)
            {
                Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                Physics2D.IgnoreCollision(enemy.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
            }*/
        }

        areaDmg += Mathf.RoundToInt(PlayerController.instance.playerDamage * areaDmg);

        if (PlayerController.instance.isMole)
        {
            if(TraitManager.instance.Mole[1] == 1 && isSkill)
            {
                stunDuration += 1f;
            }
            if (TraitManager.instance.Mole[2] == 1 && isUlt)
            {
                areaDmg += 50;
            }
            if (TraitManager.instance.Mole[5] == 1 && isUlt)
            {

                explosiveRadius += 2f;
                transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                GetComponent<CircleCollider2D>().radius += 1;

            }
        }

       
    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.activeInHierarchy && !isDot && !isFireArea)
        {
            Invoke("SetTrigger", 0.05f);
        }

        Physics2D.IgnoreLayerCollision(15, 0);

   
        if (isDot)
        {
            if (Duration > 0)
            {
                Duration -= Time.deltaTime;

                if (dmgDuration > 0)
                {
                    dmgEnemy = false;
                    dmgDuration -= Time.deltaTime;
                }
                else if (dmgDuration < 1)
                {
                    if (!isEnemy)
                    {
                        StartCoroutine(DamageEnemy());
                    }

                }
            }
            else if (Duration <= 0)
            {
              
                if (isFireArea)
                {
                    Destroy(gameObject);
                }
               
            }
        }

        if (isFireArea)
        {
           
           /* foreach(var enemy in enemies)
            {
                if (enemy != null)
                {

                    if (Vector3.Distance(transform.position, enemy.transform.position) <= distanceToBurn)
                    {
                        if (enemy.GetComponent<EnemyController>().health > 0)
                        {
                            enemy.GetComponent<EnemyController>().flameEffect.SetActive(true);
                        }

                    }
                    
                }
            }*/

            if (!isPlayer)
            {
                if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) <= distanceToBurn && PlayerHealth.instance.currentHealth > 0)
                {
                    PlayerHealth.instance.DamagePlayer();
                }
            }
        }

       
    

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
   
        if (other.gameObject.CompareTag("Dirt"))
        {

            Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
            foreach (var collider in objectToDmg)
            {

                if (collider.GetComponent<DestructibleTile>() != null)
                {
                    collider.GetComponent<DestructibleTile>().BulletCollide();


                }

            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isPlayer)
            {
                if (PlayerHealth.instance.invincCount <= 0)
                {

                    StartCoroutine(PlayerController.instance.Stunned(stunDuration));


                    while (knockbackDuration > timer1)
                    {
                        timer1 += Time.deltaTime;
                        Vector2 direction = (other.transform.position - transform.position).normalized;
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                    }

                    PlayerHealth.instance.DamagePlayer();
                }
            }
            else if (isPlayer)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
        }

        if (other.gameObject.CompareTag("Companion"))
        {
            if (!isPlayer)
            {

                other.gameObject.GetComponent<CompanionController>().DamageCompanion();


                while (knockbackDuration > timer1)
                {
                    timer1 += Time.deltaTime;
                    Vector2 direction = (other.transform.position - transform.position).normalized;
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                }


            }
            else if (isPlayer)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
        }

        if (!isEnemy)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                if (explosiveRadius == 0 && other.gameObject.GetComponent<EnemyController>() != null && areaDmg > 0)
                {

                    other.gameObject.GetComponent<EnemyController>().DamageEnemy(areaDmg);

                    other.gameObject.GetComponent<EnemyController>().DamagePop(areaDmg);

                }

                if (isFireArea)
                {
                    if (other.gameObject.GetComponent<EnemyController>().health > 0)
                    {
                        other.gameObject.GetComponent<EnemyController>().flameEffect.SetActive(true);
                    }
                }

                StartCoroutine(Knockback(knockbackDuration, knockbackForce, transform, other.gameObject));

                if (other.gameObject.GetComponent<EnemyController>() != null)
                {
                    other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
                }

                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<EnemyController>() != null)
                    {
                        collider.GetComponent<EnemyController>().DamageEnemy(areaDmg);

                        collider.GetComponent<EnemyController>().DamagePop(areaDmg);
                    }

                    if (collider.GetComponent<MiniBossController>() != null)
                    {
                        collider.GetComponent<MiniBossController>().DamageEnemy(areaDmg);

                        collider.GetComponent<MiniBossController>().DamagePop(areaDmg);
                    }
                }

                if (isDot)
                {
                    if (dmgDuration > 0)
                    {
                        dmgDuration -= Time.deltaTime;
                    }
                    else if (dmgDuration <= 0)
                    {
                        if (other.gameObject.GetComponent<EnemyController>() != null)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(areaDmg);
                            other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
                            dmgDuration = dmgSeconds;

                            other.gameObject.GetComponent<EnemyController>().DamagePop(areaDmg);
                        }

                        if (other.gameObject.GetComponent<MiniBossController>() != null)
                        {

                            other.gameObject.GetComponent<MiniBossController>().DamageEnemy(areaDmg);
                            dmgDuration = dmgSeconds;

                            other.gameObject.GetComponent<MiniBossController>().DamagePop(areaDmg);
                        }
                    }
                }

            }

            if (other.gameObject.CompareTag("Boss"))
            {
                other.gameObject.GetComponent<BossController>().TakeDamage(areaDmg);

                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<BossController>() != null)
                    {

                        if (!isPlayer)
                        {
                            collider.GetComponent<BossController>().TakeDamage(areaDmg);

                            collider.GetComponent<BossController>().DamagePop(areaDmg);
                        }

                    }

                }

            }
        }



    }
  

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.gameObject.CompareTag("Dirt"))
        {
          
            if (explosiveRadius != 0)
            {
                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                foreach (var collider in objectToDmg)
                {

                    if (collider.GetComponent<DestructibleTile>() != null)
                    {
                        collider.GetComponent<DestructibleTile>().BulletCollide();


                    }

                }
            }
            else
            {
                if (other.gameObject.GetComponent<DestructibleTile>() != null)
                {
                    
                    other.gameObject.GetComponent<DestructibleTile>().BulletCollide();
                }
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isPlayer && !isFireArea)
            {
                if (PlayerHealth.instance.invincCount <= 0)
                {

                    StartCoroutine(PlayerController.instance.Stunned(stunDuration));


                    while (knockbackDuration > timer1)
                    {
                        timer1 += Time.deltaTime;
                        Vector2 direction = (other.transform.position - transform.position).normalized;
                        other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                    }

                    PlayerHealth.instance.DamagePlayer();
                }
            }
            else if (isPlayer)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
        }

        if (other.gameObject.CompareTag("Companion"))
        {
            if (!isPlayer && !isFireArea)
            {

                other.gameObject.GetComponent<CompanionController>().DamageCompanion();


                while (knockbackDuration > timer1)
                {
                    timer1 += Time.deltaTime;
                    Vector2 direction = (other.transform.position - transform.position).normalized;
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                }


            }
            else if (isPlayer)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
            }
        }

        if (!isEnemy)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {

                StartCoroutine(Knockback(knockbackDuration, knockbackForce, PlayerController.instance.transform, other.gameObject));

                if (isFireArea)
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.GetComponent<EnemyController>().health > 0)
                        {
                            other.GetComponent<EnemyController>().flameEffect.SetActive(true);
                        }
                    }
                }
                else
                {
                    if (isIceArea)
                    {

                        if (other.gameObject.GetComponent<EnemyController>() != null)
                        {
                            if (other.GetComponent<EnemyController>().health > 0)
                            {
                                other.GetComponent<EnemyController>().iceEffect.SetActive(true);
                            }
                        }
                    }

                    if (other.gameObject.GetComponent<EnemyController>() != null && stunDuration > 0)
                    {
                        other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
                    }


                    if (other.gameObject.GetComponent<EnemyController>() != null && areaDmg > 0)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(areaDmg);

                            other.gameObject.GetComponent<EnemyController>().DamagePop(areaDmg);
                        }

                    }

                    if (other.gameObject.GetComponent<MiniBossController>() != null && areaDmg > 0)
                    {

                        other.gameObject.GetComponent<MiniBossController>().DamageEnemy(areaDmg);

                        other.gameObject.GetComponent<MiniBossController>().DamagePop(areaDmg);

                    }
                    /*
                                Collider2D[] objectToDmg = Physics2D.OverlapCircleAll(transform.position, explosiveRadius);
                                foreach (var collider in objectToDmg)
                                {

                                    if (collider.GetComponent<EnemyController>() != null)
                                    {

                                        collider.GetComponent<EnemyController>().DamageEnemy(areaDmg);

                                    }

                                    if (collider.GetComponent<MiniBossController>() != null)
                                    {

                                        collider.GetComponent<MiniBossController>().DamageEnemy(areaDmg);

                                    }
                                }*/
                }
            }

            if (other.gameObject.CompareTag("Boss"))
            {
                if (areaDmg > 0)
                {
                    other.gameObject.GetComponent<BossController>().TakeDamage(areaDmg);

                    other.gameObject.GetComponent<BossController>().DamagePop(areaDmg);
                }

            }
        }



    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDot)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (!isPlayer)
                {
                    if (PlayerHealth.instance.invincCount <= 0)
                    {

                        StartCoroutine(PlayerController.instance.Stunned(stunDuration));


                        while (knockbackDuration > timer1)
                        {
                            timer1 += Time.deltaTime;
                            Vector2 direction = (other.transform.position - transform.position).normalized;
                            other.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * knockbackForce);
                        }

                        PlayerHealth.instance.DamagePlayer();
                    }
                }
                else if (isPlayer)
                {
                    Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                }
            }

            if (other.gameObject.CompareTag("Enemy"))
            {

                if (dmgEnemy == true)
                {
                    if (other.gameObject.GetComponent<EnemyController>() != null)
                    {
                        if (other.gameObject.GetComponent<EnemyController>().health > 0)
                        {
                            other.gameObject.GetComponent<EnemyController>().DamageEnemy(areaDmg);
                            other.gameObject.GetComponent<EnemyController>().Stunned(stunDuration);
                            dmgDuration = dmgSeconds;

                            other.gameObject.GetComponent<EnemyController>().DamagePop(areaDmg);
                        }
                    }

                    if (other.gameObject.GetComponent<MiniBossController>() != null)
                    {
                        other.gameObject.GetComponent<MiniBossController>().DamageEnemy(areaDmg);
                        dmgDuration = dmgSeconds;

                        other.gameObject.GetComponent<MiniBossController>().DamagePop(areaDmg);
                    }
                }

            }

            if (other.gameObject.CompareTag("Boss"))
            {
                if (dmgEnemy == true)
                {
                    other.gameObject.GetComponent<BossController>().TakeDamage(areaDmg);
                    dmgDuration = dmgSeconds;

                    other.gameObject.GetComponent<BossController>().DamagePop(areaDmg);
                    
                }
            }
        }
    }


    public void SetTrigger()
    {
        if (GetComponent<CircleCollider2D>() != null)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        
    }

    public IEnumerator Knockback(float knockbackDuration, float knockbackForce, Transform obj, GameObject enemy)
    {

    
            float timer = 0;

            while (knockbackDuration > timer)
            {
                timer += Time.deltaTime;
                Vector2 direction = (obj.transform.position - enemy.transform.position).normalized;
                enemy.GetComponent<Rigidbody2D>().AddForce(-direction * knockbackForce);
            }

        

        yield return null;
    }

    public IEnumerator DamageEnemy()
    {
        dmgEnemy = true;
        yield return null;
         
    }
    public void delayAction()
    {
        dmgDuration = 1;
    }
    public void ChangeCollider()
    {
        GetComponent<CircleCollider2D>().radius = distanceToBurn;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

   
}
