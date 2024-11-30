using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    public Transform eventPoint1;

    public GameObject triggerBox1;
    public GameObject eventObject1, eventObject2, finalObject1;
    public GameObject wallBox;

    public bool activeEvent1;
 /*   public bool isTimer;
    public float time;
    private float countdown;
*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wallBox.SetActive(true);
            anim.SetTrigger("Event1");
            GetComponent<BoxCollider2D>().enabled = false;

            if (activeEvent1)
            {
                eventObject1.SetActive(true);
            }
        }
    }

    public void Event1()
    {
        finalObject1.SetActive(true);
    }

    // Update is called once per frame
 
}
