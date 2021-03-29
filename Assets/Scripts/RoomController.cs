using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject door;
    public List<TargetSetController> targetSets;
    public GameObject nextRoom;

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
        door.SetActive(true);
        roomStarted = false;
    }

    public void startRoom()
    {
        if (!roomStarted)
        {
            roomStarted = true;
            activeSet = 0;
            targetSets[activeSet].activateSet();
            door.SetActive(true);
            if (nextRoom != null)
                nextRoom.GetComponent<RoomController>().roomStarted = false;
        }
    }

    public void nextSet()
    {
        activeSet++;
        if (activeSet < targetSets.Count)
            targetSets[activeSet].activateSet();
        else
            door.SetActive(false);
    }
}
