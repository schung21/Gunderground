using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FixedJoystick : Joystick
{
    public static FixedJoystick instance;
    public bool pressed, canBeTapped;
    public bool tapped;
    public int stickType;
    public Color ogColor;

    private void Awake()
    {
        instance = this;
        canBeTapped = false;
        ogColor = GetComponent<Image>().color; 
    }

    private void Update()
    {
        stickType = UIController.instance.stickType;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (stickType == 0)
        {
            if (HandleRange == 0.6f && canBeTapped == true)
            {
                base.OnPointerDown(eventData);

                FireButton.instance.pressed = true;

                StartCoroutine("tapShoot");

            }
        }
        else if (stickType == 1)
        {

            if (HandleRange == 0.6f)
            {
                if (LevelManager.instance.isCamp == false)
                {
                    base.OnPointerDown(eventData);
                    GetComponentsInChildren<Image>()[1].color = Color.red;
                    FireButton.instance.pressed = true;
                }
            }

        }

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (stickType == 0)
        {
            base.OnPointerUp(eventData);

            if (HandleRange == 0.6f)
            {
                //FireButton.instance.pressed = false;
            }
        }
        else if (stickType == 1)
        {
            base.OnPointerUp(eventData);

            if (HandleRange == 0.6f)
            {
                GetComponentsInChildren<Image>()[1].color = GetComponentInChildren<HandleScript>().ogColor;
                FireButton.instance.pressed = false;
            }

        }

    }

    public IEnumerator tapShoot()
    {
        
            tapped = true;
            yield return new WaitForSeconds(0.1f);
            tapped = false;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stickType == 0)
        {
            if (LevelManager.instance.isCamp == false)//(PlayerController.instance.gameObject.activeInHierarchy)
            {
                if (collision.tag == "Handle" && (HandleRange == 0.6f))
                {
                    StartCoroutine("tapShoot");
                    FireButton.instance.pressed = true;
                    canBeTapped = true;
                    GetComponent<Image>().color = Color.red;


                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (stickType == 0)
        {
            if (collision.tag == "Handle" && (HandleRange == 0.6f))
            {
                FireButton.instance.pressed = false;
                canBeTapped = false;
                GetComponent<Image>().color = ogColor;
            }
        }
    }

}