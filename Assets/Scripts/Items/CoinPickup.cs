using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    public int coinValue = 1;
    public int gemValue = 1;
    public int partsValue = 1;
    public int runeValue = 1;
    public Rigidbody2D theRB;
    public SpriteRenderer theBody;
    public float waitToBeCollected;
    public float rangeToChasePlayer, moveSpeed;
    private Vector3 moveDirection;
    public bool isGem, isKey, isParts, isRunes;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Update()
    {

        if (PlayerController.instance.gameObject.activeInHierarchy && !isParts)
        {

            moveDirection = Vector3.zero;

            if (theBody.isVisible && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < rangeToChasePlayer)
            {

                moveDirection = PlayerController.instance.transform.position - transform.position;

            }

            moveDirection.Normalize();

            theRB.velocity = moveDirection * moveSpeed;
        }

        if (waitToBeCollected > 0)
        {
            waitToBeCollected -= Time.deltaTime;
        }

        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player") && waitToBeCollected <= 0)
        {
            if (isGem)
            {
                
                LevelManager.instance.GetGems(gemValue);

                Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);

            }
            else if (isKey)
            {
                LevelManager.instance.GetKeys(1);

                Destroy(gameObject);
            }
            else if (isParts)
            {
                if (PlayerController.instance.partsBuff)
                {
                    LevelManager.instance.GetParts(Mathf.RoundToInt(partsValue * 1.5f));
                }
                else
                {
                    LevelManager.instance.GetParts(partsValue);
                }

                UIController.instance.parts.text = "x" + LevelManager.instance.currentParts.ToString();

                Instantiate(impactEffect, transform.position, transform.rotation);

                Destroy(gameObject);
            }
            else if (isRunes)
            {
                if (LevelManager.instance.currentRunes < 999)
                {
                    LevelManager.instance.GetRunes(runeValue);
                    Instantiate(impactEffect, transform.position, transform.rotation);
                }
                Destroy(gameObject);

            }
            else
            {
                LevelManager.instance.GetCoins(coinValue);

                UIController.instance.coins.text = "x" + LevelManager.instance.currentCoins.ToString();

                Instantiate(impactEffect, transform.position, transform.rotation);
                Destroy(gameObject);
                
            }

        }


    }
}
