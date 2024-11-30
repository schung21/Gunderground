using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public Animator anim;
    public bool spawnElite, spawnNormal, realTime, startTime;
    public GameObject[] commonSpawn;
    public GameObject[] rareSpawn;
    public int eliteSpawnPercent;
    public float spawnTime;
    private int delaySpawn;
    private bool initialSpawn;
    private int realTimeRandom;

    // Start is called before the first frame update
    void Start()
    {
        realTimeRandom  = Random.Range(0, commonSpawn.Length);

        if (!realTime)
        {
            SpawnEnemy();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(!startTime && realTime && Vector3.Distance(transform.position, PlayerController.instance.transform.position) < 5f)
        {
            anim.SetBool("isOpen", true);

            startTime = true;

            delaySpawn = 1;

            Instantiate(commonSpawn[realTimeRandom], transform.position, transform.rotation);

            
        }

        if (startTime)
        {
            if (spawnTime > 0)
            {
                spawnTime -= Time.deltaTime;
          


                if (delaySpawn == 1)
                {
                    StartCoroutine(StartSpawn(realTimeRandom));
                }
                
            }
            else
            {
                startTime = false;
                realTime = false;
                StopCoroutine("StartSpawn");
            }
            

        }
    }

    public void SpawnEnemy()
    {
        int eliteChance = Random.Range(0, 100);

        if (spawnElite && spawnNormal)
        {

            if (eliteChance < eliteSpawnPercent || eliteSpawnPercent == 100)
            {
                int eliteRandomNumb = Random.Range(0, rareSpawn.Length);

                Instantiate(rareSpawn[eliteRandomNumb], transform.position, transform.rotation);
            }
            else
            {
                int randomNumb = Random.Range(0, commonSpawn.Length);

                Instantiate(commonSpawn[randomNumb], transform.position, transform.rotation);
            }

        }
        else
        {
            if (spawnNormal)
            {
                int randomNumb = Random.Range(0, commonSpawn.Length);

                Instantiate(commonSpawn[randomNumb], transform.position, transform.rotation);
            }

            if (spawnElite)
            {
                if (eliteChance < eliteSpawnPercent || eliteSpawnPercent == 100)
                {
                    int eliteRandomNumb = Random.Range(0, rareSpawn.Length);

                    Instantiate(rareSpawn[eliteRandomNumb], transform.position, transform.rotation);
                }
            }
        }

    }

   /* private void OnTriggerEnter2D(Collider2D other)
    {
        if (realTime && other.gameObject.CompareTag("Player"))
        { 
            anim.SetBool("isOpen", true);

            startTime = true;

            delaySpawn = 1;

            Instantiate(commonSpawn[realTimeRandom], transform.position, transform.rotation);

            gameObject.GetComponent<BoxCollider2D>().enabled = false;

        }
    }*/

    public IEnumerator StartSpawn(int randomNumb)
    {
        delaySpawn = 0;

        yield return new WaitForSeconds(0.8f);

        delaySpawn = 1;
        
        Instantiate(commonSpawn[randomNumb], transform.position, transform.rotation);

    }
 

   
}
