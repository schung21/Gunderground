using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneObject : MonoBehaviour
{
    public static RuneObject instance;
    public Text stats, runes;
    public bool inRuneZone;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        inRuneZone = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRuneZone = true;
            UIController.instance.interactButton.SetActive(true);
            runes.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRuneZone = false;
            UIController.instance.interactButton.SetActive(false);
            runes.gameObject.SetActive(false);
        }
    }
}
