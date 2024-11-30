using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnorePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), PlayerController.instance.GetComponent<CircleCollider2D>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
