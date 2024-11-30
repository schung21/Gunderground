using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{

    public float moveSpeed = 3f;
    private Vector3 moveDirection;

    public float deceleration = 5f;

    public float lifetime = 3f;

    public SpriteRenderer theBody;
    public float fadeSpeed = 2.5f;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection.x = Random.Range(-moveSpeed, moveSpeed);
        moveDirection.y = Random.Range(-moveSpeed, moveSpeed);
    }

    // Update is called once per frame
    void Update()
    {

        transform.position += moveDirection * Time.deltaTime;

        //move fast in the beginning then slow down by percentage
        moveDirection = Vector3.Lerp(moveDirection, Vector3.zero, deceleration * Time.deltaTime);

        lifetime -= Time.deltaTime;

        if(lifetime < 0)
        {
            //move fast in the beginning then slow down by a fixed rate
            theBody.color = new Color(theBody.color.r, theBody.color.g, theBody.color.b, Mathf.MoveTowards(theBody.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (theBody.color.a == 0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
