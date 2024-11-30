using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public static SnakeManager instance;
    [SerializeField] float distanceBetween = .2f;
    [SerializeField] float speed;
    [SerializeField] float turnSpeed;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    public List<GameObject> snakeBody = new List<GameObject>();

    float countUp = 0;
    // Start is called before the first frame update
    void Start()
    {
        /*   speed = GetComponent<HomingProjectile>().Speed;
           turnSpeed = GetComponent<HomingProjectile>().rotateSpeed;
   */
        instance = this;
        CreateBodyParts();
     
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }

        if (snakeBody[0] == null)
        {
            Destroy(gameObject);
        }
        else
        {
            SnakeMovement();
        }

       
    }

    void SnakeMovement()
    {
        // Moves the heads with inputs
        snakeBody[0].GetComponent<Rigidbody2D>().velocity = snakeBody[0].transform.right * speed * Time.deltaTime;
        snakeBody[0].transform.Rotate(new Vector3(0, 0, -turnSpeed * Time.deltaTime));

        if(snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                Marker markM = snakeBody[i - 1].GetComponent<Marker>();
                snakeBody[i].transform.position = markM.markerList[0].position;
                snakeBody[i].transform.rotation = markM.markerList[0].rotation;
                markM.markerList.RemoveAt(0);
            }
        }
    }

    void CreateBodyParts()
    {
        if(snakeBody.Count == 0)
        {

            GameObject temp1 = Instantiate(bodyParts[0], transform.position, transform.rotation, transform);
            if (!temp1.GetComponent<Marker>())
            
                temp1.AddComponent<Marker>();
            
          /*  if (!temp1.GetComponent<Rigidbody2D>())
            {
                temp1.AddComponent<Rigidbody2D>();
                temp1.GetComponent<Rigidbody2D>().gravityScale = 0;
            }*/
            snakeBody.Add(temp1);
            bodyParts.RemoveAt(0);
        }

        
        Marker markM = snakeBody[snakeBody.Count - 1].GetComponent<Marker>();
        if(countUp == 0)
        {
            markM.ClearMarkerList();
        }
        countUp += Time.deltaTime;
        if(countUp >= distanceBetween)
        {
            GameObject temp = Instantiate(bodyParts[0], markM.markerList[0].position, markM.markerList[0].rotation, snakeBody[0].transform);
            if (!temp.GetComponent<Marker>())
            
                temp.AddComponent<Marker>();
            
           /* if (!temp.GetComponent<Rigidbody2D>())
            {
                temp.AddComponent<Rigidbody2D>();
                temp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }*/
            snakeBody.Add(temp);
            bodyParts.RemoveAt(0);
            temp.GetComponent<Marker>().ClearMarkerList();
            countUp = 0;
        }
    }
}
