using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public static FollowPlayer instance;
    public bool Follow, Stop, Return;
    public Rigidbody2D theRB;
    private Vector2 moveDirection;
    public Vector3 startPoint;
    public float rangeToChasePlayer;
    public float moveSpeed;
 
    void Start()
    {
        instance = this;
        startPoint = BossController.instance.partPoints[0].transform.position;

    }

    // Update is called once per frame
    void Update()
    {
    
        
        moveDirection = Vector2.zero;

        if (Follow == true)
        {

            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {

                moveDirection = PlayerController.instance.transform.position - transform.position;

                theRB.velocity = moveDirection * moveSpeed;

            }

            else
            {
                Follow = false;
                moveDirection = Vector2.zero;
                theRB.velocity = Vector2.zero;

            }


        }
        if(Stop == true)
        {
            moveDirection = Vector2.zero;
            theRB.velocity = Vector2.zero;
           
       
        }

        if (Return == true)
        {

            moveDirection = startPoint - transform.position;

            theRB.velocity = moveDirection * moveSpeed;


        }

        if(Stop == false && Return == false && Follow == false)
        {
            transform.position = startPoint;
        }

       
    }

}
