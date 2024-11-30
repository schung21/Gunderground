using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPlayer : MonoBehaviour
{
    public static SlowPlayer instance;
    public Sprite fadeSprite;
    public bool isObstacle;
    private SpriteRenderer spriteBody;
    public float fadeSpeed;
    private bool fadeOut;
    public float slowAmount;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        spriteBody = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isObstacle)
        {
            Invoke("Disable", 4f);
        }

        if (fadeOut)
        {

            spriteBody.color = new Color(spriteBody.color.r, spriteBody.color.g, spriteBody.color.b, Mathf.MoveTowards(spriteBody.color.a, 0f, fadeSpeed * Time.deltaTime));

        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            if (PlayerController.instance.isUltActive == false)
            {
                PlayerController.instance.activeMoveSpeed = PlayerController.instance.moveSpeed / slowAmount;
            }



        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (PlayerController.instance.isUltActive == false)
            {
                PlayerController.instance.activeMoveSpeed = PlayerController.instance.moveSpeed / slowAmount;
            }

        }
    }



    void OnTriggerExit2D(Collider2D other)
    {

        if (Mathf.RoundToInt(PlayerController.instance.activeMoveSpeed) == Mathf.RoundToInt(PlayerController.instance.moveSpeed / slowAmount))
        {
            PlayerController.instance.activeMoveSpeed = PlayerController.instance.moveSpeed;
        }

      
    }

    public void Disable()
    {
        fadeOut = true;

        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        if (Mathf.RoundToInt(PlayerController.instance.activeMoveSpeed) == Mathf.RoundToInt(PlayerController.instance.moveSpeed / slowAmount))
        {
            PlayerController.instance.activeMoveSpeed = PlayerController.instance.moveSpeed;
        }

    }

 

}

