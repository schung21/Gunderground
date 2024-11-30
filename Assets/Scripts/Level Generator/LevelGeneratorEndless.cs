using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGeneratorEndless : MonoBehaviour
{
    public static LevelGeneratorEndless instance;
    public GameObject layoutRoom;
    public Color startColor, endColor;

    public int distanceToEnd;

    public Transform generatorPoint;

    public enum Direction { up, right, down, left };
    public Direction selectedDirection;

    public float xOffset = 18f, yOffset = 10;

    public LayerMask whatIsRoom;

    public RoomPrefabs rooms;

    public List<GameObject> generatedOutlines = new List<GameObject>();

    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    public RoomCenter centerStart, centerEnd;
    /*    public RoomCenter centerLoot;
        public RoomCenter centerUpgrade, centerUpgrade2, centerUpgradeBonus;*/

    public RoomCenter[] lootRooms;
    //public RoomCenter[] legendLootRooms;
    public RoomCenter[] shopRooms;
    public RoomCenter[] upgradeRooms;
    public RoomCenter[] upgradeBonusRooms;
    public RoomCenter[] timedBuffRooms;
    public RoomCenter[] mobRooms;
    public RoomCenter[] mobRooms2;
    public RoomCenter[] specialRooms;
    public RoomCenter[] corridorRooms;
    //public RoomCenter[] miniBossRooms;
    public RoomCenter[] potentialCenters;


    private GameObject endRoom;
    private GameObject lootRoom, legendLootRoom;
    private GameObject mobRoom;
    private GameObject mobRoom2;
    private GameObject upgradeRoom, upgradeRoomBonus, upgradeRoomBonus2;
    private GameObject buffRoom;
    private GameObject shopRoom;
    private GameObject specialRoom;
    private GameObject corridorRoom1, corridorRoom2, corridorRoom3;

    public float zPos = 0;

    public bool spawnShop, hasCorridor, freeDistance;
    private bool hasShop;

    //public int randomRoom;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation).GetComponent<SpriteRenderer>().color = startColor;


        selectedDirection = (Direction)Random.Range(0, 4);
        MoveGenerationPoint();

        int randomMap = Random.Range(0, 10);

        if (hasCorridor)
        {
            if (!freeDistance)
            {
                if (randomMap < 5)
                {

                    distanceToEnd = 8;

                    //Left Corridor
                    for (int i = 0; i < 5; i++)
                    {

                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (i > 0 && selectedDirection == Direction.right)
                        {
                            selectedDirection = Direction.left;
                        }


                        if (i + 1 == 2)
                        {

                            lootRoom = newRoom;
                        }
                        if (i + 1 == 3)
                        {
                            mobRoom = newRoom;

                        }
                        if (i + 1 == 4)
                        {
                            int randomRoom = Random.Range(0, 3);

                            if (randomRoom < 2)
                            {
                                upgradeRoom = newRoom;
                            }
                            else if (randomRoom == 2)
                            {
                                buffRoom = newRoom;

                            }
                        }

                        if (i + 1 == 5)
                        {

                            mobRoom2 = newRoom;

                        }
                        /*  if (i + 1 == 6)
                          {


                          }*/


                        MoveGenerationPoint();

                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                    for (int i = 5; i < 8; i++)
                    {

                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);


                        if (i + 1 == 7)
                        {
                            selectedDirection = Direction.left;
                        }

                        if (i + 1 == 8)
                        {


                            corridorRoom1 = newRoom;

                        }


                        MoveGenerationPoint();


                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                    for (int i = 7; i < distanceToEnd; i++)
                    {
                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (selectedDirection == Direction.right)
                        {
                            selectedDirection = Direction.left;
                        }

                        if (i + 1 == distanceToEnd)
                        {
                            newRoom.GetComponent<SpriteRenderer>().color = endColor;
                            //layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                            endRoom = newRoom;
                        }

                        MoveGenerationPoint();


                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }

                }

                else if (randomMap >= 5)
                {

                    distanceToEnd = 8;

                    //Right Corridor
                    for (int i = 0; i < 5; i++)
                    {

                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (i > 0 && selectedDirection == Direction.left)
                        {
                            selectedDirection = Direction.right;
                        }


                        if (i + 1 == 2)
                        {

                            lootRoom = newRoom;
                        }
                        if (i + 1 == 3)
                        {
                            mobRoom = newRoom;

                        }
                        if (i + 1 == 4)
                        {

                            int randomRoom = Random.Range(0, 3);

                            if (randomRoom < 2)
                            {
                                upgradeRoom = newRoom;
                            }
                            else if (randomRoom == 2)
                            {
                                buffRoom = newRoom;

                            }

                        }

                        if (i + 1 == 5)
                        {

                            mobRoom2 = newRoom;

                        }
                        /* if (i + 1 == 6)
                         {
                             buffRoom = newRoom;

                         }


                         if (i + 1 == 8)
                         {

                             specialRoom = newRoom;
                         }*/

                        MoveGenerationPoint();

                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                    for (int i = 5; i < 8; i++)
                    {

                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);


                        if (i + 1 == 7)
                        {
                            selectedDirection = Direction.right;
                        }

                        if (i + 1 == 8)
                        {


                            corridorRoom1 = newRoom;

                        }


                        MoveGenerationPoint();


                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                    for (int i = 7; i < distanceToEnd; i++)
                    {
                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (selectedDirection == Direction.left)
                        {
                            selectedDirection = Direction.right;
                        }

                        if (i + 1 == distanceToEnd)
                        {
                            newRoom.GetComponent<SpriteRenderer>().color = endColor;
                            //layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                            endRoom = newRoom;
                        }

                        MoveGenerationPoint();


                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                }
            }
            else if (freeDistance)
            {
                if (randomMap < 5)
                {

                    //Left Corridor
                    for (int i = 0; i < 3; i++)
                    {

                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (i > 0 && selectedDirection == Direction.right)
                        {
                            selectedDirection = Direction.left;
                        }

                        if (i + 1 == 2)
                        {
                            mobRoom = newRoom;

                        }

                        MoveGenerationPoint();

                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                    for (int i = 3; i < distanceToEnd; i++)
                    {
                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (selectedDirection == Direction.right)
                        {
                            selectedDirection = Direction.left;
                        }

                        if (i + 1 == distanceToEnd)
                        {
                            newRoom.GetComponent<SpriteRenderer>().color = endColor;
                            //layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                            endRoom = newRoom;
                        }

                        MoveGenerationPoint();


                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }

                }

                else if (randomMap >= 5)
                {

 
                    //Right Corridor
                    for (int i = 0; i < 3; i++)
                    {

                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (i > 0 && selectedDirection == Direction.left)
                        {
                            selectedDirection = Direction.right;
                        }

                        if (i + 1 == 2)
                        {
                            mobRoom = newRoom;

                        }

                        MoveGenerationPoint();

                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }    
                    for (int i = 3; i < distanceToEnd; i++)
                    {
                        GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                        layoutRoomObjects.Add(newRoom);

                        selectedDirection = (Direction)Random.Range(0, 4);

                        if (selectedDirection == Direction.left)
                        {
                            selectedDirection = Direction.right;
                        }

                        if (i + 1 == distanceToEnd)
                        {
                            newRoom.GetComponent<SpriteRenderer>().color = endColor;
                            //layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                            endRoom = newRoom;
                        }

                        MoveGenerationPoint();


                        while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                        {

                            MoveGenerationPoint();

                        }

                    }
                }
            }
        }

        else
        {
            //Regular Map
            for (int i = 0; i < distanceToEnd; i++)
            {

                GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);

                layoutRoomObjects.Add(newRoom);

                if (i + 1 == distanceToEnd)
                {
                    newRoom.GetComponent<SpriteRenderer>().color = endColor;
                    //layoutRoomObjects.RemoveAt(layoutRoomObjects.Count - 1);
                    endRoom = newRoom;
                }
                if (i + 1 == distanceToEnd - 2)
                {

                    lootRoom = newRoom;
                }
                if (i + 1 == distanceToEnd - 3)
                {
                    mobRoom = newRoom;

                }
                if (i + 1 == distanceToEnd - 4)
                {

                    int randomRoom = Random.Range(0, 3);

                    if (randomRoom < 2)
                    {
                        upgradeRoom = newRoom;
                    }
                    else if (randomRoom == 2)
                    {
                        buffRoom = newRoom;

                    }

                }


                if (i + 1 == distanceToEnd - 8)
                {

                    mobRoom2 = newRoom;

                }
                if (i + 1 == distanceToEnd - 9)
                {
                    //buffRoom = newRoom;

                }
                if (i + 1 == distanceToEnd - 10)
                {

                }

                if (i + 1 == distanceToEnd - 11)
                {
                    specialRoom = newRoom;
                }

                selectedDirection = (Direction)Random.Range(0, 4);
                MoveGenerationPoint();


                while (Physics2D.OverlapCircle(generatorPoint.position, .2f, whatIsRoom))
                {

                    MoveGenerationPoint();

                }

            }
        }

        //create room outlines
        CreateRoomOutline(Vector3.zero, false, false);
        foreach (GameObject room in layoutRoomObjects)
        {
            zPos += 0.00001f;
            if (room == corridorRoom1)
            {
                CreateRoomOutline(room.transform.position += new Vector3(0f, 0f, zPos), true, false);
            }
            else if (room == corridorRoom2)
            {
                CreateRoomOutline(room.transform.position += new Vector3(0f, 0f, zPos), false, true);
            }
         
            else
            {
                CreateRoomOutline(room.transform.position += new Vector3(0f, 0f, zPos), false, false);
            }

        }

        foreach (GameObject outline in generatedOutlines)
        {
            bool generateCenter = true;

            int extraUpgradeChance = Random.Range(0, 10);
            int shopRoomChance = Random.Range(0, 10);
            int specialRoomChance = Random.Range(0, 10);

            if (outline.transform.position == Vector3.zero)
            {
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }

            if (outline.transform.position == endRoom.transform.position)
            {
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;
            }
            if (lootRooms.Length > 0)
            {
                if (outline.transform.position == lootRoom.transform.position)
                {
                    int centerSelect = Random.Range(0, lootRooms.Length);

                    Instantiate(lootRooms[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;

                }
            }
            if (outline.transform.position == mobRoom.transform.position)
            {
                int centerSelect = Random.Range(0, mobRooms.Length);

                Instantiate(mobRooms[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                generateCenter = false;

            }
            if (mobRooms2.Length > 0)
            {
                if (outline.transform.position == mobRoom2.transform.position)
                {
                    int centerSelect = Random.Range(0, mobRooms2.Length);

                    Instantiate(mobRooms2[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;

                }
            }
            if (upgradeRooms.Length > 0)
            {
                if (upgradeRoom != null && outline.transform.position == upgradeRoom.transform.position)
                {
                    int centerSelect = Random.Range(0, upgradeRooms.Length);

                    Instantiate(upgradeRooms[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;


                }
            }
            if (corridorRooms.Length > 0)
            {
                if (corridorRoom1 != null && outline.transform.position == corridorRoom1.transform.position)
                {

                    Instantiate(corridorRooms[0], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
                if (corridorRoom2 != null && outline.transform.position == corridorRoom2.transform.position)
                {

                    Instantiate(corridorRooms[0], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (timedBuffRooms.Length > 0)
            {
                if (buffRoom != null && outline.transform.position == buffRoom.transform.position)
                {
                    int centerSelect = Random.Range(0, timedBuffRooms.Length);

                    Instantiate(timedBuffRooms[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (shopRooms.Length > 0 && shopRoomChance > 1)
            {
                if (outline.transform.position == shopRoom.transform.position)
                {
                    int centerSelect = Random.Range(0, shopRooms.Length);

                    Instantiate(shopRooms[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }
            if (specialRooms.Length > 0 && specialRoomChance > 5)
            {
                if (outline.transform.position == specialRoom.transform.position)
                {
                    int centerSelect = Random.Range(0, specialRooms.Length);

                    Instantiate(specialRooms[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
                    generateCenter = false;
                }
            }

            if (generateCenter)
            {
                int centerSelect = Random.Range(0, potentialCenters.Length);

                Instantiate(potentialCenters[centerSelect], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
#endif
    }

    public void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {

            case Direction.up:
                generatorPoint.position += new Vector3(0f, yOffset, 0f);

                break;
            case Direction.down:
                generatorPoint.position += new Vector3(0f, -yOffset, 0f);

                break;
            case Direction.right:
                generatorPoint.position += new Vector3(xOffset, 0f, 0f);

                break;
            case Direction.left:
                generatorPoint.position += new Vector3(-xOffset, 0f, 0f);

                break;

        }


    }

    public void CreateRoomOutline(Vector3 roomPos, bool isCorridor1, bool isCorridor2)
    {

        bool roomAbove = Physics2D.OverlapCircle(roomPos + new Vector3(0f, yOffset, 0f), .2f, whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPos + new Vector3(0f, -yOffset, 0f), .2f, whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPos + new Vector3(-xOffset, 0f, 0f), .2f, whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPos + new Vector3(xOffset, 0f, 0f), .2f, whatIsRoom);

        int directionCount = 0;

        if (roomAbove)
        {
            directionCount++;
        }
        if (roomBelow)
        {
            directionCount++;
        }
        if (roomLeft)
        {

            directionCount++;
        }
        if (roomRight)
        {

            directionCount++;
        }

        switch (directionCount)
        {

            case 0:

                Debug.LogError("Room doesn't exist");
                break;

            case 1:

                if (roomAbove)
                {
                    if (spawnShop)
                    {

                        if (hasShop == false)
                        {
                            generatedOutlines.Add(Instantiate(rooms.Shop[5], roomPos, transform.rotation));
                            hasShop = true;
                        }
                        else
                        {
                            generatedOutlines.Add(Instantiate(rooms.singleUp[Random.Range(0, rooms.singleUp.Length)], roomPos, transform.rotation));
                        }


                    }
                    else
                    {
                        generatedOutlines.Add(Instantiate(rooms.singleUp[Random.Range(0, rooms.singleUp.Length)], roomPos, transform.rotation));
                    }
                }
                if (roomBelow)
                {

                    if (spawnShop)
                    {

                        if (hasShop == false)
                        {
                            generatedOutlines.Add(Instantiate(rooms.Shop[Random.Range(0, 2)], roomPos, transform.rotation));
                            hasShop = true;
                        }
                        else
                        {
                            generatedOutlines.Add(Instantiate(rooms.singleDown[Random.Range(0, rooms.singleDown.Length)], roomPos, transform.rotation));
                        }
                        
                  
                    }

                    else
                    {
                        generatedOutlines.Add(Instantiate(rooms.singleDown[Random.Range(0, rooms.singleDown.Length)], roomPos, transform.rotation));
                    }
                }
                if (roomLeft)
                {


                    if (spawnShop)
                    {
                        if(hasShop == false)
                        {
                            generatedOutlines.Add(Instantiate(rooms.Shop[Random.Range(2, 4)], roomPos, transform.rotation));
                            hasShop = true;
                        }
                        else
                        {
                            generatedOutlines.Add(Instantiate(rooms.singleLeft[Random.Range(0, rooms.singleLeft.Length)], roomPos, transform.rotation));
                        }
                    }
                    else
                    {
                        generatedOutlines.Add(Instantiate(rooms.singleLeft[Random.Range(0, rooms.singleLeft.Length)], roomPos, transform.rotation));
                    }
                }
                if (roomRight)
                {
                    if (spawnShop)
                    {
                        if (hasShop == false)
                        {
                            generatedOutlines.Add(Instantiate(rooms.Shop[4], roomPos, transform.rotation));
                            hasShop = true;
                        }
                        else
                        {
                            generatedOutlines.Add(Instantiate(rooms.singleRight[Random.Range(0, rooms.singleRight.Length)], roomPos, transform.rotation));
                        }
                    }
                    else
                    {
                        generatedOutlines.Add(Instantiate(rooms.singleRight[Random.Range(0, rooms.singleRight.Length)], roomPos, transform.rotation));
                    }
                }

                break;

            case 2:

                if (roomAbove && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.upDown[Random.Range(0, rooms.upDown.Length)], roomPos, transform.rotation));
                }
                if (roomAbove && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.upLeft[Random.Range(0, rooms.upLeft.Length)], roomPos, transform.rotation));
                }
                if (roomAbove && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.upRight[Random.Range(0, rooms.upRight.Length)], roomPos, transform.rotation));
                }
                if (roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.downLeft[Random.Range(0, rooms.downLeft.Length)], roomPos, transform.rotation));
                }
                if (roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.downRight[Random.Range(0, rooms.downRight.Length)], roomPos, transform.rotation));
                }
                if (roomRight && roomLeft && isCorridor1 == false && isCorridor2 == false)
                {
                    generatedOutlines.Add(Instantiate(rooms.leftRight[Random.Range(0, rooms.leftRight.Length)], roomPos, transform.rotation));
                }
                if (roomRight && roomLeft && isCorridor1 == true && isCorridor2 == false)
                {
                    float secretChance = Random.Range(0, 10f);

                    if (secretChance >= 7 && secretChance <= 9)
                    {
                        generatedOutlines.Add(Instantiate(rooms.Corridor[1], roomPos, transform.rotation));
                    }
                    else if (secretChance > 9)
                    {
                        generatedOutlines.Add(Instantiate(rooms.Corridor[2], roomPos, transform.rotation));
                    }
                    else
                    {
                        generatedOutlines.Add(Instantiate(rooms.Corridor[0], roomPos, transform.rotation));
                    }

                }
                if (roomRight && roomLeft && isCorridor1 == false && isCorridor2 == true)
                {
                    generatedOutlines.Add(Instantiate(rooms.Corridor[1], roomPos, transform.rotation));
                }



                break;

            case 3:

                if (roomAbove && roomBelow && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.upDownLeft[Random.Range(0, rooms.upDownLeft.Length)], roomPos, transform.rotation));
                }
                if (roomAbove && roomBelow && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.upDownRight[Random.Range(0, rooms.upDownRight.Length)], roomPos, transform.rotation));
                }
                if (roomBelow && roomRight && roomLeft)
                {
                    generatedOutlines.Add(Instantiate(rooms.downLeftRight[Random.Range(0, rooms.downLeftRight.Length)], roomPos, transform.rotation));
                }
                if (roomAbove && roomLeft && roomRight)
                {
                    generatedOutlines.Add(Instantiate(rooms.leftRightUp[Random.Range(0, rooms.leftRightUp.Length)], roomPos, transform.rotation));
                }

                break;

            case 4:

                if (roomAbove && roomBelow && roomLeft && roomBelow)
                {
                    generatedOutlines.Add(Instantiate(rooms.upDownLeftRight[0], roomPos, transform.rotation));
                }

                break;


        }


    }

}

   

[System.Serializable]
public class RoomPrefabs
{
    public GameObject[] singleUp, singleDown, singleRight, singleLeft,
        upDown, upLeft, upRight, downLeft, downRight, leftRight,
        upDownLeft, upDownRight, downLeftRight, leftRightUp,
        upDownLeftRight, Corridor, Shop;
}
