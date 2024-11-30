using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{


    public GameObject[] brokenPieces;
    public int maxPieces = 5;

    public bool shouldDropItem;
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    public GameObject dirtCrumble;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Dash to destroy (currently not in use)

        /*
        if(other.gameObject.tag == "Player")
        {
            if (PlayerController.instance.dashCounter > 0)
            {

                Destroy(gameObject);

                //show broken pieces
                int piecesToDrop = Random.Range(1, maxPieces);

                for(int i = 0; i < piecesToDrop; i++)
                {

                    int randomPiece = Random.Range(0, brokenPieces.Length);

                    Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);

                }

             
            }*/
      }


    
    public void BulletCollide()
    {
        Destroy(gameObject);

        int piecesToDrop = Random.Range(1, maxPieces);

       /* for (int i = 0; i < piecesToDrop; i++)
        {

            int randomPiece = Random.Range(0, brokenPieces.Length);

            Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);



        }*/

        //drop items
        if (shouldDropItem)
        {
            float dropChance = Random.Range(0, 100f);
        

            if (dropChance < itemDropPercent || itemDropPercent == 100)
            {
                int randomItem = Random.Range(0, itemsToDrop.Length);

                Instantiate(itemsToDrop[randomItem], transform.position, transform.rotation);
            }
         
        }
    }
}
