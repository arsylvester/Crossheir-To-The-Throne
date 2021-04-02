using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject door;
    public List<TargetSetController> targetSets;
    public GameObject nextRoom;
    public Transform SpeakerPosition;

    public bool roomStarted = false;

    int activeSet = 0;

    // Start is called before the first frame update
    void Start()
    {
        resetRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetRoom()
    {
        for (int i = 0; i < targetSets.Count; i++)
        {
            targetSets[i].deactivateSet();
        }
        if(door.GetComponent<DoorController>() != null)
        {
            door.GetComponent<DoorController>().closeDoors(); //new doors
        }
        else // support for old doors
        {
            door.SetActive(true);
        }
        roomStarted = false;
    }

    public void startRoom()
    {
        if (!roomStarted)
        {
            roomStarted = true;
            activeSet = 0;
            targetSets[activeSet].activateSet();
            if (door.GetComponent<DoorController>() != null)
            {
                door.GetComponent<DoorController>().closeDoors(); // new doors
            }
            else // support for old doors
            {
                door.SetActive(true); 
            }
            if (nextRoom != null)
                nextRoom.GetComponent<RoomController>().roomStarted = false;

            MusicPlayer.MoveToRoom(SpeakerPosition.position);
        }
    }

    public void nextSet()
    {
        activeSet++;
        if (activeSet < targetSets.Count)
        {
            targetSets[activeSet].activateSet();
        }
        else
        {
            if (door.GetComponent<DoorController>() != null)
            {
                door.GetComponent<DoorController>().openDoors(); // new doors
            }
            else // support for old doors
            {
                door.SetActive(false);
            }
        }
    }
}
