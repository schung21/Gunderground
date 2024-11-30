using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleObjects : MonoBehaviour
{
    public bool notHole;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.GetComponent<FloatingEnemy>() != null)
        {
            if (notHole)
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<BoxCollider2D>());
            }
            else
            {
                Physics2D.IgnoreCollision(other.gameObject.GetComponent<CircleCollider2D>(), GetComponent<CompositeCollider2D>());
            }
        }
    

    }
}
