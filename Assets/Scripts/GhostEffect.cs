using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySeconds;
    public GameObject ghost, ghost2;
    public bool isActive, isDash1, isDash2, isTurned, Deactivate, noDodge;

    [Header("Always Active")]
    public bool noStop;

    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = ghostDelay;

    }

    // Update is called once per frame
    void Update()
    {
        if (isActive && !Deactivate)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else if (isDash1)
            {
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);
                ghostDelaySeconds = ghostDelay;

                if (isTurned)
                {
                    currentGhost.transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
            else if (isDash2)
            {
                GameObject currentGhost = Instantiate(ghost2, transform.position, transform.rotation);
                ghostDelaySeconds = ghostDelay;

                if (isTurned)
                {
                    currentGhost.transform.localScale = new Vector3(-1f, 1f, 1f);
                }

            }
            else if (noStop)
            {
                Instantiate(ghost, transform.position, transform.rotation);
                ghostDelaySeconds = ghostDelay;
            }
        }

    }

}
