using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCircle : MonoBehaviour
{
    // Start is called before the first frame update
    public float RotateSpeed = 5f;
    public float Radius = 0.1f;
    public float delayTime;
    public bool counterClock, followPlayer, fixedPlayer;

    public SpriteRenderer theBody;
    public GameObject lightObject;

    private Vector2 _centre, playerCenter, fixedPlayerCenter;
    private float _angle;

    [Header("Expand")]
    public bool isExpand;

    private void Start()
    {
        _centre = transform.position;
        fixedPlayerCenter = PlayerController.instance.transform.position;
    }

    private void Update()
    {
        playerCenter = PlayerController.instance.transform.position;

        if(isExpand)
        {
            Radius += Time.deltaTime;
        }

        if (delayTime > 0)
        {
            delayTime -= Time.deltaTime;
        }

        if (delayTime <= 0)
        {
            if (theBody != null && !theBody.enabled)
            {
                theBody.enabled = true;
                
                if(lightObject != null)
                {
                    lightObject.SetActive(true);
                }
            }

            if (counterClock)
            {
                _angle += RotateSpeed * Time.deltaTime;

                var offset = new Vector2(Mathf.Sin(-_angle), Mathf.Cos(-_angle)) * Radius;

                if (!followPlayer)
                {
                    if (fixedPlayer)
                    {
                        transform.position = fixedPlayerCenter + offset;
                    }
                    else
                    {
                        transform.position = _centre + offset;
                    }
                }
                else
                {
                    transform.position = playerCenter + offset;
                }
            }
            else
            {
                _angle += RotateSpeed * Time.deltaTime;

                var offset = new Vector2(Mathf.Sin(_angle), Mathf.Cos(_angle)) * Radius;

                if (!followPlayer)
                {

                    if (fixedPlayer)
                    {
                        transform.position = fixedPlayerCenter + offset;
                    }
                    else
                    {
                        transform.position = _centre + offset;
                    }
                }
                else
                {
                    transform.position = playerCenter + offset;
                }
            }
        }
    }
}
