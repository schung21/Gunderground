using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDrop : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public float distanceFromPlayer;
    public bool Stalag, Boulder, Spike;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(PlayerController.instance.transform.position, transform.position) < distanceFromPlayer)
        {
            if (Stalag)
            {
                anim.SetTrigger("Stalagmite");
            }
            if(Boulder)
            {
                anim.SetTrigger("Boulder");
            }
            if(Spike)
            {
                
                anim.SetTrigger("Spike");
              
            }
        }

    }
}
