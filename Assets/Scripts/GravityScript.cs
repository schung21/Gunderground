using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{

    private Transform Target;
    private Rigidbody2D targetRB;
    public float influenceRange;
    public float Intensity;
    public float distanceToTarget;
    public float Length;
    public Animator anim;
    Vector2 pullForce;

    public bool isEnemy, isPlayer;
    private GameObject[] Enemies;
    private GameObject[] Blocks;

    private bool enemyStun;
    // Start is called before the first frame update
    void Start()
    {
        Blocks = GameObject.FindGameObjectsWithTag("Dirt");

        if (Blocks != null)
        {
            foreach (var dirt in Blocks)
            {
                if (dirt != null)
                {
                    if (Vector2.Distance(dirt.transform.position, transform.position) <= influenceRange - 3)
                    {
                        dirt.GetComponent<DestructibleTile>().BulletCollide();

                    }
                }
            }
        }

        if (isEnemy)
        {

            Target = PlayerController.instance.transform;
            targetRB = PlayerController.instance.GetComponent<Rigidbody2D>();


        }

        enemyStun = true;
    }

    // Update is called once per frame
    void Update()
    {
        Length -= Time.deltaTime;

        if (Length > 0)
        {

            if (isEnemy)
            {
                if (Time.timeScale > 0)
                {
                    distanceToTarget = Vector2.Distance(Target.position, transform.position);

                    if (distanceToTarget > 1 && distanceToTarget <= influenceRange)
                    {
                        pullForce = (transform.position - Target.position) / distanceToTarget * Intensity;
                        targetRB.AddForce(pullForce, ForceMode2D.Force);
                    }
                }
            }
            else if (isPlayer)
            {
                if (Time.timeScale > 0)
                {
                    Enemies = GameObject.FindGameObjectsWithTag("Enemy");

                    foreach (var enemy in Enemies)
                    {
                        if (enemy != null)
                        {
                            targetRB = enemy.GetComponent<Rigidbody2D>();

                            distanceToTarget = Vector2.Distance(enemy.transform.position, transform.position);

                            if (distanceToTarget > 1 && distanceToTarget <= influenceRange)
                            {
                                pullForce = (transform.position - enemy.transform.position) / distanceToTarget * Intensity;
                                targetRB.AddForce(pullForce, ForceMode2D.Force);

                                if (enemy.GetComponent<HomingProjectile>() != null)
                                {

                                    enemy.GetComponent<EnemyController>().Stunned(Length + 0.2f);

                                }
                            }

                        }
                    }
                }
            }
        }
        else
        {
            anim.SetTrigger("Close");
        }

      

    }
}
