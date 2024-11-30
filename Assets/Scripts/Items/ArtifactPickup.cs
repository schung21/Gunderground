using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactPickup : MonoBehaviour
{
    [Header("Artifacts")]
    public bool isBandolier;
    public bool isSilverBlt;
    public bool isSneakers;
    public bool isDrones;
    public bool isDrill;
    public bool isKevlar;
    public bool isGunstrap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if (isBandolier)
            {
                ArtifactManager.instance.hasBandolier = true;
            }
            if (isSilverBlt)
            {
                ArtifactManager.instance.hasSilverBlt = true;
            }
            if (isSneakers)
            {
                ArtifactManager.instance.hasSneakers = true;
            }
            if (isDrones)
            {
                ArtifactManager.instance.hasDrones = true;
            }
            if (isDrill)
            {
                ArtifactManager.instance.hasDrill = true;
            }
            if (isKevlar)
            {
                ArtifactManager.instance.hasKevlar = true;
            }
            if (isGunstrap)
            {
                ArtifactManager.instance.hasGunStrap = true;
            }
        }
    }
}
