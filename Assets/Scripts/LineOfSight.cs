using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    public bool canSee, alwaysSee;

    public void Start()
    {  
        canSee = true;
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), PlayerController.instance.GetComponent<CircleCollider2D>());
    }
    private void Update()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), PlayerController.instance.GetComponent<CircleCollider2D>());
        if (alwaysSee)
        {
            canSee = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            canSee = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            canSee = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            canSee = true;
        }
    }
}
