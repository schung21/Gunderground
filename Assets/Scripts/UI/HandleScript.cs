using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleScript : MonoBehaviour
{

    public Color ogColor;
  
    // Start is called before the first frame update
    void Start()
    {
        ogColor = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (UIController.instance.stickType == 0)
        {
            GetComponent<Image>().color = Color.red;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (UIController.instance.stickType == 0)
        {
            GetComponent<Image>().color = ogColor;
        }
        
    }
}
