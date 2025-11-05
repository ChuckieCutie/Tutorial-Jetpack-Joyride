using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms = new List<GameObject>();
    private float screenWidthInPoints;

    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GeneratorCheck());
    }

    void Update()
    {
        
    }

    void AddRoom(float farthestRoomEndX)
    {
        int randomRoomIndex = Random.Range(0, availableRooms.Length);
        GameObject room = Instantiate(availableRooms[randomRoomIndex]);

        float roomWidth = room.transform.Find("floor").localScale.x;
        
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
        room.transform.position = new Vector3(roomCenter, 0, 0);
        currentRooms.Add(room);
    }


    private void GenerateRoomIfRequired()
    {
        List<GameObject> roomsToRemove = new List<GameObject>();
        bool addRooms = true;
        float playerX = transform.position.x;
        float removeRoomX = playerX - screenWidthInPoints;
        float addRoomX = playerX + screenWidthInPoints;
        float farthestRoomEndX = 0;

        if (currentRooms.Count > 0)
        {
            // Set initial value based on the first room
            // Note: This logic assumes rooms are added sequentially and currentRooms[0] is the "oldest"
            // The tutorial's logic finds the max end X, so we should initialize based on that.
            // Let's refine this to be safer, like the tutorial implies:
            float maxEndX = 0;
            foreach (var roomCheck in currentRooms)
            {
                float w = roomCheck.transform.Find("floor").localScale.x;
                float startX = roomCheck.transform.position.x - (w * 0.5f);
                float endX = startX + w;
                if (endX > maxEndX)
                {
                    maxEndX = endX;
                }
            }
            farthestRoomEndX = maxEndX;
        }


        foreach (var room in currentRooms)
        {
            float roomWidth = room.transform.Find("floor").localScale.x;
            
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            // Tutorial logic: if (roomStartX > addRoomX)
            // This is likely wrong, as it checks the *start* of the room.
            // A better check is if the *end* of the room is already past the add point.
            // Let's use the tutorial's logic first, as you're following it.
            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }

            if (roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }

            // This line was slightly misplaced in the tutorial's text,
            // it should be inside the loop.
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
            AddRoom(farthestRoomEndX);
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }
}