using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloatingEnemy : MonoBehaviour
{
    public static FloatingEnemy instance;
    public bool collideDirt;
    GameObject[] Dirt;
    GameObject[] Wall;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Activate();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.GetComponent<EnemyController>() != null)
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
        }

        if(other.gameObject.GetComponent<MiniBossController>() != null)
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
        }


    }

    public void Activate()
    {
        Dirt = GameObject.FindGameObjectsWithTag("Dirt");
        Wall = GameObject.FindGameObjectsWithTag("Wall");


        foreach (GameObject dirts in Dirt)
        {
            if (collideDirt == false)
            {
                if (dirts.GetComponent<BoxCollider2D>() != null)
                {
                    Physics2D.IgnoreCollision(dirts.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
                }
            }
        }

        foreach (GameObject walls in Wall)
        {
            if (collideDirt == false)
            {
                if (walls.GetComponent<CircleCollider2D>() != null)
                {
                    Physics2D.IgnoreCollision(walls.GetComponent<CircleCollider2D>(), GetComponent<CircleCollider2D>());
                }
            }

        }
    }

}



